using UnityEngine;

[RequireComponent( typeof( BoxCollider2D ) )]
public class CheckPoint : MonoBehaviour
{
	private CheckPointManager Manager => CheckPointManager.Instance;

	private void OnTriggerEnter2D( Collider2D collision )
	{
		Debug.Log( "in OnTriggerEnter2D of Checkpoint" );
		Manager.SetCurrent( this );
	}
}
