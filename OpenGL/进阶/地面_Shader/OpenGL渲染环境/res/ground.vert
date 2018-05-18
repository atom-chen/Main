attribute vec4 position;
attribute vec4 color;
attribute vec4 normal;

uniform mat4 ModeMatrix;
uniform mat4 ViewMatrix;
uniform mat4 ProjectionMatrix;

varying vec4 V_Color;

void main()
{
V_Color=color;
gl_Position=ProjectionMatrix*ViewMatrix*ModeMatrix*position;
{