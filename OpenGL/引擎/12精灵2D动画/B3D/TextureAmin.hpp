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

    //! 重写初始化函数
    virtual void    onInit()
    {
        _shader.initialize();
        loadTexture();

    }
    virtual void    loadTexture()
    {
        //1 获取图片格式
        FREE_IMAGE_FORMAT fifmt = FreeImage_GetFileType("res//data//image//a.png", 0);

        //2 加载图片
        FIBITMAP    *dib = FreeImage_Load(fifmt, "res//data//image//a.png",0);

        //3 转化为rgba 色
        dib     =   FreeImage_ConvertTo32Bits(dib);

        //4 获取数据指针
        BYTE    *pixels =   (BYTE*)FreeImage_GetBits(dib);

        int     width   =   FreeImage_GetWidth(dib);
        int     height  =   FreeImage_GetHeight(dib);

        /**
        *   产生一个纹理Id,可以认为是纹理句柄，后面的操作将书用这个纹理id
        */
        glGenTextures( 1, &_textureId );

        /**
        *   使用这个纹理id,或者叫绑定(关联)
        */
        glBindTexture( GL_TEXTURE_2D, _textureId );
        /**
        *   指定纹理的放大,缩小滤波，使用线性方式，即当图片放大的时候插值方式 
        */
        glTexParameteri(GL_TEXTURE_2D,GL_TEXTURE_MAG_FILTER,GL_LINEAR);
        
        glTexParameteri(GL_TEXTURE_2D,GL_TEXTURE_MIN_FILTER,GL_LINEAR);
        /**
        *   将图片的rgb数据上传给opengl.
        */
        glTexImage2D( 
            GL_TEXTURE_2D,      //! 指定是二维图片
            0,                  //! 指定为第一级别，纹理可以做mipmap,即lod,离近的就采用级别大的，远则使用较小的纹理
            GL_RGBA,            //! 纹理的使用的存储格式
            width,              //! 宽度，老一点的显卡，不支持不规则的纹理，即宽度和高度不是2^n。
            height,             //! 宽度，老一点的显卡，不支持不规则的纹理，即宽度和高度不是2^n。
            0,                  //! 是否的边
            GL_RGBA,            //! 数据的格式，bmp中，windows,操作系统中存储的数据是bgr格式
            GL_UNSIGNED_BYTE,   //! 数据是8bit数据
            pixels
            );
        /**
        *   释放内存
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

        Vertex position[6] =//顶点数据pos uv 
        {
            {-1.0,-1.0,1.0,     0,1},
            { 1.0,-1.0,1.0,     1,1},
            { 1.0, 1.0,1.0,     1,0},
            {-1.0,-1.0,1.0,     0,1},
            { 1.0, 1.0,1.0,     1,0},
            {-1.0, 1.0,1.0,     0,0}
        };

        int     _row    =   4;//该图集里有多少行
        int     _col    =   4;//该图集里有多少列
        static  float   _curFrame   =  0.0f;//当前在播第几帧

        _curFrame   +=  tm;
        if (_curFrame > 16)
        {
            _curFrame   =   0;
        }

				//缩放比例
        float   fR  =   1.0f/_row;
        float   fC  =   1.0f/_col;

        //! 计算当前行
        int     cR  =   int(_curFrame/_col);
        //! 计算当前列
        int     cC  =   int(_curFrame - cR * _col);




        for (int i = 0 ;i < 6 ; ++ i )
        {
            position[i].x   *=  200;
            position[i].x   +=  200;
            position[i].y   *=  200;
            position[i].y   +=  200;


            //! 对uv坐标进行缩放（大小）
            position[i].u    *=  fR;
            position[i].v    *=  fC;

            //! 对uv坐标进行平移(修正位置)
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