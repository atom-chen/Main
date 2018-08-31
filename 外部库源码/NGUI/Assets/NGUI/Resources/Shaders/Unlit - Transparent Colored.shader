// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Unlit/Transparent Colored"
{
	Properties
	{
		_MainTex("Base (RGB), Alpha (A)", 2D) = "black" {}
	}

		SubShader
	{
		LOD 200

		Tags
		{
			"Queue" = "Transparent"
			"IgnoreProjector" = "True"
			"RenderType" = "Transparent"
		}

		Pass
		{
			Cull Off
			Lighting Off
			ZWrite Off
			Fog { Mode Off }
			Offset -1, -1
			Blend SrcAlpha OneMinusSrcAlpha

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag			
			#include "UnityCG.cginc"

			sampler2D _MainTex;
			float4 _MainTex_ST;

			struct appdata_t
			{
				float4 vertex : POSITION;
				float2 texcoord : TEXCOORD0;
				fixed4 color : COLOR;
			};

			struct v2f
			{
				float4 vertex : SV_POSITION;
				half2 texcoord : TEXCOORD0;
				fixed4 color : COLOR;
				fixed gray : TEXCOORD1;
			};

			v2f o;

			v2f vert(appdata_t v)
			{
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.texcoord = v.texcoord;
				o.color = v.color;
				o.gray = dot(v.color, fixed4(1, 0, 0, 0));
				return o;
			}

			fixed4 frag(v2f IN) : COLOR
			{
				fixed4 col = tex2D(_MainTex, IN.texcoord);

				col.rgb = lerp(col.rgb * IN.color.rgb
					, dot(col.rgb, fixed3(0.299, 0.587, 0.114) / (IN.color.g * 0.5 + 0.5) * IN.color.b)
					, step(IN.gray,0.001));
				col.a *= IN.color.a;
				return col;
			}
			ENDCG
		}
	}

		SubShader
			{
				LOD 100

				Tags
				{
					"Queue" = "Transparent"
					"IgnoreProjector" = "True"
					"RenderType" = "Transparent"
				}

				Pass
				{
					Cull Off
					Lighting Off
					ZWrite Off
					Fog { Mode Off }
					Offset -1, -1
					ColorMask RGB
					Blend SrcAlpha OneMinusSrcAlpha
					ColorMaterial AmbientAndDiffuse

					SetTexture[_MainTex]
					{
						Combine Texture * Primary
					}
				}
			}
}
