attribute vec4 position;
attribute vec4 color;
attribute vec4 normal;
uniform mat4 ModelMatrix;
uniform mat4 ViewMatrix;
uniform mat4 ProjectionMatrix;
varying vec4 V_Color;
varying vec3 V_Normal;
varying vec3 V_WorldPos;
void main()
{
	V_Color=color;
	V_Normal=normal.xyz;
	V_WorldPos=(ModelMatrix*position).xyz;
	gl_Position=ProjectionMatrix*ViewMatrix*ModelMatrix*position;
}