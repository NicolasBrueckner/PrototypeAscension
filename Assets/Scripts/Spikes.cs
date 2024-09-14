using UnityEngine;

//[RequireComponent( typeof( BoxCollider2D ) )]
public class Spikes : MonoBehaviour
{
	public int amount;
	public GameObject spikePrefab;
	public BoxCollider2D boundingbox;

	private Vector2 _spikeSize;

	private void Start()
	{
		UpdateSpikes();
	}

	public void UpdateSpikes()
	{
		ClearSpikes();
		SetSpikeSize();

		Quaternion rotation = transform.rotation;

		for ( int i = 0; i < amount; i++ )
		{
			Vector2 position = CalculateRotatedPosition( rotation, i );
			Instantiate( spikePrefab, position, rotation, transform );
		}

		UpdateCollider();
	}

	private void ClearSpikes()
	{
		while ( transform.childCount > 0 )
		{
			DestroyImmediate( transform.GetChild( 0 ).gameObject );
		}
	}

	private void SetSpikeSize()
	{
		if ( !spikePrefab.TryGetComponent<SpriteRenderer>( out var renderer ) )
		{
			Debug.LogError( "spike prefab does not have a component SpriteRenderer" );
			return;
		}

		_spikeSize = Vector3.Scale( renderer.bounds.size, transform.localScale );
	}

	private Vector2 CalculateRotatedPosition( Quaternion rotation, int step )
	{
		Vector2 localPosition = new( step * _spikeSize.x, 0 );
		Vector2 rotatedPosition = rotation * localPosition;

		return ( Vector2 )transform.position + rotatedPosition;
	}

	private void UpdateCollider()
	{
		if ( amount > 0 )
		{
			float totalWidth = amount * _spikeSize.x;
			Vector2 scale = transform.localScale;

			boundingbox.size = new Vector2( totalWidth, _spikeSize.y ) / scale;
			boundingbox.offset = new( totalWidth / ( 2 * scale.x ), 0 );
		}
	}
}
