#pragma once

#include "CELLWinApp.hpp"


struct  Vertex
{
    float   x,y,z;
    float   u,v;
    float   r,g,b,a;
};

Vertex g_cubeVertices[] =
{
    // Quad 0
    { -1.0f,-1.0f, 1.0f,0,1,1.0f,1.0f,1.0f,1.0f },
    {  1.0f,-1.0f, 1.0f,1,1,1.0f,1.0f,1.0f,1.0f  },
    {  1.0f, 1.0f, 1.0f,1,0,1.0f,1.0f,1.0f,1.0f  },

    { -1.0f,-1.0f, 1.0f,0,1,1.0f,1.0f,1.0f,1.0f  },
    {  1.0f, 1.0f, 1.0f,1,0,1.0f,1.0f,1.0f,1.0f  },
    { -1.0f, 1.0f, 1.0f,0,0,1.0f,1.0f,1.0f,1.0f  },
    //
    { -1.0f,-1.0f,-1.0f,0,1,1.0f,0.0f,0.0f,1.0f  },
    { -1.0f, 1.0f,-1.0f,1,1,1.0f,0.0f,0.0f,1.0f  },
    {  1.0f, 1.0f,-1.0f,1,0,1.0f,0.0f,0.0f,1.0f  },

    { -1.0f,-1.0f,-1.0f,0,1,1.0f,0.0f,0.0f,1.0f  },
    {  1.0f, 1.0f,-1.0f,1,0,1.0f,0.0f,0.0f,1.0f  },
    {  1.0f,-1.0f,-1.0f,0,0,1.0f,0.0f,0.0f,1.0f  },

    //
    { -1.0f, 1.0f,-1.0f,0,1,0.0f,1.0f,0.0f,1.0f },
    { -1.0f, 1.0f, 1.0f,1,1,0.0f,1.0f,0.0f,1.0f },
    {  1.0f, 1.0f, 1.0f,1,0,0.0f,1.0f,0.0f,1.0f },

    { -1.0f, 1.0f,-1.0f,0,1,0.0f,1.0f,0.0f,1.0f },
    {  1.0f, 1.0f, 1.0f,1,0,0.0f,1.0f,0.0f,1.0f },
    {  1.0f, 1.0f,-1.0f,0,0,0.0f,1.0f,0.0f,1.0f },

    //
    { -1.0f,-1.0f,-1.0f,0,1,0.0f,1.0f,0.0f,1.0f },
    {  1.0f,-1.0f,-1.0f,1,1,0.0f,1.0f,0.0f,1.0f },
    {  1.0f,-1.0f, 1.0f,1,0,0.0f,1.0f,0.0f,1.0f },

    { -1.0f,-1.0f,-1.0f,0,1,0.0f,1.0f,0.0f,1.0f },
    {  1.0f,-1.0f, 1.0f,1,0,0.0f,1.0f,0.0f,1.0f },
    { -1.0f,-1.0f, 1.0f,0,0,0.0f,1.0f,0.0f,1.0f},

    // Quad 4
    {  1.0f,-1.0f,-1.0f,0,1,0.0f,0.0f,1.0f,1.0f },
    {  1.0f, 1.0f,-1.0f,1,1,0.0f,0.0f,1.0f,1.0f },
    {  1.0f, 1.0f, 1.0f,1,0,0.0f,0.0f,1.0f,1.0f },

    {  1.0f,-1.0f,-1.0f,0,1,0.0f,0.0f,1.0f,1.0f },
    {  1.0f, 1.0f, 1.0f,1,0,0.0f,0.0f,1.0f,1.0f },
    {  1.0f,-1.0f, 1.0f,0,0,0.0f,0.0f,1.0f,1.0f },

    //
    { -1.0f,-1.0f,-1.0f,0,1,0.0f,0.0f,1.0f,1.0f },
    { -1.0f,-1.0f, 1.0f,1,1,0.0f,0.0f,1.0f,1.0f },
    { -1.0f, 1.0f, 1.0f,1,0,0.0f,0.0f,1.0f,1.0f },

    { -1.0f,-1.0f,-1.0f,0,1,0.0f,0.0f,1.0f,1.0f },
    { -1.0f, 1.0f, 1.0f,1,0,0.0f,0.0f,1.0f,1.0f },
    { -1.0f, 1.0f,-1.0f,0,0,0.0f,0.0f,1.0f,1.0f } 
};


class   FrameBufferId
{
public:
    FrameBufferId()
    {
        _width  =   0;
        _height =   0;
        _FBOID  =   0;
        _DEPTHID  =   0;
    }
public:
    unsigned    _width;
    unsigned    _height;
    unsigned    _FBOID;
    unsigned    _DEPTHID;
    unsigned    _TEXTUREID;
public:

    /**
    *   ����һ��������
    */
    unsigned    createTexture(int width,int height,unsigned inteFmt,unsigned dataFmt,void* data)
    {
        unsigned    texId   =   0;
        /**
        *   ����һ������Id,������Ϊ��������������Ĳ����������������id
        */
        glGenTextures( 1, &texId );

        /**
        *   ʹ���������id,���߽а�(����)
        */
        glBindTexture( GL_TEXTURE_2D, texId );
        /**
        *   ָ������ķŴ�,��С�˲���ʹ�����Է�ʽ������ͼƬ�Ŵ��ʱ���ֵ��ʽ 
        */
        glTexParameteri(GL_TEXTURE_2D,GL_TEXTURE_MAG_FILTER,GL_LINEAR);
        
        glTexParameteri(GL_TEXTURE_2D,GL_TEXTURE_MIN_FILTER,GL_LINEAR);
        /**
        *   ��ͼƬ��rgb�����ϴ���opengl.
        */
        glTexImage2D( 
            GL_TEXTURE_2D,      //! ָ���Ƕ�άͼƬ
            0,                  //! ָ��Ϊ��һ�������������mipmap,��lod,����ľͲ��ü����ģ�Զ��ʹ�ý�С������
            inteFmt,            //! �����ʹ�õĴ洢��ʽ
            width,              //! ��ȣ���һ����Կ�����֧�ֲ��������������Ⱥ͸߶Ȳ���2^n��
            height,             //! ��ȣ���һ����Կ�����֧�ֲ��������������Ⱥ͸߶Ȳ���2^n��
            0,                  //! �Ƿ�ı�
            dataFmt,            //! ���ݵĸ�ʽ
            GL_UNSIGNED_BYTE,   //! ������8bit����
            data
            );
        return  texId;
    }

    /**
    *   ����һ������������
    */
    void    create(int width,int height)
    {
        _width  =   width;
        _height =   height;

        glGenFramebuffers(1, &_FBOID);
        glBindFramebuffer(GL_FRAMEBUFFER, _FBOID);

        glGenRenderbuffers(1, &_DEPTHID);
        glBindRenderbuffer(GL_RENDERBUFFER, _DEPTHID);
        glRenderbufferStorage(GL_RENDERBUFFER, GL_DEPTH_COMPONENT16, _width, _height);


        glFramebufferRenderbuffer(GL_FRAMEBUFFER, GL_DEPTH_ATTACHMENT, GL_RENDERBUFFER, _DEPTHID);

        _TEXTUREID  =   createTexture(width,height,GL_RGB,GL_RGB,0);

        glFramebufferTexture2D(GL_FRAMEBUFFER, GL_COLOR_ATTACHMENT0, GL_TEXTURE_2D, _TEXTUREID, 0);

        glBindFramebuffer(GL_FRAMEBUFFER, 0);
    }
    
    /**
    *   ʹ�ö���
    */
    void    begin(unsigned texId)
    {
        glBindFramebuffer(GL_FRAMEBUFFER, _FBOID);
    }
    /**
    *   ʹ���꣬�ظ�״̬
    */
    void    end()
    {
        glBindFramebuffer(GL_FRAMEBUFFER, 0);
    }
};

class   OffscreenApp :public CELLWinApp
{
public:
    PROGRAM_P2_T2_C3    _shader;
    unsigned            _textureId;

    //! ���㻺����
    unsigned            _vertexId;
    //! ����һ��FBO FameBufferObject����
    FrameBufferId       _fbo;
public:
    OffscreenApp()
    {
        _textureId  =   -1;
    }
public:

    /**
    *   ����һ��������
    */
    unsigned    createTexture(int width,int height,unsigned inteFmt,unsigned dataFmt,void* data)
    {
        unsigned    texId   =   0;
        /**
        *   ����һ������Id,������Ϊ��������������Ĳ����������������id
        */
        glGenTextures( 1, &texId );

        /**
        *   ʹ���������id,���߽а�(����)
        */
        glBindTexture( GL_TEXTURE_2D, texId );
        /**
        *   ָ������ķŴ�,��С�˲���ʹ�����Է�ʽ������ͼƬ�Ŵ��ʱ���ֵ��ʽ 
        */
        glTexParameteri(GL_TEXTURE_2D,GL_TEXTURE_MAG_FILTER,GL_LINEAR);
        
        glTexParameteri(GL_TEXTURE_2D,GL_TEXTURE_MIN_FILTER,GL_LINEAR);
        /**
        *   ��ͼƬ��rgb�����ϴ���opengl.
        */
        glTexImage2D( 
            GL_TEXTURE_2D,      //! ָ���Ƕ�άͼƬ
            0,                  //! ָ��Ϊ��һ�������������mipmap,��lod,����ľͲ��ü����ģ�Զ��ʹ�ý�С������
            inteFmt,            //! �����ʹ�õĴ洢��ʽ
            width,              //! ��ȣ���һ����Կ�����֧�ֲ��������������Ⱥ͸߶Ȳ���2^n��
            height,             //! ��ȣ���һ����Կ�����֧�ֲ��������������Ⱥ͸߶Ȳ���2^n��
            0,                  //! �Ƿ�ı�
            dataFmt,            //! ���ݵĸ�ʽ
            GL_UNSIGNED_BYTE,   //! ������8bit����
            data
            );
        return  texId;
    }
    //! ��д��ʼ������
    virtual void    onInit()
    {
        _shader.initialize();
        loadTexture();
        glEnable(GL_DEPTH_TEST);
        //! �������㻺����
        glGenBuffers(1,&_vertexId);
        glBindBuffer(GL_ARRAY_BUFFER,_vertexId);
        glBufferData(GL_ARRAY_BUFFER, sizeof(g_cubeVertices), g_cubeVertices, GL_STATIC_DRAW);
        glBindBuffer(GL_ARRAY_BUFFER,0);

        //! ��ʼ��FBO
        _fbo.create(_winWidth,_winHeight);
    }
    virtual void    loadTexture()
    {
        //1 ��ȡͼƬ��ʽ
        FREE_IMAGE_FORMAT fifmt = FreeImage_GetFileType("woodfloor.tga", 0);

        //2 ����ͼƬ
        FIBITMAP    *dib = FreeImage_Load(fifmt, "woodfloor.tga",0);

        //3 ת��Ϊrgb 24ɫ
        dib     =   FreeImage_ConvertTo24Bits(dib);

        //4 ��ȡ����ָ��
        BYTE    *pixels =   (BYTE*)FreeImage_GetBits(dib);

        int     width   =   FreeImage_GetWidth(dib);
        int     height  =   FreeImage_GetHeight(dib);

        _textureId  =   createTexture(width,height,GL_RGB,GL_RGB,pixels);
        /**
        *   �ͷ��ڴ�
        */
        FreeImage_Unload(dib);
    }


    void    drawCube()
    {
        static  float x = 0;

        glBindBuffer(GL_ARRAY_BUFFER,_vertexId);
        _shader.begin();
        {
            glUniform1i(_shader._texture,0);
            CELL::matrix4   matProj =   CELL::perspective(45.0f, 1.0f, 0.1f, 100.0f);

            CELL::matrix4   model(1);
            model.translate(0.0f, 0.0f, -10.0f);
            CELL::matrix4   matRot(1);
            matRot.rotateYXZ(x,x,x);

            x   +=  1.0f;

            CELL::matrix4   mvp =   matProj *  model * (matRot);

            glUniformMatrix4fv(_shader._MVP, 1, false, mvp.data());

            glVertexAttribPointer(_shader._positionAttr,3,  GL_FLOAT,   false,  sizeof(Vertex),0);
            glVertexAttribPointer(_shader._uvAttr,2,        GL_FLOAT,   false,  sizeof(Vertex),(void*)12);
            glVertexAttribPointer(_shader._colorAttr,4,     GL_FLOAT,   false,  sizeof(Vertex),(void*)20);

            glDrawArrays(GL_TRIANGLES,0,sizeof(g_cubeVertices) / sizeof(g_cubeVertices[0]) );
        }
        _shader.end();
        glBindBuffer(GL_ARRAY_BUFFER,0);
    }

    virtual void    render()
    {
        glBindTexture(GL_TEXTURE_2D,_textureId);
        _fbo.begin(0);
        {
            glClearColor(1,1,1,1);
            glClear(GL_DEPTH_BUFFER_BIT | GL_COLOR_BUFFER_BIT);
            glViewport(0,0,_fbo._width,_fbo._height);
            drawCube();
        }
        _fbo.end();


        glClearColor(0,0,0,1);
        glClear(GL_DEPTH_BUFFER_BIT | GL_COLOR_BUFFER_BIT);
        
        glViewport(0,0,_winWidth,_winHeight);

        //glBindTexture(GL_TEXTURE_2D,_textureId);
        glBindTexture(GL_TEXTURE_2D,_fbo._TEXTUREID);
        //! ���Ƶ�������
        drawCube();
        

        eglSwapBuffers(_display, _surface);

    }
};