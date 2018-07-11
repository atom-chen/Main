#ifdef GL_ES
precision mediump float;
#endif

uniform sampler2D U_Texture_1;
varying vec4 V_Texcoord;
void main()
{
vec4 color=texture2D(U_Texture_1,V_Texcoord.xy);
float gray=(color.r+color.g+color.b)/3.0;
gl_FragColor=vec4(gray,gray,gray,1.0);
}