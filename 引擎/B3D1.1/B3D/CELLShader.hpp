#pragma once
#include "ggl.h"
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
        bool error=false;
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
                _fragment._shaderId = 0;
            }
            if (_vertex._shaderId)
            {
                glDeleteShader(_vertex._shaderId);
                _vertex._shaderId = 0;
            }
            if (_programId)
            {
                glDeleteProgram(_programId);
                _programId=0;
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


class   PROGRAM_P2_C4 :public ProgramId
{
public:
    typedef int location; 
public:
    location    m_position;
    location    m_color;
    location    m_MVP;
public:
    PROGRAM_P2_C4()
    {
        m_position   =   -1;
		m_color = -1;
		m_MVP = -1;
    }
    ~PROGRAM_P2_C4()
    {
    }

    location    getPositionAttribute() const
    {
        return  m_position;
    }
    location    getColorAttribute() const
    {
        return  m_color;
    }
    location    getColorUniform()
    {
        return  m_color;
    }
    location    getMVP() const 
    {
        return  m_MVP;
    }
    /// ��ʼ������
    virtual bool    initialize()
    {
        const char* vs  =   
        {
            "precision lowp float; "
            "uniform   mat4 _MVP;"
            "attribute vec2 _position;"

            "void main()"
            "{"
            "   vec4    pos =   vec4(_position,0,1);"
            "   gl_Position =   _MVP * pos;"
            "}"
        };
        const char* ps  =   
        {
            "precision  lowp float; "
            "uniform    vec4 _color;"
            "void main()"
            "{"
            "   gl_FragColor   =   _color;"
            "}"
        };
        
        bool    res =   createProgram(vs,ps);
        if(res)
        {
            m_position   =   glGetAttribLocation(_programId,"_position");
           m_color      =   glGetUniformLocation(_programId,"_color");
            m_MVP        =   glGetUniformLocation(_programId,"_MVP");
        }
        return  res;
    }

    /**
    *   ʹ�ó���
    */
    virtual void  begin()
    {
        glUseProgram(_programId);
        glEnableVertexAttribArray(m_position);
        
    }
    /**
    *   ʹ�����
    */
    virtual void  end()
    {
        glDisableVertexAttribArray(m_position);
        glUseProgram(0);
    }
};