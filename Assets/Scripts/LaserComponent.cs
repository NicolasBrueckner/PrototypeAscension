using UnityEngine;
using static Utility;

[RequireComponent( typeof( LineRenderer ) )]
[RequireComponent( typeof( EdgeCollider2D ) )]
public class LaserComponent : MonoBehaviour
{
	public Vector2[] points;

	private LineRenderer _line;
	private EdgeCollider2D _col;

	private void Awake()
	{
		UpdateLaser();
		SetLaserStatus( true );
	}

	public void UpdateLaser()
	{
		_line = GetComponent<LineRenderer>();
		_col = GetComponent<EdgeCollider2D>();
		_line.positionCount = points.Length;
		_line.SetPositions( V2ToV3( points, 0 ) );
		_col.points = points;
	}

	public void SetLaserStatus( bool status )
	{
		_line.enabled = status;
		_col.enabled = status;
	}
}
