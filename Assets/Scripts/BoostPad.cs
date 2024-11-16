using System.Linq;
using UnityEngine;
using static Utility;

[ RequireComponent( typeof( Collider2D ) ) ]
public class BoostPad : MonoBehaviour
{
	public  float      boostStrength;
	public  LayerMask  boostableMask;
	public  GameObject arrowObject;
	private Collider2D _collider;

	private bool        _isValid;
	private Collider2D  _other;
	private GameObject  _otherObject;
	private Rigidbody2D _otherRb2D;

	private void Awake()
	{
		_collider = GetComponent<Collider2D>();
	}

	private void OnTriggerEnter2D( Collider2D other )
	{
		_other       = other;
		_otherObject = other.gameObject;
		_otherRb2D   = other.GetComponent<Rigidbody2D>();

		if( ValidateCollision( _otherObject, boostableMask ) && _otherRb2D )
			_isValid = true;
	}

	private void OnTriggerExit2D( Collider2D other )
	{
		_other       = null;
		_otherObject = null;
		_otherRb2D   = null;
		_isValid     = false;
	}

	private void OnTriggerStay2D( Collider2D other )
	{
		if( _isValid && IsFullyInside() )
			ApplyBoost( _otherRb2D );
	}


	private bool IsFullyInside()
	{
		Bounds bounds      = _collider.bounds;
		Bounds otherBounds = _other.bounds;

		Vector2[] corners = new Vector2[ 4 ];
		corners[ 0 ] = otherBounds.min;
		corners[ 1 ] = new( otherBounds.min.x, otherBounds.min.y );
		corners[ 2 ] = new( otherBounds.max.x, otherBounds.max.y );
		corners[ 3 ] = otherBounds.max;

		return corners.All( corner => bounds.Contains( corner ) );
	}

	private void ApplyBoost( Rigidbody2D rb2D )
	{
		Debug.Log( "ApplyBoost" );
		Vector2 velocity = GetBoostDirection() * boostStrength;

		rb2D.velocity = velocity;
	}

	private Vector2 GetBoostDirection()
	{
		return arrowObject.transform.up.normalized;
	}
}