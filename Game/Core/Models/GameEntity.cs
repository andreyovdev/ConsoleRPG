namespace Game.Core.Models
{
	public abstract class GameEntity
	{
		protected GameEntity(
			string gameEntityType,
			string gameEntityTypeName,
			char visualSymbol,
			int range,
			int strenght,
			int agility,
			int intelligence
			)
		{
			this.GameEntityType = gameEntityType;
			this.GameEntityTypeName = gameEntityTypeName;
			this.VisualSymbol = visualSymbol;
			this.Range = range;

			this.Strenght = strenght;
			this.Agility = agility;
			this.Intelligence = intelligence;

			CalculateStats();
		}

		public string GameEntityType { get; private set; }
		public string GameEntityTypeName { get; private set; }
		public char VisualSymbol { get; private set; }
		public int Range { get; private set; }

		public int Strenght { get; private set; }
		public int Intelligence { get; private set; }
		public int Agility { get; private set; }

		public int Health { get; private set; }
		public int Mana { get; private set; }
		public int Damage { get; private set; }

		public virtual void AddStatPoints(int strengthPoints, int agilityPoints, int intelligencePoints)
		{
			this.Strenght += strengthPoints;
			this.Agility += agilityPoints;
			this.Intelligence += intelligencePoints;

			CalculateStats();
		}

		public void TakeDamage(int amount)
		{
			this.Health -= amount;
			if (this.Health < 0) this.Health = 0;
		}

		private void CalculateStats()
		{
			this.Health = this.Strenght * 5;
			this.Mana = this.Intelligence * 3;
			this.Damage = this.Agility * 2;
		}
	}
}
