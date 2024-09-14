using UnityEngine;
using static Utility;

public class CheckPointManager : MonoBehaviour
{
	public static CheckPointManager Instance { get; private set; }

	private CheckPoint current;

	private void Awake()
	{
		Instance = CreateSingleton( Instance, gameObject );
	}

	public void SetCurrent( CheckPoint current )
	{
		this.current = current;
	}

	public void Respawn( GameObject playerObject )
	{
		PlayerJumpController jumpController = playerObject.GetComponent<PlayerJumpController>();

		if ( jumpController )
			jumpController.StopJump();

		TryStopMovement( playerObject );
		playerObject.transform.position = current.transform.position;
	}
}
