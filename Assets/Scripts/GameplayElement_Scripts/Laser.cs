using UnityEngine;
using static Utility;

[ RequireComponent( typeof( LineRenderer ) ) ]
[ RequireComponent( typeof( EdgeCollider2D ) ) ]
public class Laser : MonoBehaviour
{
	public GameObject firstZapperObject;
	public GameObject lastZapperObject;
	public Vector3[] points;

	public void UpdateLaserComponent()
	{
		Vector3[] convertedPoints = ConvertPoints();

		UpdateLineRenderer( convertedPoints );
		UpdateEdgeCollider( convertedPoints );
		UpdateZapperObjects();
	}

	private Vector3[] ConvertPoints()
	{
		Vector3[] convertedPoints = new Vector3[ points.Length ];

		for( int i = 0; i < convertedPoints.Length; i++ )
			convertedPoints[ i ] = points[ i ] - transform.position;

		return convertedPoints;
	}

	private static Vector2 AdjustPoint( Vector2 point1, Vector2 point2 )
	{
		Vector2 direction = ( point1 - point2 ).normalized;

		return point1 + direction * 0.3f;
	}

	private void UpdateLineRenderer( Vector3[] convertedPoints )
	{
		LineRenderer line = GetComponent<LineRenderer>();

		line.positionCount = convertedPoints.Length;
		line.SetPositions( convertedPoints );
	}

	private void UpdateEdgeCollider( Vector3[] convertedPoints )
	{
		EdgeCollider2D col = GetComponent<EdgeCollider2D>();

		convertedPoints[ 0 ] = AdjustPoint( convertedPoints[ 0 ], convertedPoints[ 1 ] );
		convertedPoints[ ^1 ] = AdjustPoint( convertedPoints[ ^1 ], convertedPoints[ ^2 ] );

		col.points = V3ToV2( convertedPoints );
	}

	private void UpdateZapperObjects()
	{
		firstZapperObject.transform.position = points[ 0 ];
		lastZapperObject.transform.position = points[ ^1 ];
	}
}