#ifdef GL_ES
precision mediump float;
#endif
uniform sampler2D U_Texture;
varying vec4 V_Color;
void main()
{
	gl_FragColor=texture2D(U_Texture,gl_PointCoord.xy)*V_Color;
}