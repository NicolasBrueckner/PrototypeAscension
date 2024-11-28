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
	private Vector2 _mousePosition;
	private Rigidbody2D _rb2D;

	private int _remainingJumps;

	private static InputEventManager InputEventManager => InputEventManager.Instance;
	private static RuntimeEventManager RuntimeEventManager => RuntimeEventManager.Instance;

	#region Unity Runtime Methods

	private void Awake()
	{
		_remainingJumps = jumpNumber;
		_rb2D = GetComponent<Rigidbody2D>();

		InputEventManager.InputsBound += BindInputEvents;
		RuntimeEventManager.StateChanged += OnPlayerStateChanged;
		RuntimeEventManager.BoostActivated += OnBoostActivated;
		RuntimeEventManager.PlayerResetEmpty += OnStopJump;
	}

	#endregion

	private void BindInputEvents()
	{
		InputEventManager.JumpPerformed += OnJumpPerformed;
		InputEventManager.JumpCanceled += OnJumpCanceled;
		InputEventManager.StopJump += OnStopJump;
		InputEventManager.AimPerformed += UpdateMousePosition;
	}

	private void OnPlayerStateChanged( PlayerState state )
	{
		_currentState = state;

		switch( _currentState )
		{
			case PlayerState.OnGround:
				_remainingJumps = jumpNumber;
				if( _isCharging )
					_chargeJumpCoroutine ??= StartCoroutine( ChargeJumpCoroutine() );
				break;
			case PlayerState.OnWall:
				if( _isCharging )
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
		RuntimeEventManager.OnChargeChanged( 0.0f );

		if( _isJumpStopped || _remainingJumps <= 0 )
			return;

		SetJumpForce();
		RuntimeEventManager.OnJumpStarted();
		_remainingJumps--;
	}

	private void UpdateMousePosition( Vector2 worldPosition )
	{
		_mousePosition = worldPosition;
	}

	private void SetJumpForce()
	{
		Vector2 direction = SetJumpDirection();
		Vector2 velocity = direction * _currentJumpStrength;

		if( _currentState == PlayerState.InAir )
			_rb2D.velocity = velocity;
		else
			_rb2D.velocity += velocity;
	}

	private Vector2 SetJumpDirection()
	{
		Vector2 direction = _mousePosition - ( Vector2 )gameObject.transform.position;
		direction.Normalize();

		return direction;
	}

	private IEnumerator ChargeJumpCoroutine()
	{
		_currentJumpStrength = 0;
		float timer = 0;

		while( _isCharging && !_isJumpStopped && ( timer += Time.fixedUnscaledDeltaTime ) < 2f )
		{
			_currentJumpStrength = Mathf.Lerp( _currentJumpStrength, maxJumpStrength, timer );
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
}