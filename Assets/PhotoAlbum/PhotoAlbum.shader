Shader "PhotoAlbum/PhotoArrayShader"
{
	Properties
	{
		_MyArr("Tex", 2DArray) = "" {}
		_SliceRange("Slices", Range(0,243)) = 0
		_UVScale("UVScale", Float) = 1.0
		_Page("Page",Int) = 0
		_Id("Id", Int) = 0
	}
	SubShader
	{
		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 3.5

			#include "UnityCG.cginc"

			struct v2f
			{
				float3 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
			};

			float _SliceRange;
			float _UVScale;
			int _Tiling;
			int _Page;
			int _Id;

			v2f vert(float4 vertex : POSITION)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(vertex);
				o.uv.xy = (vertex.xy + 0.5) * _UVScale;
				o.uv.x = -o.uv.x;
				if(_SliceRange==0){
					int count = _Page * 12 + _Id + 1;
					o.uv.z = (vertex.z + 0.5) * count;
				}else{
					o.uv.z = (vertex.z + 0.5) * _SliceRange;
				}
				return o;
			}

			UNITY_DECLARE_TEX2DARRAY(_MyArr);

			half4 frag(v2f i) : SV_Target
			{
				return UNITY_SAMPLE_TEX2DARRAY(_MyArr, i.uv);
			}
			ENDCG
		}
	}
}
