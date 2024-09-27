using Unity.Mathematics;
using UnityEngine;
using static Utility;

[RequireComponent( typeof( Camera ) )]
public class MainCameraManager : MonoBehaviour
{
	public static MainCameraManager Instance { get; private set; }

	public Transform targetTransform;
	public float smoothSpeed;
	public float offset;

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
		Vector3 targetPosition = new( transform.position.x, targetTransform.position.y, offset );
		Vector3 position;

		if ( smoothSpeed > 0 )
		{
			position = Vector3.Lerp( transform.position, targetPosition, smoothSpeed * Time.deltaTime );
		}
		else
		{
			position = targetPosition;
		}

		transform.position = position;
	}

	//xMin, xMax, yMin, yMax
	public float4 GetDimensionsAtDepth( float depth )
	{
		Vector3 bottomLeft = _mainCamera.ViewportToWorldPoint( new( 0, 0, depth ) );
		Vector3 topRight = _mainCamera.ViewportToWorldPoint( new( 1, 1, depth ) );

		return new( bottomLeft.x, topRight.x, bottomLeft.y, topRight.y );
	}
}
