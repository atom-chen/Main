#pragma once

#include "CELLMath.hpp"
#include <CELLFrameEvent.hpp>
#include "CELLRenderable.hpp"

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
        *   ����λ��
        */
        void    setPosition(const float3& pos)
        {
            _pos    =   pos;
            _flag           |=  FLAG_UPDATE;
        }
        /**
        *   ��ȡλ��
        */
        const float3&    getPosition() const
        {
            return  _pos;
        }
        /**
        *   �������ű���
        */
        void    setScale(const float3& scale)
        {
            _scale  =   scale;
            _flag   |=  FLAG_UPDATE;
        }
        /**
        *   ��ȡ���ű���
        */
        const float3&    getScale() const
        {
            return  _scale;
        }
        /**
        *   ������ת�Ƕ�
        */
        void    setQuat(const quatr& quat)
        {
            _quat   =   quat;
            _flag   |=  FLAG_UPDATE;
        }
        /**
        *   ��ȡ��ת����
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
        *   ������Ⱦ����
        */
        void    setRenderable(CELLRenderable* renderable)
        {
            _renderable =   renderable;
            _aabb       =   renderable->getBound();
            _aabb.transform(_local);
            _flag       |=  FLAG_UPDATE;
        }
        /**
        *   ��ȡ��Ⱦ����
        */
        CELLRenderable*    getRenderable()
        {
            return  _renderable;
        }
        /**
        *   ��ȡ�ڵ�ľֲ�����
        */
        const matrix4&  getMatrix() const
        {
            return  _local;
        }
        /**
        *   ���ø��±�־Ϊ
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
        *   �����Ƿ�ɼ�
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
        *   �Ƿ�ɼ�
        */
        bool    getVisible() const
        {
            return  (_flag & FLAG_VISIBLE) ? true : false;
        }
        /**
        *   ���ñ�־Ϊ
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
        *   ��ȡ��־λ
        */
        unsigned    getFlag() const
        {
            return  _flag;
        }
        /**
        *   ���¾���
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
        *   ���ƺ���
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