attribute vec4 position;
attribute vec4 color;
attribute vec4 normal;
attribute vec4 texcoord;

uniform mat4 ModelMatrix;
uniform mat4 ViewMatrix;
uniform mat4 ProjectionMatrix;

varying vec4 V_Color;
varying vec2 V_Texcoord;
varying vec3 V_WorldPos;
varying vec3 V_Normal;

void main()
{
	V_Color=color;
	V_Normal=normal.xyz;
	V_WorldPos=(ModelMatrix*position).xyz;
	gl_Position=ProjectionMatrix*ViewMatrix*ModelMatrix*position;
}