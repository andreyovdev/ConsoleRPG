using System.Text;

using Game.Core.Models;

namespace Game.Application.Services
{
	public class GameService
	{
		private const int PlayerStartX = 1;
		private const int PlayerStartY = 1;
		private const int MatrixSize = 10;
		private const char MatrixVisualSymbol = '▒';

		private static Random randomPositionGenerator = new Random();
		private static StringBuilder sb = new StringBuilder();

		private GameEntity character;
		private List<GameEntity> enemies;
		private int[] playerPosition;
		private List<int[]> enemyPositions;
		private int enemiesKilled;

		public GameService(GameEntity character)
		{
			this.character = character;
			this.enemies = new List<GameEntity>();
			this.playerPosition = new int[2];
			this.enemyPositions = new List<int[]>();
			this.enemiesKilled = 0;

			this.playerPosition = new int[2] { PlayerStartX, PlayerStartY };
			SpawnEnemy();
		}

		public int GetEnemiesCount() => enemyPositions.Count();
		public int GetCharacterHealth() => character.Health;
		public int GetCharacterMana() => character.Mana;
		public int GetEnemiesKilled() => enemiesKilled;

		public int[] GetEnemiesInRangeHealth()
		{
			var targetEnemies = GetEnemiesInCharacterRange();
			int[] enemiesInRangeHealth = new int[targetEnemies.Count()];
			for (int i = 0; i < targetEnemies.Count(); i++)
			{
				enemiesInRangeHealth[i] = targetEnemies[i].Health;
			}
			return enemiesInRangeHealth;
		}

		public bool CharacterStepsOutOfMatrix(int stepX, int stepY)
		{
			int newX = playerPosition[0] + stepX;
			int newY = playerPosition[1] + stepY;

			if (newX < 0 || newX >= MatrixSize || newY < 0 || newY >= MatrixSize)
				return true;

			return false;

		}

		public bool CharacterStepsOnEnemy(int stepX, int stepY)
		{
			int newX = playerPosition[0] + stepX;
			int newY = playerPosition[1] + stepY;

			if (enemyPositions.Any(p => p[0] == newX && p[1] == newY))
				return true;

			return false;

		}

		public string DisplayMatrix()
		{
			sb = new StringBuilder();

			int enemyIndex = 0;

			for (int x = 0; x < MatrixSize; x++)
			{
				for (int y = 0; y < MatrixSize; y++)
				{
					bool isCharacter = x == this.playerPosition[0] && y == playerPosition[1];
					bool isEnemy = enemyPositions.SingleOrDefault(p => p[0] == x && p[1] == y) != null;

					if (isCharacter)
					{
						sb.Append(character.VisualSymbol.ToString());
					}
					else if (isEnemy)
					{
						sb.Append(enemies[enemyIndex].VisualSymbol);
						enemyIndex++;
					}
					else
					{
						sb.Append(MatrixVisualSymbol);
					}
				}

				if (x != MatrixSize - 1)
					sb.AppendLine();
			}

			return sb.ToString();
		}

		public void CharacterMove(int stepX, int stepY)
		{
			playerPosition[0] += stepX;
			playerPosition[1] += stepY;

			MoveEnemies();
			SpawnEnemy();
		}

		public void CharacterAttack(int enemyIndex)
		{
			var enemiesInRange = GetEnemiesInCharacterRange();

			GameEntity targetEnemy = enemiesInRange[enemyIndex];
			targetEnemy.TakeDamage(character.Damage);

			if (targetEnemy.Health <= 0)
			{
				int indexInFullList = enemies.IndexOf(targetEnemy);
				enemies.RemoveAt(indexInFullList);
				enemyPositions.RemoveAt(indexInFullList);
				enemiesKilled++;
			}

			MoveEnemies();
			SpawnEnemy();
		}

		private List<GameEntity> GetEnemiesInCharacterRange()
		{
			var enemiesInRange = new List<GameEntity>();

			for (int i = 0; i < enemyPositions.Count; i++)
			{
				int[] enemyPos = enemyPositions[i];

				int dx = playerPosition[0] - enemyPos[0];
				int dy = playerPosition[1] - enemyPos[1];

				int distance = Math.Max(Math.Abs(dx), Math.Abs(dy));

				if (distance <= character.Range)
				{
					enemiesInRange.Add(enemies[i]);
				}
			}

			return enemiesInRange;
		}

		private void MoveEnemies()
		{
			for (int i = 0; i < enemies.Count; i++)
			{
				var enemy = enemies[i];
				var pos = enemyPositions[i];

				int dx = playerPosition[0] - pos[0];
				int dy = playerPosition[1] - pos[1];
				double distance = Math.Sqrt(dx * dx + dy * dy);

				if (distance <= enemy.Range)
				{
					character.TakeDamage(enemy.Damage);
				}
				else
				{
					int stepX = dx == 0 ? 0 : dx / Math.Abs(dx);
					int stepY = dy == 0 ? 0 : dy / Math.Abs(dy);

					int newX = pos[0] + stepX;
					int newY = pos[1] + stepY;

					bool overlapWithPlayer = newX == playerPosition[0] && newY == playerPosition[1];
					bool overlapWithOtherEnemy = enemyPositions.Where((_, idx) => idx != i).Any(p => p[0] == newX && p[1] == newY);
					bool insideBounds = newX >= 0 && newX < MatrixSize && newY >= 0 && newY < MatrixSize;

					if (!overlapWithPlayer && !overlapWithOtherEnemy && insideBounds)
					{
						enemyPositions[i][0] = newX;
						enemyPositions[i][1] = newY;
					}
				}
			}
		}

		private void SpawnEnemy()
		{
			var enemy = new Monster();
			this.enemies.Add(enemy);
			this.enemyPositions.Add(RandomSpawnPosition());
		}

		private int[] RandomSpawnPosition()
		{
			int x = 0, y = 0;

			do
			{
				x = randomPositionGenerator.Next(0, MatrixSize);
				y = randomPositionGenerator.Next(0, MatrixSize);
			}
			while (playerPosition[0] == x && playerPosition[1] == y || enemyPositions.Any(p => p[0] == x && p[1] == y));

			return new int[2] { x, y };
		}
	}
}
