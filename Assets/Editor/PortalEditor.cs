using UnityEditor;
using UnityEngine;

[ CustomEditor( typeof( Portal ) ) ]
public class PortalEditor : Editor
{
	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();

		if( GUILayout.Button( "Apply Color" ) )
			( target as Portal )?.ChangePortalDoorColor();
	}
}