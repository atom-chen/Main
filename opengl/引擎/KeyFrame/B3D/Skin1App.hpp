#pragma once

#include "CELLMath.hpp"
#include "CELLWinApp.hpp"
#include "CELLTimestamp.hpp"

using namespace CELL;

/**
*   ����һ���������Ϣ
*/

struct Vertex
{
    //! ��ɫ
    float r, g, b, a;
    //! λ��
    float x, y, z;
    //! Ӱ���
    float weights[2];
    //! ���������
    short matrixIndices[2];
    //! Ӱ����������Ĺ�ͷ����
    short numBones;
};

Vertex g_quadVertices[12] =
{
    { 1.0f,1.0f,0.0f,1.0f,   -1.0f,0.0f,0.0f,  1.0f,0.0f,  0,0,  1 }, // ��ɫ
    { 1.0f,1.0f,0.0f,1.0f,    1.0f,0.0f,0.0f,  1.0f,0.0f,  0,0,  1 },
    { 1.0f,1.0f,0.0f,1.0f,    1.0f,2.0f,0.0f,  0.5f,0.5f,  0,1,  2 },
    { 1.0f,1.0f,0.0f,1.0f,   -1.0f,2.0f,0.0f,  0.5f,0.5f,  0,1,  2 },

    { 0.0f,1.0f,0.0f,1.0f,   -1.0f,2.0f,0.0f,  0.5f,0.5f,  0,1,  2 }, // ��ɫ
    { 0.0f,1.0f,0.0f,1.0f,    1.0f,2.0f,0.0f,  0.5f,0.5f,  0,1,  2},
    { 0.0f,1.0f,0.0f,1.0f,    1.0f,4.0f,0.0f,  0.5f,0.5f,  0,1,  2 },
    { 0.0f,1.0f,0.0f,1.0f,   -1.0f,4.0f,0.0f,  0.5f,0.5f,  0,1,  2 },

    { 0.0f,0.0f,1.0f,1.0f,   -1.0f,4.0f,0.0f,  0.5f,0.5f,  0,1,  2 }, // ��ɫ
    { 0.0f,0.0f,1.0f,1.0f,    1.0f,4.0f,0.0f,  0.5f,0.5f,  0,1,  2 },
    { 0.0f,0.0f,1.0f,1.0f,    1.0f,6.0f,0.0f,  1.0f,0.0f,  1,0,  1 },
    { 0.0f,0.0f,1.0f,1.0f,   -1.0f,6.0f,0.0f,  1.0f,0.0f,  1,0,  1 }
};


struct  BoneVertex
{
    float   x,y,z;
    float   r,g,b,a;
};
/**
*   ���ƹ���ʹ�õ��Ķ�������
*/

BoneVertex   arBone[]    =   
{
    {0.0f, 0.0f, 0.0f,0,0,1,1},	
    {-0.2f, 0.2f,-0.2f,0,0,1,1},
    {0.2f, 0.2f,-0.2f,0,0,1,1},
    {0.0f, 3.0f, 0.0f,0,0,1,1},
    {-0.2f, 0.2f,-0.2f,0,0,1,1},
    {-0.2f, 0.2f, 0.2f,0,0,1,1},
    {0.0f, 0.0f, 0.0f,0,0,1,1},
    {0.2f, 0.2f,-0.2f,0,0,1,1},
    {0.2f, 0.2f, 0.2f,0,0,1,1},
    {0.0f, 0.0f, 0.0f,0,0,1,1},
    {-0.2f, 0.2f, 0.2f,0,0,1,1},
    {0.0f, 3.0f, 0.0f,0,0,1,1},
    {0.2f, 0.2f, 0.2f,0,0,1,1},
    {-0.2f, 0.2f, 0.2f,0,0,1,1},
};

/**
*   ������һЩ�е�֡��ɣ�����һ֡���ݵ���Ϣ
*/
class   Frame
{
public:
    tmat4x4<float> _bone[2];
};


/**
*   ����һ����������
*/
class   SkinAnimation
{
public:
    void    calcFrame(float t,Frame& frame)
    {

        frame._bone[0]  =   interpolate(_keyFrame[0]._bone[0],_keyFrame[1]._bone[0],t);
        frame._bone[1]  =   interpolate(_keyFrame[0]._bone[1],_keyFrame[1]._bone[1],t);

    }
public:
    /**
    *   �ö�������֡����
    */
    Frame   _keyFrame[2];
};


class   Skin1App :public CELLWinApp
{
public:
    PROGRAM_P3_C4   _shader;
    SkinAnimation   _skinAnima;
    tmat4x4<float>  _matrixToRenderBone[2];
    float           _timeElpased;
    CELLTimestamp   _timeStamp;

public:
    Skin1App()
    {
        _timeElpased    =   0;
        /**
        *   ��һ���ؼ�֡
        */
        _skinAnima._keyFrame[0]._bone[0].identify();
        _skinAnima._keyFrame[0]._bone[1].identify();

        _skinAnima._keyFrame[1]._bone[0].identify();
        _skinAnima._keyFrame[1]._bone[1].identify();


        tmat4x4<float> rotationMatrixY;
        tmat4x4<float> rotationMatrixZ;

        rotationMatrixY.rotateY( 60);
        rotationMatrixZ.rotateZ(-60);
        _skinAnima._keyFrame[1]._bone[1]    =   (rotationMatrixY * rotationMatrixZ);
    }
public:

    //! ��д��ʼ������
    virtual void    onInit()
    {
        _shader.initialize();
    }
    /**
    *   ���ƺ���
    */
    virtual void    render()
    {
        glClear(GL_DEPTH_BUFFER_BIT | GL_COLOR_BUFFER_BIT);
        glViewport(0,0,_winWidth,_winHeight);

        float   elapseTime   =   (float)_timeStamp.getElapsedSecond();
        //! ���¶�ʱ��
        _timeStamp.update();

        /**
        *   �����������Ĺ���֡����
        *   ��֮֡����в�ֵ����õ��Ĺ���֡����
        */
        Frame   frame;
        _timeElpased    +=  elapseTime;
        /**
        *   ���ݹؼ�֡��������µĹ���λ��
        */
        _skinAnima.calcFrame(_timeElpased,frame);
        {
            /**
            *   ����ļ�����Ҫ�ǻ��ƹ�ͷʹ��
            */
            tmat4x4<float> offsetMatrix_toBoneEnd;
            tmat4x4<float> offsetMatrix_backFromBoneEnd;
            
            offsetMatrix_toBoneEnd.translate(0.0f, 3.0f,0.0f );
            offsetMatrix_backFromBoneEnd.translate(0.0f, -3.0f ,0.0f);

            _matrixToRenderBone[0]  =   frame._bone[0];


            CELL::matrix4   temp    =   frame._bone[1];
            frame._bone[1]          =   offsetMatrix_toBoneEnd * frame._bone[1];
            _matrixToRenderBone[1]  =   offsetMatrix_toBoneEnd * temp;
            frame._bone[1]          =   frame._bone[1] * offsetMatrix_backFromBoneEnd;
        }
        Vertex  calQuadVertices[12];
        memcpy(calQuadVertices,g_quadVertices,sizeof(g_quadVertices));
        for (int i = 0 ;i < 12 ; ++ i )
        {
            tvec3<float>    vec(0,0,0);
            tvec3<float>    vecSrc(g_quadVertices[i].x,g_quadVertices[i].y,g_quadVertices[i].z);
            for (int x = 0 ; x < calQuadVertices[i].numBones ; ++ x)
            {
                //! ����λ��
                tvec3<float>    temp    =   vecSrc* frame._bone[g_quadVertices[i].matrixIndices[x]];
                //! ����Ȩ��λ��
                vec  += temp * g_quadVertices[i].weights[x];
            }
            calQuadVertices[i].x    =   vec.x;
            calQuadVertices[i].y    =   vec.y;
            calQuadVertices[i].z    =   vec.z;
        }

        _shader.begin();
        {
            
            CELL::matrix4   matProj =   CELL::perspective(45.0f, (GLfloat)_winWidth / (GLfloat)_winHeight, 0.1f, 100.0f);
            CELL::matrix4   model(1);
            CELL::matrix4   view(1);

            model.translate(0.0f, 0.0f, -20.0f);

            CELL::matrix4   mvp =   matProj * model * view;
            glUniformMatrix4fv(_shader._MVP, 1, false, mvp.data());

            glVertexAttribPointer(_shader._positionAttr,3,  GL_FLOAT,   false,  sizeof(Vertex),&calQuadVertices[0].x);
            glVertexAttribPointer(_shader._colorAttr,4,     GL_FLOAT,   false,  sizeof(Vertex),&calQuadVertices[0].r);

            for (int i = 0 ;i < 3 ; ++ i )
            {
                glDrawArrays(GL_LINE_LOOP,i * 4,4);
            }

            CELL::matrix4   matBone0    =   matProj * (model* _matrixToRenderBone[0]) * view;
            CELL::matrix4   matBone1    =   matProj * (model* _matrixToRenderBone[1]) * view;
            glUniformMatrix4fv(_shader._MVP, 1, false, matBone0.data());

            glVertexAttribPointer(_shader._positionAttr,3,  GL_FLOAT,   false,  sizeof(BoneVertex),&arBone[0].x);
            glVertexAttribPointer(_shader._colorAttr,4,     GL_FLOAT,   false,  sizeof(BoneVertex),&arBone[0].r);
            glDrawArrays(GL_LINE_STRIP,0,sizeof(arBone)/sizeof(arBone[0]));

            glUniformMatrix4fv(_shader._MVP, 1, false, matBone1.data());
            glDrawArrays(GL_LINE_STRIP,0,sizeof(arBone)/sizeof(arBone[0]));

        }
        _shader.end();

        eglSwapBuffers(_display, _surface);

    }
};