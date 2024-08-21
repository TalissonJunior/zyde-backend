using Zyde.Application.Repositories.PositionRepository;
using Coffee.Infrastructure.DapperDataAccess;
using Zyde.Application.ViewModels.Position;
using Zyde.Model;

namespace Zyde.Infrastructure.DapperDataAccess.Repositories.PositionRepository;

public class PositionRepository : CoffeeDapperRepository<PositionQueries, Position, PositionView>, IDapperPositionRepository
{
    public PositionRepository(string connectionString) : base(connectionString)
    {
    }
}