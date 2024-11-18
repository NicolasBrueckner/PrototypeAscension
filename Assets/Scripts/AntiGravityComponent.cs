using UnityEngine;
using static Utility;

[ RequireComponent( typeof( Collider2D ) ) ]
public class AntiGravityComponent : MonoBehaviour
{
	public  float       gravityChange;
	public  LayerMask   affectedLayers;
	private Rigidbody2D _currentRb2D;
	private float       _gravityOriginal;

	private bool _isValid;

	private void OnTriggerEnter2D( Collider2D other )
	{
		_currentRb2D = other.GetComponent<Rigidbody2D>();

		if( !_currentRb2D || !ValidateCollision( other.gameObject, affectedLayers ) )
			return;

		_gravityOriginal = _currentRb2D.gravityScale;
		_isValid         = true;
	}

	private void OnTriggerExit2D( Collider2D other )
	{
		_isValid                  = false;
		_currentRb2D.gravityScale = _gravityOriginal;
		_currentRb2D              = null;
	}

	private void OnTriggerStay2D( Collider2D other )
	{
		if( _isValid )
			_currentRb2D.gravityScale = gravityChange;
	}
}