#ifdef GL_ES
precision mediump float
#endif

uniform vec4 U_AmbientMaterial;
uniform vec4 U_DiffuseMaterial;
uniform vec4 U_SpecularMaterial;

uniform vec4 U_Light1_Pos;
uniform vec4 U_Light1_Ambient;
uniform vec4 U_Light1_Diffuse;
uniform vec4 U_Light1_Specular;
uniform vec4 U_Light1_Opt;
uniform float U_Light1_Constant;
uniform float U_Light1_Linear;
uniform float U_Light1_Quadric;


uniform vec4 U_CameraPos;
uniform sampler2D U_Texture_1;

varying vec4 V_Color;
varying vec4 V_Normal;
varying vec4 V_WorldPos;
varying vec2 V_Texcoord;

//π‚‘¥1
vec4 GetLight1Color()
{
vec4 color=vec4(1.0,1.0,1.0,1.0);
//direction
if(U_Light1_Opt.x==1.0)
{
vec4 ambientColor=U_Light1_Ambient*U_AmbientMaterial;

vec3 L=U_Light1_Pos.xyz;
L=normalize(L);
vec3 N=normalize(V_Normal.xyz);
float diffuseIntensity=max(0.0,dot(L,N));
vec4 diffuseColor=U_Light1_Diffuse*U_DiffuseMaterial*diffuseIntensity;

vec4 specularColor=vec4(0.0,0.0,0.0,1.0);
if(diffuseIntensity!=0.0)
{
vec3 reflectDirection=normalize(reflect(-L,N));
vec3 viewDirection=normalize(U_CameraPos.xyz-V_WorldPos.xyz);
specularColor=U_Light1_Specular*U_SpecularMaterial*pow(max(0.0,dot(viewDirection,reflectDirection)),U_Light1_Opt.w);
}
color=ambientColor+diffuseColor+specularColor;
}

//point
else if(U_Light1_Opt.x==2.0)
{
vec4 ambientColor=U_Light1_Ambient*U_AmbientMaterial;

vec3 L=U_Light1_Pos.xyz-V_WorldPos.xyz;
float distance=length(L);
float attenuation=1.0/(U_Light1_Constant+U_Light1_Linear*distance+distance*U_Light1_Quadric*U_Light1_Quadric);
L=normalize(L);
vec3 N=normalize(V_Normal.xyz);
float diffuseIntensity=max(0.0,dot(L,N));
vec4 diffuseColor=U_Light1_Diffuse*U_DiffuseMaterial*diffuseIntensity*attenuation*2.0;
color=ambientColor+diffuseColor;
}
return color;
}
//Ã˘Õº1
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
//gl_FragData[0]=vec4((light1Color*color).xyz,1.0);
//gl_FragData[1]=vec4((light1Color*color).xyz,1.0);
}