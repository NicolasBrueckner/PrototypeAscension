using UnityEngine;

[RequireComponent( typeof( LineRenderer ) )]
public class AimVisualComponent : MonoBehaviour
{
	public float lineLength;

	private bool _displayLine;
	private Vector2 _aimDirection;
	private Material _lineMaterial;
	private LineRenderer _aimLine;

	private static readonly int LengthPropertyID = Shader.PropertyToID( "_LineLength" );
	private InputEventManager _inputEventManager => InputEventManager.Instance;

	private void Awake()
	{
		_aimLine = GetComponent<LineRenderer>();
		_aimLine.positionCount = 2;

		_lineMaterial = _aimLine.material;
		_lineMaterial.SetFloat( LengthPropertyID, lineLength );

		_inputEventManager.AimPerformed += SetAimDirection;
		_inputEventManager.JumpPerformed += OnJumpPerformed;
		_inputEventManager.JumpCanceled += OnJumpCanceled;
	}

	private void Update()
	{
		if ( _displayLine )
			UpdateLine();
	}

	private void UpdateLine()
	{
		Vector2 point1 = transform.position;
		Vector2 point2 = point1 + ( _aimDirection * lineLength );

		_aimLine.SetPosition( 0, point1 );
		_aimLine.SetPosition( 1, point2 );
	}

	private void OnJumpPerformed() => ToggleLine( true );
	private void OnJumpCanceled() => ToggleLine( false );
	private void ToggleLine( bool isActive )
	{
		_displayLine = isActive;
		_aimLine.enabled = isActive;
	}

	private void SetAimDirection( Vector2 mousePosition )
	{
		_aimDirection = mousePosition - ( Vector2 )transform.position;
		_aimDirection.Normalize();
	}
}
