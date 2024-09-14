using System.Collections;
using UnityEngine;
using static Utility;

[RequireComponent( typeof( CircleCollider2D ) )]
public class PortalDoor : MonoBehaviour
{
	public PortalDoor targetDoor;
	public LayerMask portableMask;

	private CircleCollider2D _boundingCircle;
	private Coroutine _coolDownCoroutine;
	private Transform _targetTransform;

	private void Awake()
	{
		_boundingCircle = GetComponent<CircleCollider2D>();
		_targetTransform = targetDoor.transform;
	}

	private void OnTriggerEnter2D( Collider2D collision )
	{
		GameObject collisionObject = collision.gameObject;
		Rigidbody2D rb2D = collisionObject.GetComponent<Rigidbody2D>();

		if ( IsValidCollision( collisionObject, portableMask ) && rb2D )
		{
			Teleport( rb2D );
		}
	}

	private void Teleport( Rigidbody2D rb2D )
	{
		targetDoor.DeactivatePortalDoor();
		rb2D.position = _targetTransform.position;
	}

	public void DeactivatePortalDoor()
	{
		if ( _coolDownCoroutine == null )
		{
			_boundingCircle.enabled = false;
			_coolDownCoroutine = StartCoroutine( CooldownCoroutine() );
		}
	}

	private IEnumerator CooldownCoroutine()
	{
		yield return new WaitForSeconds( 1.0f );
		_boundingCircle.enabled = true;

		_coolDownCoroutine = null;
	}
}
