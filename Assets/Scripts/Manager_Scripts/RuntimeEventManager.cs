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
	public event Action<Vector2> JumpStarted;
	public event Action JumpStartedEmpty;
	public event Action JumpInvalid;
	public event Action PlayerDeathInitiated;
	public event Action<GameObject> PlayerDeathCompleted;
	public event Action PlayerDeathCompletedEmpty;
	public event Action<string> TimerUpdate;

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

	public void OnJumpStarted( Vector2 jumpDirection )
	{
		JumpStarted?.Invoke( jumpDirection );
		JumpStartedEmpty?.Invoke();
	}

	public void OnJumpInvalid()
	{
		JumpInvalid?.Invoke();
	}

	public void OnPlayerDeathInitiated()
	{
		PlayerDeathInitiated?.Invoke();
	}

	public void OnPlayerDeathCompleted( GameObject player )
	{
		PlayerDeathCompleted?.Invoke( player );
		PlayerDeathCompletedEmpty?.Invoke();
	}

	public void OnTimerUpdate( string value )
	{
		TimerUpdate?.Invoke( value );
	}
}