#ifdef GL_ES
precision mediump float;
#endif
varying vec4 V_Color;
varying vec3 V_Normal;
varying vec3 V_WorldPos;
vec4 GetPointLight()
{
	float distance=0.0;
	float constantFactor=1.0;
	float linearFactor=0.0;
	float quadricFactor=0.0;
	vec4 ambientColor=vec4(1.0,1.0,1.0,1.0)*vec4(0.1,0.1,0.1,1.0);
	vec3 L=vec3(0.0,1.0,0.0)-V_WorldPos;
	distance=length(L);
	float attenuation=1.0/(constantFactor+linearFactor*distance+quadricFactor*quadricFactor*distance);
	L=normalize(L);
	vec3 n=normalize(V_Normal);
	float diffuseIntensity=max(0.0,dot(L,n));
	vec4 diffuseColor=vec4(1.0,1.0,1.0,1.0)*vec4(0.1,0.4,0.6,1.0)*diffuseIntensity*attenuation;
	return ambientColor+diffuseColor;
}
void main()
{
	gl_FragColor=V_Color*GetPointLight();
}