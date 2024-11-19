using Manager_Scripts;
using UnityEngine;
using static Utility;

[ RequireComponent( typeof( Collider2D ) ) ]
public class Portal : MonoBehaviour
{
	public Transform door1Transform;
	public Transform door2Transform;
	public LayerMask portableMask;

	private bool _isActive = true;

	private GameplayEventManager _GameplayEventManager => GameplayEventManager.Instance;

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.red;

		Gizmos.DrawWireSphere( door1Transform.position, 1 );
		Gizmos.DrawWireSphere( door2Transform.position, 1 );
	}

	private void OnTriggerEnter2D( Collider2D other )
	{
		GameObject collisionObject = other.gameObject;
		Rigidbody2D rb2D = collisionObject.GetComponent<Rigidbody2D>();

		if( !ValidateCollision( collisionObject, portableMask ) || !rb2D || !_isActive )
			return;

		_isActive = false;
		_GameplayEventManager.JumpStarted += ReactivatePortal;
		Transform targetTransform = GetEntranceTransform( other.transform );
		Teleport( rb2D, targetTransform );
	}

	private Transform GetEntranceTransform( Transform portableTransform )
	{
		return Vector3.Distance( portableTransform.position, door1Transform.position ) <
		       Vector3.Distance( portableTransform.position, door2Transform.position )
			       ? door2Transform
			       : door1Transform;
	}

	private void Teleport( Rigidbody2D rb2D, Transform targetTransform )
	{
		rb2D.position = targetTransform.position;
	}

	private void ReactivatePortal()
	{
		_isActive = true;
		_GameplayEventManager.JumpStarted -= ReactivatePortal;
	}
}