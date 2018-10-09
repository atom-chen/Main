precision mediump float;
varying vec2 vTextureCoord;
varying vec3 vLightWeighting;

uniform sampler2D _texture;

void main(void)
{
    vec4 textureColor = texture2D(_texture, vec2(vTextureCoord.s, vTextureCoord.t));
    gl_FragColor = vec4(vLightWeighting, textureColor.a);
}