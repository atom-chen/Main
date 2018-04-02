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
*   ����
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
    *   ���غ���
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
    *   ʹ�ó���
    */
    virtual void    begin()
    {
        glUseProgram(_programId);
        
    }
    /**
    *   ʹ�����
    */
    virtual void    end()
    {
        glUseProgram(0);
    }
};


class   PROGRAM_P2_T2_C3 :public ProgramId
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
    PROGRAM_P2_T2_C3()
    {
        _positionAttr   =   -1;
        _colorAttr      =   -1;
        _uvAttr         =   -1;
        _MVP            =   -1;
    }
    ~PROGRAM_P2_T2_C3()
    {
    }
   
    /// ��ʼ������
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
            "   if(color.a <= 0.2) \n"
            "       discard;\n"
            "   else \n"
            "       gl_FragColor   =   color * _outColor;"
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
    *   ʹ�ó���
    */
    virtual void    begin()
    {
        glUseProgram(_programId);
        glEnableVertexAttribArray(_positionAttr);
        glEnableVertexAttribArray(_uvAttr);
        glEnableVertexAttribArray(_colorAttr);
        
    }
    /**
    *   ʹ�����
    */
    virtual void    end()
    {
        glDisableVertexAttribArray(_positionAttr);
        glDisableVertexAttribArray(_uvAttr);
        glDisableVertexAttribArray(_colorAttr);
        glUseProgram(0);
    }
};