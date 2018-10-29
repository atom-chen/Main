#ifdef GL_ES
precision mediump float;
#endif

varying vec4 V_Texcoord;

uniform  sampler2D U_Texture_1;
void main()
{
	vec4 color = vec4(0.0);
	int coreSize = 3;
	float texelOffset = 1/200.0;
	float kernal[9];
	kernal[6]=1;
	kernal[7]=2;
	kernal[8]=1;
	kernal[3]=2;
	kernal[4]=4;
	kernal[5]=2;
	kernal[0]=1;
	kernal[1]=2;
	kernal[2]=1;
	int idx = 0;
	//遍历周围的9个点
	for(int y = 0;y<coreSize;y++)
	{
		for(int x = 0;x<coreSize;x++)
		{
			vec4 currentColor = texture2D(U_Texture_1,V_Texcoord.xy+vec2(float(-1+x)*texelOffset,float(-1+y)*texelOffset));
			color+=currentColor*kernal[idx++];
		}
	}
	color/=16.0;
	gl_FragColor = color;
}