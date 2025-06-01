namespace Game.Core.Models
{
	public class Archer : GameEntity
	{
		public Archer()
			: base(
				  gameEntityType: "Character",
				  gameEntityTypeName: "Archer",
				  visualSymbol: '#',
				  range: 2,
				  strenght: 2,
				  intelligence: 0,
				  agility: 4
				  )
		{
		}
	}
}
