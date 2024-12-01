using System;
using System.Collections;
using UnityEngine;
using static Utility;

[ RequireComponent( typeof( Rigidbody2D ) ) ]
[ RequireComponent( typeof( PlayerCollisionController ) ) ]
public class PlayerJumpController : MonoBehaviour
{
	public int jumpNumber;
	public float maxJumpStrength;
	private Coroutine _chargeJumpCoroutine;
	private float _currentJumpStrength;
	private PlayerState _currentState;
	private bool _isCharging;
	private bool _isJumpStopped;
	private Vector2 _jumpDirection;
	private Vector2 _mousePosition;
	private Rigidbody2D _rb2D;

	private int _remainingJumps;

	private static InputEventManager InputEventManager => InputEventManager.Instance;
	private static RuntimeEventManager RuntimeEventManager => RuntimeEventManager.Instance;

	private void Awake()
	{
		_remainingJumps = jumpNumber;
		_rb2D = GetComponent<Rigidbody2D>();

		InputEventManager.InputsBound += BindInputEvents;
		RuntimeEventManager.StateChanged += OnPlayerStateChanged;
		RuntimeEventManager.BoostActivated += OnBoostActivated;
		RuntimeEventManager.PlayerResetEmpty += OnStopJump;
		RuntimeEventManager.JumpInvalid += OnJumpInvalid;
	}

	private void BindInputEvents()
	{
		InputEventManager.JumpPerformed += OnJumpPerformed;
		InputEventManager.JumpCanceled += OnJumpCanceled;
		InputEventManager.StopJump += OnStopJump;
		InputEventManager.AimPerformed += UpdateMousePosition;
	}

	// coroutine is started so charge can start immediately after touching surface while already holding charge button
	private void OnPlayerStateChanged( PlayerState state )
	{
		_currentState = state;

		switch( _currentState )
		{
			case PlayerState.OnGround:
				_remainingJumps = jumpNumber;
				_chargeJumpCoroutine ??= StartCoroutine( ChargeJumpCoroutine() );
				break;
			case PlayerState.OnWall:
				_chargeJumpCoroutine ??= StartCoroutine( ChargeJumpCoroutine() );
				break;
			case PlayerState.InAir:
				if( _remainingJumps == jumpNumber )
					_remainingJumps = jumpNumber - 1;
				break;
			default:
				throw new ArgumentOutOfRangeException();
		}
	}

	private void OnBoostActivated()
	{
		if( _remainingJumps == 0 )
			_remainingJumps = 1;
	}

	private void OnJumpInvalid()
	{
		_remainingJumps++;
	}

	private void OnJumpPerformed()
	{
		_isCharging = true;
		_isJumpStopped = false;

		SetRuntimeSpeed( 0.3f );

		if( _currentState == PlayerState.InAir )
		{
			_currentJumpStrength = maxJumpStrength * 0.7f;
			return;
		}

		_chargeJumpCoroutine ??= StartCoroutine( ChargeJumpCoroutine() );
	}

	private void OnJumpCanceled()
	{
		SetRuntimeSpeed( 1.0f );

		_isCharging = false;
		_jumpDirection = SetJumpDirection();

		RuntimeEventManager.OnChargeChanged( 0.0f );

		if( _isJumpStopped || _remainingJumps <= 0 )
		{
			_currentJumpStrength = 0.0f;
			return;
		}

		RuntimeEventManager.OnJumpStarted( _jumpDirection );
		ExecuteJump();
	}

	private Vector2 SetJumpDirection()
	{
		Vector2 direction = _mousePosition - ( Vector2 )gameObject.transform.position;
		direction.Normalize();

		return direction;
	}

	private void ExecuteJump()
	{
		SetJumpForce();
		_remainingJumps--;
		_currentJumpStrength = 0.0f;
	}

	private void SetJumpForce()
	{
		Vector2 velocity = _jumpDirection * _currentJumpStrength;

		if( _currentState == PlayerState.InAir )
			_rb2D.velocity = velocity;
		else
			_rb2D.velocity += velocity;
	}

	private IEnumerator ChargeJumpCoroutine()
	{
		const float startingJumpStrength = 2.0f;
		const float timeToCharge = 0.5f;
		float timer = 0;

		while( _isCharging && !_isJumpStopped && _currentJumpStrength < maxJumpStrength )
		{
			timer += Time.fixedUnscaledDeltaTime;

			_currentJumpStrength = Mathf.Lerp( startingJumpStrength, maxJumpStrength, timer / timeToCharge );
			RuntimeEventManager.OnChargeChanged( _currentJumpStrength / maxJumpStrength );

			yield return new WaitForFixedUpdate();
		}

		_chargeJumpCoroutine = null;
	}

	public void OnStopJump()
	{
		_isJumpStopped = true;
		RuntimeEventManager.OnChargeChanged( 0.0f );
	}

	private void UpdateMousePosition( Vector2 worldPosition )
	{
		_mousePosition = worldPosition;
	}
}