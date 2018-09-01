#pragma once

#include "CELLWinApp.hpp"
#include "CELLTimestamp.hpp"

using namespace CELL;

class   TextureAmin :public CELLWinApp
{
public:
    PROGRAM_P3_UV2  _shader;
    unsigned        _textureId;
    CELLTimestamp   _time;
public:
    TextureAmin()
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
        FREE_IMAGE_FORMAT fifmt = FreeImage_GetFileType("res/data/image/a.png", 0);

        //2 ����ͼƬ
        FIBITMAP    *dib = FreeImage_Load(fifmt, "res/data/image/a.png",0);

        //3 ת��Ϊrgba ɫ
        dib     =   FreeImage_ConvertTo32Bits(dib);

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
            GL_RGBA,            //! �����ʹ�õĴ洢��ʽ
            width,              //! ��ȣ���һ����Կ�����֧�ֲ��������������Ⱥ͸߶Ȳ���2^n��
            height,             //! ��ȣ���һ����Կ�����֧�ֲ��������������Ⱥ͸߶Ȳ���2^n��
            0,                  //! �Ƿ�ı�
            GL_RGBA,            //! ���ݵĸ�ʽ��bmp�У�windows,����ϵͳ�д洢��������bgr��ʽ
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

        float   tm  =   _time.getElapsedSecond() * 16;
        _time.update();

        
        CELL::matrix4r  matProj =   CELL::ortho<float>(0,float(_winWidth),float(_winHeight),0,-100.0f,10000.0f);


        struct  Vertex
        {
            float       x,y,z;
            float       u,v;
        };

        Vertex position[6] =//��������pos uv 
        {
            {-1.0,-1.0,1.0,     0,1},
            { 1.0,-1.0,1.0,     1,1},
            { 1.0, 1.0,1.0,     1,0},
            {-1.0,-1.0,1.0,     0,1},
            { 1.0, 1.0,1.0,     1,0},
            {-1.0, 1.0,1.0,     0,0}
        };

        int     _row    =   4;//��ͼ�����ж�����
        int     _col    =   4;//��ͼ�����ж�����
        static  float   _curFrame   =  0.0f;//��ǰ�ڲ��ڼ�֡

        _curFrame   +=  tm;
        if (_curFrame > 16)
        {
            _curFrame   =   0;
        }

				//���ű���
        float   fR  =   1.0f/_row;
        float   fC  =   1.0f/_col;

        //! ���㵱ǰ��
        int     cR  =   int(_curFrame/_col);
        //! ���㵱ǰ��
        int     cC  =   int(_curFrame - cR * _col);




        for (int i = 0 ;i < 6 ; ++ i )
        {
            position[i].x   *=  200;
            position[i].x   +=  200;
            position[i].y   *=  200;
            position[i].y   +=  200;


            //! ��uv����������ţ���С��
            position[i].u    *=  fR;
            position[i].v    *=  fC;

            //! ��uv�������ƽ��(����λ��)
            position[i].u    +=  cC * fC;
            position[i].v    +=  cR * fR;
        }


        _shader.begin();
        {
            CELL::matrix4r  matView(1);
            CELL::matrix4r  matModel(1);
            CELL::matrix4r  MVP = matProj * matView * matModel;

            glUniformMatrix4fv(_shader.getMVPUniform(),1,false,MVP.data());
            glUniform1i(_shader.getTexture1Uniform(),0);
            glVertexAttribPointer(_shader.getPositionAttribute(),3,GL_FLOAT,GL_FALSE,sizeof(Vertex),position);
            glVertexAttribPointer(_shader.getUVAttribute(),2,GL_FLOAT,GL_FALSE,sizeof(Vertex),&position[0].u);
            glDrawArrays(GL_TRIANGLES,0,6);

        }
        _shader.end();


        eglSwapBuffers(_display, _surface);

    }
};