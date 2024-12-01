using System.Collections;
using UnityEngine;

[ RequireComponent( typeof( PlayerJumpController ) ) ]
[ RequireComponent( typeof( PlayerCollisionController ) ) ]
public class PlayerVisualController : MonoBehaviour
{
	private static readonly int EmissionPropertyID = Shader.PropertyToID( "_EmissionFactor" );
	private static readonly int DissolvePropertyID = Shader.PropertyToID( "_DissolveFactor" );

	public GameObject visualContainer;
	public float rotationSpeed;
	public float minEmissionStrength;
	public float maxEmissionStrength;
	private Coroutine _dissolveCoroutine;

	private bool _canSpin;
	private Material _material;

	private static RuntimeEventManager RuntimeEventManager => RuntimeEventManager.Instance;

	private void Awake()
	{
		_material = visualContainer.GetComponent<SpriteRenderer>().material;

		RuntimeEventManager.ChargeChanged += OnChargeChanged;
		RuntimeEventManager.StateChanged += OnPositionStateChanged;
		RuntimeEventManager.PlayerDeathInitiated += OnPlayerDeathInitiated;
	}

	private void FixedUpdate()
	{
		if( _canSpin )
			visualContainer.transform.Rotate( Vector3.forward, rotationSpeed );
	}

	//sets flag for spinning when in Air
	private void OnPositionStateChanged( PlayerState state )
	{
		_canSpin = state == PlayerState.InAir;
	}

	//brighter when more jump charge
	private void OnChargeChanged( float emissionFraction )
	{
		_material.SetFloat( EmissionPropertyID,
			Mathf.Lerp( minEmissionStrength, maxEmissionStrength, emissionFraction ) );
	}

	private void OnPlayerDeathInitiated()
	{
		_dissolveCoroutine ??= StartCoroutine( DissolveCoroutine() );
	}

	private IEnumerator DissolveCoroutine()
	{
		float dissolve = _material.GetFloat( DissolvePropertyID );
		const float dissolveDuration = 0.8f;
		float timer = 0.0f;

		_canSpin = false;

		while( dissolve > 0.0f )
		{
			timer += Time.fixedUnscaledDeltaTime;

			dissolve = Mathf.Lerp( 1.0f, 0.0f, timer / dissolveDuration );
			_material.SetFloat( DissolvePropertyID, dissolve );

			yield return new WaitForFixedUpdate();
		}

		RuntimeEventManager.OnPlayerDeathCompleted( gameObject );
		_material.SetFloat( DissolvePropertyID, 1.0f );
		_dissolveCoroutine = null;
	}
}