using System;
using UnityEngine;
using static Utility;

public class RuntimeEventManager : MonoBehaviour
{
	public static RuntimeEventManager Instance{ get; private set; }

	private void Awake()
	{
		Instance = CreateSingleton( Instance, gameObject );
	}

	public event Action GameStarted;
	public event Action GameEnded;
	public event Action<PlayerState> StateChanged;
	public event Action BoostActivated;
	public event Action<float> ChargeChanged;
	public event Action JumpStarted;
	public event Action<GameObject> PlayerReset;
	public event Action PlayerResetEmpty;

	public void OnGameStarted()
	{
		GameStarted?.Invoke();
	}

	public void OnGameEnded()
	{
		GameEnded?.Invoke();
	}

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

	public void OnJumpStarted()
	{
		JumpStarted?.Invoke();
	}

	public void OnPlayerReset( GameObject player )
	{
		PlayerReset?.Invoke( player );
		PlayerResetEmpty?.Invoke();
	}
}