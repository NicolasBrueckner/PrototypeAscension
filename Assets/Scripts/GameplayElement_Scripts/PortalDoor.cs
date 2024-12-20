using UnityEngine;
using static Utility;

[ RequireComponent( typeof( Collider2D ) ) ]
public class PortalDoor : MonoBehaviour
{
	public PortalDoor target;
	public LayerMask portableMask;

	private bool _isActive = true;

	private static RuntimeEventManager RuntimeEventManager => RuntimeEventManager.Instance;


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
		RuntimeEventManager.JumpStartedEmpty += ReactivatePortal;
	}

	private void ReactivatePortal()
	{
		_isActive = true;
		RuntimeEventManager.JumpStartedEmpty -= ReactivatePortal;
	}
}