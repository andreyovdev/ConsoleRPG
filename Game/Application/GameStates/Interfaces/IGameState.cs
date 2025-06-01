using Game.Core.Enums;

namespace Game.Application.GameStates.Interfaces
{
	public interface IGameState
	{
		void Render();
		void HandleInput(GameContext context);
	}
}
