#ifdef GL_ES
precision mediump float
#endif

uniform vec4 U_LightPos;
uniform vec4 U_LightAmbient;
uniform vec4 U_AmbientMaterial;
uniform vec4 U_LightDiffuse;
uniform vec4 U_DiffuseMaterial;
uniform vec4 U_LightSpecular;
uniform vec4 U_SpecularMaterial;
uniform vec4 U_CameraPos;
uniform vec4 U_LightOpt;
uniform sampler2D U_Texture_1;

varying vec4 V_Color;
varying vec4 V_Normal;
varying vec4 V_WorldPos;
varying vec2 V_Texcoord;

vec4 GetPointLight()
{
	float constant=1.0;
	float linear=0.0;
	float quadric=0.0;

	vec4 ambientColor=U_LightAmbient*U_AmbientMaterial;

	vec3 L=vec3(0.0,1.0,0.0)-V_WorldPos.xyz;
	float distance=length(L);
	float attenuation=1.0/(constant+linear*distance+distance*quadric*quadric);
	L=normalize(L);
	vec3 N=normalize(V_Normal.xyz);
	float diffuseIntensity=max(0.0,dot(L,N));
	vec4 diffuseColor=vec4(1.0,1.0,1.0,1.0)*vec4(0.1,0.4,0.6,1.0)*diffuseIntensity*attenuation*2.0;
	return ambientColor+diffuseColor;

}

void main()
{
	vec4 color=vec4(0.0,0.0,0.0,0.0);
	vec4 ambientColor=U_LightAmbient*U_AmbientMaterial;

	vec3 L=U_LightPos.xyz;
	L=normalize(L);
	vec3 N=normalize(V_Normal.xyz);
	float diffuseIntensity=max(0.0,dot(L,N));
	vec4 diffuseColor=U_LightDiffuse*U_DiffuseMaterial*diffuseIntensity;

	vec4 specularColor=vec4(0.0,0.0,0.0,0.0);
	if(diffuseIntensity!=0.0)
	{
		vec3 reflectDirection=normalize(reflect(-L,N));
		vec3 viewDirection=normalize(U_CameraPos.xyz-V_WorldPos.xyz);
		specularColor=U_LightSpecular*U_SpecularMaterial*pow(max(0.0,dot(viewDirection,reflectDirection)),U_LightOpt.x);
	}

	if(U_LightOpt.w==1.0)
	{
		color=(ambientColor+diffuseColor+specularColor)*texture2D(U_Texture_1,V_Texcoord.xy);
	}
	else
	{
		color=(ambientColor+diffuseColor+GetPointLight())*texture2D(U_Texture_1,V_Texcoord.xy);
	}


	gl_FragColor=color;
}