#ifdef GL_ES
precision mediump float;
#endif

varying vec4 V_Texcoord;
uniform sampler2D U_Texture_1;

//平滑效果：当前像素点根据当前像素点采样
void main()
{
    vec4 color = vec4(1.0);
    int coreSize = 3;
    float texelOffset = 1/300.0;
    float kernal[9];
    kernal[0] = 1;
    kernal[1] = 1;
    kernal[2] = 1;
    kernal[3] = 1;
    kernal[4] = 1;
    kernal[5] = 1;
    kernal[6] = 1;
    kernal[7] = 1;
    kernal[8] = 1;
	int idx = 0;
	for(int y = 0;y<coreSize;y++)
	{
		for(int x = 0;x<coreSize;x++)
		{
			vec4 currentColor = texture2D(U_Texture_1,V_Texcoord.xy + vec2(float(x-1) * texelOffset,
			                    float(-1+y)*texelOffset));
			color += currentColor*kernal[idx++];
		}
	}
	color /= 9.0;
	gl_FragColor = color;
}