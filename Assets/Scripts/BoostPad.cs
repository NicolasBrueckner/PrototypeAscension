using UnityEngine;
using static Utility;

[RequireComponent( typeof( BoxCollider2D ) )]
public class BoostPad : MonoBehaviour
{
	public float boostStrength;
	public LayerMask boostableMask;
	public GameObject arrowObject;

	private void OnTriggerEnter2D( Collider2D collision )
	{
		GameObject collisionObject = collision.gameObject;
		Rigidbody2D rb2D = collisionObject.GetComponent<Rigidbody2D>();

		if ( IsValidCollision( collisionObject, boostableMask ) && rb2D )
		{
			ApplyBoost( rb2D );
		}
	}

	private void ApplyBoost( Rigidbody2D rb2D )
	{
		Vector2 velocity = GetBoostDirection() * boostStrength;

		rb2D.position = transform.position;
		rb2D.velocity = velocity;
	}

	private Vector2 GetBoostDirection()
	{
		return arrowObject.transform.up.normalized;
	}
}
