using Zyde.Application.Repositories.PositionEventRepository;
using Coffee.Infrastructure.DapperDataAccess;
using Zyde.Application.ViewModels.PositionEvent;
using Zyde.Model;

namespace Zyde.Infrastructure.DapperDataAccess.Repositories.PositionEventRepository;

public class PositionEventRepository : CoffeeDapperRepository<PositionEventQueries, PositionEvent, PositionEventView>, IDapperPositionEventRepository
{
    public PositionEventRepository(string connectionString) : base(connectionString)
    {
    }
}