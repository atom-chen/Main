#ifdef GL_ES
precision mediump float;
#endif
attribute vec4 position;
attribute vec4 texcoord;
attribute vec4 normal;

uniform mat4 ModelMatrix;
uniform mat4 ViewMatrix;
uniform mat4 ProjectionMatrix;
uniform mat4 IT_ModelMatrix;

uniform vec4 U_AmbientMaterial;
uniform vec4 U_DiffuseMaterial;
uniform vec4 U_SpecularMaterial;

uniform vec4 U_CameraPos;

uniform vec4 U_Light1_Pos;
uniform vec4 U_Light1_Ambient;
uniform vec4 U_Light1_Diffuse;
uniform vec4 U_Light1_Specular;
uniform vec4 U_Light1_Opt;
uniform float U_Light1_Constant;
uniform float U_Light1_Linear;
uniform float U_Light1_Quadric;

varying vec4 V_Color;

vec4 GetLight1Color()
{
vec4 color=vec4(1.0,1.0,1.0,1.0);

//方向光
if(U_Light1_Opt.x==1.0)
{
vec4 ambientColor=U_AmbientMaterial*U_Light1_Ambient;

vec4 diffuseColor=vec4(0.0,0.0,0.0,0.0);
vec3 L=normalize((IT_ModelMatrix*U_Light1_Pos).xyz);
vec3 N=normalize(normal.xyz);
float diffuseIntensity=max(0.0,dot(L,N));
diffuseColor=U_Light1_Diffuse*U_DiffuseMaterial*diffuseIntensity;

vec4 specularColor=(0.0,0.0,0.0,0.0);
if(diffuseIntensity!=0.0)
{
vec3 worldPos=(ModelMatrix*position).xyz;
vec3 viewDirection=normalize(U_CameraPos.xyz-worldPos.xyz);//视线方向
vec3 halfDir=normalize(L+viewDirection);//用view和入射光线的半角模拟反射方向
specularColor=U_Light1_Specular*U_SpecularMaterial*pow(max(0.0,dot(N,halfDir)),U_Light1_Opt.w);
}

color=ambientColor+diffuseColor+specularColor;
}

//point
else if(U_Light1_Opt.x==2.0)
{

}
return color;
}

void main()
{
V_Color=GetLight1Color();
gl_Position=ProjectionMatrix*ViewMatrix*ModelMatrix*position;
}