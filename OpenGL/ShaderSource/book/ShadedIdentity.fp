// The ShadedIdentity Shader
// Fragment Shader
// Richard S. Wright Jr.
// OpenGL SuperBible
#version 130

out vec4 vFragColor;
in vec4 vVaryingColor;   //当使用flat时，取最后一个顶点的attribute

void main(void)
{ 
   vFragColor = vVaryingColor;
}