#ifdef GL_ES
precision mediump float;
#endif

varying vec4 V_Texcoord;

uniform  sampler2D U_Texture_1;
void main()
{
	int coreSize = 5;
	float texelOffset = 1/200.0;
	float weight[5] = float[](0.22,0.19,0.12,0.08,0.01);
	vec4 color = texture2D(U_Texture_1,V_Texcoord.xy)*weight[0];
	//遍历水平方向周围的5个点
	for(int i = 1;i<coreSize;i++)
	{
		color+=texture2D(U_Texture_1,vec2(V_Texcoord.x+texelOffset*float(i),V_Texcoord.y))*weight[i];        //在水平方向做采样
		color+=texture2D(U_Texture_1,vec2(V_Texcoord.x-texelOffset*float(i),V_Texcoord.y))*weight[i];		
	}
	gl_FragColor = color;
}