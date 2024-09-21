using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class BackgroundBlocksController : MonoBehaviour
{
	public int rows;
	public int columns;
	public int depth;
	public Transform _targetTransform;
	public GameObject backgroundBlockPrefab;

	private float _width;
	private float _height;

	private DoubleSidedQueue<List<GameObject>> _backgroundBlockLists;
	private MainCameraManager _mainCameraManager => MainCameraManager.Instance;

	private void Start()
	{
		float4 dimensions = _mainCameraManager.GetDimensionsAtDepth( depth );

		_width = dimensions.y - dimensions.x;
		_height = dimensions.w - dimensions.z;

		PreBuildBackground();
	}

	private void PreBuildBackground()
	{
		float blockWidth = _width / rows;
		float blockHeight = _height / columns;
		float blockDepth = backgroundBlockPrefab.transform.localScale.z;

		_backgroundBlockLists = new();

		for ( int i = 0; i < rows; i++ )
		{
			List<GameObject> blockList = new();

			for ( int j = 0; j < columns; j++ )
			{
				float x = i * blockWidth - _width / 2 + blockWidth / 2;
				float y = j * blockHeight - _height / 2 + blockHeight / 2;

				Vector3 position = new( x, y, depth );

				GameObject block = Instantiate( backgroundBlockPrefab, position, Quaternion.identity );
				block.transform.localScale = new( blockWidth, blockHeight, blockDepth );
				blockList.Add( block );
			}

			_backgroundBlockLists.EnqueueFront( blockList );
		}
	}
}
