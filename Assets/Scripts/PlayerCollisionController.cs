using System;
using System.Collections;
using UnityEngine;
using static Utility;

public enum PlayerState
{
	OnGround,
	OnWall,
	InAir,
}

[RequireComponent( typeof( Collider2D ) )]
[RequireComponent( typeof( Rigidbody2D ) )]
public class PlayerCollisionController : MonoBehaviour
{
	public LayerMask compoundMask;
	public LayerMask groundMask;
	public LayerMask wallMask;
	public float wallHoldTime;

	public event Action<PlayerState> PlayerStateChanged;

	private bool _isColliding;
	private PlayerState _currentState;
	private Coroutine _doubleCheckCoroutine;
	private Coroutine _holdCoroutine;
	private Collider2D _playerCollider;
	private Rigidbody2D _rb2D;

	private InputEventManager _inputEventManager => InputEventManager.Instance;

	private void Awake()
	{
		_playerCollider = GetComponent<Collider2D>();
		_rb2D = GetComponent<Rigidbody2D>();

		_inputEventManager.JumpCanceled += OnJumpCanceled;
	}

	private void Start()
	{
		_currentState = PlayerState.InAir;
	}

	private void OnCollisionEnter2D( Collision2D collision )
	{
		Debug.Log( "enter" );
		_isColliding = true;

		if ( TryValidateCollision( collision, groundMask ) )
		{
			HandleCollision( collision, PlayerState.OnGround );
		}

		else if ( TryValidateCollision( collision, wallMask ) )
		{
			HandleCollision( collision, PlayerState.OnWall );

			_holdCoroutine ??= StartCoroutine( HoldCoroutine( collision ) );
		}
	}

	private void OnCollisionExit2D( Collision2D collision )
	{
		Debug.Log( "exit" );
		_isColliding = false;

		OnPlayerStateChanged( PlayerState.InAir );
	}

	private void HandleCollision( Collision2D collision, PlayerState state )
	{
		OnPlayerStateChanged( state );
		ToggleHold( true );
		SetParent( collision.gameObject );
	}

	private void SetParent( GameObject parentObject )
	{
		Transform parent = ( parentObject != null )
			? FindParentWithLayer( parentObject.transform, compoundMask )
			: null;

		transform.SetParent( parent );
	}

	private void OnPlayerStateChanged( PlayerState state )
	{
		_currentState = state;
		PlayerStateChanged?.Invoke( _currentState );
	}

	private void ToggleHold( bool hold )
	{
		if ( hold )
		{
			_rb2D.gravityScale = 0.0f;
			_rb2D.velocity = Vector3.zero;
		}
		else
		{
			SetParent( null );
			_rb2D.gravityScale = 1.0f;
		}
	}

	private IEnumerator HoldCoroutine( Collision2D collision )
	{
		yield return new WaitForSeconds( wallHoldTime );

		if ( _isColliding )
		{
			ToggleHold( false );
			TrySeparateFromCollision( collision );
		}

		_holdCoroutine = null;
	}

	private IEnumerator DoubleCheckCoroutine()
	{
		yield return new WaitForFixedUpdate();

		if ( _isColliding )
		{
			_playerCollider.enabled = false;
			_playerCollider.enabled = true;
		}

		_doubleCheckCoroutine = null;
	}

	private void TrySeparateFromCollision( Collision2D collision )
	{
		if ( collision.contacts.Length == 0 )
			return;

		SeparateFromCollision( collision );
	}

	private void SeparateFromCollision( Collision2D collision )
	{
		Vector2 collisionNormal = collision.contacts[ 0 ].normal;

		_rb2D.AddForce( collisionNormal * 0.1f, ForceMode2D.Impulse );
	}

	private void OnJumpCanceled()
	{
		ToggleHold( false );
		_doubleCheckCoroutine ??= StartCoroutine( DoubleCheckCoroutine() );
	}
}
