using Game.Application.GameStates.Interfaces;
using Game.Application.Services;
using Game.Core.Enums;
using Game.Core.Models;

namespace Game.Application.GameStates
{
	public class CharacterSelectState : IGameState
	{
		private readonly CharacterService _characterService;

		private CharacterSelectSubState subState = CharacterSelectSubState.ChooseCharacter;
		private GameEntity selectedCharacter = null;
		private string[] characterTypes = Enum.GetNames(typeof(CharacterType));
		private int selectedCharacterIndex = -1;
		private int[] statPoints = new int[3];
		private int remainingPoints = 3;
		private int selectedStatIndex = 0;
		private string statMessage = "";

		public CharacterSelectState(GameContext context)
		{
			_characterService = context.CharacterService;
		}

		public void Render()
		{
			switch (subState)
			{
				case CharacterSelectSubState.ChooseCharacter:
					RenderCharacterSelection();
					break;
				case CharacterSelectSubState.ConfirmStatChange:
					RenderConfirmStatChange();
					break;
				case CharacterSelectSubState.AdjustStats:
					RenderAdjustStats();
					break;
			}
		}

		public void HandleInput(GameContext context)
		{
			switch (subState)
			{
				case CharacterSelectSubState.ChooseCharacter:
					HandleCharacterSelectionInput(context);
					break;
				case CharacterSelectSubState.ConfirmStatChange:
					HandleConfirmStatChangeInput(context);
					break;
				case CharacterSelectSubState.AdjustStats:
					HandleAdjustStatsInput(context);
					break;
			}
		}

		//Render substates
		private void RenderCharacterSelection()
		{
			Console.WriteLine("Choose character type:");
			for (int i = 0; i < characterTypes.Length; i++)
				Console.WriteLine($"{i + 1}) {characterTypes[i]}");

			RenderMessage();
		}

		private void RenderConfirmStatChange()
		{
			Console.WriteLine("Would you like to buff up your stats before starting?");
			Console.WriteLine("y - yes");
			Console.WriteLine("n - no");
		}

		private void RenderAdjustStats()
		{
			Console.WriteLine("Up/Down Arrow to change stat");
			Console.WriteLine("Left/Right Arrow to adjust stat");
			Console.WriteLine("-------------------------");
			Console.WriteLine($"Remaining Points: {remainingPoints}");

			string[] statNames = { "Strength", "Agility", "Intelligence" };

			for (int i = 0; i < statPoints.Length; i++)
			{
				string arrow = (i == selectedStatIndex) ? "> " : "  ";
				Console.WriteLine($"{arrow}{statNames[i]}: {statPoints[i]}");
			}

			RenderMessage();
		}

		//Handle Input for substates
		private void HandleCharacterSelectionInput(GameContext context)
		{
			string input = Console.ReadLine();
			bool inputIsNumber = int.TryParse(input, out int index);
			if (inputIsNumber && index > 0 && index <= characterTypes.Length)
			{
				selectedCharacterIndex = index - 1;
				subState = CharacterSelectSubState.ConfirmStatChange;
				statMessage = "";
			}
			else
			{
				statMessage = "Invalid character!";
			}
		}

		private void HandleConfirmStatChangeInput(GameContext context)
		{
			ConsoleKey key = Console.ReadKey(true).Key;
			if (key == ConsoleKey.Y)
			{
				subState = CharacterSelectSubState.AdjustStats;
			}
			else if (key == ConsoleKey.N)
			{
				CreateCharacter();
				context.SetState(new InGameState(selectedCharacter));
			}
			else
			{
				HandleConfirmStatChangeInput(context);
			}
		}

		private void HandleAdjustStatsInput(GameContext context)
		{
			var key = Console.ReadKey(true).Key;

			switch (key)
			{
				case ConsoleKey.Enter:
					CreateCharacter();
					context.SetState( new InGameState(selectedCharacter));
					break;
				case ConsoleKey.UpArrow:
					selectedStatIndex = (selectedStatIndex - 1 + 3) % 3;
					break;
				case ConsoleKey.DownArrow:
					selectedStatIndex = (selectedStatIndex + 1) % 3;
					break;
				case ConsoleKey.RightArrow:
					if (remainingPoints > 0)
					{
						statPoints[selectedStatIndex]++;
						remainingPoints--;
					}
					else statMessage = "You don't have enough points!";
					break;
				case ConsoleKey.LeftArrow:
					if (statPoints[selectedStatIndex] > 0)
					{
						statPoints[selectedStatIndex]--;
						remainingPoints++;
					}
					else statMessage = "Stat can't go below zero!";
					break;
				default:
					HandleAdjustStatsInput(context);
					break;
			}
		}

		//Helper functions
		private GameEntity CreateCharacter()
		{
			CharacterType selectedType = (CharacterType)Enum.Parse(typeof(CharacterType), characterTypes[selectedCharacterIndex]);
			switch (selectedType)
			{
				case CharacterType.Warrior:
					selectedCharacter = new Warrior();
					break;
				case CharacterType.Mage:
					selectedCharacter = new Mage();
					break;
				case CharacterType.Archer:
					selectedCharacter = new Archer();
					break;
			}

			selectedCharacter.AddStatPoints(statPoints[0], statPoints[1], statPoints[2]);

			_characterService.CreateCharacter(selectedCharacter);

			return selectedCharacter;

		}

		private void RenderMessage()
		{
			if (!string.IsNullOrWhiteSpace(statMessage))
			{
				Console.WriteLine("-------------------------");
				Console.WriteLine(statMessage);
				Console.WriteLine("-------------------------");
				statMessage = "";
			}
		}
	}
}
