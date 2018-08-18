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

varying vec4 V_Color;
varying vec4 V_Texcoord;

//获取任意方向光的光强
vec4 GetDirectionLightColor(vec4 lightAmbient,vec4 lightDiffuse,vec4 lightSpecular,vec4 lightDir,vec4 lightOpt)
{
    vec4 color=vec4(1.0,1.0,1.0,1.0);
    vec4 ambientColor=lightAmbient*U_AmbientMaterial;

    vec3 L=normalize((IT_ModelMatrix*lightDir).xyz);
    vec3 N=normalize(normal.xyz);
    float diffuseIntensity=max(0.0,dot(L,N));
    vec4 diffuseColor=lightDiffuse*U_DiffuseMaterial*diffuseIntensity;

    vec4 specularColor=vec4(0.0,0.0,0.0,1.0);
    if(diffuseIntensity!=0.0)
        {
            vec3 worldPos=(ModelMatrix*position).xyz;
            vec3 viewDirection=normalize(U_CameraPos.xyz-worldPos.xyz);//视线方向
            vec3 halfDir=normalize(L+viewDirection);//用view和入射光线的半角模拟反射方向
            specularColor=U_Light1_Specular*U_SpecularMaterial*pow(max(0.0,dot(N,halfDir)),lightOpt.w);
        }
    color=ambientColor+diffuseColor+specularColor;
    return color;
}

//获取任意点光源颜色
vec4 GetPointLightColor(vec4 lightAmbient,vec4 lightDiffuse,vec4 lightSpecular,vec4 lightPos,vec4 lightOpt,float constant,float linear,float quadric)
{
    vec4 ambientColor=lightAmbient*U_AmbientMaterial;

    vec3 L=(IT_ModelMatrix*lightPos).xyz;
    float distance=length(L);
    L=normalize(L);
    vec3 N=normalize(normal.xyz);

    float attenuation=1.0/(constant+linear*distance+distance*quadric*quadric);//衰减系数
    float diffuseIntensity=max(0.0,dot(L,N));
    vec4 diffuseColor=lightDiffuse*U_DiffuseMaterial*diffuseIntensity*attenuation*2.0;

    vec4 specularColor=vec4(0.0,0.0,0.0,0.0);
    if(diffuseIntensity!=0.0)
        {
            vec3 worldPos=(ModelMatrix*position).xyz;
            vec3 viewDirection=normalize(U_CameraPos.xyz-worldPos.xyz);//视线方向
            vec3 halfDir=normalize(L+viewDirection);//用view和入射光线的半角模拟反射方向
            specularColor=U_Light1_Specular*U_SpecularMaterial*pow(max(0.0,dot(N,halfDir)),lightOpt.w);
        }
    vec4 color=ambientColor+diffuseColor+specularColor;
    return color;
}

//获取任意聚光灯颜色
vec4 GetSpotLightColor(vec4 lightAmbient,vec4 lightDiffuse,vec4 lightSpecular,vec4 lightPos,vec4 lightDir,vec4 lightOpt,float constant,float linear,float quadric,float exponent,float cutOff)
{
    vec4 color=vec4(1.0,1.0,1.0,1.0);

    vec3 L=(IT_ModelMatrix*lightDir).xyz;
    float distance=length(L);
    float attenuation=1.0/(constant+linear*distance+distance*quadric*quadric);//衰减系数
    L=normalize(L);
    vec3 N=normalize(normal.xyz);
    float diffuseIntensity=max(0.0,dot(L,N));//反射光角度的余弦
//计算衰减
    if(diffuseIntensity>0.0)
        {
            float cos_ObjLight=max(0.0,dot(-L,normalize(lightDir.xyz)));//人和光的夹角余弦
            float cos_CutOff=cos(cutOff*3.14/180.0);//最大可见光的余弦
            float cos_Exponent=cos(exponent*3.14/180.0);//不衰减光的余弦

//如果在不变域
            if(cos_ObjLight>cos_Exponent)
                {
                    vec4 diffuseColor=lightDiffuse*U_DiffuseMaterial*attenuation;

                    vec3 worldPos=(ModelMatrix*position).xyz;
                    vec3 viewDirection=normalize(U_CameraPos.xyz-worldPos.xyz);//视线方向
                    vec3 halfDir=normalize(L+viewDirection);//用view和入射光线的半角模拟反射方向
                    vec4 specularColor=U_Light1_Specular*U_SpecularMaterial*pow(max(0.0,dot(N,halfDir)),lightOpt.w);
                    color=lightAmbient*U_AmbientMaterial+diffuseColor+specularColor;
                }
//如果角度小于衰减范围，说明在衰减域
            else if(cos_ObjLight>cos_CutOff)
                {
                    diffuseIntensity=pow(cos_ObjLight,lightOpt.w);
                    vec4 diffuseColor=lightDiffuse*U_DiffuseMaterial*attenuation*diffuseIntensity*2.0;

                    vec3 worldPos=(ModelMatrix*position).xyz;
                    vec3 viewDirection=normalize(U_CameraPos.xyz-worldPos.xyz);//视线方向
                    vec3 halfDir=normalize(L+viewDirection);//用view和入射光线的半角模拟反射方向
                    vec4 specularColor=U_Light1_Specular*U_SpecularMaterial*pow(max(0.0,dot(N,halfDir)),lightOpt.w);
                    color=lightAmbient*U_AmbientMaterial+diffuseColor+specularColor;
                }
//如果不可见，不对color做任何处理
        }
    return color;
}

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


void main()
{
    V_Texcoord=texcoord;
    V_Color=GetLight1Color();
    gl_Position=ProjectionMatrix*ViewMatrix*ModelMatrix*position;
}
