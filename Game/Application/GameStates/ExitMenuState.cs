using Game.Application.GameStates.Interfaces;
using Game.Core.Enums;

namespace Game.Application.GameStates
{
	public class ExitMenuState : IGameState
	{
		public void Render()
		{
			Console.WriteLine("Are you sure you want to exit?");
			Console.WriteLine("y - yes");
			Console.WriteLine("n - no");
		}

		public void HandleInput(GameContext context)
		{

			ConsoleKey input = Console.ReadKey(true).Key;

			if (input == ConsoleKey.Y)
			{
				context.isRunning = false;
			}
			else if (input == ConsoleKey.N)
			{
				context.SetState(new MainMenuState());
			}
			else
			{
				HandleInput(context);
			}
		}
	}
}
