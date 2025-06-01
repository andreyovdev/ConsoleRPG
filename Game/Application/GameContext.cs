using Game.Application.GameStates;
using Game.Application.GameStates.Interfaces;
using Game.Application.Services;

namespace Game.Application
{
	public class GameContext
	{
		public IGameState CurrentState { get; set; }
		public bool isRunning { get; set; }

		public CharacterService CharacterService { get; private set; }
		public GameDbContext GameDbContext { get; private  set; }

		public GameContext()
		{
			this.GameDbContext = new GameDbContext();
			this.CharacterService = new CharacterService(this.GameDbContext);

			Console.OutputEncoding = System.Text.Encoding.UTF8;

		}

		public void Run()
		{
			isRunning = true;

			while (isRunning)
			{
				Console.Clear();
				CurrentState.Render();
				CurrentState.HandleInput(this);
			}
		}

		public void SetState(IGameState newState)
		{
			CurrentState = newState;
		}
	}
}
