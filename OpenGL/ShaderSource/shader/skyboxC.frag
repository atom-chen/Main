#ifdef GL_ES
precision mediump float;
#endif

varying vec3 V_Texcoord;

uniform samplerCube U_Texture_CubeMap;

void main()
{
	gl_FragColor=texture(U_Texture_CubeMap,normalize(V_Texcoord));
}