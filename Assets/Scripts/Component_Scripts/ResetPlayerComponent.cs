using UnityEngine;
using static Utility;

[ RequireComponent( typeof( Collider2D ) ) ]
public class ResetPlayerComponent : MonoBehaviour
{
	public LayerMask resetableMask;

	private static RuntimeEventManager RuntimeEventManager => RuntimeEventManager.Instance;

	private void OnTriggerEnter2D( Collider2D collision )
	{
		if( ValidateCollision( collision.gameObject, resetableMask ) )
			ResetPlayer();
	}

	private static void ResetPlayer()
	{
		RuntimeEventManager.OnPlayerDeathInitiated();
	}
}