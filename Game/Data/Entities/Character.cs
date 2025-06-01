using System.ComponentModel.DataAnnotations;

namespace Game.Data.Entities
{
	public class Character
	{
		[Key]
		public int Id { get; set; }
		[Required]
		public string Class { get; set; }
		public int Range { get; set; }
		public int Strenght { get; set; }
		public int Intelligence { get; set; }
		public int Agility { get; set; }

		public int Health { get; set; }
		public int Mana { get; set; }
		public int Damage { get; set; }

		public DateTime DateCreated { get; set; }
	}
}
