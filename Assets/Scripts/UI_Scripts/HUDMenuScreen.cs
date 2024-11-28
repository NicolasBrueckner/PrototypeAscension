using Manager_Scripts;
using UnityEngine.UIElements;

namespace UI_Scripts
{
	public class HUDMenuScreen : MenuScreen
	{
		private Label _timerLabel;

		public HUDMenuScreen( VisualTreeAsset asset, MenuScreenType type, MenuScreenController controller ) : base(
			asset, type, controller )
		{
		}

		private static string TimerString => Timer.TimerText;

		private static Timer Timer => Timer.Instance;


		protected override void GetElements()
		{
			base.GetElements();

			_timerLabel = new Label( "TimerLabel" );
		}

		protected override void BindElements()
		{
			base.BindElements();

			_timerLabel.text = TimerString;
		}
	}
}