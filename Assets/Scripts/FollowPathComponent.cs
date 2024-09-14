using UnityEngine;

[RequireComponent( typeof( Rigidbody2D ) )]
public class FollowPathComponent : MonoBehaviour
{
	public Vector2[] targets;
	public float speed;

	private int _currentIndex;
	private bool _isMovingForward = true;
	private Rigidbody2D _rb2D;

	private void Awake()
	{
		_rb2D = GetComponent<Rigidbody2D>();
		transform.position = targets[ 0 ];
		_currentIndex = 1;
	}

	private void FixedUpdate()
	{
		if ( IsCurrentTargetReached() )
		{
			UpdateCurrentIndex();
		}
		MoveToCurrentTarget();
	}

	private void MoveToCurrentTarget()
	{
		Vector2 direction = GetDirectionToCurrentTarget();

		_rb2D.velocity = direction * speed;
	}

	private Vector2 GetDirectionToCurrentTarget()
	{
		Vector2 direction = targets[ _currentIndex ] - ( Vector2 )transform.position;
		direction.Normalize();
		return direction;
	}

	private bool IsCurrentTargetReached()
	{
		return Vector2.Distance( transform.position, targets[ _currentIndex ] ) < 0.1f;
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
