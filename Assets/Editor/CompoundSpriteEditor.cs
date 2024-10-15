using UnityEditor;
using UnityEngine;

[CustomEditor( typeof( CompoundSprite ) )]
public class CompoundSpriteEditor : Editor
{
	private bool _isCompoundCreated;
	private SerializedProperty tiledSize;

	private void OnEnable()
	{
		tiledSize = serializedObject.FindProperty( "tiledSize" );
	}

	public override void OnInspectorGUI()
	{
		CompoundSprite compound = ( CompoundSprite )target;
		serializedObject.Update();

		EditorGUILayout.PropertyField( tiledSize );

		if ( GUILayout.Button( "Create Compound Data" ) )
		{
			_isCompoundCreated = true;
			compound.GatherCompound();
		}

		if ( serializedObject.ApplyModifiedProperties() && _isCompoundCreated )
		{
			compound.UpdateCompoundSize();
		}

		if ( GUILayout.Button( "Update Size" ) && _isCompoundCreated )
		{
			compound.UpdateCompoundSize();
		}
	}
}
