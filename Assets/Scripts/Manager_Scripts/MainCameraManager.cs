using UnityEngine;
using static Utility;

[RequireComponent( typeof( Camera ) )]
public class MainCameraManager : MonoBehaviour
{
	public static MainCameraManager Instance { get; private set; }

	public Transform targetTransform;
	public float smoothSpeed;

	private Camera _mainCamera;

	private void Awake()
	{
		Instance = CreateSingleton( Instance, gameObject );

		_mainCamera = GetComponent<Camera>();
	}

	private void Update()
	{
		if ( targetTransform )
			FollowTarget();
	}

	private void FollowTarget()
	{
		Vector3 targetPosition = new( transform.position.x, targetTransform.position.y, -10.0f );
		Vector3 smoothedPosition = Vector3.Lerp( transform.position, targetPosition, smoothSpeed * Time.deltaTime );

		transform.position = smoothedPosition;
	}
}
