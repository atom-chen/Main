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
	gl_FragColor=baseColor+blendColor;
}