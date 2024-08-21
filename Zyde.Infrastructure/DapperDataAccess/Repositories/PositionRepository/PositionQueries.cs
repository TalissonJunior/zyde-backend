using Coffee.Infrastructure.DapperDataAccess;

namespace Zyde.Infrastructure.DapperDataAccess.Repositories.PositionRepository;

public class PositionQueries : CoffeeQuery
{
    public PositionQueries() : base("t_position")
    {
    }
}