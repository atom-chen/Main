#pragma once

#include "CELLWinApp.hpp"
#include "CELLTimestamp.hpp"
#include "CELL3RDCamera.hpp"
#include "CELLModel.hpp"
#include "CELLNode.hpp"
#include "CELLTerrain.hpp"

using namespace CELL;

struct Vertex
{
    float x, y, z;
    float u,v;
    float r, g, b,a;
};

class   Role
{
public:
    float3  _pos;
    float3  _target;
    float   _speed;
public:
    Role()
    {
        _speed  =   5;
    }
    /**
    *   �����ƶ���Ŀ���
    */
    void    setTarget(float3 target)
    {
        _target =   target;
    }
    /**
    *   ����λ��
    */
    void    setPosition(float3 pos)
    {
        _pos    =   pos;
        _pos.y  =   1;
    }

    void moveCheck(const float elasped)
    {
        /**
        *   Ŀ��λ�ò��ǵ�ǰλ�á�
        */
        if (_target == _pos)
        {
            return;
        }

        /**
        *   ��ȡ��ǰ���λ����Ŀ��λ�õ�ƫ����
        */
        float3  offset  =   _target - _pos;
        
        /**
        *   ��ȡ���ƶ��ķ���
        */
        float3  dir     =   normalize(offset);
        
        if (distance(_target,_pos) > 1)
        {
            float   speed   =   elasped * _speed;

            _pos    +=  float3(dir.x * speed,0,dir.z  * speed) ;
        }
        else
        {
            _target  = _pos;
        }
    }
    /**
    *   ���ƽ�ɫ
    */
    void    render(float fElapsed,CELL3RDCamera& camera,PROGRAM_P2_T2_C3& shader)
    {

        moveCheck(fElapsed);

        Vertex vertexs[] =
        {
            { -1.0f,-1.0f, 1.0f,0.0f, 0.0f,1.0f, 1.0f, 1.0f,1.0f },
            {  1.0f,-1.0f, 1.0f,1.0f, 0.0f,1.0f, 1.0f, 1.0f,1.0f },
            {  1.0f, 1.0f, 1.0f,1.0f, 1.0f,1.0f, 1.0f, 1.0f,1.0f },

            { -1.0f,-1.0f, 1.0f,0.0f, 0.0f,1.0f, 1.0f, 1.0f,1.0f },
            {  1.0f, 1.0f, 1.0f,1.0f, 1.0f,1.0f, 1.0f, 1.0f,1.0f },
            { -1.0f, 1.0f, 1.0f,0.0f, 1.0f,1.0f, 1.0f, 1.0f,1.0f },
        };
        CELL::matrix4   matModel(1);
        matModel.translate(_pos);

        CELL::matrix4   MVP     =   camera.getProject() * camera.getView() *  matModel;

        shader.begin();
        {
            glUniform1i(shader._texture,0);
            //! ���Ƶ���
            glUniformMatrix4fv(shader._MVP, 1, false, MVP.data());

            glVertexAttribPointer(shader._positionAttr,3,  GL_FLOAT,   false,  sizeof(Vertex),&vertexs[0].x);
            glVertexAttribPointer(shader._uvAttr,2,        GL_FLOAT,   false,  sizeof(Vertex),&vertexs[0].u);
            glVertexAttribPointer(shader._colorAttr,4,     GL_FLOAT,   false,  sizeof(Vertex),&vertexs[0].r);

            glDrawArrays(GL_TRIANGLES,0,sizeof(vertexs) / sizeof(vertexs[0]) );
        }
        shader.end();
    }
};
class   NodeApp :public CELLWinApp
{
public:
    PROGRAM_P2_T2_C3    _shader;
    unsigned            _textureId;
    unsigned            _texGround;
    CELL3RDCamera       _camera3rd;
    Role                _role;
    float2              _rightDown;
    bool                _bRightFlg;
    CELLModelStd        _model;
    Frustum             _frustum;
    CELLTerrain         _terrain;
public:
    NodeApp()
    {
        _textureId  =   -1;
        _texGround  =   -1;
        _bRightFlg  =   0;

        _role.setPosition(float3(0,0,-10));
        _role.setTarget(float3(0,0,-10));

    }
public:

    //! ��д��ʼ������
    virtual void    onInit()
    {
        glEnable(GL_DEPTH_TEST);
        _shader.initialize();
        _textureId  =   loadTexture("data/image/woodfloor.tga");
        _texGround  =   loadTexture("data/image/1.jpg");

        _camera3rd.setRadius(50);
        _camera3rd.perspective(45.0f, float(_winWidth) / float(_winHeight), 0.1f, 100000.0f);
        _camera3rd.setEye(float3(50,50,50));
        _camera3rd.setTarget(_role._pos);
        _camera3rd.calcDir();
        _camera3rd.setUp(float3(0,1,0));

        _terrain.loadTerrain("data/image/Terrain.raw",1024,1024);

        _model.load("data/model/Box01.sm.xml");

    }
    /**
    *   ���ļ���������
    */
    virtual unsigned    loadTexture(const char* fileName)
    {
        unsigned    texId   =   0;
        //1 ��ȡͼƬ��ʽ
        FREE_IMAGE_FORMAT fifmt = FreeImage_GetFileType(fileName, 0);

        //2 ����ͼƬ
        FIBITMAP    *dib = FreeImage_Load(fifmt, fileName,0);

        //3 ת��Ϊrgb 24ɫ
        dib     =   FreeImage_ConvertTo32Bits(dib);

        //4 ��ȡ����ָ��
        BYTE    *pixels =   (BYTE*)FreeImage_GetBits(dib);

        int     width   =   FreeImage_GetWidth(dib);
        int     height  =   FreeImage_GetHeight(dib);

        for (int i = 0 ; i < width * height * 4 ; i += 4 )
        {
            BYTE r = pixels[i];
            pixels[i]       =   pixels[i + 2];
            pixels[i + 2]   =   r;

        }
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

        glTexParameteri(GL_TEXTURE_2D,GL_TEXTURE_WRAP_S,GL_REPEAT);
        glTexParameteri(GL_TEXTURE_2D,GL_TEXTURE_WRAP_T,GL_REPEAT);
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

        return  texId;
    }

    void    renderGround()
    {
        CELL::matrix4   matModel(1);
        CELL::matrix4   matView =   _camera3rd.getView();
        CELL::matrix4   matProj =   _camera3rd.getProject();

        float   gSize   =   100;
        float   gPos    =   0;
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

        _shader.begin();
        {
            CELL::matrix4   MVP     =   matProj * matView * matModel;

            glUniform1i(_shader._texture,0);
            //! ���Ƶ���
            glBindTexture(GL_TEXTURE_2D,_texGround);
            glUniformMatrix4fv(_shader._MVP, 1, false, MVP.data());

            glVertexAttribPointer(_shader._positionAttr,3,  GL_FLOAT,   false,  sizeof(Vertex),&grounds[0].x);
            glVertexAttribPointer(_shader._uvAttr,2,        GL_FLOAT,   false,  sizeof(Vertex),&grounds[0].u);
            glVertexAttribPointer(_shader._colorAttr,4,     GL_FLOAT,   false,  sizeof(Vertex),&grounds[0].r);

            glDrawArrays(GL_TRIANGLES,0,sizeof(grounds) / sizeof(grounds[0]) );

        }
        _shader.end();
    }
    /**
    *   ��д���ƺ���
    */
    virtual void    render(const CELLFrameEvent& evt)
    {
        glClear(GL_DEPTH_BUFFER_BIT | GL_COLOR_BUFFER_BIT);
        glViewport(0,0,_winWidth,_winHeight);

        
        //! ���¶�ʱ��
        _timeStamp.update();

        _camera3rd.setTarget(_role._pos);
        _camera3rd.update();


        CELL::matrix4   matView =   _camera3rd.getView();
        CELL::matrix4   matProj =   _camera3rd.getProject();
        CELL::matrix4   matMVP  =   matProj * matView;
        CELL::matrix4   temp    =   matMVP.transpose();
        _frustum.loadFrustum(temp);

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
        //renderGround();
        glBindTexture(GL_TEXTURE_2D,_textureId);
        _role.render(evt._sinceLastFrame,_camera3rd,_shader);

        glBindTexture(GL_TEXTURE_2D,_texGround);
        _terrain.render(evt,_camera3rd);
        
    }

    virtual int     events(unsigned msg, unsigned wParam, unsigned lParam)
    {  
        switch( msg )
        {
        case WM_SIZE:
            {
                RECT    rt;
                GetClientRect(_hWnd,&rt);
                _winWidth   =   rt.right - rt.left;
                _winHeight  =   rt.bottom - rt.top;

                _camera3rd.perspective(45.0f, float(_winWidth) / float(_winHeight), 0.1f, 100000.0f);
                _camera3rd.setViewSize(float(_winWidth),float(_winHeight));
            }
            break;
        case WM_LBUTTONDOWN:
            {
                //! ����
                int x   =   GET_X_LPARAM(lParam);
                int y   =   GET_Y_LPARAM(lParam);
                Ray ray =   _camera3rd.createRayFromScreen(x,y);
                float3  dir     =   ray.getDirection();
                float3  pos     =   ray.getOrigin();
                float   tm      =   abs((pos.y) / dir.y);
                float3  tp      =   ray.getPoint(tm);
                _role.setTarget(tp);
            }
            break;
        case WM_RBUTTONDOWN:
            {
                int x   =   GET_X_LPARAM(lParam);
                int y   =   GET_Y_LPARAM(lParam);
                _bRightFlg  =   true;
                _rightDown  =   float2(x,y);
            }
            break;
        case WM_RBUTTONUP:
            {
                _bRightFlg  =   false;
            }
            break;
        case WM_MOUSEMOVE:
            {
                if (_bRightFlg)
                {
                    int x   =   GET_X_LPARAM(lParam);
                    int y   =   GET_Y_LPARAM(lParam);

                    float2  offset  =   float2(x,y) - _rightDown;
                    _rightDown =   float2(x,y);
                    _camera3rd.rotateView(offset.x * 0.1f);
                    _camera3rd.update();   
                }
            }
            break;
        case WM_MOUSEWHEEL:
            {
                int delta   =   GET_WHEEL_DELTA_WPARAM(wParam);
                if (delta > 0)
                {
                    _camera3rd.setRadius(_camera3rd.getRadius() * 1.2f);
                }
                else if (delta  < 0)
                {
                    _camera3rd.setRadius(_camera3rd.getRadius() * 0.8f);
                }
            }
            break;
        }
        return  __super::events(msg,wParam,lParam);
    }
};