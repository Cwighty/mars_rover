using System.Diagnostics.CodeAnalysis;

namespace Mars.Web;

public class GameManager
{
	public GameManager(IEnumerable<MissionControl.Cell>[]? parsedMaps = null)
	{
		CreatedOn = DateTime.Now;
		GameStartOptions = new GameStartOptions
		{
			Height = 100,
			Width = 100,
			ParsedMaps = parsedMaps
		};
		StartNewGame(GameStartOptions);
	}

	/// <summary>
	/// If you were to restart this game instance, what options would you use?
	/// </summary>
	public GameStartOptions GameStartOptions { get; }

	/// <summary>
	/// When this instance of the Mars Rover game was instantiated
	/// </summary>
	public DateTime CreatedOn { get; }

	/// <summary>
	/// The game instance
	/// </summary>
	public Game Game { get; private set; }

	/// <summary>
	/// Did something important in the game change?
	/// </summary>
	public event EventHandler? GameStateChanged;

	/// <summary>
	/// Start a new game
	/// </summary>
	/// <param name="startOptions"></param>
	[MemberNotNull(nameof(Game))]
	public void StartNewGame(GameStartOptions startOptions)
	{
		//unsubscribe from old event
		if (Game != null)
		{
			Game.GameStateChanged -= Game_GameStateChanged;
			Game.Dispose();
		}

		Game = new Game(startOptions);
		GameStateChanged?.Invoke(this, EventArgs.Empty);

		//subscribe to new event
		Game.GameStateChanged += Game_GameStateChanged;
	}

	public void PlayGame(GamePlayOptions playOptions)
	{
		Game?.PlayGame(playOptions);
	}

	private void Game_GameStateChanged(object? sender, EventArgs e)
	{
		GameStateChanged?.Invoke(this, e);
	}
}
