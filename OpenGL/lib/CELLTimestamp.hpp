#pragma once

namespace   CELL
{
    class   CELLTimestamp
    {
    protected:
        LARGE_INTEGER   _start;
        LARGE_INTEGER   _frequency;
    public:
        CELLTimestamp()
        {
            ::QueryPerformanceFrequency(&_frequency);
            ::QueryPerformanceCounter(&_start);
        }

        void    update()
        {
            ::QueryPerformanceCounter(&_start);
        }
        

        double   getElapsedSecond() const 
        {
            LARGE_INTEGER   cur;
            QueryPerformanceCounter(&cur);
            return  double(cur.QuadPart-_start.QuadPart)/double(_frequency.QuadPart);
        }

    };
}