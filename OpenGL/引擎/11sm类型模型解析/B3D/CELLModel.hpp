#pragma once
#include "rapidxml.hpp"

namespace   CELL
{
    /**
    *   标准模型，pos,uv,normal
    */
    class   CELLModelStd
    {
    public:
        struct  Vertex
        {
            float   x,y,z;
            float   u,v;
            float   nx,ny,nz;
        };
    public:
        /// 索引缓冲区ID
        unsigned    _indexBufferId;
        /// 顶点缓冲ID
        unsigned    _vertexBufferId;
        /// 索引的数量
        unsigned    _indexSize;
    public:
        CELLModelStd()
        {
            _indexBufferId  =   0;
            _vertexBufferId =   0;
        }
        /**
        *   加载模型数据
        */
        bool    load(const char* fileName)
        {
            /// 读文件
            size_t  length  =   0;
            char*   xmlData =   readFile(fileName,length);
            if (xmlData == 0)
            {
                return  false;
            }

            try
            {
                rapidxml::xml_document<>    doc;
                rapidxml::xml_node<>*	    rootNode    =   0;
                rapidxml::xml_node<>*	    meshNode    =   0;
                rapidxml::xml_node<>*	    faceRoot    =   0;
                rapidxml::xml_node<>*	    vertRoot    =   0;
                doc.parse<0>(xmlData); 
                rootNode    =	doc.first_node("MeshRoot");
                if (rootNode == 0)
                {
                    return  false;
                }
                meshNode    =   rootNode->first_node();
                if (meshNode == 0)
                {
                    return  false;
                }

                /// 解析面索引
                faceRoot    =   meshNode->first_node("faceIndex");
                parseFaceIndex(faceRoot);
                vertRoot    =   meshNode->first_node("vertex");
                /// 解析顶点数据
                parseVertex(vertRoot);

                delete [] xmlData;
                return  true;
            }
            catch (...)
            {
                return  false;
            }
        }

        /**
        *   解析面信息
        */
        void    parseFaceIndex(rapidxml::xml_node<>* faceRoot)
        {
            std::vector<short>          arIndex;
            rapidxml::xml_node<>*       pFaceIndex  =   faceRoot->first_node();
            for ( ; pFaceIndex ; pFaceIndex = pFaceIndex->next_sibling())
            {
                const char* pzFace  =   pFaceIndex->value();
                int     a,b,c;
                sscanf(pzFace,"%d %d %d",&a,&b,&c);
                arIndex.push_back(short(a));
                arIndex.push_back(short(b));
                arIndex.push_back(short(c));
            }

            _indexSize  =   arIndex.size();
            glGenBuffers(1,&_indexBufferId);
            glBindBuffer(GL_ELEMENT_ARRAY_BUFFER,_indexBufferId);
            glBufferData(GL_ELEMENT_ARRAY_BUFFER,arIndex.size() * sizeof(short),&arIndex.front(),GL_STATIC_DRAW);
            glBindBuffer(GL_ELEMENT_ARRAY_BUFFER,0);
        }
        /**
        *   解析顶点信息
        */
        void    parseVertex(rapidxml::xml_node<>* vertRoot)
        {
            std::vector<Vertex>         arVert;
            rapidxml::xml_attribute<>*  attrSize    =   vertRoot->first_attribute("size");
            rapidxml::xml_node<>*       vertNode    =   vertRoot->first_node();
            for ( ; vertNode ; vertNode = vertNode->next_sibling())
            {
                const char* pzVert  =   vertNode->value();
                Vertex      vertex;
                sscanf(pzVert,"%f %f %f %f %f %f %f %f",&vertex.x,&vertex.y,&vertex.z,&vertex.u,&vertex.v,&vertex.nx,&vertex.ny,&vertex.nz);
                arVert.push_back(vertex);
            }
            glGenBuffers(1,&_vertexBufferId);
            glBindBuffer(GL_ARRAY_BUFFER,_vertexBufferId);
            glBufferData(GL_ARRAY_BUFFER,arVert.size() * sizeof(Vertex),&arVert.front(),GL_STATIC_DRAW);
            glBindBuffer(GL_ARRAY_BUFFER,0);
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
        void    render(float fElapsed,CELL3RDCamera& camera,PROGRAM_DirLight& shader)
        {
            
            glBindBuffer(GL_ARRAY_BUFFER,_vertexBufferId);
            glBindBuffer(GL_ELEMENT_ARRAY_BUFFER,_indexBufferId);
            shader.begin();
            {
                CELL::matrix4   matModel(1);
                CELL::matrix4   matView =   camera.getView();
                CELL::matrix4   matProj =   camera.getProject();

                CELL::matrix4   MVP     =    matView * matModel;

                CELL::matrix3   matNor(1);
                matrix4ToMatrix3(matNor,MVP);
                glUniform1i(shader._texture,0);
                //! 绘制地面
                glUniformMatrix4fv(shader._mv, 1, false, MVP.data());
                glUniformMatrix4fv(shader._project, 1, false, matProj.data());
                glUniformMatrix3fv(shader._normalMat, 1, false, &matNor[0][0]);
                glUniform3f(shader._ambientColor, 0.2f,0.2f,0.2f);
                glUniform3f(shader._lightDirection, 0.0f,1.0f,0.0f);
                glUniform3f(shader._diffuseColor, 0.3f,0.3f,0.3f);

                glVertexAttribPointer(shader._position,3,   GL_FLOAT,   false,  sizeof(Vertex),0);
                glVertexAttribPointer(shader._uv,2,         GL_FLOAT,   false,  sizeof(Vertex),(void*)12);
                glVertexAttribPointer(shader._normal,3,     GL_FLOAT,   false,  sizeof(Vertex),(void*)20);

                glDrawElements(GL_TRIANGLES,_indexSize,GL_UNSIGNED_SHORT,0);

            }
            shader.end();

            glBindBuffer(GL_ARRAY_BUFFER,0);
            glBindBuffer(GL_ELEMENT_ARRAY_BUFFER,0);
        }


        void    render(float fElapsed,CELL3RDCamera& camera,BasicLighting& shader)
        {

            glBindBuffer(GL_ARRAY_BUFFER,_vertexBufferId);
            glBindBuffer(GL_ELEMENT_ARRAY_BUFFER,_indexBufferId);
            shader.begin();
            {
                CELL::matrix4   matModel(1);
                CELL::matrix4   matView =   camera.getView();
                CELL::matrix4   matProj =   camera.getProject();

                CELL::matrix4   MVP     =    matProj * matView * matModel;

                CELL::matrix3   matNor(1);
                matrix4ToMatrix3(matNor,MVP);


                const float redPlasticEmissive[3]   = {0.0,  0.0,  0.0},
                            redPlasticAmbient[3]    = {0.0, 0.0, 0.0},
                            redPlasticDiffuse[3]    = {0.5f, 0.0, 0.0},
                            redPlasticSpecular[3]   = {0.7f, 0.6f, 0.6f},
                            redPlasticShininess     = 32.0f;

                glUniform3fv(shader.Ke, 1,redPlasticEmissive);
                glUniform3fv(shader.Ka, 1,redPlasticAmbient);
                glUniform3fv(shader.Kd, 1,redPlasticDiffuse);
                glUniform3fv(shader.Ks, 1,redPlasticSpecular);
                glUniform1f(shader.shininess, redPlasticShininess);


                const float eyePosition[4] = { 0, 0, 13, 1 };
                const float lightPosition[4] = { 5, 
                    1.5,
                    5, 1 };
                glUniform3fv(shader.eyePosition, 1,eyePosition);
                glUniform3fv(shader.lightPosition, 1,lightPosition);


                float myGlobalAmbient[3] = { 0.1f, 0.1f, 0.1f };  /* Dim */
                float myLightColor[3] = { 0.95f, 0.95f, 0.95f };  /* White */

                glUniform3fv(shader.globalAmbient, 1,myGlobalAmbient);
                glUniform3fv(shader.lightColor, 1,myLightColor);

               

                glUniformMatrix4fv(shader.modelViewProj, 1, false, MVP.data());

                glVertexAttribPointer(shader._position,3,   GL_FLOAT,   false,  sizeof(Vertex),0);
                glVertexAttribPointer(shader._normal,3,     GL_FLOAT,   false,  sizeof(Vertex),(void*)20);

                glDrawElements(GL_TRIANGLES,_indexSize,GL_UNSIGNED_SHORT,0);

            }
            shader.end();

            glBindBuffer(GL_ARRAY_BUFFER,0);
            glBindBuffer(GL_ELEMENT_ARRAY_BUFFER,0);
        }

    protected:
        /**
        *   读取文件内容,需要外部释放内存
        */
        char*   readFile(const char* fileName,size_t& size)
        {
            FILE*   file    =   fopen(fileName,"rb");
            if (file == 0 )
            {
                return  0;
            }
            fseek( file, 0, SEEK_END );
            size_t length = ftell( file );
            fseek(file, 0, SEEK_SET );
            char* buffer =   new char[length + 1];
            fread( buffer, 1, length, file );
            buffer[length] = 0;
            size    =   length;
            fclose( file );
            return  buffer;
        }

    };
}