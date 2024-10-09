using System;
using UnityEditor;
using UnityEngine;

[CustomEditor( typeof( Component ), true )]
public class PointsComponentEditor : Editor
{
	private readonly string _actionName = "Move Point";

	private void OnSceneGUI()
	{
		if ( target is FollowPathComponent followPathComponent )
		{
			Transform transform = followPathComponent.transform;
			Vector2[] localPoints = CoordinateSpaceConversion( followPathComponent.targets, transform, true );
			HandlePoints( localPoints, pos => followPathComponent.targets = CoordinateSpaceConversion( pos, transform, false ) );
		}
		else if ( target is LaserComponent laserComponent )
		{
			Transform transform = laserComponent.transform;
			Vector2[] localPoints = CoordinateSpaceConversion( laserComponent.points, transform, true );
			HandlePoints( localPoints, pos => laserComponent.points = CoordinateSpaceConversion( pos, transform, false ) );
			laserComponent.UpdateLaser();
		}
	}

	private void HandlePoints( Vector2[] points, Action<Vector2[]> setPoints )
	{
		if ( points == null || points.Length == 0 )
			return;

		EditorGUI.BeginChangeCheck();

		for ( int i = 0; i < points.Length; i++ )
		{
			points[ i ] = DrawHandle( points[ i ], i );
		}

		if ( EditorGUI.EndChangeCheck() )
		{
			EditorGUI.BeginChangeCheck();

			Undo.RecordObject( target, _actionName );
			setPoints( points );
			EditorUtility.SetDirty( target );
		}
	}

	private Vector2 DrawHandle( Vector2 position, int index )
	{
		float size = HandleUtility.GetHandleSize( position ) * 0.1f;

		Handles.color = new( 0.25f, 0.6f, 1.0f );
		Handles.DrawSolidDisc( position, Vector3.forward, size );
		Handles.color = new( 0.8f, 0.8f, 0.8f );

		GUIStyle style = new();
		style.normal.textColor = Color.white;
		Handles.Label( position, index.ToString(), style );

		return Handles.FreeMoveHandle(
			position,
			size,
			Vector3.zero,
			Handles.CircleHandleCap );
	}

	private Vector2[] CoordinateSpaceConversion( Vector2[] points, Transform transform, bool toLocal )
	{
		Vector2[] transformedPoints = new Vector2[ points.Length ];

		for ( int i = 0; i < transformedPoints.Length; i++ )
			transformedPoints[ i ] = toLocal
				? transform.InverseTransformPoint( points[ i ] )
				: transform.TransformPoint( points[ i ] );

		return transformedPoints;
	}
}
