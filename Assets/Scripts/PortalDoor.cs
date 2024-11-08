using System;
using System.Threading.Tasks;
using UnityEngine;
using static Utility;

[ RequireComponent( typeof( CircleCollider2D ) ) ]
public class PortalDoor : MonoBehaviour
{
	public PortalDoor targetDoor;
	public LayerMask  portableMask;

	private bool      _isActive = true;
	private bool      _isColliding;
	private Transform _targetTransform;

	private void Awake()
	{
		_targetTransform = targetDoor.transform;
	}

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.red;

		Gizmos.DrawWireSphere( transform.position, GetComponent<CircleCollider2D>().radius );
	}

	private void OnTriggerEnter2D( Collider2D other )
	{
		GameObject  collisionObject = other.gameObject;
		Rigidbody2D rb2D            = collisionObject.GetComponent<Rigidbody2D>();

		_isColliding = true;
		Debug.Log( $"is colliding in: {gameObject.name}" );
		if( !ValidateCollision( collisionObject, portableMask ) || !rb2D || !_isActive )
			return;

		Teleport( rb2D );
	}

	private void OnTriggerExit2D( Collider2D collision )
	{
		Debug.Log( $"exit from portal: {gameObject.name}" );
		_isColliding = false;
	}

	private async void Teleport( Rigidbody2D rb2D )
	{
		Debug.Log( $"porting from {gameObject.name} to {targetDoor.gameObject.name}" );
		rb2D.position = _targetTransform.position;
		await targetDoor.DeactivatePortalDoor();
	}

	private async Task DeactivatePortalDoor()
	{
		Debug.Log( $"deactivate {gameObject.name}" );
		_isActive = false;

		while( _isColliding )
		{
			Debug.Log( $"delaying... in {gameObject.name}" );
			await Task.Delay( TimeSpan.FromSeconds( 1 ) );
		}

		Debug.Log( $"delaying last... in {gameObject.name}" );
		await Task.Delay( TimeSpan.FromSeconds( 1 ) );

		_isActive = true;
	}
}