#ifdef GL_ES
precision mediump float
#endif

uniform vec4 U_AmbientMaterial;
uniform vec4 U_DiffuseMaterial;

uniform vec4 U_Light1_Pos;
uniform vec4 U_Light1_Ambient;
uniform vec4 U_Light1_Diffuse;
uniform vec4 U_Light1_Opt;
float U_Light1_Constant;
float U_Light1_Linear;
float U_Light1_Quadric;


uniform sampler2D U_Texture_1;

varying vec4 V_Color;
varying vec2 V_Texcoord;
varying vec3 V_WorldPos;
varying vec3 V_Normal;


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

color=ambientColor+diffuseColor;
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

void main()
{
vec4 lightColor=GetLight1Color();
vec4 textureColor=texture2D(U_Texture_1,V_Texcoord.xy);
gl_FragColor   =   textureColor*V_Color*lightColor;
}