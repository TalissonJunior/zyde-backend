using Coffee.Infrastructure;
using Coffee.Infrastructure.DapperDataAccess;

namespace Zyde.Infrastructure.DapperDataAccess.Repositories.DeviceRepository;

public class DeviceQueries : CoffeeQuery
{
    public DeviceQueries() : base("t_device")
    {
    }
    
    public SqlBuilder GetAllWithLastPosition()
    {
        var SQL = new SqlBuilder();
         var query = SQL
            .SELECT("*")
            .FROM("t_device")
            .LEFT_JOIN("t_position ON t_position.fk_device_id = t_device.id")
            .WHERE("t_position.id = (SELECT MAX(inner_pos.id) FROM t_position inner_pos WHERE inner_pos.fk_device_id = t_device.id)");

        return query;
    }

}