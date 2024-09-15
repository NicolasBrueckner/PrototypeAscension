using System;
using UnityEngine;
using static Utility;

[RequireComponent( typeof( Rigidbody2D ) )]
[RequireComponent( typeof( PlayerCollisionController ) )]
public class PlayerJumpController : MonoBehaviour
{
	public int jumpNumber;
	public float jumpStrength;

	public event Action<int> JumpsChanged;

	private int _remainingJumps;
	private bool _isJumpStopped;
	private Vector2 _mousePosition;
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
		_playerCollisionController.PlayerStateChanged += OnPlayerStateChanged;
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
		switch ( state )
		{
			case PlayerState.OnGround:
				_remainingJumps = jumpNumber;
				OnJumpsChanged( _remainingJumps );
				break;
			case PlayerState.OnWall:
				break;
			case PlayerState.InAir:
				if ( _remainingJumps == jumpNumber )
				{
					_remainingJumps = jumpNumber - 1;
					OnJumpsChanged( _remainingJumps );
				}
				break;
		}
	}

	private void OnJumpPerformed()
	{
		SetSlowMotion( 0.3f );
		_isJumpStopped = false;
	}

	private void OnJumpCanceled()
	{
		SetSlowMotion( 1.0f );

		if ( _isJumpStopped )
			return;

		if ( _remainingJumps > 0 )
		{
			SetJumpForce();
			OnJumpsChanged( --_remainingJumps );
		}
	}

	private void UpdateMousePosition( Vector2 worldPosition )
	{
		_mousePosition = worldPosition;
	}

	private void SetJumpForce()
	{
		Vector2 direction = SetJumpDirection();
		Vector2 velocity = direction * jumpStrength;

		_rb2D.velocity = velocity;
	}

	private Vector2 SetJumpDirection()
	{
		Vector2 direction = _mousePosition - ( Vector2 )gameObject.transform.position;
		direction.Normalize();

		return direction;
	}

	public void OnJumpsChanged( int value )
	{
		JumpsChanged?.Invoke( value );
	}

	public void StopJump()
	{
		_isJumpStopped = true;
	}
}
