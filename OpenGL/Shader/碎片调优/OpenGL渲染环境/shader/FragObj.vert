#ifdef GL_ES
precision mediump float;
#endif

attribute vec4 position;
attribute vec4 color;
attribute vec4 normal;
attribute vec4 texcoord;

uniform mat4 ModelMatrix;
uniform mat4 ViewMatrix;
uniform mat4 ProjectionMatrix;
uniform mat4 IT_ModelMatrix;

varying vec4 V_Color;
varying vec4 V_Normal;
varying vec4 V_WorldPos;
varying vec2 V_Texcoord;
void main()
{
    V_Color=color;
    V_Normal=IT_ModelMatrix*normal;
    V_WorldPos=ModelMatrix*position;
    V_Texcoord=texcoord.xy;
    gl_Position=ProjectionMatrix*ViewMatrix*ModelMatrix*position;
}