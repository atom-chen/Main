
Shader "KP/MaskDissolution_Blend" {
    Properties {
        _MainTex ("Diffuse Texture", 2D) = "white" {}
        _RT ("Mask Texture", 2D) = "black" {}
		_Speed ("Speed", Float ) = 1 
		_DissolveTex("DissolveTexture",2D) = "white" {}
		_DissolveVaule ("DissolveVaule", Float ) = 50
		_DissolveWidth("DissolveWidth",Range(0.01,1)) = 1
		[HDR]_ColorBeg("ColorBegin",Color) = (0,0,0,0)
		[HDR]_ColorEnd("ColorEnd",Color) = (0,0,0,0)
        [HideInInspector]_Cutoff ("Alpha cutoff", Range(0,1)) = 0.5

    }
    SubShader {
        Tags {
            "IgnoreProjector"="True"
			"RenderType" = "Opaque"
        }
        Pass {
            Name "FORWARD"
            Tags {
                "LightMode"="ForwardBase"
            }
            //Blend SrcAlpha OneMinusSrcAlpha
            ZWrite On
            Cull Off
            Fog {Mode Off}
			AlphaToMask True

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            uniform sampler2D _RT;
			uniform float4 _RT_ST;
            uniform sampler2D _MainTex;
			uniform float4 _MainTex_ST;
			uniform sampler2D _DissolveTex;
			uniform float4 _DissolveTex_ST;
			uniform float _DissolveVaule;
			uniform float _Speed;
			uniform	half4 _ColorBeg;
			uniform	half4 _ColorEnd;
			uniform fixed _DissolveWidth;

            struct appdata_t {
                float4 vertex : POSITION;
				half4 texcoord0 : TEXCOORD0;
				fixed4 vertexColor : COLOR;
            };
            struct v2f {
                float4 pos : SV_POSITION;
				half4 uv0 : TEXCOORD0;
				fixed4 vertexColor : COLOR;
				
            };
            v2f vert (appdata_t v) {
                v2f o ;
                o.uv0 = v.texcoord0;
                o.vertexColor = v.vertexColor;
                o.pos = UnityObjectToClipPos(v.vertex );
				o.uv0.xy = TRANSFORM_TEX(v.texcoord0,_MainTex);
				o.uv0.zw = TRANSFORM_TEX((v.texcoord0+_Time.x*_Speed), _DissolveTex);
                return o;
            }
			fixed4 frag(v2f i) : COLOR {

				fixed4 tex = tex2D(_MainTex,i.uv0.xy);
				fixed4 rt = tex2D(_RT,i.uv0.xy);
				fixed4 dt = tex2D(_DissolveTex,i.uv0.zw);
				half  mask = rt.r*dt.r*_DissolveVaule.r;

				half f = 1- mask;

				clip(f - 0.001);

				half t = 1 - smoothstep(0.0001, _DissolveWidth, f);
				fixed3 col = lerp(_ColorBeg, _ColorEnd, t) * mask;
				fixed3 final = lerp(tex.rgb, tex.rgb + col, t);
				return fixed4(final, 1);
            }

            ENDCG
        }
    }
    FallBack "Diffuse"
 
}
