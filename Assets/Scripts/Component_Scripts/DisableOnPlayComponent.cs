using UnityEngine;

public class DisableOnPlayComponent : MonoBehaviour
{
	private void Start()
	{
		gameObject.SetActive( false );
	}
}