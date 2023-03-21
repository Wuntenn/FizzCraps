using System;
namespace fizzBuzz
{
	public static class Craps
	{
		// Run in debug mode to output the individual craps games
		private static bool debugMode = false;
		public static bool crapsSolution(string unusedString)
		{
			playAllCraps();
			return true;
		}

		private static void playAllCraps()
		{
			// Set up vars/flags for input
			bool inputIsValid = false;
			string input = String.Empty;
			int numberOfCraps = 0;

            // Ask how many games the player wants to play
            while ((!inputIsValid) || (numberOfCraps <= 0))
            {
                Console.WriteLine("Please enter the number of games you want to simulate: ");
                input = Console.ReadLine() ?? "not set";
                inputIsValid = int.TryParse(input, out numberOfCraps);
            }


			// Create game
			var game = new CrapsGame();

			// Create game stats
			var gameLogs = new List<CrapsGameLog>();

			// set up current game log
			var currentGameLog = new CrapsGameLog();

            // Simulate specified number of games
            for (int playGames = 0; playGames < numberOfCraps; playGames++)
			{
				// play current game and append it's logs to the gameLogs 
				currentGameLog = game.playGame();
				gameLogs.Add(currentGameLog);
			}

			// Calculate the stats from dictionary based on all games
			printCrapsStatistics(gameLogs);
		}

		private static void printCrapsStatistics(List<CrapsGameLog> logs)
		{
			// Get the stats
			double avgRollsPerGame = getAvgRollsPerGame(logs);
			int highestNumberOfRolls = getHighestNumberOfRolls(logs);
			int lowestNumberOfRolls = getLowestNumberOfRolls(logs);
			string mostCommonRoll = getMostCommonRoll(logs);
			decimal winningPercentage = getAvgWinningPercentage(logs);
			int numberOfWins = getNumberOfWins(logs);
			int numberOfLosses = getNumberOfLosses(logs);

			if (!debugMode) Console.Clear();
			Console.WriteLine($"---- Games Stats: ");
			Console.WriteLine($"- Average rolls per game: {avgRollsPerGame}");
			Console.WriteLine($"- Highest numbers of rolls: {highestNumberOfRolls}");
			Console.WriteLine($"- Lowest number of rolls: {lowestNumberOfRolls}");
			Console.WriteLine($"- Most frequently rolled: {mostCommonRoll}");
			Console.WriteLine($"- Winning percentage: {winningPercentage:0.00}");
			Console.WriteLine($"- Number of games won: {numberOfWins}");
			Console.WriteLine($"- Number of games lost: {numberOfLosses}");
        }

		private static double getAvgRollsPerGame(List<CrapsGameLog> logs)
		{
			double avgCountsPerGame = logs.Where(x => x.rounds.Count > 0).Select(x => x.rounds.Count).Average();
			return avgCountsPerGame; 
		}

		private static int getHighestNumberOfRolls(List<CrapsGameLog> logs)
		{
            int highest = logs.Where(x => x.rounds.Count > 0).Select(x => x.rounds.Count).Max();
            return highest;
        }

		private static int getLowestNumberOfRolls(List<CrapsGameLog> logs)
		{
            int lowest = logs.Where(x => x.rounds.Count > 0).Select(x => x.rounds.Count).Min();
            return lowest;
		}

		private static string getMostCommonRoll(List<CrapsGameLog> logs)
		{
			var query = logs
				.SelectMany(log => log.rounds, (cLogs, cRounds) => new { log = cLogs, rounds = cRounds })
				.GroupBy(x => x.rounds.total)
				.Select(x => new { cRolled = x.Key, cCount = x.Count() })
				.OrderByDescending(x => x.cCount);

			var results = new Dictionary<int, List<int>>();

			foreach (var obj in query)
			{
				if (debugMode) Console.WriteLine($"Object is: {obj}");
				int crapsKey = obj.cCount;
				int crapsRolled = obj.cRolled;

				if (!results.ContainsKey(crapsKey))
				{
					// create a list and add rolled value
					var listValue = new List<int>();
					listValue.Add(crapsRolled);

					// add this to the result
					results.Add(crapsKey, listValue);
				}
				else
				{
					// add the value to the list
					results[crapsKey].Add(crapsRolled);
				}
			}

			
			var commonKey = results.Keys.Max();
			if (debugMode) Console.WriteLine($"The results are: {commonKey}");

			string isAre = (results[commonKey].Count > 1) ? "are" : "is";
			string resultString = $"{isAre} {string.Join(",", results[commonKey])} with {commonKey} rolls";
			if (debugMode) Console.WriteLine(resultString);

			return resultString;
		}

		private static decimal getAvgWinningPercentage(List<CrapsGameLog> logs)
		{
			int winningGameCount = logs.Where(x => x.shooterWon == true).Select(x => x.shooterWon).Count();
			decimal totalGames = new decimal(logs.Count());
			decimal wins = winningGameCount/totalGames;
			decimal winPercentage = wins * 100;

			return winPercentage;
		}

		private static int getNumberOfWins(List<CrapsGameLog> logs)
		{
			int numberOfWins = logs.Where(x => x.shooterWon == true).Select(x => x.shooterWon).Count();
			return numberOfWins; 
		}

		private static int getNumberOfLosses(List<CrapsGameLog> logs)
		{
			int numberOfLosses = logs.Where(x => x.shooterWon == false).Select(x => x.shooterWon).Count();
			return numberOfLosses; 
		}
	}

	// used to keep the round values
	public struct CrapsRound
	{
		public int die1Value;
		public int die2Value;
		public int total;
    }


	public struct CrapsGameLog
	{
		public bool shooterWon;
		public List<CrapsRound> rounds;

		public CrapsGameLog(List<CrapsRound> rounds)
		{
			this.shooterWon = false;
			this.rounds = rounds;
		}
    } 


	public class CrapsGame
	{
		// two dice are used
		private Die die1 = new Die();
		private Die die2 = new Die();

		// Create a new game
		public CrapsGameLog playGame()
		{ 
			// Create round
			CrapsRound currentRound;

			Console.WriteLine("---new game--");

			// Roll the dice initially 
			currentRound = rollDice();

			// Set up the stats
			var crapsRoundsList = new List<CrapsRound>();
			crapsRoundsList.Add(currentRound);
			var gameLog = new CrapsGameLog(crapsRoundsList);

			printCurrent(currentRound);
			
			// Check if lost
			if (isFirstShotLoser(currentRound))
			{
				gameLog.shooterWon = false;
			}

			else if (isFirstShotWinner(currentRound))
			{
				gameLog.shooterWon = true;
			}

			// We must've rolled 4,5,6,8,9, or 10
			else
			{
				// set the points
				int points = currentRound.total;

				// flag to control longer game
				bool longGameComplete = false;

				while (!longGameComplete)
				{
					// roll the dice and store the stats
					currentRound = rollDice();
					gameLog.rounds.Add(currentRound);

					printCurrent(currentRound);

					// check if won or lost
					if (isInGameShotWinner(points, currentRound))
					{ 
						gameLog.shooterWon = true;
						longGameComplete = true;
					}

					else if (isInGameShotLoser(points, currentRound))
					{
						gameLog.shooterWon = false;
						longGameComplete = true;
					}
				}
			}
			

			return gameLog;

		}

		// If the dice add up to 2, 3, or 12 on the first round they lose
		private bool isFirstShotLoser(CrapsRound round)
		{
			if ((round.total == 2) || (round.total == 3) || (round.total == 12)) return true;
			return false;
		}

		// If the dice add up to 7 or 11 on the first round they win
		private bool isFirstShotWinner(CrapsRound round)
		{
			if ((round.total == 7) || (round.total == 11)) return true;
			return false;
		}

		// If the dice add to the game points they win
		private bool isInGameShotWinner(int points, CrapsRound round)
		{
			if (round.total == points) return true;
			return false;
		}

		// If the dice add to the game points they win
		private bool isInGameShotLoser(int points, CrapsRound round)
		{
			if (round.total == 7) return true;
			return false;
		}

		public void printCurrent(CrapsRound round)
		{
			Console.WriteLine($"Craps: Dice1: {round.die1Value} and Dice2: {round.die2Value} - total: {round.total}");
		}

		public CrapsRound rollDice()
		{
			CrapsRound currentRound;
			currentRound.die1Value = this.die1.roll();
			currentRound.die2Value = this.die2.roll();
			currentRound.total = die1.value + die2.value;

			return currentRound;
		}
    }

	// model the Die
	public class Die
	{
		const int DICE_MIN_FACE = 1;
		const int DICE_MAX_FACE = 6;

		private Random random =  new Random();

		public Die()
		{ 
			value = random.Next(DICE_MIN_FACE, DICE_MAX_FACE + 1);
		}

		public int value { get; private set; }
		public int roll()
		{
			value = random.Next(DICE_MIN_FACE, DICE_MAX_FACE + 1);
			return value;
		}
    }
}

