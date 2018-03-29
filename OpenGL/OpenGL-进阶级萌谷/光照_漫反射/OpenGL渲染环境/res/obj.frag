#ifdef GL_ES
precision mediump float
#endif
uniform vec4 U_LightPos;

uniform vec4 U_LightAmbient;
uniform vec4 U_AmbientMaterial;
uniform vec4 U_LightDiffuse;
uniform vec4 U_DiffuseMaterial;

varying vec4 V_Color;
varying vec4 V_Normal;
void main()
{
vec4 color=vec4(0.0,0.0,0.0,0.0);
vec4 ambientColor=U_LightAmbient*U_AmbientMaterial;

vec3 lightPos=U_LightPos;
vec3 L=lightPos;
L=normalize(L);
vec3 N=normalize(V_Normal.xyz);
float diffuseIntensity=max(0.0,dot(L,N));
vec4 diffuseColor=U_LightDiffuse*U_DiffuseMaterial*diffuseIntensity;

color=ambientColor+diffuseColor;
gl_FragColor=color;
}