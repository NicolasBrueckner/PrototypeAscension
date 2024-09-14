using System.Collections.Generic;
using UnityEngine;

[RequireComponent( typeof( SpriteRenderer ) )]
[RequireComponent( typeof( PlayerJumpController ) )]
public class PlayerVisualController : MonoBehaviour
{
	private SpriteRenderer _renderer;
	private PlayerJumpController _jumpController;
	private Dictionary<int, Color> _colorByRemainingJumps;

	private void Awake()
	{
		SetDefaults();
	}

	private void SetDefaults()
	{
		_renderer = GetComponent<SpriteRenderer>();
		_jumpController = GetComponent<PlayerJumpController>();
		_jumpController.JumpsChanged += SetPlayerColor;

		InitilizeDictionaries();
	}

	private void InitilizeDictionaries()
	{
		_colorByRemainingJumps = new Dictionary<int, Color>
		{
			{ 0, Color.red },
			{ 1, Color.yellow },
			{ 2, Color.green },
		};
	}

	private void SetPlayerColor( int remainingJumps )
	{
		_renderer.color = _colorByRemainingJumps[ remainingJumps ];
	}
}
