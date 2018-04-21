#ifdef GL_ES
precision mediump float;
#endif

varying vec3 V_Texcoord;

uniform samplerCube U_TextureCube;

void main()
{
	gl_FragColor=textureCube(U_TextureCube,normalize(V_Texcoord));
}