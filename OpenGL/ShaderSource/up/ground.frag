#ifdef GL_ES
precision mediump float
#endif

uniform vec4 U_LightPos;
uniform vec4 U_LightAmbient;
uniform vec4 U_AmbientMaterial;
uniform vec4 U_LightDiffuse;
uniform vec4 U_DiffuseMaterial;
float diffuseConstant;
float diffuseLinear;
float diffuseQuadric;
uniform vec4 U_LightOpt;

varying vec4 V_Color;
varying vec2 V_Texcoord;
varying vec3 V_WorldPos;
varying vec3 V_Normal;

gl_LightSourceParameters Init()
{
	gl_LightSourceParameters light;
	return light;
}

void main()
{
	vec4 color=vec4(0.0,0.0,0.0,0.0);
	vec4 ambientColor=U_LightAmbient*U_AmbientMaterial;

	float diffuseConstant=1.0;
	float diffuseLinear=0.0;
	float diffuseQuadric=0.0;
	vec3 L=normalize(U_LightPos.xyz-V_WorldPos);
	float distance=length(L);
	float attenuation=1.0/(diffuseConstant+diffuseLinear*distance+distance*distance*diffuseQuadric);
	vec3 N=normalize(V_Normal.xyz);
	float diffuseIntensity=max(0.0,dot(L,N));
	vec4 diffuseColor=U_LightDiffuse*U_DiffuseMaterial*diffuseIntensity*attenuation*4.0;

	color=ambientColor+diffuseColor;
	gl_FragColor=color*V_Color;
}