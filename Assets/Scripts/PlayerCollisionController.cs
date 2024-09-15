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

	private PlayerState _currentState;
	private Coroutine _holdCoroutine;
	private Rigidbody2D _rb2D;

	private InputEventManager _inputEventManager => InputEventManager.Instance;

	private void Awake()
	{
		_currentState = PlayerState.InAir;
		_rb2D = GetComponent<Rigidbody2D>();

		_inputEventManager.JumpCanceled += () => ToggleHold( false );
	}

	private void OnCollisionEnter2D( Collision2D collision )
	{
		GameObject collisionObject = collision.gameObject;

		if ( IsValidCollision( collisionObject, groundMask ) )
		{
			HandleCollision( collision, PlayerState.OnGround );
		}

		else if ( IsValidCollision( collisionObject, wallMask ) )
		{
			HandleCollision( collision, PlayerState.OnWall );

			_holdCoroutine ??= StartCoroutine( FreezeCotoutine( collision ) );
		}
	}

	private void OnCollisionExit2D( Collision2D collision )
	{
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

	private IEnumerator FreezeCotoutine( Collision2D collision )
	{
		yield return new WaitForSeconds( wallHoldTime );

		ToggleHold( false );
		TrySeparateFromCollision( collision );

		_holdCoroutine = null;
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
}
