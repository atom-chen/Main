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
    *   设置移动的目标点
    */
    void    setTarget(float3 target)
    {
        _target =   target;
    }
    /**
    *   更新位置
    */
    void    setPosition(float3 pos)
    {
        _pos    =   pos;
        _pos.y  =   1;
    }

    void moveCheck(const float elasped)
    {
        /**
        *   目标位置不是当前位置。
        */
        if (_target == _pos)
        {
            return;
        }

        /**
        *   获取当前玩家位置与目标位置的偏移量
        */
        float3  offset  =   _target - _pos;
        
        /**
        *   获取到移动的方向
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
    *   绘制角色
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
            //! 绘制地面
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


    //! 重写初始化函数
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
    *   从文件加载文理
    */
    virtual unsigned    loadTexture(const char* fileName)
    {
        unsigned    texId   =   0;
        //1 获取图片格式
        FREE_IMAGE_FORMAT fifmt = FreeImage_GetFileType(fileName, 0);

        //2 加载图片
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

        //4 获取数据指针
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
            fmt,                //! 纹理的使用的存储格式
            width,              //! 宽度，老一点的显卡，不支持不规则的纹理，即宽度和高度不是2^n。
            height,             //! 宽度，老一点的显卡，不支持不规则的纹理，即宽度和高度不是2^n。
            0,                  //! 是否的边
            fmt,                //! 数据的格式，bmp中，windows,操作系统中存储的数据是bgr格式
            GL_UNSIGNED_BYTE,   //! 数据是8bit数据
            pixels
            );
        /**
        *   释放内存
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
            //! 绘制地面
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
    *   重写绘制函数
    */
    virtual void    render()
    {
        glClear(GL_DEPTH_BUFFER_BIT | GL_COLOR_BUFFER_BIT);
        glViewport(0,0,_winWidth,_winHeight);

        
        float   elapseTime   =   (float)_timeStamp.getElapsedSecond();
        //! 更新定时器
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
        //! 绘制角色
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
                //! 鼠标点
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