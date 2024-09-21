using UnityEngine;

public class TestRotation : MonoBehaviour
{
	public float rotationSpeed;

	private void Update()
	{
		transform.Rotate( new( 0, 0, rotationSpeed ) );
	}
}
