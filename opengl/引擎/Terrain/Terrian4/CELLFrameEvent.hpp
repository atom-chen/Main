#pragma once

#include    "CELLMath.hpp"

namespace   CELL
{
    /**
    *   时间
    */
    class   CELLFrameEvent
    {
    public:
        CELLFrameEvent()
            :_sinceLastFrame(0)
            ,_sinceLastEvent(0)
        {}
        /**
        *   单位秒
        */
        real    _sinceLastFrame;
        /**
        *   单位秒
        */
        real    _sinceLastEvent;
    };
}


