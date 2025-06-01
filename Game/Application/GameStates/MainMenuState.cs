using Game.Application.GameStates.Interfaces;
using Game.Core.Enums;

namespace Game.Application.GameStates
{
	public class MainMenuState : IGameState
	{
		public void Render()
		{
			Console.WriteLine("Welcome!\nPress any key to play\nPress Escape to Exit");
		}

		public void HandleInput(GameContext context)
		{
			var key = Console.ReadKey().Key;
			if (key == ConsoleKey.Escape)
				context.SetState(new ExitMenuState());
			else
				context.SetState(new CharacterSelectState(context));
		}
	}
}
