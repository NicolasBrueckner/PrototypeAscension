using Manager_Scripts;
using UnityEngine;

[ RequireComponent( typeof( BoxCollider2D ) ) ]
public class CheckPoint : MonoBehaviour
{
	private CheckPointManager Manager => CheckPointManager.Instance;

	private void OnTriggerEnter2D( Collider2D collision )
	{
		Manager.SetCurrent( this );
	}
}