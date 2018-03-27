#ifdef GL_ES
precision mediump float;
#endif
uniform sampler2D U_Texture;
uniform vec4 U_LightPos;
uniform vec4 U_LightAmbient;
uniform vec4 U_LightDiffuse;
uniform vec4 U_LightSpecular;
uniform vec4 U_AmbientMaterial;
uniform vec4 U_DiffuseMaterial;
uniform vec4 U_SpecularMaterial;
uniform vec4 U_CameraPos;
uniform vec4 U_LightOpt;
varying vec4 V_Texcoord;
varying vec4 V_WorldPos;
varying vec4 V_Normal;

vec4 GetPointLight()
{
	float distance=0.0;
	float constantFactor=1.0;
	float linearFactor=0.0;
	float quadricFactor=0.0;
	vec4 ambientColor=vec4(1.0,1.0,1.0,1.0)*vec4(0.1,0.1,0.1,1.0);
	vec3 L=vec3(0.0,1.0,0.0)-V_WorldPos.xyz;
	distance=length(L);
	float attenuation=1.0/(constantFactor+linearFactor*distance+quadricFactor*quadricFactor*distance);
	L=normalize(L);
	vec3 n=normalize(V_Normal.xyz);
	float diffuseIntensity=max(0.0,dot(L,n));
	vec4 diffuseColor=vec4(1.0,1.0,1.0,1.0)*vec4(0.1,0.4,0.6,1.0)*diffuseIntensity*attenuation;
	return ambientColor+diffuseColor;
}
void main()
{
	vec4 ambientColor=U_LightAmbient*U_AmbientMaterial;
	vec3 lightPos=U_LightPos.xyz;
	vec3 L=lightPos;
	L=normalize(L);
	vec3 n=normalize(V_Normal.xyz);
	float diffuseIntensity=max(0.0,dot(L,n));
	vec4 diffuseColor=U_LightDiffuse*U_DiffuseMaterial*diffuseIntensity;
	vec4 specularColor=vec4(0.0,0.0,0.0,0.0);
	if(diffuseIntensity!=0.0){
		vec3 reflectDir=normalize(reflect(-L,n));
		vec3 viewDir=normalize(U_CameraPos.xyz-V_WorldPos.xyz);
		specularColor=U_LightSpecular*U_SpecularMaterial*pow(max(0.0,dot(viewDir,reflectDir)),U_LightOpt.x);
	}
	if(U_LightOpt.y==1.0){
		gl_FragColor=(ambientColor+diffuseColor)*texture2D(U_Texture,V_Texcoord.xy)+specularColor;
	}else if(U_LightOpt.z==1.0){
		gl_FragColor=(ambientColor+diffuseColor+GetPointLight())*texture2D(U_Texture,V_Texcoord.xy);
	}else if(U_LightOpt.w==1.0){
		gl_FragColor=ambientColor+diffuseColor+specularColor;
	}
}