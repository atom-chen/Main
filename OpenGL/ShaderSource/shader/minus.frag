#ifdef GL_ES
precision mediump float;
#endif

varying vec4 V_Texcoord;
uniform sampler2D U_Texture_1;
uniform sampler2D U_Texture_2;

void main()
{
	vec4 baseColor=texture2D(U_Texture_1,V_Texcoord.xy);
	vec4 blendColor=texture2D(U_Texture_2,V_Texcoord.xy);
	if(min(baseColor,blendColor) == baseColor)
	{
		gl_FragColor=vec4(baseColor.r-blendColor.r,baseColor.g-blendColor.g,baseColor.b-blendColor.b,1.0);
	}
	else
	{
		gl_FragColor=vec4(blendColor.r-baseColor.r,blendColor.g-baseColor.g,blendColor.b-baseColor.b,1.0);		
	}
}

