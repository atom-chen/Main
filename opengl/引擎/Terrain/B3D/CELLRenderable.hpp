#pragma once

#include "CELLMath.hpp"
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
        *   ����İ�Χ����
        */
        const CELL::aabbr&getBound()
        {
            return  _aabb;
        }
        
        /**
        *   ���ƺ���
        */
        virtual void    render(const CELLFrameEvent& evt,CELL3RDCamera& camera,const matrix4& model)
        {}
    };
}