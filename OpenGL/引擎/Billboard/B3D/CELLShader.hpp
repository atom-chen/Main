#pragma once

#include <assert.h>

class    ShaderId
{
public:
    ShaderId()
    {
        _shaderId   =   -1;
    }
    int _shaderId;
};


/**
*   程序
*/
class    ProgramId
{
public:
    int         _programId;
    ShaderId    _vertex;
    ShaderId    _fragment;
public:
    ProgramId()
    {
        _programId  =   -1;
    }
public:
    /**
    *   加载函数
    */
    bool    createProgram( const char* vertex,const char* fragment )
    {
        bool        error   =   false;
        do 
        {
            if (vertex)
            {
                _vertex._shaderId   = glCreateShader( GL_VERTEX_SHADER );
                glShaderSource( _vertex._shaderId, 1, &vertex, 0 );
                glCompileShader( _vertex._shaderId );

                GLint   compileStatus;
                glGetShaderiv( _vertex._shaderId, GL_COMPILE_STATUS, &compileStatus );
                error   =   compileStatus == GL_FALSE;
                if( error )
                {
                    GLchar messages[256];
                    glGetShaderInfoLog( _vertex._shaderId, sizeof(messages), 0,messages);
                    assert( messages && 0 != 0);
                    break;
                }
            }
            if (fragment)
            {
                _fragment._shaderId   = glCreateShader( GL_FRAGMENT_SHADER );
                glShaderSource( _fragment._shaderId, 1, &fragment, 0 );
                glCompileShader( _fragment._shaderId );

                GLint   compileStatus;
                glGetShaderiv( _fragment._shaderId, GL_COMPILE_STATUS, &compileStatus );
                error   =   compileStatus == GL_FALSE;

                if( error )
                {
                    GLchar messages[256];
                    glGetShaderInfoLog( _fragment._shaderId, sizeof(messages), 0,messages);
                    assert( messages && 0 != 0);
                    break;
                }
            }
            _programId  =   glCreateProgram( );

            if (_vertex._shaderId)
            {
                glAttachShader( _programId, _vertex._shaderId);
            }
            if (_fragment._shaderId)
            {
                glAttachShader( _programId, _fragment._shaderId);
            }

            glLinkProgram( _programId );

            GLint linkStatus;
            glGetProgramiv( _programId, GL_LINK_STATUS, &linkStatus );
            if (linkStatus == GL_FALSE)
            {
                GLchar messages[256];
                glGetProgramInfoLog( _programId, sizeof(messages), 0, messages);
                break;
            }
            glUseProgram(_programId);

        } while(false);

        if (error)
        {
            if (_fragment._shaderId)
            {
                glDeleteShader(_fragment._shaderId);
                _fragment._shaderId =   0;
            }
            if (_vertex._shaderId)
            {
                glDeleteShader(_vertex._shaderId);
                _vertex._shaderId   =   0;
            }
            if (_programId)
            {
                glDeleteProgram(_programId);
                _programId          =   0;
            }
        }
        return  true;
    }

    /**
    *   使用程序
    */
    virtual void    begin()
    {
        glUseProgram(_programId);
        
    }
    /**
    *   使用完成
    */
    virtual void    end()
    {
        glUseProgram(0);
    }
};


class   PROGRAM_P3_T2_C4 :public ProgramId
{
public:
    typedef int location; 
public:
    location    _positionAttr;
    location    _colorAttr;
    location    _uvAttr;
    location    _MVP;
    location    _texture;
public:
    PROGRAM_P3_T2_C4()
    {
        _positionAttr   =   -1;
        _colorAttr      =   -1;
        _uvAttr         =   -1;
        _MVP            =   -1;
    }
    ~PROGRAM_P3_T2_C4()
    {
    }
   
    /// 初始化函数
    virtual bool    initialize()
    {
        const char* vs  =   
        {
            "precision  lowp float; "
            "uniform   mat4 _MVP;"
            "attribute vec3 _position;"
            "attribute vec4 _color;"
            "attribute vec2 _uv;"

            "varying   vec4 _outColor;"
            "varying   vec2 _outUV;"
            "void main()"
            "{"
            "   vec4    pos =   vec4(_position.x,_position.y,_position.z,1);"
            "   gl_Position =   _MVP * pos;"
            "   _outColor   =   _color;"
            "   _outUV      =   _uv;"
            "}"
        };
        const char* ps  =   
        {
            "precision  lowp float; "
            "uniform    sampler2D   _texture;\n"
            "varying    vec4        _outColor;\n"
            "varying    vec2        _outUV;"
            "void main()"
            "{"
            "   vec4   color   =   texture2D(_texture,_outUV);"
            "   if( color.a < 0.2) discard;"
            "   gl_FragColor   =   color * _outColor;"
            "}"
        };
        
        bool    res =   createProgram(vs,ps);
        if(res)
        {
            _positionAttr   =   glGetAttribLocation(_programId, "_position");
            _colorAttr      =   glGetAttribLocation(_programId, "_color");
            _uvAttr         =   glGetAttribLocation(_programId, "_uv");
            _MVP            =   glGetUniformLocation(_programId,"_MVP");
            _texture        =   glGetUniformLocation(_programId,"_texture");
        }
        return  res;
    }

    /**
    *   使用程序
    */
    virtual void    begin()
    {
        glUseProgram(_programId);
        glEnableVertexAttribArray(_positionAttr);
        glEnableVertexAttribArray(_uvAttr);
        glEnableVertexAttribArray(_colorAttr);
        
    }
    /**
    *   使用完成
    */
    virtual void    end()
    {
        glDisableVertexAttribArray(_positionAttr);
        glDisableVertexAttribArray(_uvAttr);
        glDisableVertexAttribArray(_colorAttr);
        glUseProgram(0);
    }
};


class   PROGRAM_DirLight :public ProgramId
{
public:
    typedef int uniform;
    typedef int attribute;
public:
    uniform     _mv;
    uniform     _project;
    uniform     _normalMat;
    uniform     _ambientColor;
    uniform     _lightDirection;
    uniform     _diffuseColor;
    uniform     _texture;


    attribute   _position;
    attribute   _uv;
    attribute   _normal;

public:
    PROGRAM_DirLight()
    {
    }
    ~PROGRAM_DirLight()
    {
    }
   
    /// 初始化函数
    virtual bool    initialize()
    {
        const char* vs  =   
        {
            "attribute vec3 _position;\n"
            "attribute vec3 _normal;\n"
            "attribute vec2 _uv;\n"

            "uniform mat4 _mv;\n"
            "uniform mat4 _project;\n"
            "uniform mat3 _normalMat;\n"
            "uniform vec3 _ambientColor;\n"

            "uniform vec3 _lightDirection;\n"
            "uniform vec3 _diffuseColor;\n"


            "varying vec2 vTextureCoord;\n"
            "varying vec3 vLightWeighting;\n"

            "void main(void) {\n"
                "gl_Position = _project * _mv * vec4(_position, 1.0);\n"
                "vTextureCoord = _uv;\n"
                
                "vec3 tNormal = _normalMat * _normal;\n"
                ///得到2者夹角的余弦指 截取到[0,1]
                "float NdotL = max(dot(tNormal, _lightDirection), 0.0);\n"
                "vLightWeighting = _ambientColor + _diffuseColor * NdotL;\n"
                
            "}\n"
        };
        const char* ps  =   
        {
            "precision mediump float;\n"
            "varying vec2 vTextureCoord;\n"
            "varying vec3 vLightWeighting;\n"

            "uniform sampler2D _texture;\n"

            "void main(void) {\n"
                "vec4 textureColor = texture2D(_texture, vec2(vTextureCoord.s, vTextureCoord.t));\n"
                "gl_FragColor = vec4(vLightWeighting, textureColor.a);\n"
            "}\n"
        };
        
        bool    res =   createProgram(vs,ps);
        if(res)
        {
            _position   =   glGetAttribLocation(_programId, "_position");
            _uv         =   glGetAttribLocation(_programId, "_uv");
            _normal     =   glGetAttribLocation(_programId, "_normal");

            _mv           =   glGetUniformLocation(_programId,"_mv");
            _project            =   glGetUniformLocation(_programId,"_project");
            _normalMat          =   glGetUniformLocation(_programId,"_normalMat");
            _ambientColor       =   glGetUniformLocation(_programId,"_ambientColor");
            _lightDirection     =   glGetUniformLocation(_programId,"_lightDirection");
            _diffuseColor       =   glGetUniformLocation(_programId,"_diffuseColor");
            _texture            =   glGetUniformLocation(_programId,"_texture");
        }
        return  res;
    }

    /**
    *   使用程序
    */
    virtual void    begin()
    {
        glUseProgram(_programId);
        glEnableVertexAttribArray(_position);
        glEnableVertexAttribArray(_uv);
        glEnableVertexAttribArray(_normal);
        
    }
    /**
    *   使用完成
    */
    virtual void    end()
    {
        glDisableVertexAttribArray(_position);
        glDisableVertexAttribArray(_uv);
        glDisableVertexAttribArray(_normal);
        glUseProgram(0);
    }
};