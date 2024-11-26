using Manager_Scripts;
using UnityEngine;
using static Utility;

[ RequireComponent( typeof( Collider2D ) ) ]
public class ResetPlayerComponent : MonoBehaviour
{
	public LayerMask resetableMask;

	private static CheckPointManager _checkPointManager => CheckPointManager.Instance;

	private void OnTriggerEnter2D( Collider2D collision )
	{
		GameObject collisionObject = collision.gameObject;

		if( ValidateCollision( collisionObject, resetableMask ) )
			ResetPlayer( collisionObject );
	}

	private static void ResetPlayer( GameObject playerObject )
	{
		_checkPointManager.Respawn( playerObject );
	}
}