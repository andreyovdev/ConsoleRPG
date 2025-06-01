namespace Game.Core.Models
{
	public class Monster : GameEntity
	{
		private static Random randomStatGenerator = new Random();

		public Monster()
			: base(
				  gameEntityType: "Enemy",
				  gameEntityTypeName: "Monster",
				  visualSymbol: '◙',
				  range: 1,
				  strenght: RandomStat(),
				  intelligence: RandomStat(),
				  agility: RandomStat()
				  )
		{
		}

		private static int RandomStat()
		{
			return randomStatGenerator.Next(1, 4);
		}
	}
}
