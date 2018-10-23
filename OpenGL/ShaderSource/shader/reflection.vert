#ifdef GL_ES
precision mediump float;
#endif

attribute vec4 position;
attribute vec4 normal;          //做环境映射需要法线

uniform mat4 ModelMatrix;
uniform mat4 ViewMatrix;
uniform mat4 ProjectionMatrix;
uniform mat4 IT_ModelMatrix;

varying vec4 V_WorldPos;
varying vec4 V_Normal;

void main()
{
	V_WorldPos = ModelMatrix*position;
	V_Normal = normal * IT_ModelMatrix;
    gl_Position=ProjectionMatrix*ViewMatrix*ModelMatrix*position;
}