#ifdef GL_ES
precision mediump float;
#endif



varying vec4 V_WorldPos;
varying vec4 V_Normal;

uniform vec4 U_CameraPos;        //根据视点和法线 计算目标位置
uniform samplerCube U_Texture_CubeMap;

void main()
{
	vec3 eyeDir = normalize(V_WorldPos.xyz - U_CameraPos.xyz);
	vec3 nNormal = normalize(V_WorldPos.xyz);
	vec3 r = refract(eyeDir,nNormal,1.0/1.52);          //折射系数
	vec4 rColor = textureCube(U_Texture_CubeMap,r);
	gl_FragColor = rColor;
}