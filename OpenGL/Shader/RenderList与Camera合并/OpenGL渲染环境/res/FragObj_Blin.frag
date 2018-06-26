#ifdef GL_ES
precision mediump float
#endif
uniform vec4 U_AmbientMaterial;
uniform vec4 U_DiffuseMaterial;
uniform vec4 U_SpecularMaterial;

uniform vec4 U_Light1_Pos;
uniform vec4 U_Light1_Dir;
uniform vec4 U_Light1_Ambient;
uniform vec4 U_Light1_Diffuse;
uniform vec4 U_Light1_Specular;
uniform vec4 U_Light1_Opt;
uniform float U_Light1_Constant;
uniform float U_Light1_Linear;
uniform float U_Light1_Quadric;
uniform float U_Light1_CutOff;
uniform float U_Light1_Exponent;

uniform vec4 U_CameraPos;
uniform sampler2D U_Texture_1;

varying vec4 V_Color;
varying vec4 V_Normal;
varying vec4 V_WorldPos;
varying vec2 V_Texcoord;

//��ȡ���ⷽ���Ĺ�ǿ
vec4 GetDirectionLightColor(vec4 lightAmbient,vec4 lightDiffuse,vec4 lightSpecular,vec4 lightDir,vec4 lightOpt)
{
vec4 color=vec4(1.0,1.0,1.0,1.0);
vec4 ambientColor=lightAmbient*U_AmbientMaterial;

vec3 L=normalize(lightDir.xyz);
vec3 N=normalize(V_Normal.xyz);
float diffuseIntensity=max(0.0,dot(L,N));
vec4 diffuseColor=lightDiffuse*U_DiffuseMaterial*diffuseIntensity;

vec4 specularColor=vec4(0.0,0.0,0.0,1.0);
if(diffuseIntensity!=0.0)
{
vec3 viewDirection=normalize(U_CameraPos.xyz-V_WorldPos.xyz);
vec3 halfDir=normalize(viewDirection+L);
specularColor=lightSpecular*U_SpecularMaterial*pow(max(0.0,dot(L,halfDir)),lightOpt.w);
}
color=ambientColor+diffuseColor+specularColor;
return color;
}

vec4 GetPointLightColor(vec4 lightAmbient,vec4 lightDiffuse,vec4 lightSpecular,vec4 lightPos,vec4 lightOpt,float constant,float linear,float quadric)
{
vec4 ambientColor=lightAmbient*U_AmbientMaterial;

vec3 L=lightPos.xyz-V_WorldPos.xyz;
float distance=length(L);
float attenuation=1.0/(constant+linear*distance+distance*quadric*quadric);//˥��ϵ��
L=normalize(L);
vec3 N=normalize(V_Normal.xyz);
float diffuseIntensity=max(0.0,dot(L,N));
vec4 diffuseColor=lightDiffuse*U_DiffuseMaterial*diffuseIntensity*attenuation*2.0;

vec4 specularColor=vec4(0.0,0.0,0.0,0.0);
if(diffuseIntensity!=0.0)
{
vec3 viewDirection=normalize(U_CameraPos.xyz-V_WorldPos.xyz);
vec3 halfDir=normalize(viewDirection+L);
specularColor=lightSpecular*U_SpecularMaterial*pow(max(0.0,dot(L,halfDir)),lightOpt.w);
}

vec4 color=ambientColor+diffuseColor+specularColor;
return color;
}
//��ȡ����۹����ɫ
vec4 GetSpotLightColor(vec4 lightAmbient,vec4 lightDiffuse,vec4 lightSpecular,vec4 lightPos,vec4 lightDir,vec4 lightOpt,float constant,float linear,float quadric,float exponent,float cutOff)
{
vec4 color=vec4(1.0,1.0,1.0,1.0);

vec3 L=lightPos.xyz-V_WorldPos.xyz;//��ָ���˵�����
float distance=length(L);
float attenuation=1.0/(constant+linear*distance+distance*quadric*quadric);//˥��ϵ��
L=normalize(L);
vec3 N=normalize(V_Normal.xyz);
float diffuseIntensity=max(0.0,dot(L,N));//�����Ƕȵ�����
//����˥��
if(diffuseIntensity>0.0)
{
float cos_ObjLight=max(0.0,dot(-L,normalize(lightDir.xyz)));//�˺͹�ļн�����
float cos_CutOff=cos(cutOff*3.14/180.0);//���ɼ��������
float cos_Exponent=cos(exponent*3.14/180.0);//��˥���������

//����ڲ�����
if(cos_ObjLight>cos_Exponent)
{
vec4 diffuseColor=lightDiffuse*U_DiffuseMaterial*attenuation;

vec3 reflectDirection=normalize(reflect(-L,N));
vec3 viewDirection=normalize(U_CameraPos.xyz-V_WorldPos.xyz);
vec4 specularColor=lightSpecular*U_SpecularMaterial*pow(max(0.0,dot(viewDirection,reflectDirection)),lightOpt.w);

color=lightAmbient*U_AmbientMaterial+diffuseColor+specularColor;
}
//����Ƕ�С��˥����Χ��˵����˥����
else if(cos_ObjLight>cos_CutOff)
{
diffuseIntensity=pow(cos_ObjLight,U_Light1_Opt.w);
vec4 diffuseColor=lightDiffuse*U_DiffuseMaterial*attenuation*diffuseIntensity*2.0;

vec3 reflectDirection=normalize(reflect(-L,N));
vec3 viewDirection=normalize(U_CameraPos.xyz-V_WorldPos.xyz);
vec3 halfDir=normalize(viewDirection+L);
vec4 specularColor=lightSpecular*U_SpecularMaterial*pow(max(0.0,dot(L,halfDir)),lightOpt.w);

color=lightAmbient*U_AmbientMaterial+diffuseColor+specularColor;
}
//������ɼ�������color���κδ���
}
return color;
}

//��Դ1
vec4 GetLight1Color()
{
vec4 color=vec4(1.0,1.0,1.0,1.0);
//direction
if(U_Light1_Opt.x==1.0)
{
color=GetDirectionLightColor(U_Light1_Ambient,U_Light1_Diffuse,U_Light1_Specular,U_Light1_Dir,U_Light1_Opt);
}

//point
else if(U_Light1_Opt.x==2.0)
{
color=GetPointLightColor(U_Light1_Ambient,U_Light1_Diffuse,U_Light1_Specular,U_Light1_Pos,U_Light1_Opt,U_Light1_Constant,U_Light1_Linear,U_Light1_Quadric);
}

//spot
else if(U_Light1_Opt.x==3.0)
{
color=GetSpotLightColor(U_Light1_Ambient,U_Light1_Diffuse,U_Light1_Specular,U_Light1_Pos,U_Light1_Dir,U_Light1_Opt,U_Light1_Constant,U_Light1_Linear,U_Light1_Quadric,U_Light1_Exponent,U_Light1_CutOff);
}
return color;
}

//��ͼ1
vec4 GetTexture1()
{
vec4 color=vec4(1.0,1.0,1.0,1.0);
vec4 textureColor=texture2D(U_Texture_1,V_Texcoord.xy);
if(textureColor!=vec4(0.0,0.0,0.0,1.0))
{
color=textureColor;
}
return color;
}

void main()
{
vec4 color=GetTexture1()*GetLight1Color();
gl_FragColor=color;
}
