#ifdef GL_ES
precision mediump float;
#endif

uniform sampler2D U_Texture_1;
varying vec4 V_Texcoord;
varying vec4 V_Color;
void main()
{
	vec4 color=texture2D(U_Texture_1,V_Texcoord.xy)*V_Color;
    float gray = (color.r+color.g+color.b)/3.0;	
    color = vec4(gray,gray,gray,1.0);
    gl_FragColor = color;
}