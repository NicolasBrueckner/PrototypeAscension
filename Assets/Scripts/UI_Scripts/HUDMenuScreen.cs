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

		protected override void GetElements()
		{
			base.GetElements();

			_timerLabel = new Label( "TimerLabel" );
		}

		protected override void BindElements()
		{
			base.BindElements();
		}
	}
}