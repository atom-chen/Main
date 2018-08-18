#ifdef GL_ES
#ifdef SIMPLE
precision mediump float;
#else
precision highp float;
#endif
#else

#define highp
#define mediump
#define lowp
#endif

//#define FOAM
#define SIMPLE  //是否使用simple版本

#ifndef SIMPLE
//#ifndef MEDIUM
#define LIGHTMAP
//#endif // MEDIUM
#define REFLECTION
#endif // SIMPLE 

#ifdef FOAM
#ifndef SIMPLE
#define USE_FOAM
#endif // SIMPLE  
#endif // FOAM  
/////////////////////////////////////////////////attribute begin////////////////////////////////////////////////////
attribute vec4 position;
attribute vec2 texcoord;
attribute vec4 color;//用作海水信息值 r:form,用于浪花，值越大浪花越明显   g:wave，产生海浪效果，值越大波动越明显   b:wind，产生风的效果，值越大越明显   a:depth,值越大水越浅

// r = foam
// g = wave
// b = wind
// a = depth
/////////////////////////////////////////////////out end////////////////////////////////////////////////////





/////////////////////////////////////////////////varying begin////////////////////////////////////////////////////
varying vec4 v_wave;
varying highp vec2 v_bumpUv1;

#ifdef USE_FOAM
varying highp vec2 v_foamUv;
varying float v_foamPower;
#endif

varying vec3 v_darkColor;
varying vec3 v_lightColor;
varying float v_reflectionPower;

#ifdef LIGHTMAP
varying highp vec2 v_worldPos;
#endif
/////////////////////////////////////////////////varying end////////////////////////////////////////////////////


/////////////////////////////////////////////////uniform begin////////////////////////////////////////////////////
uniform mat4 ModelMatrix;
uniform mat4 ViewMatrix;
uniform mat4 ProjectionMatrix;

uniform highp float u_time;//随每一帧递增来控制海水波动快慢
//控制海水的远近
uniform mediump float u_1DivLevelWidth;
uniform mediump float u_1DivLevelHeight;
uniform mediump float WAVE_HEIGHT;  //浪高，配合wave值产生层层海浪的效果
uniform mediump float ;  //浪的移动速度，与wind配合产生风吹海浪的效果

uniform mediump vec3 SHORE_DARK;  //岸的暗rgb
uniform mediump vec3 SHORE_LIGHT;  //暗的明rgb
uniform mediump vec3 SEA_DARK;  //水的暗rgb
uniform mediump vec3 SEA_LIGHT;  //水的明rgb

uniform mediump vec3 u_lightPos;  //光源位置
/////////////////////////////////////////////////uniform end////////////////////////////////////////////////////
void main()
{
    vec4 pos = position;

    // Calculate new vertex position with wave
    float animTime = texcoord.y + u_time;
    highp float wave = cos(animTime);
    float waveHeightFactor = (wave + 1.0) * 0.5; // 0,1

    pos.y += WAVE_MOVEMENT * waveHeightFactor * color.g * color.b;

    pos.z += wave * WAVE_HEIGHT * a_color.b;
    gl_Position = ProjectionMatrix*ViewMatrix*ModelMatrix*pos;

    // Water alpha
    float maxValue = 0.55;//0.5;
    v_wave.x = 1.0 - (color.a - maxValue) * (1.0 / maxValue);
    v_wave.x = v_wave.x * v_wave.x;
    v_wave.x = v_wave.x * 0.8 + 0.2;
    v_wave.x -= wave * color.b * 0.1;
    v_wave.x = min(1.0, v_wave.x);

    // UV coordinates
    vec2 texcoordMap = vec2(position.x * u_1DivLevelWidth, position.y * u_1DivLevelHeight) * 4.0;
    v_bumpUv1.xy =  texcoordMap + vec2(0.0, u_time * 0.005) * 1.5;

#ifdef USE_FOAM
    v_foamUv = (texcoordMap + vec2(u_time * 0.005)) * 5.5;

    // Calculate foam params
    float foamFactor = color.r * 2.0 + pow(color.r, 3.0);
    // Add some wave tops
    foamFactor += min(5.0, pow((1.0 - waveHeightFactor) * color.g * 3.5, 3.0)) * 0.2 * (1.0 - color.r);
    v_foamPower = foamFactor * 0.8 * 3.0;//vec4(foamFactor * 0.6, foamFactor * 0.6, foamFactor * 0.8, 0.0);//foamFactor * 0.1);

    float temppi = (color.a - maxValue) * (1.0 / maxValue);
    v_foamPower += min(1.0, min(1.0, max(0.0, -wave + 0.5) * 4.0) * temppi) * 0.5;
    v_foamPower = max(0.0, v_foamPower * color.b);

    //v_wave.z = 0.0;
#endif

    vec3 lightDir = normalize(vec3(-1.0, 1.0, 0.0));
    vec3 lightVec = normalize(u_lightPos - pos.xyz);
    v_wave.z = (1.0 - abs(dot(lightDir, lightVec)));
    v_wave.z = v_wave.z * 0.2 + (v_wave.z * v_wave.z) * 0.8;
    v_wave.z += 1.1 - (length(u_lightPos - pos.xyz) * 0.008);
    v_wave.w = (1.0 + (1.0 - v_wave.z * 0.5) * 7.0);

#ifdef LIGHTMAP
    v_worldPos = vec2(pos.x * u_1DivLevelWidth, pos.y * u_1DivLevelHeight);
#endif

    v_wave.y = (cos((position.x + u_time) * position.y * 0.003 + u_time) + 1.0) * 0.5;

    // Calculate colors
    float blendFactor = 1.0 - min(1.0, a_color.a * 1.6);

    float tx = position.x * u_1DivLevelWidth - 0.5;
    float ty = position.y * u_1DivLevelHeight - 0.5;

    // Here is the code that is commented out below done without a branch and is 2 cycles faster
    float tmp = (tx * tx + ty * ty) / (0.75 * 0.75);
    float blendFactorMul = step(1.0, tmp);
    tmp = pow(tmp, 3.0);
    // Can't be above 1.0, so no clamp needed
    float blendFactor2 = max(blendFactor - (1.0 - tmp) * 0.5, 0.0);
    blendFactor = mix(blendFactor2, blendFactor, blendFactorMul);

//  if ((tx * tx + ty * ty) < (0.75 * 0.75)) {
//      float tmp = pow(((tx * tx + ty * ty) / (0.75 * 0.75)), 3.0);
//      blendFactor = clamp(blendFactor - (1.0 - tmp) * 0.5, 0.0, 1.0);
//  }

    v_darkColor = mix(SHORE_DARK, SEA_DARK, blendFactor);
    v_lightColor = mix(SHORE_LIGHT, SEA_LIGHT, blendFactor);

    v_reflectionPower = ((1.0 - color.a) + blendFactor) * 0.5;//blendFactor;

    // Put to log2 here because there's pow(x,y)*z in the fragment shader calculated as exp2(log2(x) * y + log2(z)), where this is is the log2(z)
    v_reflectionPower = log2(v_reflectionPower);
}