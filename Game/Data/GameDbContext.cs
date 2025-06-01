using Microsoft.EntityFrameworkCore;

using Game.Data.Entities;

public class GameDbContext : DbContext
{
	public GameDbContext()
	{
		Database.EnsureCreated();
	}
	public DbSet<Character> Characters { get; set; }

	protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
	{
		optionsBuilder.UseSqlServer("Server=localhost;Database=ConsoleRPG;Trusted_Connection=True;");
	}
}