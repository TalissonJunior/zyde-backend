using Coffee.Application.Repositories;
using Coffee.Application.ViewModels;
using Zyde.Application.ViewModels.Device;
using Zyde.Model;

namespace Zyde.Application.Repositories.DeviceRepository;

public interface IDapperDeviceRepository : ICoffeeDapperRepository<Device, DeviceView>
{
    public Task<List<Device>> GetAllWithLastPosition(
        FilterView filterView = null
    );
}