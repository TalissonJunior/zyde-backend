using Zyde.Application.Repositories.DeviceRepository;
using Coffee.Infrastructure.DapperDataAccess;
using Zyde.Application.ViewModels.Device;
using Zyde.Model;
using Coffee.Application.ViewModels;

namespace Zyde.Infrastructure.DapperDataAccess.Repositories.DeviceRepository;

public class DeviceRepository : CoffeeDapperRepository<DeviceQueries, Device, DeviceView>, IDapperDeviceRepository
{
    public DeviceRepository(string connectionString) : base(connectionString)
    {
    }

    public Task<List<Device>> GetAllWithLastPosition(
        FilterView filterView = null
    )
    {
        return Query<Device>()
        .UseSql(query.GetAllWithLastPosition())
        .Join(model => model.Positions)
        .ExecuteAsyncAsList(filterView);
    }

}