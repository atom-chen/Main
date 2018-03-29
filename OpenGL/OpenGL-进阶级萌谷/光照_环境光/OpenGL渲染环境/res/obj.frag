#ifdef GL_ES
precision mediump float
#endif

uniform vec4 U_LightAmbient;
uniform vec4 U_AmbientMatrial;


varying vec4 V_Color;

void main()
{
vec4 color=vec4(0,0,0,0);
vec4 ambientColor=U_LightAmbient*U_AmbientMatrial;
color=ambientColor;
gl_FragColor=ambientColor;
}