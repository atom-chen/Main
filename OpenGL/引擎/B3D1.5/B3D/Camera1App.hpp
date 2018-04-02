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
        //! �������windows������ȡ���̵�״̬
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


    //! ��д��ʼ������
    virtual void    onInit()
    {
        _shader.initialize();
        _textureId  =   loadTexture("woodfloor.tga");
        _texGround  =   loadTexture("1.jpg");
        _camera1._eye   =   CELL::float3(1, 1, 1);
        _camera1._look  =   CELL::float3(0.5f, -0.4f, -5.5f);
        _camera1._up    =   CELL::float3(0.0f, 1.0f, 0.0f);
        _camera1._right =   CELL::float3(1.0f, 0.0f, 0.0f);
    }
    virtual unsigned    loadTexture(const char* fileName)
    {
        unsigned    texId   =   0;
        //1 ��ȡͼƬ��ʽ
        FREE_IMAGE_FORMAT fifmt = FreeImage_GetFileType(fileName, 0);

        //2 ����ͼƬ
        FIBITMAP    *dib = FreeImage_Load(fifmt, fileName,0);

        //3 ת��Ϊrgb 24ɫ
        dib     =   FreeImage_ConvertTo24Bits(dib);

        //4 ��ȡ����ָ��
        BYTE    *pixels =   (BYTE*)FreeImage_GetBits(dib);

        int     width   =   FreeImage_GetWidth(dib);
        int     height  =   FreeImage_GetHeight(dib);

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

        return  texId;
    }


    /**
    *   ��д���ƺ���
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
        //! ���¶�ʱ��
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
            //! ���Ƶ���
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