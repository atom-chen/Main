#ifdef GL_ES
precision mediump float;
#endif

uniform sampler2D U_Texture_1;

varying vec4 V_Color;
varying vec4 V_Texcoord;
//ÌùÍ¼1
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
    gl_FragColor=V_Color*GetTexture1();
}