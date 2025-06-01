using Game.Core.Models;
using Game.Data.Entities;

namespace Game.Application.Services
{
	public class CharacterService
	{
		private readonly GameDbContext _dbContext;

		public CharacterService(GameDbContext dbContext)
		{
			_dbContext = dbContext;
		}

		public void CreateCharacter(GameEntity character)
		{
			var c = new Character
			{
				Class = character.GetType().Name,
				Strenght = character.Strenght,
				Intelligence = character.Intelligence,
				Agility = character.Agility,
				Health = character.Health,
				Mana = character.Mana,
				Damage = character.Damage,
				DateCreated = DateTime.Now
			};

			_dbContext.Characters.Add(c);
			_dbContext.SaveChanges();
		}
	}
}
