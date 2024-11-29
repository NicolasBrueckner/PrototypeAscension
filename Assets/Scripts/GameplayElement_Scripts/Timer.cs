using UnityEngine;
using static Utility;

public class Timer : MonoBehaviour
{
	private float _startTime;
	private bool _timerActive;

	public static Timer Instance{ get; private set; }
	public int Minutes{ get; private set; }
	public int Seconds{ get; private set; }
	public int Milliseconds{ get; private set; }
	public string TimerText{ get; private set; }

	private static RuntimeEventManager RuntimeEventManager => RuntimeEventManager.Instance;

	private void Awake()
	{
		Instance = CreateSingleton( Instance, gameObject );

		RuntimeEventManager.GameStarted += OnGameStarted;
		RuntimeEventManager.GameEnded += OnGameEnded;
	}

	private void Update()
	{
		if( _timerActive )
			UpdateTimer();
	}

	private void OnGameStarted()
	{
		_startTime = Time.time;
		_timerActive = true;

		UpdateTimer();
	}

	private void OnGameEnded()
	{
		_timerActive = false;
	}

	private void UpdateTimer()
	{
		float elapsedTime = Time.time - _startTime;

		Minutes = ( int )elapsedTime / 60;
		Seconds = ( int )elapsedTime % 60;
		Milliseconds = ( int )( ( elapsedTime - ( int )elapsedTime ) * 1000 );

		TimerText = $"{Minutes:00}:{Seconds:00}:{Milliseconds:000}";
		RuntimeEventManager.OnTimerUpdate( TimerText );
	}
}