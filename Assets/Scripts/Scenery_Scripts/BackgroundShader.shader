Shader "Custom/BackgroundShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _GridSize ("Grid Size", Vector) = (10, 10, 0, 0)
        _Speed ("Speed", Float) = 1.0
        _MaxZOffset ("Max Z Offset", Float) = 0.5
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
#include "UnityCG.cginc"

struct appdata
{
	float4 vertex : POSITION;
	float2 uv : TEXCOORD0;
};

struct v2f
{
	float2 uv : TEXCOORD0;
	float4 vertex : SV_POSITION;
};

sampler2D _MainTex;
float4 _GridSize;
float _Speed;
float _MaxZOffset;

            // Simple hash function to generate random values based on position
float hash(float2 p)
{
	return frac(sin(dot(p, float2(127.1, 311.7))) * 43758.5453123);
}

v2f vert(appdata v)
{
	v2f o;

                // Extract the UV coordinates to determine grid position
	float2 gridPosition = floor(v.uv * _GridSize.xy);
                
                // Generate random offset for Z-axis based on grid position
	float randomZ = hash(gridPosition) * 2.0 - 1.0; // Random value between -1 and 1
	float time = _Time.y * _Speed;

                // Apply Z-axis random offset over time
	float zOffset = randomZ * _MaxZOffset * sin(time + randomZ);

                // Offset the vertex's Z position
	float4 worldPosition = UnityObjectToClipPos(v.vertex);
	worldPosition.z += zOffset;

	o.vertex = worldPosition;
	o.uv = v.uv;

	return o;
}

float4 frag(v2f i) : SV_Target
{
	float4 col = tex2D(_MainTex, i.uv);
	return col;
}
            ENDCG
        }
    }
FallBack"Diffuse"
}
