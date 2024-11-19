using System;
using UnityEngine;
using static Utility;

namespace Manager_Scripts
{
	public class GameplayEventManager : MonoBehaviour
	{
		public static GameplayEventManager Instance{ get; private set; }

		private void Awake()
		{
			Instance = CreateSingleton( Instance, gameObject );
		}

		public event Action<PlayerState> StateChanged;
		public event Action BoostActivated;
		public event Action<float> ChargeChanged;
		public event Action JumpStarted;

		public void OnStateChanged( PlayerState state ) => StateChanged?.Invoke( state );
		public void OnBoostActivated()                  => BoostActivated?.Invoke();
		public void OnChargeChanged( float value )      => ChargeChanged?.Invoke( value );
		public void OnJumpStarted()                     => JumpStarted?.Invoke();
	}
}