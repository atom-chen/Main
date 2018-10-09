#pragma once

#include    "CELLMath.hpp"

namespace   CELL
{
    /**
    *   ʱ��
    */
    class   CELLFrameEvent
    {
    public:
        CELLFrameEvent()
            :_sinceLastFrame(0)
            ,_sinceLastEvent(0)
        {}
        /**
        *   ��λ��
        */
        real    _sinceLastFrame;
        /**
        *   ��λ��
        */
        real    _sinceLastEvent;
    };
}


