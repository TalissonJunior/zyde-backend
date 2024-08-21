using Coffee.Infrastructure.DapperDataAccess;

namespace Zyde.Infrastructure.DapperDataAccess.Repositories.PositionEventRepository;

public class PositionEventQueries : CoffeeQuery
{
    public PositionEventQueries() : base("t_position_event")
    {
    }
}