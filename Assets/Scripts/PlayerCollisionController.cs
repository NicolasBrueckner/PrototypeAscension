using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using static Utility;

public enum PlayerState
{
	OnGround,
	OnWall,
	InAir,
}

[Serializable]
public struct PlayerStateData
{
	public PlayerState State;
	public LayerMask Mask;
	public int holdTime; //-1 is infinite
}

[RequireComponent( typeof( Collider2D ) )]
[RequireComponent( typeof( Rigidbody2D ) )]
public class PlayerCollisionController : MonoBehaviour
{
	public List<PlayerStateData> states;

	public PlayerState state;

	public event Action<PlayerState> StateChanged;

	private bool _isHolding;
	private bool _isColliding;
	private Rigidbody2D _rb2D;

	private InputEventManager _inputEventManager => InputEventManager.Instance;


	private void Awake()
	{
		_rb2D = GetComponent<Rigidbody2D>();
		_inputEventManager.JumpCanceled += OnJumpCanceled;
	}

	private void Start()
	{
		OnStateChanged( PlayerState.InAir );
	}

	private void OnCollisionEnter2D( Collision2D collision )
	{
		if ( !_isColliding )
		{
			_isColliding = true;
			CheckCollision( collision );
		}
	}

	private async void CheckCollision( Collision2D collision )
	{
		foreach ( PlayerStateData data in states )
		{
			if ( TryValidateCollision( collision, data.Mask ) )
			{
				await HandleCollision( collision, data );
				return;
			}
		}
	}

	private async Task HandleCollision( Collision2D collision, PlayerStateData data )
	{
		OnStateChanged( data.State );

		using ( new DisposableHold( _rb2D ) )
		{
			using ( new DisposableSetParent( transform, collision.gameObject ) )
			{
				_isHolding = true;

				if ( data.holdTime == -1 )
					await HoldIndefinitely();
				else
					await HoldForDuration( data.holdTime );
			}
		}

		SeperateFromCollision( collision );
		OnStateChanged( PlayerState.InAir );
		_isColliding = false;
	}

	private void OnStateChanged( PlayerState state )
	{
		this.state = state;
		StateChanged?.Invoke( state );
	}

	private void SeperateFromCollision( Collision2D collision )
	{
		if ( collision.contacts.Length == 0 )
			return;

		Vector2 normal = GetAverageCollisionNormal( collision );

		_rb2D.AddForce( normal * 0.1f, ForceMode2D.Impulse );
	}

	private async Task HoldIndefinitely()
	{
		while ( _isHolding )
			await Task.Yield();
	}

	private async Task HoldForDuration( float duration )
	{
		float timer = duration;
		while ( _isHolding && timer > 0.0f )
		{
			await Task.Delay( TimeSpan.FromSeconds( Time.fixedDeltaTime ) );
			timer -= Time.fixedDeltaTime;
		}
	}

	private void OnJumpCanceled()
	{
		_isHolding = false;
	}
}

public class DisposableHold : IDisposable
{
	private readonly Rigidbody2D _rb2D;

	public DisposableHold( Rigidbody2D rb2D )
	{
		_rb2D = rb2D;
		_rb2D.gravityScale = 0.0f;
		_rb2D.velocity = Vector3.zero;
	}

	public void Dispose()
	{
		_rb2D.gravityScale = 1.0f;
	}
}

public class DisposableSetParent : IDisposable
{
	private Transform _child;
	private LayerMask _parentMask = LayerMask.NameToLayer( "CompoundCollision" );

	public DisposableSetParent( Transform child, GameObject parent )
	{
		_child = child;

		SetParent( parent );
	}

	public void Dispose()
	{
		_child.SetParent( null );
	}

	private void SetParent( GameObject parent )
	{
		Transform target = ( parent != null ) ? FindParentWithLayer( parent.transform, _parentMask ) : null;

		_child.SetParent( target );
	}
}

public class DisposableState : IDisposable
{
	public DisposableState( PlayerState state )
	{

	}
}