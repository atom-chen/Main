#ifdef GL_ES
precision mediump float;
#endif


void main()
{
	gl_FragData[0]=vec4(0.0,0.0,0.0,1.0);
	gl_FragData[1]=vec4(1.0,1.0,1.0,1.0);	
}