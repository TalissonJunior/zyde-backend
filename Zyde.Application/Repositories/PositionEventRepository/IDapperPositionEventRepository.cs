using Coffee.Application.Repositories;
using Zyde.Application.ViewModels.PositionEvent;
using Zyde.Model;

namespace Zyde.Application.Repositories.PositionEventRepository;

public interface IDapperPositionEventRepository : ICoffeeDapperRepository<PositionEvent, PositionEventView>
{
}