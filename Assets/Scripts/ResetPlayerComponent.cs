using UnityEngine;
using static Utility;

[RequireComponent( typeof( Collider2D ) )]
public class ResetPlayerComponent : MonoBehaviour
{
	public LayerMask resetableMask;

	private CheckPointManager _checkPointManager => CheckPointManager.Instance;

	private void OnTriggerEnter2D( Collider2D collision )
	{
		GameObject collisionObject = collision.gameObject;

		if ( ValidateCollision( collisionObject, resetableMask ) )
		{
			ResetPlayer( collisionObject );
		}
	}

	private void ResetPlayer( GameObject playerObject )
	{
		_checkPointManager.Respawn( playerObject );
	}
}
