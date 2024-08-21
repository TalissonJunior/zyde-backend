using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using StackExchange.Redis;
using Zyde.Application.Repositories.DeviceRepository;
using Zyde.Application.Repositories.PositionEventRepository;
using Zyde.Application.Repositories.PositionRepository;
using Zyde.Model;
using Zyde.Model.Constants;

namespace Zyde.Application.Services.CacheService;

public class CacheService : ICacheService
{
    private readonly IDapperDeviceRepository dapperDeviceRepository;
    private readonly IDapperPositionRepository dapperPositionRepository;
    private readonly IDapperPositionEventRepository dapperPositionEventRepository;
    private readonly IDistributedCache cache;
    private readonly IConnectionMultiplexer redisConnection;
    private readonly ILogger<CacheService> _logger;

    public CacheService(
        IDapperDeviceRepository dapperDeviceRepository,
        IDapperPositionRepository dapperPositionRepository,
        IDapperPositionEventRepository dapperPositionEventRepository,
        IDistributedCache cache,
        IConnectionMultiplexer redisConnection,
        ILogger<CacheService> logger 
    )
    {
        this.dapperDeviceRepository = dapperDeviceRepository;
        this.dapperPositionRepository = dapperPositionRepository;
        this.dapperPositionEventRepository = dapperPositionEventRepository;
        this.cache = cache;
        this.redisConnection = redisConnection;
        this._logger = logger;
    }

    public async Task InitAsync()
    {
        // Clear all cache entries
        await ClearAllCacheAsync();

        var devices = await dapperDeviceRepository.GetAllWithLastPosition();
        foreach (var device in devices)
        {
            var cacheKey = GetDeviceCacheKey(device.Identifier);
            await cache.SetStringAsync(cacheKey, JsonConvert.SerializeObject(device));
        }
    }

    public async Task SetOrUpdateDeviceAsync(Device device)
    {
        var cacheKey = GetDeviceCacheKey(device.Identifier);
        var cachedDeviceJson = await cache.GetStringAsync(cacheKey);

        // Determine if the device is moving
        device.Positions.First().IsMoving = DetermineIfMoving(device.Positions.First());

        if (cachedDeviceJson == null)
        {
            // Log the new device detection
            _logger.LogInformation($"🆕 New device detected with identifier: {device.Identifier}");

            // Device does not exist in cache, save it to the database and cache it
            device = await dapperDeviceRepository.SaveAsync(device);
            await cache.SetStringAsync(cacheKey, JsonConvert.SerializeObject(device));
        }
        else
        {
            var cachedDevice = JsonConvert.DeserializeObject<Device>(cachedDeviceJson);
            device.Id = cachedDevice.Id;

            var newPosition = device.Positions.First();
            var lastPosition = cachedDevice.Positions?.FirstOrDefault();

            if (lastPosition != null && lastPosition.Latitude == newPosition.Latitude && lastPosition.Longitude == newPosition.Longitude)
            {
                // If the latitude and longitude are the same, update the last position with new properties
                lastPosition.Date = newPosition.Date;
                lastPosition.SpeedInKnots = newPosition.SpeedInKnots;
                lastPosition.Course = newPosition.Course;
                lastPosition.IsMoving = newPosition.IsMoving;
                lastPosition.Device = new Device() { Id = cachedDevice.Id };

                if(newPosition.Attributes.Trim() != "{}") {
                    lastPosition.Attributes = newPosition.Attributes;
                }

                lastPosition = await dapperPositionRepository.SaveAsync(lastPosition);
                device.Positions = new List<Position>() { lastPosition };

                // Also check and log any new events if necessary
                await CheckEventAsync(cachedDevice, lastPosition);
            }
            else
            {
                // Save the new position to the database
                newPosition.Device = new Device() { Id = cachedDevice.Id };
                var savedPosition = await dapperPositionRepository.SaveAsync(newPosition);

                await CheckEventAsync(cachedDevice, savedPosition);

                device.Positions = new List<Position> { savedPosition };
            }

            // Device exists, update the cache
            await cache.SetStringAsync(cacheKey, JsonConvert.SerializeObject(device));
        }
    }

    private async Task CheckEventAsync(Device cachedDevice, Position newPosition)
    {
        var lastPosition = cachedDevice.Positions?.FirstOrDefault();

        if (lastPosition != null && newPosition.Date > lastPosition.Date)
        {
            var newAttributes = JsonConvert.DeserializeObject<Dictionary<string, bool>>(newPosition.Attributes);
            var lastAttributes = JsonConvert.DeserializeObject<Dictionary<string, bool>>(lastPosition.Attributes);

            foreach (var kvp in newAttributes)
            {
                if (lastAttributes.TryGetValue(kvp.Key, out var lastValue) && lastValue != kvp.Value)
                {
                    await OnNewEventAsync(newPosition, cachedDevice.Id, kvp.Key, kvp.Value);
                }
            }
        }
    }

    private async Task OnNewEventAsync(Position position, int deviceId, string eventCode, bool value)
    { 
        // Log the new event detection
        _logger.LogInformation($"🔔 New event detected: Device {deviceId}, Event {eventCode}, Value {value}");

        var positionEvent = new PositionEvent
        {
            Date = position.Date,
            Value = value, 
            Code = eventCode,
            Device = new Device() { Id = deviceId },
            Position = new Position() { Id = position.Id }
        };

        await dapperPositionEventRepository.SaveAsync(positionEvent);
    }

    private bool DetermineIfMoving(Position position)
    {
        // Default to not moving
        bool isMoving = false;

        // Check if speed is greater than 0
        if (position.SpeedInKnots > 0)
        {
            isMoving = true;
        }

        // Check engine status if available
        var engineOn = position.GetAttribute<bool>(PositionEventCode.EngineOn);

        // If engine is on and speed is > 0, definitely moving
        if (engineOn && position.SpeedInKnots > 0)
        {
            isMoving = true;
        }

        return isMoving;
    }

    private async Task ClearAllCacheAsync()
    {
        var endpoints = redisConnection.GetEndPoints();
        var server = redisConnection.GetServer(endpoints.First());

        // Retrieve all keys that match the pattern
        var keys = server.Keys(pattern: $"zyde{CacheCode.Device}-*").ToArray();

        foreach (var key in keys)
        {
            await cache.RemoveAsync(key);
        }
    }

    private string GetDeviceCacheKey(string identifier)
    {
        return $"{CacheCode.Device}-{identifier}";
    }
}