// Unity built-in shader source. Copyright (c) 2016 Unity Technologies. MIT license (see license.txt)

Shader "Sprites/Default"
{
	Properties
	{
		 _MainTex("Sprite Texture", 2D) = "white" {}
	_Color("Tint", Color) = (1,1,1,1)
		[MaterialToggle] PixelSnap("Pixel snap", Float) = 0
		[HideInInspector] _RendererColor("RendererColor", Color) = (1,1,1,1)
		[HideInInspector] _Flip("Flip", Vector) = (1,1,1,1)
		[PerRendererData] _AlphaTex("External Alpha", 2D) = "white" {}
	[PerRendererData] _EnableExternalAlpha("Enable External Alpha", Float) = 0
		_TargetTex("TargetTex", 2D) = "white" {}
		_SceneSize("SceneSize", Vector) = (1920, 1080, 1, 1)
	}

		SubShader
	{
		/*Tags
	{
		"Queue" = "Transparent"
		"IgnoreProjector" = "True"
		"RenderType" = "Transparent"
		"PreviewType" = "Plane"
		"CanUseSpriteAtlas" = "True"
	}*/
			Tags{"RenderType" = "Opaque"}

		Cull Back
		Lighting Off
		ZWrite Off
		//Blend One OneMinusSrcAlpha
		//Blend One Zero
		BlendOp Max

		Pass
	{
		CGPROGRAM
#pragma vertex SpriteVert
#pragma fragment SpriteFrag2
#pragma target 2.0
#pragma multi_compile_instancing
#pragma multi_compile _ PIXELSNAP_ON
#pragma multi_compile _ ETC1_EXTERNAL_ALPHA
#include "UnitySprites.cginc"

		sampler2D _TargetTex;
		vector _SceneSize;
		 
		fixed4 SpriteFrag2(v2f IN) : SV_Target
		{
			/*fixed4 c = SampleSpriteTexture(IN.texcoord);
			fixed4 targetColor = tex2D(_TargetTex, IN.texcoord);
			c.r *= c.a;
			c.r = max(c.r, targetColor.r);
			return c;*/
			//return fixed4(c.rgb, 1);
			//return fixed4(1, 0, 0, 1);
		fixed4 c = SampleSpriteTexture(IN.texcoord) * IN.color;
		fixed2 targetuv = fixed2(IN.vertex.x / _SceneSize.x, IN.vertex.y / _SceneSize.y);
		fixed4 targetC = tex2D(_TargetTex, targetuv);
	c.rgb *= c.a;
	c.g = 0;
	c.b = 0;
	c.r = c.a;
	//c.r = max(c.r, targetC.r);
		//c = fixed4(targetuv, 0, 1);
	//c = targetC;
	return c;
		}
		ENDCG
	}
	}
}
