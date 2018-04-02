#pragma once

#include "CELLMath.hpp"
#include "CELLShader.hpp"

namespace   CELL
{
    class   CELLBillboard
    {
    public:
         /**
        *   大小
        */
        float2      _size;
        /**
        *   位置
        */
        float3      _pos;
        /**
        *   颜色信息
        */
        Rgba4Byte   _color;
    public:

        void    render(const CELLFrameEvent& evt,const CELL3RDCamera& camera,PROGRAM_P3_T2_C4& prg)
        {

            struct  ObjectVertex
            {
                float3      _pos;
                float2      _uv;
                Rgba4Byte   _color;
            };

            float3  faceDir =   camera.getRight();

            float3  lb      =   _pos - faceDir * _size.x * 0.5f;
            float3  rb      =   _pos + faceDir * _size.x * 0.5f;

            float3  lt      =   lb + float3(0,_size.y,0);
            float3  rt      =   rb + float3(0,_size.y,0) ;

            ObjectVertex vert[6] =   
            {
                {   lb, float2(0,0),   Rgba4Byte()},
                {   rb, float2(1,0),   Rgba4Byte()},
                {   rt, float2(1,1),   Rgba4Byte()},

                {   lb, float2(0,0),   Rgba4Byte()},
                {   rt, float2(1,1),   Rgba4Byte()},
                {   lt, float2(0,1),   Rgba4Byte()},
            };
            glVertexAttribPointer(prg._positionAttr,    3, GL_FLOAT,        GL_FALSE,   sizeof(ObjectVertex),vert);
            glVertexAttribPointer(prg._uvAttr,          2, GL_FLOAT,        GL_FALSE,   sizeof(ObjectVertex),(void*)&vert[0]._uv);
            glVertexAttribPointer(prg._colorAttr,       4, GL_UNSIGNED_BYTE,GL_TRUE,    sizeof(ObjectVertex),(void*)&vert[0]._color);
            glDrawArrays(GL_TRIANGLES,0,sizeof(vert) / sizeof(vert[0]) );
        }
    };
}