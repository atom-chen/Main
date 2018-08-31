// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Unlit/Transparent Colored Outline"
{
	Properties
	{
		_MainTex("Base (RGB), Alpha (A)", 2D) = "black" {}
		_BlurWidth("Blur Width", Float) = 0.5
		_OutlineColor("Outline Color", Color) = (0,0,0,1)
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
			Blend SrcAlpha OneMinusSrcAlpha

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag			
			#include "UnityCG.cginc"

			sampler2D _MainTex;
			float4 _MainTex_ST;
			fixed4 _OutlineColor;
			uniform half _BlurWidth;

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
			};

			v2f o;

			v2f vert(appdata_t v)
			{
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.texcoord = v.texcoord;
				o.color = v.color;
				return o;
			}

			fixed4 frag(v2f IN) : COLOR
			{
				fixed4 col = tex2D(_MainTex, IN.texcoord);

			//additional samples towards center of screen
			half4 sum = half4(0,0,0,0);
			sum += tex2D(_MainTex, IN.texcoord + fixed2(0.01, 0.00) * _BlurWidth);
			sum += tex2D(_MainTex, IN.texcoord + fixed2(0.01, 0.01) * _BlurWidth);
			sum += tex2D(_MainTex, IN.texcoord + fixed2(-0.01, 0.00) * _BlurWidth);
			sum += tex2D(_MainTex, IN.texcoord + fixed2(-0.01, -0.01) * _BlurWidth);
			sum += tex2D(_MainTex, IN.texcoord + fixed2(0.00, 0.01) * _BlurWidth);
			sum += tex2D(_MainTex, IN.texcoord + fixed2(0.00, -0.01) * _BlurWidth);
			sum += tex2D(_MainTex, IN.texcoord + fixed2(-0.01, 0.01) * _BlurWidth);
			sum += tex2D(_MainTex, IN.texcoord + fixed2(0.01, -0.01) * _BlurWidth);

			//eleven samples...
			sum *= 1.0 / 8.0;
			sum += col;

			//sum.a = step(0.1,sum.a);

			fixed3 stepCol = lerp(_OutlineColor.rgb, IN.color.rgb, step(0.9,sum.a));
			//sum.a = step(0.1,sum.a);

			return fixed4(stepCol, IN.color.a * sum.a);
			}
			ENDCG
		}
	}
}
