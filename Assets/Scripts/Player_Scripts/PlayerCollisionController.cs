using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using static Utility;

public enum PlayerState
{
	OnGround,
	OnWall,
	InAir,
}

[ Serializable ]
public struct PlayerStateData
{
	public PlayerState state;
	public LayerMask mask;
	public int holdTime; //-1 is infinite
}

[ RequireComponent( typeof( Collider2D ) ) ]
[ RequireComponent( typeof( Rigidbody2D ) ) ]
public class PlayerCollisionController : MonoBehaviour
{
	public List<PlayerStateData> states;
	private Collision2D _currentCollision;
	private bool _isColliding;

	private bool _isHolding;

	private Rigidbody2D _rb2D;

	private static RuntimeEventManager RuntimeEventManager => RuntimeEventManager.Instance;

	private void Awake()
	{
		_rb2D = GetComponent<Rigidbody2D>();
		RuntimeEventManager.JumpStarted += OnJumpStarted;
		RuntimeEventManager.PlayerResetEmpty += OnPlayerReset;
	}

	private void Start()
	{
		RuntimeEventManager.OnStateChanged( PlayerState.InAir );
	}

	private void OnCollisionEnter2D( Collision2D collision )
	{
		if( _isColliding || CheckForOneWay( collision ) )
			return;

		_isColliding = true;
		_currentCollision = collision;

		CheckCollision( collision );
	}

	// This event function is necessary to ensure continuous update of "_currentCollision.contacts".
	// Unity's physics system does not automatically update collision data so this explicit call is
	// important. Removing this will interfere with the functionality of "OnJumpStarted()".
	private void OnCollisionStay2D( Collision2D other )
	{
	}

	private async void CheckCollision( Collision2D collision )
	{
		foreach( PlayerStateData data in states.Where( data => TryValidateCollision( collision, data.mask ) ) )
		{
			await HandleCollision( collision, data );
			return;
		}

		_isColliding = false;
	}

	private async Task HandleCollision( Collision2D collision, PlayerStateData data )
	{
		RuntimeEventManager.OnStateChanged( data.state );

		using( new DisposableHold( _rb2D ) )
		{
			using( new DisposableSetParent( transform, collision.gameObject ) )
			{
				_isHolding = true;

				if( data.holdTime == -1 )
					await HoldIndefinitely();
				else
					await HoldForDuration( data.holdTime );
			}
		}

		SeparateFromCollision( collision );

		_isColliding = false;
		_currentCollision = null;

		RuntimeEventManager.OnStateChanged( PlayerState.InAir );
	}

	private void SeparateFromCollision( Collision2D collision )
	{
		if( collision.contacts.Length == 0 )
			return;

		Vector2 normal = GetAverageCollisionNormal( collision );

		_rb2D.AddForce( normal * 0.2f, ForceMode2D.Impulse );
	}

	private async Task HoldIndefinitely()
	{
		while( _isHolding )
			await Task.Yield();
	}

	private async Task HoldForDuration( float duration )
	{
		float timer = duration;
		while( _isHolding && timer > 0.0f )
		{
			await Task.Delay( TimeSpan.FromSeconds( Time.fixedDeltaTime ) );
			timer -= Time.fixedDeltaTime;
		}
	}

	private void OnJumpStarted( Vector2 expectedVelocity )
	{
		if( !ValidateVelocity( expectedVelocity, GetAverageCollisionNormal( _currentCollision ), 85f ) )
		{
			RuntimeEventManager.OnJumpInvalid();
			return;
		}

		_isHolding = false;
	}

	private void OnPlayerReset()
	{
		_isHolding = false;
	}
}

public class DisposableHold : IDisposable
{
	private readonly float _drag;
	private readonly Rigidbody2D _rb2D;

	public DisposableHold( Rigidbody2D rb2D )
	{
		_rb2D = rb2D;
		_drag = _rb2D.drag;
		_rb2D.drag = float.MaxValue;
	}

	public void Dispose()
	{
		_rb2D.drag = _drag;
	}
}

public class DisposableSetParent : IDisposable
{
	private readonly Transform _child;
	private readonly LayerMask _parentMask = 1 << LayerMask.NameToLayer( "CompoundCollision" );

	public DisposableSetParent( Transform child, GameObject parent )
	{
		_child = child;

		SetParent( parent );
	}

	public void Dispose()
	{
		SetParent( null );
	}

	private void SetParent( GameObject parent )
	{
		Transform target = parent ? FindParentWithLayer( parent.transform, _parentMask ) : null;

		_child.SetParent( target );
	}
}