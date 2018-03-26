attribute vec4 position;
uniform mat4 ModeMatrix;
uniform mat4 ViewMatrix;
uniform mat4 ProjectionMatrixl;
void main()
{
gl_Position=ProjectionMatrixl*ViewMatrix*ModeMatrix*position;
}