#ifdef GL_ES
precision mediump float;
#endif

uniform sampler2D U_Texture_1;
uniform sampler2D U_Texture_2;
varying vec4 V_Color;
varying vec4 V_Texcoord;
void main()
{
    gl_FragColor=V_Color*texture2D(U_Texture_1,V_Texcoord.xy)*texture2D(U_Texture_2,V_Texcoord.xy);
}