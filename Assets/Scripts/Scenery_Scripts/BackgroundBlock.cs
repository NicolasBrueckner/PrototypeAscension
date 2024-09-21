using UnityEngine;

public class BackgroundBlock : MonoBehaviour
{
	public bool useCustomProperties;
	public float customSpeed;
	public float customRange;
	public Texture customTexture;

	private Material _material;

	private void Start()
	{
		SetMaterialDefaults();
	}

	private void SetMaterialDefaults()
	{
		_material = GetComponent<Renderer>().sharedMaterial;

		float randomOffset = Random.Range( 0.0f, Mathf.PI * 2.0f );
		_material.SetFloat( "_RandomOffset", randomOffset );
		Debug.Log( $"calculated offset: {randomOffset}, material offset: {_material.GetFloat( "_RandomOffset" )}" );

		if ( useCustomProperties )
		{
			_material.SetFloat( "_Speed", customSpeed );
			_material.SetFloat( "_Range", customRange );


			if ( customTexture != null )
				_material.SetTexture( "_MainTex", customTexture );
		}
	}
}
