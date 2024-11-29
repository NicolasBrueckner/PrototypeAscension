using UnityEngine;

[ RequireComponent( typeof( LineRenderer ) ) ]
public class AimVisualComponent : MonoBehaviour
{
	private static readonly int LengthPropertyID = Shader.PropertyToID( "_LineLength" );
	public float lineLength;
	private LineRenderer _aimLine;
	private Vector2 _aimPosition;

	private bool _displayLine;
	private Material _lineMaterial;
	private InputEventManager _InputEventManager => InputEventManager.Instance;

	private void Awake()
	{
		_aimLine = GetComponent<LineRenderer>();
		_aimLine.positionCount = 2;

		_lineMaterial = _aimLine.material;
		_lineMaterial.SetFloat( LengthPropertyID, lineLength );

		_InputEventManager.AimPerformed += SetAimPosition;
		_InputEventManager.JumpPerformed += OnJumpPerformed;
		_InputEventManager.JumpCanceled += OnJumpCanceled;
		_InputEventManager.StopJump += OnJumpCanceled;
	}

	private void Update()
	{
		if( _displayLine )
			UpdateLine();
	}

	private void UpdateLine()
	{
		Vector2 point1 = transform.position;
		Vector2 point2 = point1 + GetAimDirection() * lineLength;

		_aimLine.SetPosition( 0, point1 );
		_aimLine.SetPosition( 1, point2 );
	}

	private void OnJumpPerformed()
	{
		ToggleLine( true );
	}

	private void OnJumpCanceled()
	{
		ToggleLine( false );
	}

	private void ToggleLine( bool isActive )
	{
		_displayLine = isActive;
		_aimLine.enabled = isActive;
	}

	private void SetAimPosition( Vector2 mousePosition )
	{
		_aimPosition = mousePosition;
	}

	private Vector2 GetAimDirection()
	{
		Vector2 direction = _aimPosition - ( Vector2 )transform.position;
		direction.Normalize();

		return direction;
	}
}