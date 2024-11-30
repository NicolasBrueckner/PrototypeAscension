using System;
using UnityEngine;
using UnityEngine.UIElements;

public class MenuScreenFactory : MonoBehaviour
{
	public static MenuScreen CreateScreen( VisualTreeAsset asset, MenuScreenType type,
		MenuScreenController controller )
	{
		return type switch
		{
			MenuScreenType.Main     => new MainMenuScreen( asset, type, controller ),
			MenuScreenType.HUD      => new HUDMenuScreen( asset, type, controller ),
			MenuScreenType.Credits  => new CreditsMenuScreen( asset, type, controller ),
			MenuScreenType.Tutorial => new TutorialMenuScreen( asset, type, controller ),
			MenuScreenType.Win      => new WinMenuScreen( asset, type, controller ),
			_                       => throw new ArgumentException( "not a screen type" ),
		};
	}
}