using UnityEngine;
using static Utility;

public class CheckPointManager : MonoBehaviour
{
	private       CheckPoint        current;
	public static CheckPointManager Instance{ get; private set; }

	private void Awake()
	{
		Instance = CreateSingleton( Instance, gameObject );
	}

	public void SetCurrent( CheckPoint current )
	{
		this.current = current;
		Debug.Log( $"current: {current.gameObject.name}" );
	}

	public void Respawn( GameObject playerObject )
	{
		PlayerJumpController jumpController = playerObject.GetComponent<PlayerJumpController>();
		if( !current )
			Debug.Log( "current is null" );

		if( jumpController )
			jumpController.StopJump();

		TryStopMovement( playerObject );
		playerObject.transform.position = current.transform.position;
	}
}