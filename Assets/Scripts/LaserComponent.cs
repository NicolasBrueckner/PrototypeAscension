using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static Utility;

[ RequireComponent( typeof( LineRenderer ) ) ]
[ RequireComponent( typeof( EdgeCollider2D ) ) ]
public class LaserComponent : MonoBehaviour
{
	public           GameObject                  zapperPrefab;
	public           Vector3[]                   points;
	private readonly Dictionary<int, GameObject> _zappers = new();

	private EdgeCollider2D _col;
	private LineRenderer   _line;

	private void Awake()
	{
		SetLaserStatus( true );
	}

	public void UpdateLaserComponent()
	{
		_line = GetComponent<LineRenderer>();
		_col  = GetComponent<EdgeCollider2D>();

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
		DeleteExcessZappers();

		foreach( int index in points.Select( ( p, i ) => i ) )
		{
			if( !_zappers.ContainsKey( index ) )
				_zappers[ index ] = Instantiate( zapperPrefab, points[ index ], Quaternion.identity, transform );
			else
				_zappers[ index ].transform.position = points[ index ];
		}
	}

	private void DeleteExcessZappers()
	{
		List<int> remove = _zappers.Keys.Except( points.Select( ( p, i ) => i ) ).ToList();
		foreach( int index in remove )
		{
			DestroyImmediate( _zappers[ index ] );
			_zappers[ index ] = null;
			_zappers.Remove( index );
		}
	}

	private Vector3[] ConvertPoints()
	{
		Vector3[] convertedPoints = new Vector3[ points.Length ];

		for( int i = 0; i < convertedPoints.Length; i++ )
			convertedPoints[ i ] = points[ i ] - transform.position;

		return convertedPoints;
	}

	public void SetLaserStatus( bool status )
	{
		_line.enabled = status;
		_col.enabled  = status;
	}
}