using UnityEngine;
using static Utility;

// this monobehaviour is pretty much a dumpster for behaviour that doesn't fit anywhere else
// or should run globally, but I was too lazy to encapsulate them properly. Also magic numbers.
// If anyone sees this please close the file and disregard. Thank you :)
[ RequireComponent( typeof( Camera ) ) ]
public class MainCameraManager : MonoBehaviour
{
	private static readonly int ZapperEmissionPropertyID = Shader.PropertyToID( "_EmissionFactor" );
	public AudioSource audioSource;
	public Transform targetTransform;
	public float smoothSpeed;
	public float offset;
	public Material zapperMaterial;

	private Camera _mainCamera;
	public static MainCameraManager Instance{ get; private set; }
	private static RuntimeEventManager RuntimeEventManager => RuntimeEventManager.Instance;

	private void Awake()
	{
		Instance = CreateSingleton( Instance, gameObject );

		_mainCamera = GetComponent<Camera>();
		RuntimeEventManager.GameStarted += OnGameStarted;
		RuntimeEventManager.GameEnded += OnGameEnded;
	}

	private void Update()
	{
		if( targetTransform )
			FollowTarget();

		if( zapperMaterial )
			PulsateZapper();
	}

	private void FollowTarget()
	{
		Vector3 targetPosition = new( transform.position.x, targetTransform.position.y, offset );

		Vector3 position = smoothSpeed > 0
			                   ? Vector3.Lerp( transform.position, targetPosition, smoothSpeed * Time.deltaTime )
			                   : targetPosition;

		position.y = Mathf.Max( position.y, -2.93f ); // magic number because of specific lower grid end
		transform.position = position;
	}

	private void PulsateZapper()
	{
		const float min = 0.3f;
		const float max = 0.7f;
		const float time = 2.0f;

		float phase = Mathf.Sin( Time.time / time * Mathf.PI * 2.0f );
		float intensity = Mathf.Lerp( min, max, ( phase + 1f ) * 0.5f );

		zapperMaterial.SetFloat( ZapperEmissionPropertyID, intensity );
	}

	private void OnGameStarted()
	{
		audioSource.Play();
	}

	private void OnGameEnded()
	{
		audioSource.Stop();
	}
}