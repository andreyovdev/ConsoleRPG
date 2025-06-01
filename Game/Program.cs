using Game.Application;
using Game.Application.GameStates;

namespace Game
{
	internal class Program
	{
		static void Main(string[] args)
		{

			var game = new GameContext
			{
				CurrentState = new MainMenuState()

			};

			game.Run();
		}
	}
}
