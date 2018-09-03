precision  lowp float;
uniform    sampler2D   _texture;
varying    vec4        _outColor;
varying    vec2        _outUV;
void main()
{
    vec4   color   =   texture2D(_texture,_outUV);
    gl_FragColor   =   color * _outColor;
}