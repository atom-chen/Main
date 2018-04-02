#pragma once

#include "CELLMath.hpp"

namespace   CELL
{
    class   CELLNode
    {
    public:
        enum    FLAG
        {
            FLAG_UPDATE     =   1<<0,
            FLAG_VISIBLE    =   1<<1,
        };
    public:
        CELL::quatr         _quat;
        CELL::float3        _scale;
        CELL::float3        _pos;
        CELL::matrix4       _local;
        CELL::aabbr         _aabb;
        unsigned            _flag;
        CELLRenderable*     _renderable;
    public:
        CELLNode(void* user = 0)
        {
            _pos            =   CELL::float3(0,0,0);
            _scale          =   CELL::float3(1,1,1);
            _quat           =   angleAxis(float(0),float3(1,1,1));
            _local          =   makeTransform(_pos,_scale,_quat);
            _flag           =   FLAG_UPDATE;
            _renderable     =   0;
        }
        virtual ~CELLNode()
        {
        }
        /**
        *   设置位置
        */
        void    setPosition(const float3& pos)
        {
            _pos    =   pos;
            _flag           |=  FLAG_UPDATE;
        }
        /**
        *   获取位置
        */
        const float3&    getPosition() const
        {
            return  _pos;
        }
        /**
        *   设置缩放比例
        */
        void    setScale(const float3& scale)
        {
            _scale  =   scale;
            _flag   |=  FLAG_UPDATE;
        }
        /**
        *   获取缩放比例
        */
        const float3&    getScale() const
        {
            return  _scale;
        }
        /**
        *   设置旋转角度
        */
        void    setQuat(const quatr& quat)
        {
            _quat   =   quat;
            _flag   |=  FLAG_UPDATE;
        }
        /**
        *   获取旋转数据
        */
        const quatr&getQuat() const 
        {
            return  _quat;
        }


        const aabbr&getAABB() const 
        {
            return  _aabb;
        }
        /**
        *   设置渲染对象
        */
        void    setRenderable(CELLRenderable* renderable)
        {
            _renderable =   renderable;
            _aabb       =   renderable->getBound();
            _aabb.transform(_local);
            _flag       |=  FLAG_UPDATE;
        }
        /**
        *   获取渲染对象
        */
        CELLRenderable*    getRenderable()
        {
            return  _renderable;
        }
        /**
        *   获取节点的局部矩阵
        */
        const matrix4&  getMatrix() const
        {
            return  _local;
        }
        /**
        *   设置更新标志为
        */
        void    setUpdate(bool flag = true)
        {
            if (flag)
            {
                _flag   |=  FLAG_UPDATE;
            }
            else
            {
                _flag   &=  ~FLAG_UPDATE;
            }
            
        }
        bool    needUpdate()
        {
            return  _flag & FLAG_UPDATE;
        }
        /**
        *   设置是否可见
        */
        void    setVisible(bool flag)
        {
            if (flag)
            {
                _flag   |=  FLAG_VISIBLE;
            }
            else
            {
                _flag   &=  ~FLAG_VISIBLE;
            }
        }
        /**
        *   是否可见
        */
        bool    getVisible() const
        {
            return  (_flag & FLAG_VISIBLE) ? true : false;
        }
        /**
        *   设置标志为
        */
        void    setFlag(unsigned flag,bool merge = true)
        {
            if (merge)
            {
                _flag   |=  flag;
            }
            else
            {
                _flag   =   flag;
            }
        }
        /**
        *   获取标志位
        */
        unsigned    getFlag() const
        {
            return  _flag;
        }
        /**
        *   更新矩阵
        */
        void        update(bool force = false)
        {
            if (force)
            {
                _local  =   makeTransform(_pos,_scale,_quat);
                _aabb   =   _renderable->getBound();
                _aabb.transform(_local);
            }
            else if (_flag & FLAG_UPDATE)
            {
                _local  =   makeTransform(_pos,_scale,_quat);
                _aabb   =   _renderable->getBound();
                _aabb.transform(_local);
            }
        }

        /**
        *   绘制函数
        */
        virtual void    doRender(const CELLFrameEvent& evt,CELL3RDCamera& camera,const matrix4& model)
        {
            if (_renderable)
            {
                _renderable->render(evt,camera,model);
            }
        }
    };
}