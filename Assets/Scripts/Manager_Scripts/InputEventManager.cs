using System;
using UnityEngine;
using UnityEngine.InputSystem;
using static Utility;

public class InputEventManager : MonoBehaviour
{
	public static InputEventManager Instance { get; private set; }

	public event Action InputsBound;
	public event Action JumpPerformed;
	public event Action JumpCanceled;
	public event Action<Vector2> AimPerformed;

	private PlayerInputs _inputs;
	private InputAction _jumpInput;
	private InputAction _aimInput;

	private Camera _mainCamera;

	#region Unity Runtime Methods
	private void Awake()
	{
		Instance = CreateSingleton( Instance, gameObject );

		_inputs = new();
		_mainCamera = Camera.main;

		SetInputActions();
		BindInputActions();
	}

	private void Start()
	{
		InputsBound?.Invoke();
	}

	private void OnEnable()
	{
		EnableInputActions();
	}

	private void OnDisable()
	{
		DisableInputActions();
	}

	private void OnDestroy()
	{
		UnbindInputActions();
	}
	#endregion

	#region Input Setup Methods
	private void SetInputActions()
	{
		_jumpInput = _inputs.Runtime.Jump;
		_aimInput = _inputs.Runtime.Aim;
	}

	private void BindInputActions()
	{
		_jumpInput.performed += OnJumpPerformed;
		_jumpInput.canceled += OnJumpCanceled;
		_aimInput.performed += OnAimPerformed;
	}

	private void UnbindInputActions()
	{
		_jumpInput.performed -= OnJumpPerformed;
		_jumpInput.canceled -= OnJumpCanceled;
		_aimInput.performed -= OnAimPerformed;
	}

	private void EnableInputActions()
	{
		_jumpInput.Enable();
		_aimInput.Enable();
	}

	private void DisableInputActions()
	{
		_jumpInput.Disable();
		_aimInput.Disable();
	}
	#endregion

	#region Input Event Methods
	private void OnJumpPerformed( InputAction.CallbackContext context )
	{
		JumpIsPressed = true;
		JumpPerformed?.Invoke();
	}

	private void OnJumpCanceled( InputAction.CallbackContext context )
	{
		JumpIsPressed = false;
		JumpCanceled?.Invoke();
	}

	private void OnAimPerformed( InputAction.CallbackContext context )
	{
		Vector2 position = context.ReadValue<Vector2>();
		Vector2 adjustedPosition = _mainCamera.ScreenToWorldPoint( position );

		MousePosition = adjustedPosition;
		AimPerformed?.Invoke( adjustedPosition );
	}
	#endregion

	#region Debug Variables
	public bool JumpIsPressed { get; private set; }
	public Vector2 MousePosition { get; private set; }
	#endregion
}
