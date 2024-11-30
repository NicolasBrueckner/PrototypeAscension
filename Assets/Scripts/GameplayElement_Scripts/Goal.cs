using UnityEngine;
using static Utility;

[ RequireComponent( typeof( BoxCollider2D ) ) ]
public class Goal : MonoBehaviour
{
	public LayerMask finishableMask;
	private static RuntimeEventManager RuntimeEventManager => RuntimeEventManager.Instance;

	private void OnTriggerEnter2D( Collider2D other )
	{
		if( ValidateCollision( other.gameObject, finishableMask ) )
			RuntimeEventManager.OnGameEnded();
	}
}