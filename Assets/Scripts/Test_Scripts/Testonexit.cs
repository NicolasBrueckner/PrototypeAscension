using UnityEngine;

namespace Test_Scripts
{
	public class Testonexit : MonoBehaviour
	{
		private void OnDrawGizmos()
		{
			Gizmos.color = Color.red;

			Gizmos.DrawWireSphere( transform.position, GetComponent<CircleCollider2D>().radius );
		}

		private void OnTriggerEnter2D( Collider2D other )
		{
			Debug.Log( "OnTriggerEnter2D" );
		}

		private void OnTriggerExit2D( Collider2D other )
		{
			Debug.Log( "OnTriggerExit2D" );
		}
	}
}