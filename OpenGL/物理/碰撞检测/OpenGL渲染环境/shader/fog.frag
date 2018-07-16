#ifdef GL_ES
precision mediump float;
#endif

uniform sampler2D U_Texture_1;
uniform float U_FogFar;
uniform float U_FogNear;
uniform float U_FogMul;
uniform float U_FogPow;

varying vec4 V_Color;
varying vec4 V_EyeSpacePos;//���������������ϵ��λ��
varying vec4 V_Texcoord;


//������
float GetCalculateLinearFog(float distance)
{
    float fogAlpha=(distance-U_FogNear)/(U_FogFar-U_FogNear);//������۾�Խ����Խ����
    fogAlpha=1.0-clamp(fogAlpha,0.2,1.0);
    return fogAlpha;
}

//��������
float GetCalculateExpFog(float distance)
{
    float fogAlpha=exp(-(distance*U_FogMul));
    fogAlpha=clamp(fogAlpha,0.2,1.0);
    return fogAlpha;
}

//��������Ex
float GetCalculateExpExFog(float distance)
{
    float fogAlpha=exp(-pow(distance*U_FogMul,U_FogPow));
    fogAlpha=clamp(fogAlpha,0.2,1.0);
    return fogAlpha;
}

//��ͼ1
vec4 GetTexture1()
{
    vec4 color=vec4(1.0,1.0,1.0,1.0);
    vec4 textureColor=texture2D(U_Texture_1,V_Texcoord.xy);
    if(textureColor.xyz!=vec3(0.0,0.0,0.0))
        {
            color=textureColor;
        }
    return color;
}

void main()
{
    float fogAlpha=GetCalculateExpExFog(abs(V_EyeSpacePos.z/V_EyeSpacePos.w));
    vec4 color=V_Color*GetTexture1();
    gl_FragColor=mix(vec4(0.7,0.7,0.7,1.0),color,fogAlpha);
}