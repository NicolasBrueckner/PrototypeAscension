using System;
using System.Collections;
using UnityEngine;
using static Utility;

[RequireComponent( typeof( Rigidbody2D ) )]
[RequireComponent( typeof( PlayerCollisionController ) )]
public class PlayerJumpController : MonoBehaviour
{
	public int jumpNumber;
	public float maxJumpStrength;

	public event Action<float> ChargeChanged;

	private int _remainingJumps;
	private bool _isCharging;
	private bool _isJumpStopped;
	private float _currentJumpStrength;
	private Vector2 _mousePosition;
	private PlayerState _currentState;
	private Coroutine _chargeJumpCoroutine;
	private Rigidbody2D _rb2D;
	private PlayerCollisionController _playerCollisionController;

	private InputEventManager _InputEventManager => InputEventManager.Instance;

	#region Unity Runtime Methods
	private void Awake()
	{
		_remainingJumps = jumpNumber;
		_rb2D = GetComponent<Rigidbody2D>();
		_playerCollisionController = GetComponent<PlayerCollisionController>();

		_InputEventManager.InputsBound += BindInputEvents;
		_playerCollisionController.PositionStateChanged += OnPlayerStateChanged;
	}
	#endregion

	private void BindInputEvents()
	{
		_InputEventManager.JumpPerformed += OnJumpPerformed;
		_InputEventManager.JumpCanceled += OnJumpCanceled;
		_InputEventManager.AimPerformed += UpdateMousePosition;
	}

	private void OnPlayerStateChanged( PlayerState state )
	{
		_currentState = state;

		switch ( _currentState )
		{
			case PlayerState.OnGround:
				_remainingJumps = jumpNumber;
				break;
			case PlayerState.OnWall:
				break;
			case PlayerState.InAir:
				if ( _remainingJumps == jumpNumber )
					_remainingJumps = jumpNumber - 1;
				break;
		}
	}

	private void OnJumpPerformed()
	{
		_isCharging = true;
		_isJumpStopped = false;

		SetRuntimeSpeed( 0.3f );

		if ( _currentState == PlayerState.InAir )
		{
			_isCharging = false;
			_currentJumpStrength = maxJumpStrength * 0.6f;
			return;
		}

		_chargeJumpCoroutine ??= StartCoroutine( ChargeJumpCoroutine() );
	}

	private void OnJumpCanceled()
	{
		SetRuntimeSpeed( 1.0f );
		_isCharging = false;
		OnChargeChanged( 0.0f );

		if ( _isJumpStopped )
			return;

		if ( _remainingJumps > 0 )
		{
			SetJumpForce();
			_remainingJumps--;
		}
	}

	private void UpdateMousePosition( Vector2 worldPosition )
	{
		_mousePosition = worldPosition;
	}

	private void SetJumpForce()
	{
		Vector2 direction = SetJumpDirection();
		Vector2 velocity = direction * _currentJumpStrength;

		_rb2D.velocity = velocity;
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

		while ( _isCharging )
		{
			_currentJumpStrength += 20f * Time.fixedDeltaTime;
			_currentJumpStrength = Mathf.Min( _currentJumpStrength, maxJumpStrength );

			OnChargeChanged( _currentJumpStrength / maxJumpStrength );

			yield return new WaitForFixedUpdate();
		}

		_chargeJumpCoroutine = null;
	}

	private void OnChargeChanged( float fraction )
	{
		ChargeChanged?.Invoke( fraction );
	}

	public void StopJump()
	{
		_isJumpStopped = true;
	}
}
