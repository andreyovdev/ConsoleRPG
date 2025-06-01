namespace Game.Core.Models
{
	public class Warrior : GameEntity
	{
		public Warrior()
			: base(
				  gameEntityType: "Character",
				  gameEntityTypeName: "Warrior",
				  visualSymbol: '@',
				  range: 1,
				  strenght: 3,
				  intelligence: 0,
				  agility: 3
				  )
		{
		}

	}
}
