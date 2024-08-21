using Coffee.Application.Repositories;
using Zyde.Application.ViewModels.Position;
using Zyde.Model;

namespace Zyde.Application.Repositories.PositionRepository;

public interface IDapperPositionRepository : ICoffeeDapperRepository<Position, PositionView>
{
}