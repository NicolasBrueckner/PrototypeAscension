using UnityEditor;
using UnityEngine;

[CustomEditor( typeof( Spikes ) )]
public class SpikesEditor : Editor
{
	public override void OnInspectorGUI()
	{
		DrawDefaultInspector();

		Spikes spikes = ( Spikes )target;

		if ( GUILayout.Button( "Generate Spikes" ) )
			spikes.UpdateSpikes();

		if ( GUI.changed )
			spikes.UpdateSpikes();
	}
}
