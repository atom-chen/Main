#pragma once

#include <assert.h>
#include <Utils.hpp>

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


/**
*   λ������ float3 + uv2
*/
class   PROGRAM_P3_UV2 :public ProgramId
{
public:
    typedef int     location;
protected:
    location    _position;
    location    _uv;
    /**
    *   uniform
    */
    location    _MVP;
    location    _texture;
public:
    PROGRAM_P3_UV2()
    {
        _position   =   -1;
        _uv         =   -1;
        _MVP        =   -1;
        _texture    =   -1;
    }

    virtual ~PROGRAM_P3_UV2()
    {}

    location    getPositionAttribute() const
    {
        return  _position;
    }
    location    getUVAttribute() const
    {
        return  _uv;
    }
    location    getMVPUniform() const
    {
        return  _MVP;
    }
    location    getTexture1Uniform() const 
    {
        return  _texture;
    }
    /**
    *   ���ʻ���������������OpenGL��Ӧ�ó���ֱ�ӵĽӿ�
    */
    virtual void    initialize()
    {
		char vs[1024];

		char ps[1024];
		LoadFileContent("ShaderSource//gles//texture2d.vp", vs);
		LoadFileContent("ShaderSource//gles//texture2d.fp", ps);

        createProgram(vs,ps);
        _position   =   glGetAttribLocation(_programId,    "_position");
        _uv         =   glGetAttribLocation(_programId,    "_uv");

        _texture    =   glGetUniformLocation(_programId,   "_texture");
        _MVP        =   glGetUniformLocation(_programId,   "_MVP");
    }
    /**
    *   ʹ�ó���
    */
    virtual void    begin()
    {
        glUseProgram(_programId);
        if (_position != -1)
        {
            glEnableVertexAttribArray(_position);
        }
        if (_uv != -1)
        {
            glEnableVertexAttribArray(_uv);
        }
    }
    /**
    *   ʹ�����
    */
    virtual void    end()
    {
        if (_position != -1)
        {
            glDisableVertexAttribArray(_position);
        }
        if (_uv != -1)
        {
            glDisableVertexAttribArray(_uv);
        }
        glUseProgram(0);
    }
};