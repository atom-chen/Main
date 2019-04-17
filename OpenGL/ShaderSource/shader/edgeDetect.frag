#ifdef GL_ES
precision mediump float;
#endif

varying vec4 V_Texcoord;
uniform sampler2D U_Texture_1;

//锐化效果：突出中间像素
void main()
{
    vec4 color = vec4(1.0);
    int coreSize = 3;
    float texelOffset = 1/300.0;
    float kernal[9];
    kernal[0] = 0;
    kernal[1] = 1;
    kernal[2] = 0;
    kernal[3] = 1;
    kernal[4] = -4;
    kernal[5] = 1;
    kernal[6] = 0;
    kernal[7] = 1;
    kernal[8] = 0;
	int idx = 0;
	for(int y = 0;y<coreSize;y++)
	{
		for(int x = 0;x<coreSize;x++)
		{
			vec4 currentColor = texture2D(U_Texture_1,V_Texcoord.xy + vec2(float(x-1) * texelOffset,
			                    float(-1+y)*texelOffset));
			color += currentColor*kernal[idx++];     //当前点的像素颜色，乘上一个因子
		}
	}
	gl_FragColor = 0.5*color+texture2D(U_Texture_1,V_Texcoord.xy);
}
