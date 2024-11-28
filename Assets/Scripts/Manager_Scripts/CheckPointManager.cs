using UnityEngine;
using static Utility;

public class CheckPointManager : MonoBehaviour
{
	private CheckPoint _current;
	public static CheckPointManager Instance{ get; private set; }

	private static RuntimeEventManager RuntimeEventManager => RuntimeEventManager.Instance;

	private void Awake()
	{
		Instance = CreateSingleton( Instance, gameObject );

		RuntimeEventManager.PlayerReset += OnPlayerReset;
	}

	public void SetCurrent( CheckPoint current )
	{
		_current = current;
	}

	public void OnPlayerReset( GameObject playerObject )
	{
		PlayerJumpController jumpController = playerObject.GetComponent<PlayerJumpController>();

		if( !_current )
			return;

		TryStopMovement( playerObject );
		playerObject.transform.position = _current.transform.position;
	}
}