using UnityEngine;

[RequireComponent( typeof( VelocityComponent ) )]
public class FollowPathComponent : MonoBehaviour
{
	public Vector2[] targets;
	public float speed;

	private int _currentIndex;
	private bool _isMovingForward = true;
	private VelocityComponent _moveComponent;

	private void Awake()
	{
		_moveComponent = GetComponent<VelocityComponent>();
		_moveComponent.TargetReached += UpdateCurrentIndex;

		if ( targets.Length > 0 )
			transform.position = targets[ 0 ];

		_currentIndex = 1;
	}

	private void FixedUpdate()
	{
		if ( targets.Length == 0 )
			return;

		_moveComponent.SetMovement( targets[ _currentIndex ], speed );
	}

	//updates the next target to move back and forth all points
	private void UpdateCurrentIndex()
	{
		int last = targets.Length - 1;

		if ( _isMovingForward )
		{
			_currentIndex++;
			if ( _currentIndex >= last )
			{
				_currentIndex = targets.Length - 1;
				_isMovingForward = false;
			}
		}
		else
		{
			_currentIndex--;
			if ( _currentIndex < 0 )
			{
				_currentIndex = 0;
				_isMovingForward = true;
			}
		}
	}
}
