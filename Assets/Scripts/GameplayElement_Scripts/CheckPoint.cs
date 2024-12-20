using UnityEngine;

[ RequireComponent( typeof( BoxCollider2D ) ) ]
public class CheckPoint : MonoBehaviour
{
	private static CheckPointManager Manager => CheckPointManager.Instance;

	private void OnTriggerEnter2D( Collider2D collision )
	{
		Manager.SetCurrent( this );
	}
}