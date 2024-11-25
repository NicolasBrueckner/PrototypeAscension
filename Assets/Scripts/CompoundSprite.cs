using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CompoundSprite : MonoBehaviour
{
	public Vector2 tiledSize = new( 1f, 1f );

	private BoxCollider2D _box;
	private IEnumerable _renderers;
	private Dictionary<BoxCollider2D, Vector2> _sizeByColliders;
	private Dictionary<BoxCollider2D, Vector2> _offsetByColliders;

	public void UpdateCompoundSize()
	{
		foreach ( SpriteRenderer renderer in _renderers )
		{
			renderer.size = tiledSize;

			if ( _box = renderer.GetComponent<BoxCollider2D>() )
			{
				_box.size = _sizeByColliders[ _box ] * tiledSize;
				_box.offset = _offsetByColliders[ _box ] * tiledSize;
			}
		}
	}

	public void GatherCompound()
	{
		_renderers = GetValidRenderers();
		_sizeByColliders = new();
		_offsetByColliders = new();

		foreach ( SpriteRenderer renderer in _renderers )
		{
			if ( _box = renderer.GetComponent<BoxCollider2D>() )
			{
				_sizeByColliders.Add( _box, _box.size );
				_offsetByColliders.Add( _box, _box.offset );
			}
		}
	}

	private IEnumerable GetValidRenderers()
	{
		return GetComponentsInChildren<SpriteRenderer>().Where( r => r.drawMode == SpriteDrawMode.Sliced );
	}
}
