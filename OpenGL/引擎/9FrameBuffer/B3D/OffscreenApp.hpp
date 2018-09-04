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
public:
    /**
    *   创建一个缓冲区对象
    */
    void    create(int width,int height)
    {
        _width  =   width;
        _height =   height;

        glGenFramebuffers(1, &_FBOID);//fbo
        glBindFramebuffer(GL_FRAMEBUFFER, _FBOID);
        glGenRenderbuffers(1, &_DEPTHID);//depth缓冲区
        glBindRenderbuffer(GL_RENDERBUFFER, _DEPTHID);

				//说明深度缓冲区信息
        glRenderbufferStorage(GL_RENDERBUFFER, GL_DEPTH_COMPONENT16, _width, _height);//rendbuffer，存储内容是什么，宽，高
				//将FrameBuf和Depth关联
        glFramebufferRenderbuffer(GL_FRAMEBUFFER, GL_DEPTH_ATTACHMENT, GL_RENDERBUFFER, _DEPTHID);

        glBindFramebuffer(GL_FRAMEBUFFER, 0);
    }
    
    /**
    *   使用对象(输入一个2D纹理)
    */
    void    begin(unsigned texId)
    {
			  //使用fbo
        glBindFramebuffer(GL_FRAMEBUFFER, _FBOID);    
				//使用颜色缓冲区和2D纹理进行关联，  2D纹理使用texId作为缓冲区，使用纹理的第0级
        glFramebufferTexture2D(GL_FRAMEBUFFER, GL_COLOR_ATTACHMENT0, GL_TEXTURE_2D, texId, 0); //目标Buffer，附件是啥，用什么纹理，用哪个纹理
    }
    /**
    *   使用完，回复状态
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

    //! 顶点缓冲区
    unsigned            _vertexId;
    //! 创建一个FBO FameBufferObject对象
    FrameBufferId       _fbo;
    //! 文理对象
    unsigned            _dynamicTexture;//数据由帧缓存提供，把我们绘制的数据存在这张纹理上
public:
    OffscreenApp()
    {
        _textureId  =   -1;
    }
public:

    /**
    *   创建一个文理函数
    */
    unsigned    createTexture(int width,int height,unsigned inteFmt,unsigned dataFmt,void* data)
    {
        unsigned    texId   =   0;
        /**
        *   产生一个纹理Id,可以认为是纹理句柄，后面的操作将书用这个纹理id
        */
        glGenTextures( 1, &texId );

        /**
        *   使用这个纹理id,或者叫绑定(关联)
        */
        glBindTexture( GL_TEXTURE_2D, texId );
        /**
        *   指定纹理的放大,缩小滤波，使用线性方式，即当图片放大的时候插值方式 
        */
        glTexParameteri(GL_TEXTURE_2D,GL_TEXTURE_MAG_FILTER,GL_LINEAR);
        
        glTexParameteri(GL_TEXTURE_2D,GL_TEXTURE_MIN_FILTER,GL_LINEAR);
        glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_WRAP_S, GL_CLAMP_TO_EDGE );
        glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_WRAP_T, GL_CLAMP_TO_EDGE );
        /**
        *   将图片的rgb数据上传给opengl.
        */
        glTexImage2D( 
            GL_TEXTURE_2D,      //! 指定是二维图片
            0,                  //! 指定为第一级别，纹理可以做mipmap,即lod,离近的就采用级别大的，远则使用较小的纹理
            inteFmt,            //! 纹理的使用的存储格式
            width,              //! 宽度，老一点的显卡，不支持不规则的纹理，即宽度和高度不是2^n。
            height,             //! 宽度，老一点的显卡，不支持不规则的纹理，即宽度和高度不是2^n。
            0,                  //! 是否的边
            dataFmt,            //! 数据的格式
            GL_UNSIGNED_BYTE,   //! 数据是8bit数据
            data
            );
        return  texId;
    }
    //! 重写初始化函数
    virtual void    onInit()
    {
        _shader.initialize();
        loadTexture();
        glEnable(GL_DEPTH_TEST);
        //! 创建顶点缓冲区
        glGenBuffers(1,&_vertexId);
        glBindBuffer(GL_ARRAY_BUFFER,_vertexId);
        glBufferData(GL_ARRAY_BUFFER, sizeof(g_cubeVertices), g_cubeVertices, GL_STATIC_DRAW);
        glBindBuffer(GL_ARRAY_BUFFER,0);

        //! 初始化FBO
        _fbo.create(_winWidth,_winHeight);
				///创建纹理对象
        _dynamicTexture =   createTexture(_fbo._width,_fbo._height,GL_RGBA,GL_RGBA,0);
    }
    virtual void    loadTexture()
    {
        //1 获取图片格式
        FREE_IMAGE_FORMAT fifmt = FreeImage_GetFileType("res//woodfloor.tga", 0);

        //2 加载图片
        FIBITMAP    *dib = FreeImage_Load(fifmt, "res//woodfloor.tga",0);

        //3 转化为rgb 24色
        dib     =   FreeImage_ConvertTo24Bits(dib);

        //4 获取数据指针
        BYTE    *pixels =   (BYTE*)FreeImage_GetBits(dib);

        int     width   =   FreeImage_GetWidth(dib);
        int     height  =   FreeImage_GetHeight(dib);

        _textureId  =   createTexture(width,height,GL_RGB,GL_RGB,pixels);
        /**
        *   释放内存
        */
        FreeImage_Unload(dib);
    }


    void    drawCube(int viewW,int viewH)
    {
        static  float x = 0;

        glBindBuffer(GL_ARRAY_BUFFER,_vertexId);
        _shader.begin();
        {
            glUniform1i(_shader._texture,0);
            CELL::matrix4   matProj =   CELL::perspective(45.0f, float(viewW)/float(viewH), 0.1f, 100.0f);

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
        _fbo.begin(_dynamicTexture);
        {
            glClearColor(1,1,1,1);
            glClear(GL_DEPTH_BUFFER_BIT | GL_COLOR_BUFFER_BIT);
            glViewport(0,0,_fbo._width,_fbo._height);
            glBindTexture(GL_TEXTURE_2D,_textureId);
            drawCube(_fbo._width,_fbo._height);
        }
        _fbo.end();

				//把绘制的内容（已经在texure上）当成贴图，贴到_dynamicTexture上去，再把_dynamicTexture画出来->实现缓冲区离屏绘制
        glClearColor(0,0,0,1);
        glClear(GL_DEPTH_BUFFER_BIT | GL_COLOR_BUFFER_BIT);
        
        glViewport(0,0,_winWidth,_winHeight);

        //glBindTexture(GL_TEXTURE_2D,_textureId);
        glBindTexture(GL_TEXTURE_2D,_dynamicTexture);
        //! 绘制到窗口上
        drawCube(_winWidth,_winHeight);
        

        eglSwapBuffers(_display, _surface);

    }
};