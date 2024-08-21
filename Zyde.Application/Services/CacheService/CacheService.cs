using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using Zyde.Application.Repositories.DeviceRepository;
using Zyde.Application.Repositories.PositionRepository;
using Zyde.Model;

namespace Zyde.Application.Services.CacheService;

public class CacheService : ICacheService
{
    private readonly IDapperDeviceRepository dapperDeviceRepository;
    private readonly IDapperPositionRepository dapperPositionRepository;
    private readonly IDistributedCache cache;

    public CacheService(
        IDapperDeviceRepository dapperDeviceRepository,
        IDapperPositionRepository dapperPositionRepository,
        IDistributedCache cache
    )
    {
        this.dapperDeviceRepository = dapperDeviceRepository;
        this.dapperPositionRepository = dapperPositionRepository;
        this.cache = cache;
    }

    
    public async Task InitAsync()
    {
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

        if (cachedDeviceJson == null)
        {
            // Device does not exist in cache, save it to the database and cache it
            await dapperDeviceRepository.SaveAsync(device);
            await cache.SetStringAsync(cacheKey, JsonConvert.SerializeObject(device));
        }
        else
        {
            var cachedDevice = JsonConvert.DeserializeObject<Device>(cachedDeviceJson);
            device.Id = cachedDevice.Id;

            if(device.Positions != null && device.Positions.First().Id == 0) {
                device.Positions.First().Device = new Device() {
                    Id = cachedDevice.Id
                };

                var position = await dapperPositionRepository.SaveAsync(device.Positions.First());

                device.Positions = new List<Position>() {position };
            }

            // Device exists, update the cache
            await cache.SetStringAsync(cacheKey, JsonConvert.SerializeObject(device));
        }
    }


    private string GetDeviceCacheKey(string identifier)
    {
        return $"{CacheCode.Device}-{identifier}";
    }
}