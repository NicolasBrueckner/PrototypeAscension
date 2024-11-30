using UnityEngine.UIElements;

public class WinMenuScreen : MenuScreen
{
	private Label _timerLabel;

	public WinMenuScreen( VisualTreeAsset asset, MenuScreenType type, MenuScreenController controller ) : base( asset,
		type, controller )
	{
	}

	private static RuntimeEventManager RuntimeEventManager => RuntimeEventManager.Instance;

	protected override void GetElements()
	{
		base.GetElements();

		_timerLabel = Root.Q<Label>( "TimerLabel" );
	}

	protected override void BindEvents()
	{
		base.BindEvents();

		RuntimeEventManager.TimerUpdate += OnTimerUpdate;
		RuntimeEventManager.GameEnded += OnGameEnded;
	}

	private void OnTimerUpdate( string value )
	{
		_timerLabel.text = value;
	}

	private void OnGameEnded()
	{
		MenuScreenController.ToggleScreen( Type );
	}
}