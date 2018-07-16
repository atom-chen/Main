#version 120
#ifdef GL_ES
precision mediump float;
#endif

uniform sampler2D U_Texture_1;
varying vec4 V_Color;


void main()
{
    vec4 color=V_Color*texture2D(U_Texture_1,gl_PointCoord.xy);
    gl_FragData[0]=color;
    gl_FragData[1]=color;
}