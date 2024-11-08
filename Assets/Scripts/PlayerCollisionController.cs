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
	public PlayerState State;
	public LayerMask   Mask;
	public int         holdTime; //-1 is infinite
}

[ RequireComponent( typeof( Collider2D ) ) ]
[ RequireComponent( typeof( Rigidbody2D ) ) ]
public class PlayerCollisionController : MonoBehaviour
{
	public  List<PlayerStateData> states;
	private Collider2D            _collider;
	private Collision2D           _currentCollision;
	private bool                  _isColliding;

	private bool        _isHolding;
	private Rigidbody2D _rb2D;

	private InputEventManager _inputEventManager => InputEventManager.Instance;


	private void Awake()
	{
		_rb2D                           =  GetComponent<Rigidbody2D>();
		_collider                       =  GetComponent<Collider2D>();
		_inputEventManager.JumpCanceled += OnJumpCanceled;
	}

	private void Start()
	{
		OnStateChanged( PlayerState.InAir );
	}

	private void OnCollisionEnter2D( Collision2D collision )
	{
		if( CheckForOneWay( collision ) )
			return;

		if( !_isColliding )
		{
			_isColliding = true;
			CheckCollision( collision );
		}
	}

	public event Action<PlayerState> StateChanged;

	private async void CheckCollision( Collision2D collision )
	{
		foreach( PlayerStateData data in states.Where( data => TryValidateCollision( collision, data.Mask ) ) )
		{
			await HandleCollision( collision, data );
			return;
		}
	}

	private async Task HandleCollision( Collision2D collision, PlayerStateData data )
	{
		OnStateChanged( data.State );

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
		OnStateChanged( PlayerState.InAir );
		_isColliding = false;
		await DoubleCheckCollision( collision );
	}

	private void OnStateChanged( PlayerState state )
	{
		StateChanged?.Invoke( state );
	}

	private void SeparateFromCollision( Collision2D collision )
	{
		if( collision.contacts.Length == 0 )
			return;

		Vector2 normal = GetAverageCollisionNormal( collision );

		_rb2D.AddForce( normal * 0.1f, ForceMode2D.Impulse );
	}

	private async Task DoubleCheckCollision( Collision2D collision )
	{
		if( ValidateVelocity( _rb2D.velocity, GetAverageCollisionNormal( collision ), 90f ) )
			return;

		Debug.Log( "not a valid velocity" );

		await Task.Delay( TimeSpan.FromSeconds( Time.fixedUnscaledDeltaTime ) );

		_collider.enabled = false;
		_collider.enabled = true;
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

	private void OnJumpCanceled()
	{
		_isHolding = false;
	}
}

public class DisposableHold : IDisposable
{
	private readonly float       _drag;
	private readonly Rigidbody2D _rb2D;

	public DisposableHold( Rigidbody2D rb2D )
	{
		_rb2D      = rb2D;
		_drag      = _rb2D.drag;
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
	private readonly LayerMask _parentMask = LayerMask.NameToLayer( "CompoundCollision" );

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
		Transform target = parent != null ? FindParentWithLayer( parent.transform, _parentMask ) : null;

		_child.SetParent( target );
	}
}