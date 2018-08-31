attribute vec4 position;
attribute vec4 color;
attribute vec4 texcoord;



varying vec4 V_Color;
varying vec4 V_Texcoord;

void main()
{
    V_Color=color;
    V_Texcoord=texcoord;
    gl_Position=position;
}