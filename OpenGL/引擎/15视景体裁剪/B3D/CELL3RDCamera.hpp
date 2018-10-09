#pragma once

#include    "CELLMath.hpp"
namespace   CELL
{
    class   CELL3RDCamera
    {
    public:
        float3      _eye;
        float3      _up;
        float3      _right;
        float3      _target;
        float3      _dir;
        float       _radius;
        matrix4     _matView;
        matrix4     _matProj;
        matrix4     _matWorld;
        float2      _viewSize;

        float       _yaw;

    public:
        CELL3RDCamera()
        {
            _radius     =   400;
            _yaw        =   0;
            _viewSize   =   float2(100,100);
            _matView.identify();
            _matProj.identify();
            _matWorld.identify();
        }

        float   getRadius() const 
        {
            return _radius;
        }
        void    setRadius(float val)
        { 
            _radius = val; 
        }

        CELL::float3 getEye() const 
        { 
            return _eye;
        }
        /**
        *   设置眼睛的位置
        */
        void    setEye(CELL::float3 val)
        { 
            _eye    =   val; 
        }
        /**
        *   计算方向
        */
        void    calcDir()
        {
            _dir    =   _target - _eye;	
            _dir    =   normalize(_dir);
        }

        CELL::float3 getTarget() const 
        { 
            return _target;
        }

        void    setTarget(CELL::float3 val) 
        { 
            _target = val;
        }
        
        CELL::float3 getUp() const 
        { 
            return _up;
        }
        void    setUp(CELL::float3 val)
        {
            _up = val;
        }
        float3  getDir() const
        {
            return  _dir;
        }

        float3  getRight() const
        {
            return  _right;
        }
        void    update()
        {
            float3  upDir   =   normalize(_up);
            _eye        =   _target - _dir * _radius;
            _right      =   normalize(cross(_dir, upDir));

            _matView    =   CELL::lookAt(_eye,_target,_up);
        }

        void    setViewSize(const float2& viewSize)
        {
            _viewSize   =   viewSize;
        }
        void    setViewSize(float x,float y)
        {
            _viewSize   =   float2(x,y);
        }

        float2  getViewSize()
        {
            return  _viewSize;
        }
        void    ortho( float left, float right, float bottom, float top, float zNear, float zFar )
        {
            _matProj    =   CELL::ortho<float>(left,right,bottom,top,zNear,zFar);
        }

        void    setProject(const matrix4& proj)
        {
            _matProj    =   proj;
        }
        const matrix4& getProject() const
        {
            return  _matProj;
        }
        const matrix4&  getView() const
        {
            return  _matView;
        }

        void    perspective(float fovy, float aspect, float zNear, float zFar)
        {
            _matProj    =   CELL::perspective<float>(fovy,aspect,zNear,zFar);
        }


        /**
        *   世界坐标转化为窗口坐标
        */
        bool    project( const float4& world, float4& screen )
        {
            screen  =   (_matProj * _matView * _matWorld) * world;
            if (screen.w == 0.0f)
            {
                return false;
            }
            screen.x    /=  screen.w;
            screen.y    /=  screen.w;
            screen.z    /=  screen.w;

            // map to range 0 - 1
            screen.x    =   screen.x * 0.5f + 0.5f;
            screen.y    =   screen.y * 0.5f + 0.5f;
            screen.z    =   screen.z * 0.5f + 0.5f;

            // map to viewport
            screen.x    =   screen.x * _viewSize.x;
            screen.y    =   _viewSize.y - (screen.y * _viewSize.y);
            return  true;
        }


        /**
        *   世界坐标转化为窗口坐标
        */
        float2  wordToScreen( const float3& world)
        {
            float4  worlds(world.x,world.y,world.z,1);
            float4  screens;
            project(worlds,screens);
            return  float2(screens.x,screens.y);
        }

        /**
        *   窗口坐标转化为世界坐标
        */
        float3  screenToWorld(const float2& screen)
        {
            float4  screens(screen.x,screen.y,0,1);
            float4  world;
            unProject(screens,world);
            return  float3(world.x,world.y,world.z);
        }

        float3  screenToWorld(float x,float y)
        {
            float4  screens(x,y,0,1);
            float4  world;
            unProject(screens,world);
            return  float3(world.x,world.y,world.z);
        }


        /**
        *   窗口坐标转化为世界坐标
        */
        bool    unProject( const float4& screen, float4& world )
        {
            float4 v;
            v.x =   screen.x;
            v.y =   screen.y;
            v.z =   screen.z;
            v.w =   1.0;

            // map from viewport to 0 - 1
            v.x =   (v.x) /_viewSize.x;
            v.y =   (_viewSize.y - v.y) /_viewSize.y;
            //v.y = (v.y - _viewPort.Y) / _viewPort.Height;

            // map to range -1 to 1
            v.x =   v.x * 2.0f - 1.0f;
            v.y =   v.y * 2.0f - 1.0f;
            v.z =   v.z * 2.0f - 1.0f;

            CELL::matrix4  inverse = (_matProj * _matView * _matWorld).inverse();

            v   =   v * inverse;
            if (v.w == 0.0f)
            {
                return false;
            }
            world   =   v / v.w;
            return true;
        }

        Ray createRayFromScreen(int x,int y)
        {
            float4  minWorld;
            float4  maxWorld;

            float4  screen(float(x),float(y),0,1);
            float4  screen1(float(x),float(y),1,1);

            unProject(screen,minWorld);
            unProject(screen1,maxWorld);
            Ray     ray;
            ray.setOrigin(float3(minWorld.x,minWorld.y,minWorld.z));

            float3  dir(maxWorld.x - minWorld.x,maxWorld.y - minWorld.y, maxWorld.z - minWorld.z);
            ray.setDirection(normalize(dir));
            return  ray;
        }
        /**
        *   下面的函数的功能是将摄像机的观察方向绕某个方向轴旋转一定的角度 
        *   改变观察者的位置，目标的位置不变化
        */
        void    rotateView(float angle) 
        { 
            _dir    =   rotateY<float>(_dir,angle);
        }
    };
}