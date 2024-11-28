using UnityEngine;
using static Utility;

[ RequireComponent( typeof( Collider2D ) ) ]
public class ResetPlayerComponent : MonoBehaviour
{
	public LayerMask resetableMask;

	private static RuntimeEventManager RuntimeEventManager => RuntimeEventManager.Instance;

	private void OnTriggerEnter2D( Collider2D collision )
	{
		GameObject collisionObject = collision.gameObject;

		if( ValidateCollision( collisionObject, resetableMask ) )
			ResetPlayer( collisionObject );
	}

	private static void ResetPlayer( GameObject playerObject )
	{
		RuntimeEventManager.OnPlayerReset( playerObject );
	}
}