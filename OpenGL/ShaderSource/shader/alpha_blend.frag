#ifdef GL_ES
precision mediump float;
#endif

varying vec4 V_Texcoord;
uniform sampler2D U_Texture_1;
uniform sampler2D U_Texture_2;

void main()
{
	gl_FragColor=texture2D(U_Texture_1,V_Texcoord.xy)*0.5+texture2D(U_Texture_2,V_Texcoord.xy)*0.5;
}