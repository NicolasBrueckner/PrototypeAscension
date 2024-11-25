using UnityEngine;

[RequireComponent( typeof( SpriteRenderer ) )]
public class Background : MonoBehaviour
{
	public Camera targetCamera;

	private Vector2 _cameraBounds;
	private SpriteRenderer _renderer;

	private void Awake()
	{
		_renderer = GetComponent<SpriteRenderer>();
	}

	private void Start()
	{
		ResizeBackground();
	}

	private void ResizeBackground()
	{
		float cameraHeight = 2f * targetCamera.orthographicSize;
		float cameraWidth = cameraHeight * targetCamera.aspect;
		float spriteHeight = _renderer.sprite.bounds.size.y;
		float spriteWidth = _renderer.sprite.bounds.size.x;

		Vector3 newScale = transform.localScale;
		newScale.x = cameraWidth / spriteWidth;
		newScale.y = cameraHeight / spriteHeight;

		transform.localScale = newScale;
	}
}
