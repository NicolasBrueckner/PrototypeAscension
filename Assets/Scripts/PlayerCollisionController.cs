using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Animations;
using static Utility;
using constraints = UnityEngine.RigidbodyConstraints2D;

public enum PlayerState
{
	OnGround,
	OnWall,
	InAir,
}

[RequireComponent( typeof( Collider2D ) )]
[RequireComponent( typeof( Rigidbody2D ) )]
[RequireComponent( typeof( PositionConstraint ) )]
public class PlayerCollisionController : MonoBehaviour
{
	public LayerMask groundMask;
	public LayerMask wallMask;
	public float wallFreezeTime;

	public event Action<PlayerState> PlayerStateChanged;

	private PlayerState _currentState;
	private Coroutine _freezeCoroutine;
	private Rigidbody2D _rb2D;
	private PositionConstraint _constraint;

	private InputEventManager _inputEventManager => InputEventManager.Instance;

	private void Awake()
	{
		_currentState = PlayerState.InAir;
		_rb2D = GetComponent<Rigidbody2D>();
		_constraint = GetComponent<PositionConstraint>();

		_inputEventManager.JumpCanceled += () => SetConstraints( constraints.None );
	}

	private void OnCollisionEnter2D( Collision2D collision )
	{
		GameObject collisionObject = collision.gameObject;

		if ( IsValidCollision( collisionObject, groundMask ) )
		{
			OnPlayerStateChanged( PlayerState.OnGround );
			SetConstraints( constraints.FreezeAll );
		}

		else if ( IsValidCollision( collisionObject, wallMask ) )
		{
			OnPlayerStateChanged( PlayerState.OnWall );
			SetConstraints( constraints.FreezeAll );

			_freezeCoroutine ??= StartCoroutine( FreezeCotoutine( collision ) );
		}
	}

	private void OnCollisionExit2D( Collision2D collision )
	{
		OnPlayerStateChanged( PlayerState.InAir );
		SetConstraints( constraints.None );
	}

	private void OnPlayerStateChanged( PlayerState state )
	{
		_currentState = state;
		PlayerStateChanged?.Invoke( _currentState );
	}

	private void SetConstraints( constraints constraints )
	{
		_rb2D.constraints = constraints;
	}

	private IEnumerator FreezeCotoutine( Collision2D collision )
	{
		yield return new WaitForSeconds( wallFreezeTime );

		SetConstraints( constraints.None );
		TrySeparateFromCollision( collision );

		_freezeCoroutine = null;
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
