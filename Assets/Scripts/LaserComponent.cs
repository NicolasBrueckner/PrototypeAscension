using UnityEngine;
using static Utility;

[ RequireComponent( typeof( LineRenderer ) ) ]
[ RequireComponent( typeof( EdgeCollider2D ) ) ]
public class LaserComponent : MonoBehaviour
{
	public GameObject firstZapperObject;
	public GameObject lastZapperObject;
	public Vector3[] points;

	private EdgeCollider2D _col;
	private LineRenderer _line;

	public void UpdateLaserComponent()
	{
		_line = GetComponent<LineRenderer>();
		_col = GetComponent<EdgeCollider2D>();

		Vector3[] convertedPoints = ConvertPoints();

		UpdateLaser( convertedPoints );
		UpdateZapperObjects();
	}

	private void UpdateLaser( Vector3[] convertedPoints )
	{
		_line.positionCount = convertedPoints.Length;
		_line.SetPositions( convertedPoints );
		_col.points = V3ToV2( convertedPoints );
	}

	private void UpdateZapperObjects()
	{
		firstZapperObject.transform.position = points[ 0 ];
		lastZapperObject.transform.position = points[ ^1 ];
	}

	private Vector3[] ConvertPoints()
	{
		Vector3[] convertedPoints = new Vector3[ points.Length ];

		for( int i = 0; i < convertedPoints.Length; i++ )
			convertedPoints[ i ] = points[ i ] - transform.position;

		return convertedPoints;
	}
}