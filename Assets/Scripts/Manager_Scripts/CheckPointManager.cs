using UnityEngine;
using static Utility;

namespace Manager_Scripts
{
	public class CheckPointManager : MonoBehaviour
	{
		private CheckPoint _current;
		public static CheckPointManager Instance{ get; private set; }

		private void Awake()
		{
			Instance = CreateSingleton( Instance, gameObject );
		}

		public void SetCurrent( CheckPoint current )
		{
			_current = current;
		}

		public void Respawn( GameObject playerObject )
		{
			PlayerJumpController jumpController = playerObject.GetComponent<PlayerJumpController>();

			if( !_current )
				return;

			if( jumpController )
				jumpController.OnStopJump();

			TryStopMovement( playerObject );
			playerObject.transform.position = _current.transform.position;
		}
	}
}