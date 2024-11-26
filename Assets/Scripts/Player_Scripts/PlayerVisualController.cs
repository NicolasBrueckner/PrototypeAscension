using Manager_Scripts;
using UnityEngine;

[ RequireComponent( typeof( PlayerJumpController ) ) ]
[ RequireComponent( typeof( PlayerCollisionController ) ) ]
public class PlayerVisualController : MonoBehaviour
{
	private static readonly int EmissionPropertyID = Shader.PropertyToID( "_EmissionFactor" );
	public GameObject visualContainer;
	public float rotationSpeed;
	public float maxEmissionStrength;

	private bool _isInAir;
	private Material _material;

	private static RuntimeEventManager RuntimeEventManager => RuntimeEventManager.Instance;

	private void Awake()
	{
		_material = visualContainer.GetComponent<SpriteRenderer>().material;

		RuntimeEventManager.ChargeChanged += OnChargeChanged;
		RuntimeEventManager.StateChanged += OnPositionStateChanged;
	}

	private void FixedUpdate()
	{
		if( _isInAir )
			visualContainer.transform.Rotate( Vector3.forward, rotationSpeed );
	}

	//spin when in Air
	private void OnPositionStateChanged( PlayerState state )
	{
		_isInAir = state == PlayerState.InAir;
	}

	//brighter when more jump charge
	private void OnChargeChanged( float emissionFraction )
	{
		_material.SetFloat( EmissionPropertyID, Mathf.Lerp( 1, maxEmissionStrength, emissionFraction ) );
	}
}