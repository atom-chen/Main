#pragma once

#include "CELLWinApp.hpp"



class   Texture2dApp :public CELLWinApp
{
public:
    PROGRAM_P2_T2_C3    _shader;
    unsigned            _textureId;
public:
    Texture2dApp()
    {
        _textureId  =   -1;
    }
public:

    //! ��д��ʼ������
    virtual void    onInit()
    {
        _shader.initialize();
        loadTexture();

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

        /**
        *   ����һ������Id,������Ϊ��������������Ĳ����������������id
        */
        glGenTextures( 1, &_textureId );

        /**
        *   ʹ���������id,���߽а�(����)
        */
        glBindTexture( GL_TEXTURE_2D, _textureId );
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
            GL_RGB,             //! �����ʹ�õĴ洢��ʽ
            width,              //! ��ȣ���һ����Կ�����֧�ֲ��������������Ⱥ͸߶Ȳ���2^n��
            height,             //! ��ȣ���һ����Կ�����֧�ֲ��������������Ⱥ͸߶Ȳ���2^n��
            0,                  //! �Ƿ�ı�
            GL_RGB,             //! ���ݵĸ�ʽ��bmp�У�windows,����ϵͳ�д洢��������bgr��ʽ
            GL_UNSIGNED_BYTE,   //! ������8bit����
            pixels
            );
        /**
        *   �ͷ��ڴ�
        */
        FreeImage_Unload(dib);
    }


    virtual void    render()
    {
        glClear(GL_DEPTH_BUFFER_BIT | GL_COLOR_BUFFER_BIT);
        glViewport(0,0,_winWidth,_winHeight);

        struct Vertex
        {
            float x, y, z;
            float u,v;
            float r, g, b,a;
        };

        Vertex vertexs[] =
        {
            { -1.0f,-1.0f, 1.0f,0.0f, 0.0f,1.0f, 1.0f, 1.0f,1.0f },
            {  1.0f,-1.0f, 1.0f,1.0f, 0.0f,1.0f, 1.0f, 1.0f,1.0f },
            {  1.0f, 1.0f, 1.0f,1.0f, 1.0f,1.0f, 1.0f, 1.0f,1.0f },

            { -1.0f,-1.0f, 1.0f,0.0f, 0.0f,1.0f, 1.0f, 1.0f,1.0f },
            {  1.0f, 1.0f, 1.0f,1.0f, 1.0f,1.0f, 1.0f, 1.0f,1.0f },
            { -1.0f, 1.0f, 1.0f,0.0f, 1.0f,1.0f, 1.0f, 1.0f,1.0f },

        };

        static  float x = 0;
        _shader.begin();
        {
            glUniform1i(_shader._texture,0);
            CELL::matrix4   matProj =   CELL::perspective(45.0f, (GLfloat)_winWidth / (GLfloat)_winHeight, 0.1f, 100.0f);

            CELL::matrix4   model(1);
            model.translate(0.0f, 0.0f, -10.0f);
            CELL::matrix4   matRot(1);
            matRot.rotateZ(x);
            x   +=  1.0f;
            
            CELL::matrix4   mvp =   matProj * (matRot * model);

            glUniformMatrix4fv(_shader._MVP, 1, false, mvp.data());

            glVertexAttribPointer(_shader._positionAttr,3,  GL_FLOAT,   false,  sizeof(Vertex),&vertexs[0].x);
            glVertexAttribPointer(_shader._uvAttr,2,        GL_FLOAT,   false,  sizeof(Vertex),&vertexs[0].u);
            glVertexAttribPointer(_shader._colorAttr,4,     GL_FLOAT,   false,  sizeof(Vertex),&vertexs[0].r);

            glDrawArrays(GL_TRIANGLES,0,sizeof(vertexs) / sizeof(vertexs[0]) );
        }
        

        _shader.end();

        eglSwapBuffers(_display, _surface);

    }
};