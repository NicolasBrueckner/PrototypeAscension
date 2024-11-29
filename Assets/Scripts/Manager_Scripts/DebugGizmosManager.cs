using UnityEngine;
using static Utility;

public class DebugGizmosManager : MonoBehaviour
{
	[ Header( "Aim Line" ) ] public GameObject playerObject;

	public Color lineColor;
	public static DebugGizmosManager Instance{ get; private set; }

	private InputEventManager _InputEventManager => InputEventManager.Instance;
	private Vector2 _LinePos1 => playerObject != null ? playerObject.transform.position : Vector2.zero;
	private Vector2 _LinePos2 => _InputEventManager != null ? _InputEventManager.MousePosition : Vector2.zero;
	private bool _DrawLine => _InputEventManager != null && _InputEventManager.JumpIsPressed;

	private void Awake()
	{
		Instance = CreateSingleton( Instance, gameObject );
	}

	private void OnDrawGizmos()
	{
		if( _DrawLine )
		{
			Gizmos.color = lineColor;

			Gizmos.DrawLine( _LinePos1, _LinePos2 );
		}
	}
}