using Manager_Scripts;
using UnityEngine;
using static Utility;

[ RequireComponent( typeof( Collider2D ) ) ]
public class Portal : MonoBehaviour
{
	public Portal target;
	public LayerMask portableMask;
	public Color portalColor;

	private bool _isActive = true;

	private GameplayEventManager _GameplayEventManager => GameplayEventManager.Instance;

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere( transform.position, 1 );
	}

	private void OnTriggerEnter2D( Collider2D other )
	{
		GameObject collisionObject = other.gameObject;
		Rigidbody2D rb2D = collisionObject.GetComponent<Rigidbody2D>();

		if( !ValidateCollision( collisionObject, portableMask ) || !rb2D || !_isActive )
			return;

		target.DeactivatePortal();
		target.Teleport( rb2D );
	}

	private void Teleport( Rigidbody2D rb2D )
	{
		rb2D.position = transform.position;
	}

	private void DeactivatePortal()
	{
		_isActive = false;
		_GameplayEventManager.JumpStarted += ReactivatePortal;
	}

	private void ReactivatePortal()
	{
		_isActive = true;
		_GameplayEventManager.JumpStarted -= ReactivatePortal;
	}
}