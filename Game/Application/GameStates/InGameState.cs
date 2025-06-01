using Game.Application.GameStates.Interfaces;
using Game.Application.Services;
using Game.Core.Enums;
using Game.Core.Models;

namespace Game.Application.GameStates
{
	public class InGameState : IGameState
	{
		private GameService _gameService;
		private InGameSubState subState = InGameSubState.ActionChoice;
		private string msg = "";

		public InGameState(GameEntity character)
		{
			_gameService = new GameService(character);
		}

		public void Render()
		{
			switch (subState)
			{
				case InGameSubState.ActionChoice:
					RenderActionChoice();
					break;
				case InGameSubState.Attack:
					RenderAttack();
					break;
				case InGameSubState.Move:
					RenderMove();
					break;
				case InGameSubState.Death:
					RenderDeath();
					break;
			}


		}

		public void HandleInput(GameContext context)
		{
			switch (subState)
			{
				case InGameSubState.ActionChoice:
					HandleActionChoiceInput();
					break;
				case InGameSubState.Attack:
					HandleAttackInput();
					break;
				case InGameSubState.Move:
					HandleMoveInput();
					break;
				case InGameSubState.Death:
					HandleDeathInput(context);
					break;
			}
		}

		//Render substates
		private void RenderActionChoice()
		{
			Console.WriteLine($"Health: {_gameService.GetCharacterHealth()}  Mana: {_gameService.GetCharacterMana()}");
			Console.WriteLine();
			Console.WriteLine(_gameService.DisplayMatrix());
			Console.WriteLine("Choose an action");
			Console.WriteLine("1) Attack");
			Console.WriteLine("2) Move");
			RenderMessage();
		}

		private void RenderAttack()
		{
			RenderActionChoice();

			int[] enemiesInRangeHealth = _gameService.GetEnemiesInRangeHealth();

			if (enemiesInRangeHealth.Length == 0)
			{
				msg = "No available targets in your range";
			}
			else
			{
				Console.WriteLine();
				Console.WriteLine("Which one to attack");
				for (int i = 0; i < enemiesInRangeHealth.Length; i++)
				{
					Console.WriteLine($"{i}) target with remaining blood {enemiesInRangeHealth[i]}");
				}

			}
		}

		private void RenderMove()
		{
			RenderActionChoice();
			Console.WriteLine();
			Console.WriteLine("Moving direction:");
		}

		private void RenderDeath()
		{
			Console.WriteLine("GAME OVER");
			Console.WriteLine();
			Console.WriteLine($"Kills: {_gameService.GetEnemiesKilled()}");
			Console.WriteLine();
			Console.WriteLine("Press any key to continue");
		}

		//Input handle for Substates
		private void HandleActionChoiceInput()
		{
			if (_gameService.GetCharacterHealth() <= 0)
			{
				subState = InGameSubState.Death;
				return;
			}

			ConsoleKey key = Console.ReadKey(true).Key;

			if (key == ConsoleKey.D1)
				subState = InGameSubState.Attack;
			else if (key == ConsoleKey.D2)
				subState = InGameSubState.Move;
			else
				HandleActionChoiceInput();
		}

		private void HandleAttackInput()
		{

			int[] enemiesInRangeHealth = _gameService.GetEnemiesInRangeHealth();

			if (enemiesInRangeHealth.Length == 0)
			{
				msg = "No available targets in your range";
				subState = InGameSubState.ActionChoice;
				return;
			}

			string input = Console.ReadLine();
			bool inputIsNumber = int.TryParse(input, out int enemyIndex);
			if (inputIsNumber && enemyIndex >= 0 && enemyIndex < _gameService.GetEnemiesCount())
			{
				_gameService.CharacterAttack(enemyIndex);
				subState = InGameSubState.ActionChoice;
				msg = "";
			}
			else
			{
				msg = "Invalid enemy number!";
			}
		}

		private void HandleMoveInput()
		{
			ConsoleKey key = Console.ReadKey(true).Key;

			int stepX = 0, stepY = 0;

			switch (key)
			{
				case ConsoleKey.W: stepX = -1; stepY = 0; break;
				case ConsoleKey.S: stepX = 1; stepY = 0; break;
				case ConsoleKey.A: stepX = 0; stepY = -1; break;
				case ConsoleKey.D: stepX = 0; stepY = 1; break;
				case ConsoleKey.Q: stepX = -1; stepY = -1; break;
				case ConsoleKey.E: stepX = -1; stepY = 1; break;
				case ConsoleKey.Z: stepX = 1; stepY = -1; break;
				case ConsoleKey.X: stepX = 1; stepY = 1; break;
				default: HandleMoveInput(); break;
			}

			bool characterOutOfMatrix = _gameService.CharacterStepsOutOfMatrix(stepX, stepY);
			if (characterOutOfMatrix)
			{
				msg = "Can't move outside the field!";
				subState = InGameSubState.ActionChoice;
				return;
			}

			bool characterOnEnemy = _gameService.CharacterStepsOnEnemy(stepX, stepY);
			if (characterOnEnemy)
			{
				msg = "Can't move in this direction. There's enemy!";
				subState = InGameSubState.ActionChoice;
				return;
			}

			subState = InGameSubState.ActionChoice;
			_gameService.CharacterMove(stepX, stepY);
		}

		private void HandleDeathInput(GameContext context)
		{
			var key = Console.ReadKey().Key;
			context.SetState(new MainMenuState());
		}

		//Helper functions
		private void RenderMessage()
		{
			if (!string.IsNullOrWhiteSpace(msg))
			{
				Console.WriteLine("-------------------------");
				Console.WriteLine(msg);
				Console.WriteLine("-------------------------");
				msg = "";
			}
		}
	}
}
