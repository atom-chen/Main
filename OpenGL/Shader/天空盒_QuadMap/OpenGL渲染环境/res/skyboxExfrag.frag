#ifdef GL_ES
precision mediump float;
#endif

uniform samplerCube U_Texture_1;
varying vec3 V_Texcoord;
void main()
{
gl_FragColor=textureCube(U_Texture_1,normalize(V_Texcoord.xyz));
}