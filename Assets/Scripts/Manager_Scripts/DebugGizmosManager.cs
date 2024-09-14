using UnityEngine;
using static Utility;

public class DebugGizmosManager : MonoBehaviour
{
	public static DebugGizmosManager Instance { get; private set; }

	[Header( "Aim Line" )]
	public GameObject playerObject;
	public Color lineColor;

	private InputEventManager _InputEventManager => InputEventManager.Instance;
	private Vector2 _LinePos1 => playerObject?.transform.position ?? Vector2.zero;
	private Vector2 _LinePos2 => _InputEventManager?.MousePosition ?? Vector2.zero;
	private bool _DrawLine => _InputEventManager?.JumpIsPressed ?? false;

	private void Awake()
	{
		Instance = CreateSingleton( Instance, gameObject );
	}

	private void OnDrawGizmos()
	{
		if ( _DrawLine )
		{
			Gizmos.color = lineColor;

			Gizmos.DrawLine( _LinePos1, _LinePos2 );
		}
	}
}
