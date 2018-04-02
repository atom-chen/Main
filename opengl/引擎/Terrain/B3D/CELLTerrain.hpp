#pragma once

#include "CELLMath.hpp"
#include "CELLFrameEvent.hpp"
#include "CELLShader.hpp"

namespace   CELL
{
    struct  TerrainVertex
    {
        float3      pos;
        float2      uv;
        float3      nxyz;
    };

    class   CELLTerrain
    {
    public:
        unsigned            _VBO;
        unsigned            _IBO;
        unsigned            _STEP;
        unsigned            _SIZE;
        float2              _scale;
        PROGRAM_DirLight    _shader;
    public:
        CELLTerrain()
        {
            _STEP       =   16;
            _SIZE       =   1024;
            _scale      =   float2(4,4);
            
        }
        /**
        *   读取地形文件数据
        */
        void    loadTerrain(const char* terrainFile,int w,int h)
        {
            FILE*   pFile   =   fopen( terrainFile, "rb" );
            if (pFile == 0)
            {
                return;
            }
            unsigned char*  terrain =   new unsigned char[w * h];
            fread(terrain,1,w*h,pFile);
            fclose(pFile);

            _shader.initialize();
            buildTerrain(terrain);
            delete []terrain;
        }

        /**
        *   获取高度数据
        */
        int height(unsigned char* pHeightMap, int X, int Y)
        {
            int x = X % _SIZE;
            int y = Y % _SIZE;

            if(!pHeightMap)
            {
                return 0;
            }
            return pHeightMap[x + (y * _SIZE)] - 200;
        }

        float3 faceNormal(float3 vertex1, float3 vertex2, float3 vertex3)   
        {   
            float3 normal;   
            float3 vector1	=	vertex2-vertex1;   
            float3 vector2	=	vertex3-vertex1;   
            //求叉积   
            normal  =   cross(vector1, vector2);   
            normal  =   normalize(normal);   
            return normal;   
        } 
        /**
        *   产生地形绘制数据
        */
        void    buildTerrain(unsigned char *pHeightMap)
        {
            
            unsigned        arSize  =   _SIZE/_STEP * _SIZE/_STEP;
            unsigned        tileSz  =   _SIZE/_STEP;
            TerrainVertex*  verts   =   new TerrainVertex[arSize];
            for (int y = 0 ; y < _SIZE/_STEP ; ++ y)
            {
                for (int x = 0 ; x < _SIZE/_STEP ; ++ x)
                {
                    verts[y * tileSz + x].pos   =   float3(x * _scale.x,height(pHeightMap,x,y),y * _scale.y);
                    //verts[y * tileSz + x].uv    =   float2(x/8,y/8);
                    verts[y * tileSz + x].uv    =   float2(float(x)/float(tileSz) * 64,float(y)/float(tileSz) * 64);
                }
            }

            typedef short Face[3];

            int     faceIndex   =   0;
            int     faceSize    =   (tileSz - 1) * (tileSz - 1) * 2;
            Face*   face        =   new Face[faceSize];
            for (int y = 0 ; y < tileSz - 1; ++ y)
            {
                for (int x = 0 ; x < tileSz - 1; ++ x )
                {
                    face[faceIndex + 0][0] =   short(y * tileSz+ x);
                    face[faceIndex + 0][1] =   short(y * tileSz + x + 1);
                    face[faceIndex + 0][2] =   short((y + 1) * tileSz + x);

                    

                    face[faceIndex + 1][0] =   short((y + 1) * tileSz + x);
                    face[faceIndex + 1][1] =   short(y * tileSz + x + 1);
                    face[faceIndex + 1][2] =   short((y + 1) * tileSz + x + 1);

                    //! 计算法线
                    float3  nxyz    =   faceNormal(verts[face[faceIndex + 0][0]].pos,verts[face[faceIndex + 0][1]].pos,verts[face[faceIndex + 0][2]].pos);
                    verts[face[faceIndex + 0][0]].nxyz  =   nxyz;
                    verts[face[faceIndex + 0][1]].nxyz  =   nxyz;
                    verts[face[faceIndex + 0][2]].nxyz  =   nxyz;

                    float3  nxyz1   =   faceNormal(verts[face[faceIndex + 1][0]].pos,verts[face[faceIndex + 1][1]].pos,verts[face[faceIndex + 1][2]].pos);
                    verts[face[faceIndex + 1][0]].nxyz  =   nxyz1;
                    verts[face[faceIndex + 1][1]].nxyz  =   nxyz1;
                    verts[face[faceIndex + 1][2]].nxyz  =   nxyz1;

                    faceIndex       +=  2;
                }
            }

            glGenBuffers(1,&_VBO);
            glBindBuffer(GL_ARRAY_BUFFER,_VBO);

            glBufferData(GL_ARRAY_BUFFER, sizeof(TerrainVertex) *  arSize, verts,GL_STATIC_DRAW);
            glBindBuffer(GL_ARRAY_BUFFER,0);
            delete[]verts;
            

            glGenBuffers(1,&_IBO);
            glBindBuffer(GL_ELEMENT_ARRAY_BUFFER,_IBO);
            
            glBufferData(GL_ELEMENT_ARRAY_BUFFER, sizeof(Face) * faceSize, face,GL_STATIC_DRAW);
            glBindBuffer(GL_ELEMENT_ARRAY_BUFFER,0);
            delete []face;

        }

        /**
        *   绘制函数
        */
        void matrix4ToMatrix3(CELL::matrix3 & t, const CELL::matrix4 & src)
        {
            t[0][0] = src[0][0];
            t[0][1] = src[0][1];
            t[0][2] = src[0][2];
            t[1][0] = src[1][0];
            t[1][1] = src[1][1];
            t[1][2] = src[1][2];
            t[2][0] = src[2][0];
            t[2][1] = src[2][1];
            t[2][2] = src[2][2];
        }

        /**
        *   绘制函数
        */
        void    render(const CELLFrameEvent& evt,const CELL3RDCamera& camera)
        {
            CELL::matrix4   MVP     =   camera.getProject() * camera.getView();

            glBindBuffer(GL_ARRAY_BUFFER,_VBO);
            glBindBuffer(GL_ELEMENT_ARRAY_BUFFER,_IBO);

            _shader.begin();
            {
                CELL::matrix4   matModel(1);
                CELL::matrix4   matView =   camera.getView();
                CELL::matrix4   matProj =   camera.getProject();

                CELL::matrix4   MVP     =    matView * matModel;

                CELL::matrix3   matNor(1);
                matrix4ToMatrix3(matNor,MVP);
                glUniform1i(_shader._texture,0);
                //! 绘制地面
                glUniformMatrix4fv(_shader._mv, 1, false, MVP.data());
                glUniformMatrix4fv(_shader._project, 1, false, matProj.data());
                glUniformMatrix3fv(_shader._normalMat, 1, false, &matNor[0][0]);
                glUniform3f(_shader._ambientColor, 0.8f,0.8f,0.8f);
                glUniform3f(_shader._lightDirection, 0.0f,1.0f,0.0f);
                glUniform3f(_shader._diffuseColor, 1,1,1);


                glVertexAttribPointer(_shader._position,3,   GL_FLOAT,   false,  sizeof(TerrainVertex),0);
                glVertexAttribPointer(_shader._uv,2,         GL_FLOAT,   false,  sizeof(TerrainVertex),(void*)12);
                glVertexAttribPointer(_shader._normal,3,     GL_FLOAT,   false,  sizeof(TerrainVertex),(void*)20);
                
                glDrawElements(GL_TRIANGLES,(_SIZE/_STEP - 1) * (_SIZE/_STEP - 1) * 2 * 3,GL_UNSIGNED_SHORT,0);

            }
            glBindBuffer(GL_ARRAY_BUFFER,0);
            glBindBuffer(GL_ELEMENT_ARRAY_BUFFER,0);
        }
    };
}