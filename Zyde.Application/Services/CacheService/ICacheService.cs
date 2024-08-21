
using Zyde.Model;

namespace Zyde.Application.Services.CacheService;

public interface ICacheService
{
    public Task InitAsync();
    public Task SetOrUpdateDeviceAsync(Device device);
}