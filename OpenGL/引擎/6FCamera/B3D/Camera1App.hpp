#pragma once

#include "CELLWinApp.hpp"
#include "CELLTimestamp.hpp"
using namespace CELL;



class   FirstCameraInfor
{
public:
    FirstCameraInfor()
    {
        _moveSpeed  =   5;
    }
    CELL::float3    _eye;
    CELL::float3    _look;
    CELL::float3    _up;
    CELL::float3    _right;
    float           _moveSpeed;
public:

    void    updateCamera(float fElapsed)
    {

        CELL::float3    tmpLook =   _look;
        CELL::float3    dir     =   _look - _eye;
        dir =   normalize(dir);
        //! 这里调用windows函数获取键盘的状态
        unsigned char keys[256];
        GetKeyboardState( keys );

        if( keys[VK_UP] & 0x80 )
        {
            _eye    -=  dir*-_moveSpeed * fElapsed;
            _look   -=  dir*-_moveSpeed * fElapsed;
        }

        if( keys[VK_DOWN] & 0x80 )
        {
            _eye    +=  (dir*-_moveSpeed) * fElapsed;
            _look   +=  (dir*-_moveSpeed) * fElapsed;
        }

        if( keys[VK_LEFT] & 0x80 )
        {
            _eye    -=  (_right*_moveSpeed) * fElapsed;
            _look   -=  (_right*_moveSpeed) * fElapsed;
        }

        if( keys[VK_RIGHT] & 0x80 )
        {
            _eye    +=  (_right*_moveSpeed) * fElapsed;
            _look   +=  (_right*_moveSpeed) * fElapsed;
        }
    }
};

class   Camera1App :public CELLWinApp
{
public:
    PROGRAM_P2_T2_C3    _shader;
    unsigned            _textureId;
    unsigned            _texGround;
    FirstCameraInfor    _camera1;
    CELLTimestamp       _timeStamp;
public:
    Camera1App()
    {
        _textureId  =   -1;
        _texGround  =   -1;
    }
public:


    //! 重写初始化函数
    virtual void    onInit()
    {
        _shader.initialize();
        _textureId  =   loadTexture("res//woodfloor.tga");
        _texGround  =   loadTexture("res//1.jpg");
        _camera1._eye   =   CELL::float3(1, 1, 1);
        _camera1._look  =   CELL::float3(0.5f, -0.4f, -5.5f);
        _camera1._up    =   CELL::float3(0.0f, 1.0f, 0.0f);
        _camera1._right =   CELL::float3(1.0f, 0.0f, 0.0f);
    }
    virtual unsigned    loadTexture(const char* fileName)
    {
        unsigned    texId   =   0;
        //1 获取图片格式
        FREE_IMAGE_FORMAT fifmt = FreeImage_GetFileType(fileName, 0);

        //2 加载图片
        FIBITMAP    *dib = FreeImage_Load(fifmt, fileName,0);

        //3 转化为rgb 24色
        dib     =   FreeImage_ConvertTo24Bits(dib);

        //4 获取数据指针
        BYTE    *pixels =   (BYTE*)FreeImage_GetBits(dib);

        int     width   =   FreeImage_GetWidth(dib);
        int     height  =   FreeImage_GetHeight(dib);

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
        /**
        *   将图片的rgb数据上传给opengl.
        */
        glTexImage2D( 
            GL_TEXTURE_2D,      //! 指定是二维图片
            0,                  //! 指定为第一级别，纹理可以做mipmap,即lod,离近的就采用级别大的，远则使用较小的纹理
            GL_RGB,             //! 纹理的使用的存储格式
            width,              //! 宽度，老一点的显卡，不支持不规则的纹理，即宽度和高度不是2^n。
            height,             //! 宽度，老一点的显卡，不支持不规则的纹理，即宽度和高度不是2^n。
            0,                  //! 是否的边
            GL_RGB,             //! 数据的格式，bmp中，windows,操作系统中存储的数据是bgr格式
            GL_UNSIGNED_BYTE,   //! 数据是8bit数据
            pixels
            );
        /**
        *   释放内存
        */
        FreeImage_Unload(dib);

        return  texId;
    }


    /**
    *   重写绘制函数
    */
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
        float   elapseTime   =   (float)_timeStamp.getElapsedSecond();
        //! 更新定时器
        _timeStamp.update();

        _camera1.updateCamera(elapseTime);

        CELL::matrix4   matView =   CELL::lookAt(_camera1._eye,_camera1._look,_camera1._up);

        
        Vertex vertexs[] =
        {
            { -1.0f,-1.0f, 1.0f,0.0f, 0.0f,1.0f, 1.0f, 1.0f,1.0f },
            {  1.0f,-1.0f, 1.0f,1.0f, 0.0f,1.0f, 1.0f, 1.0f,1.0f },
            {  1.0f, 1.0f, 1.0f,1.0f, 1.0f,1.0f, 1.0f, 1.0f,1.0f },

            { -1.0f,-1.0f, 1.0f,0.0f, 0.0f,1.0f, 1.0f, 1.0f,1.0f },
            {  1.0f, 1.0f, 1.0f,1.0f, 1.0f,1.0f, 1.0f, 1.0f,1.0f },
            { -1.0f, 1.0f, 1.0f,0.0f, 1.0f,1.0f, 1.0f, 1.0f,1.0f },
        };

        float   gSize   =   100;
        float   gPos    =   -5;
        float   rept    =   100;

        Vertex grounds[] =
        {
            { -gSize, gPos,-gSize,0.0f, 0.0f,1.0f, 1.0f, 1.0f,1.0f },
            {  gSize, gPos,-gSize,rept, 0.0f,1.0f, 1.0f, 1.0f,1.0f },
            {  gSize, gPos, gSize,rept, rept,1.0f, 1.0f, 1.0f,1.0f },

            { -gSize, gPos,-gSize,0.0f, 0.0f,1.0f, 1.0f, 1.0f,1.0f },
            {  gSize, gPos, gSize,rept, rept,1.0f, 1.0f, 1.0f,1.0f },
            { -gSize, gPos, gSize,0.0f, rept,1.0f, 1.0f, 1.0f,1.0f },
        };
        
        // MVP

        static  float x = 0;
        _shader.begin();
        {
            
            CELL::matrix4   matWorld(1);
            CELL::matrix4   matProj =   CELL::perspective(45.0f, (GLfloat)_winWidth / (GLfloat)_winHeight, 0.1f, 100.0f);

            CELL::matrix4   MVP     =   matProj * matView * matWorld;

            glUniform1i(_shader._texture,0);
            //! 绘制地面
            glBindTexture(GL_TEXTURE_2D,_texGround);
            glUniformMatrix4fv(_shader._MVP, 1, false, MVP.data());

            glVertexAttribPointer(_shader._positionAttr,3,  GL_FLOAT,   false,  sizeof(Vertex),&grounds[0].x);
            glVertexAttribPointer(_shader._uvAttr,2,        GL_FLOAT,   false,  sizeof(Vertex),&grounds[0].u);
            glVertexAttribPointer(_shader._colorAttr,4,     GL_FLOAT,   false,  sizeof(Vertex),&grounds[0].r);

            glDrawArrays(GL_TRIANGLES,0,sizeof(grounds) / sizeof(grounds[0]) );

            CELL::matrix4   model(1);

            model.translate(0.0f, 0.0f, -10.0f);
            CELL::matrix4   matRot(1);
            matRot.rotateZ(x);
            x   +=  1.0f;
            
            CELL::matrix4   mvp =   matProj * matView * (matRot * model);

            glBindTexture(GL_TEXTURE_2D,_textureId);
            glUniform1i(_shader._texture,0);
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