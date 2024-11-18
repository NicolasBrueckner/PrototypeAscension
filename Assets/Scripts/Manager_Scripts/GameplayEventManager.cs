using System;
using UnityEngine;
using static Utility;

public class GameplayEventManager : MonoBehaviour
{
	public static GameplayEventManager Instance{ get; private set; }

	private void Awake()
	{
		Instance = CreateSingleton( Instance, gameObject );
	}

	public event Action<PlayerState> StateChanged;
	public event Action              BoostActivated;
	public event Action<float>       ChargeChanged;

	public void OnStateChanged( PlayerState state )
	{
		StateChanged?.Invoke( state );
	}

	public void OnBoostActivated()
	{
		BoostActivated?.Invoke();
	}

	public void OnChargeChanged( float value )
	{
		ChargeChanged?.Invoke( value );
	}
}