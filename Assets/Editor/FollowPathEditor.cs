using UnityEditor;
using UnityEngine;

[CustomEditor( typeof( FollowPathComponent ) )]
public class FollowPathEditor : Editor
{
	private void OnSceneGUI()
	{
		FollowPathComponent component = ( FollowPathComponent )target;

		if ( component.targets == null || component.targets.Length == 0 )
			return;

		for ( int i = 0; i < component.targets.Length; i++ )
		{
			if ( component.targets[ i ] != null )
			{
				EditorGUI.BeginChangeCheck();

				Vector3 position = DrawHandle( component.targets[ i ], i );

				if ( EditorGUI.EndChangeCheck() )
				{
					Undo.RecordObject( target, "Move Point" );
					component.targets[ i ] = position;
				}
			}
		}
	}

	private Vector2 DrawHandle( Vector2 position, int index )
	{
		float size = 0.2f;
		Color fillColor = new( 0.25f, 0.6f, 1.0f );
		Color outlineColor = new( 0.8f, 0.8f, 0.8f );

		Handles.color = fillColor;
		Handles.DrawSolidDisc( position, Vector3.forward, size );
		Handles.color = outlineColor;

		GUIStyle style = new();
		style.normal.textColor = Color.white;
		Handles.Label( position, index.ToString(), style );

		return Handles.FreeMoveHandle(
			position,
			size,
			Vector3.zero,
			Handles.CircleHandleCap );
	}
}
