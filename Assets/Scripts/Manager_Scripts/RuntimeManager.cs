using UnityEngine;
using static Utility;


public class RuntimeManager : MonoBehaviour
{
	public GameObject runtimeObject;

	private GameObject _runtimeObjectCopy;
	public static RuntimeManager Instance{ get; private set; }
	private static RuntimeEventManager RuntimeEventManager => RuntimeEventManager.Instance;

	private void Awake()
	{
		Instance = CreateSingleton( Instance, gameObject );

		RuntimeEventManager.GameStarted += OnGameStarted;
		RuntimeEventManager.GameEnded += OnGameEnded;
	}

	private void OnGameStarted()
	{
		_runtimeObjectCopy = Instantiate( runtimeObject, transform );
	}

	private void OnGameEnded()
	{
		Destroy( _runtimeObjectCopy );
		_runtimeObjectCopy = null;
	}
}