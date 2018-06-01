attribute vec4 position;
attribute vec4 color;

uniform mat4 ModelMatrix;
uniform mat4 ViewMatrix;
uniform mat4 ProjectionMatrix;
uniform float U_PointSize;

varying vec4 V_Color;

void main()
{
	V_Color=color;
	gl_PointSize=50.0;
	gl_Position=ProjectionMatrix*ViewMatrix*ModelMatrix*position;
}