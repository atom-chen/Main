#pragma once

#include "CELLWinApp.hpp"
#include "CELLTimestamp.hpp"
#include "CELL3RDCamera.hpp"
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
    float   _alpha;
    float   _opt;
public:
    Role()
    {
        _speed  =   5;
        _alpha  =   1;
        _opt    =   1;
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

        
        if (_alpha > 1)
            _opt = -1;
        else if( _alpha < 0)
            _opt = 1;
        _alpha  +=  fElapsed * _opt;
        
        Vertex vertexs[] =
        {
            { -1.0f,-1.0f, 1.0f,0.0f, 0.0f,1.0f, 1.0f, 1.0f,_alpha },
            {  1.0f,-1.0f, 1.0f,1.0f, 0.0f,1.0f, 1.0f, 1.0f,_alpha },
            {  1.0f, 1.0f, 1.0f,1.0f, 1.0f,1.0f, 1.0f, 1.0f,_alpha },

            { -1.0f,-1.0f, 1.0f,0.0f, 0.0f,1.0f, 1.0f, 1.0f,_alpha },
            {  1.0f, 1.0f, 1.0f,1.0f, 1.0f,1.0f, 1.0f, 1.0f,_alpha },
            { -1.0f, 1.0f, 1.0f,0.0f, 1.0f,1.0f, 1.0f, 1.0f,_alpha },
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
class   AlphaBlendApp :public CELLWinApp
{
public:
    PROGRAM_P2_T2_C3    _shader;
    unsigned            _textureId;
    unsigned            _texGround;
    unsigned            _texGrass;
    CELLTimestamp       _timeStamp;
    CELL3RDCamera       _camera3rd;
    Role                _role;
    float2              _rightDown;
    bool                _bRightFlg;
public:
    AlphaBlendApp()
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
        _shader.initialize();
        glEnable(GL_DEPTH_TEST);
        glEnable(GL_BLEND);
        glBlendFunc(GL_SRC_ALPHA, GL_ONE_MINUS_SRC_ALPHA);
        //glBlendFunc((GL_SRC_ALPHA,GL_ONE_MINUS_SRC_ALPHA))
        _textureId  =   loadTexture("res//woodfloor.tga");
        _texGround  =   loadTexture("res//1.jpg");
        _texGrass   =   loadTexture("res//grass.png");

        _camera3rd.setRadius(50);
        _camera3rd.perspective(45.0f, float(_winWidth) / float(_winHeight), 0.1f, 100000.0f);
        _camera3rd.setEye(float3(50,50,50));
        _camera3rd.setTarget(_role._pos);
        _camera3rd.calcDir();
        _camera3rd.setUp(float3(0,1,0));

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
        FIBITMAP    *dib=   FreeImage_Load(fifmt, fileName,0);
        unsigned    fmt =   GL_RGB;

        if (FreeImage_GetColorType(dib) == FIC_RGBALPHA)
        {
            fmt =   GL_RGBA;
        }
        else
        {
            dib     =   FreeImage_ConvertTo24Bits(dib);
        }

        int     width   =   FreeImage_GetWidth(dib);
        int     height  =   FreeImage_GetHeight(dib);

        //4 ��ȡ����ָ��
        BYTE    *pixels =   (BYTE*)FreeImage_GetBits(dib);

        if (fmt == GL_RGBA )
        {
            for (int i = 0 ; i < width * height * 4 ; i += 4 )
            {
                BYTE r = pixels[i];
                pixels[i]       =   pixels[i + 2];
                pixels[i + 2]   =   r;

            }
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
        /**
        *   ��ͼƬ��rgb�����ϴ���opengl.
        */
        glTexImage2D( 
            GL_TEXTURE_2D,      //! ָ���Ƕ�άͼƬ
            0,                  //! ָ��Ϊ��һ�������������mipmap,��lod,����ľͲ��ü����ģ�Զ��ʹ�ý�С������
            fmt,                //! �����ʹ�õĴ洢��ʽ
            width,              //! ��ȣ���һ����Կ�����֧�ֲ��������������Ⱥ͸߶Ȳ���2^n��
            height,             //! ��ȣ���һ����Կ�����֧�ֲ��������������Ⱥ͸߶Ȳ���2^n��
            0,                  //! �Ƿ�ı�
            fmt,                //! ���ݵĸ�ʽ��bmp�У�windows,����ϵͳ�д洢��������bgr��ʽ
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

    void    renderTree()
    {

    }
    /**
    *   ��д���ƺ���
    */
    virtual void    render()
    {
        glClear(GL_DEPTH_BUFFER_BIT | GL_COLOR_BUFFER_BIT);
        glViewport(0,0,_winWidth,_winHeight);

        
        float   elapseTime   =   (float)_timeStamp.getElapsedSecond();
        //! ���¶�ʱ��
        _timeStamp.update();

        _camera3rd.setTarget(_role._pos);
        _camera3rd.update();

        CELL::matrix4   matView =   _camera3rd.getView();
        CELL::matrix4   matProj =   _camera3rd.getProject();
        
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
        renderGround();
        glBindTexture(GL_TEXTURE_2D,_texGrass);
        //! ���ƽ�ɫ
        _role.render(elapseTime,_camera3rd,_shader);
        eglSwapBuffers(_display, _surface);
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