namespace Game.Core.Models
{
	public class Mage : GameEntity
	{
		public Mage()
			: base(
				  gameEntityType: "Character",
				  gameEntityTypeName: "Mage",
				  visualSymbol: '*',
				  range: 3,
				  strenght: 2,
				  intelligence: 3,
				  agility: 1
				  )
		{
		}
	}
}
