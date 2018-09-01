#ifdef GL_ES
precision mediump float;
#endif

uniform sampler2D U_Texture_1;
varying vec4 V_Texcoord;
void main()
{
    gl_FragColor=texture2D(U_Texture_1,V_Texcoord.xy);
}