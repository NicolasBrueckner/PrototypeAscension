using UnityEngine.UIElements;

public class HUDMenuScreen : MenuScreen
{
	private Label _timerLabel;

	public HUDMenuScreen( VisualTreeAsset asset, MenuScreenType type, MenuScreenController controller ) : base(
		asset, type, controller )
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
	}

	private void OnTimerUpdate( string value )
	{
		_timerLabel.text = value;
	}
}