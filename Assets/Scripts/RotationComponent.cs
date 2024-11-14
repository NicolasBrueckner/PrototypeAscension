using UnityEngine;

public class RotationComponent : MonoBehaviour
{
	public bool  isRotating;
	public float rotationSpeed;

	private void FixedUpdate()
	{
		if( isRotating )
			transform.Rotate( Vector3.forward, rotationSpeed );
	}
}