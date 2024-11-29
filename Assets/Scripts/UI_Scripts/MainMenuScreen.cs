using UnityEngine;
using UnityEngine.UIElements;

public class MainMenuScreen : MenuScreen
{
	private Button _creditsButton;
	private Button _quitButton;
	private Button _startButton;
	private Button _tutorialButton;

	public MainMenuScreen( VisualTreeAsset asset, MenuScreenType type, MenuScreenController controller ) : base(
		asset, type, controller )
	{
	}

	protected override void GetElements()
	{
		base.GetElements();

		_startButton = Root.Q<Button>( "StartButton" );
		_creditsButton = Root.Q<Button>( "CreditsButton" );
		_tutorialButton = Root.Q<Button>( "TutorialButton" );
		_quitButton = Root.Q<Button>( "QuitButton" );
	}

	protected override void BindElements()
	{
		base.BindElements();

		_startButton.clicked += OnStartButtonClicked;
		_creditsButton.clicked += OnCreditsButtonClicked;
		_tutorialButton.clicked += OnTutorialButtonClicked;
		_quitButton.clicked += OnQuitButtonClicked;
	}

	private void OnStartButtonClicked()
	{
		RuntimeEventManager.Instance.OnGameStarted();
		MenuScreenController.ToggleScreen( MenuScreenType.HUD );
	}

	private void OnCreditsButtonClicked()
	{
		MenuScreenController.ToggleScreen( MenuScreenType.Credits );
	}

	private void OnTutorialButtonClicked()
	{
		MenuScreenController.ToggleScreen( MenuScreenType.Tutorial );
	}

	private void OnQuitButtonClicked()
	{
		Application.Quit();
	}
}