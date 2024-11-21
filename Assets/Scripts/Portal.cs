using UnityEngine;

public class Portal : MonoBehaviour
{
	private static readonly int ColorPropertyID = Shader.PropertyToID( "_Color" );
	public Color portalColor;

	private void Awake()
	{
		ChangePortalDoorColor();
	}

	public void ChangePortalDoorColor()
	{
		foreach( Transform child in transform )
		{
			SpriteRenderer rend = child.GetComponent<SpriteRenderer>();

			if( !rend )
				continue;

			MaterialPropertyBlock block = new();

			rend.GetPropertyBlock( block );
			block.SetColor( ColorPropertyID, portalColor );
			rend.SetPropertyBlock( block );
		}
	}
}