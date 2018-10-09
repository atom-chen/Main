#pragma once

#include "CELL3RDCamera.hpp"
#include "CELLFrameEvent.hpp"

namespace   CELL
{
    class   CELLRenderable
    {
    protected:
        CELL::aabbr _aabb;
    public:
        CELLRenderable()
        {
        }
        virtual ~CELLRenderable()
        {}
        /**
        *   物体的包围盒子
        */
        const CELL::aabbr&getBound()
        {
            return  _aabb;
        }
        
        /**
        *   绘制函数
        */
        virtual void    render(const CELLFrameEvent& evt,CELL3RDCamera& camera,const matrix4& model)
        {}
    };
}