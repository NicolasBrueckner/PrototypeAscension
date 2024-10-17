using UnityEngine;

[RequireComponent( typeof( SpriteRenderer ) )]
[RequireComponent( typeof( PlayerJumpController ) )]
[RequireComponent( typeof( PlayerCollisionController ) )]
public class PlayerVisualController : MonoBehaviour
{
	public float maxEmissionStrength;

	private bool _isInAir;
	private Coroutine _spinCoroutine;
	private Rigidbody2D _rb2D;
	private Material _material;
	private PlayerJumpController _playerJumpController;
	private PlayerCollisionController _playerCollisionController;

	private static readonly int EmissionPropertyID = Shader.PropertyToID( "_EmissionFactor" );

	private void Awake()
	{
		_rb2D = GetComponent<Rigidbody2D>();
		_material = GetComponent<SpriteRenderer>().material;
		_playerJumpController = GetComponent<PlayerJumpController>();
		_playerCollisionController = GetComponent<PlayerCollisionController>();

		_playerJumpController.ChargeChanged += OnChargeChanged;
		_playerCollisionController.StateChanged += OnPositionStateChanged;
	}

	private void FixedUpdate()
	{
		if ( _isInAir )
			_rb2D.MoveRotation( _rb2D.rotation + 25f );
	}

	//spin when in Air
	private void OnPositionStateChanged( PlayerState state )
	{
		_isInAir = state == PlayerState.InAir;
		_rb2D.constraints = _isInAir ? RigidbodyConstraints2D.None : RigidbodyConstraints2D.FreezeRotation;
	}

	//brighter when more jump charge
	private void OnChargeChanged( float emissionFraction )
	{
		_material.SetFloat( EmissionPropertyID, Mathf.Lerp( 1, maxEmissionStrength, emissionFraction ) );
	}
}
