#include "objmodel.h"
#include "utils.h"
#include <string>
#include <sstream>
#include <vector>

void ObjModel::Init(const char*objModel)
{
	struct FloatData
	{
		float v[3];
	};

	struct VerticeDefine
	{
		int posIndex;
		int texcoordIndex;
		int normalIndex;
	};
	std::vector<FloatData> positions,texcoords,normals;
	std::vector<VerticeDefine> vertexes;//unique vertex
	std::vector<int> indexes;//

	unsigned char*fileContent = LoadFileContent(objModel);
	std::stringstream ssFileContent((char*)fileContent);
	std::string temp;
	char szOneLine[256];
	while (!ssFileContent.eof())
	{
		memset(szOneLine, 0, 256);
		ssFileContent.getline(szOneLine, 256);
		if (strlen(szOneLine)>0)
		{
			if (szOneLine[0]=='v')
			{
				std::stringstream ssOneLine(szOneLine);
				if (szOneLine[1]=='t')
				{
					ssOneLine >> temp;
					FloatData floatData;
					ssOneLine >> floatData.v[0];
					ssOneLine >> floatData.v[1];
					texcoords.push_back(floatData);
				}
				else if (szOneLine[1]=='n')
				{
					ssOneLine >> temp;
					FloatData floatData;
					ssOneLine >> floatData.v[0];
					ssOneLine >> floatData.v[1];
					ssOneLine >> floatData.v[2];
					normals.push_back(floatData);
				}
				else
				{
					ssOneLine >> temp;
					FloatData floatData;
					ssOneLine >> floatData.v[0];
					ssOneLine >> floatData.v[1];
					ssOneLine >> floatData.v[2];
					positions.push_back(floatData);
				}
			}
			else if (szOneLine[0]=='f')
			{
				std::stringstream ssOneLine(szOneLine);
				ssOneLine >> temp;//
				std::string vertexStr;// 1/1/1
				for (int i=0;i<3;i++)
				{
					ssOneLine >> vertexStr;
					size_t pos = vertexStr.find_first_of('/');
					std::string posIndexStr = vertexStr.substr(0, pos);
					size_t pos2 = vertexStr.find_first_of('/', pos + 1);
					std::string texcoordIndexStr = vertexStr.substr(pos + 1, pos2 - 1 - pos);
					std::string normalIndexStr = vertexStr.substr(pos2 + 1, vertexStr.length() - 1 - pos2);
					VerticeDefine vd;
					vd.posIndex = atoi(posIndexStr.c_str());
					vd.texcoordIndex = atoi(texcoordIndexStr.c_str());
					vd.normalIndex = atoi(normalIndexStr.c_str());
					int nCurrentVertexIndex = -1;
					int nCurrentVertexCount = (int)vertexes.size();
					for (int j=0;j<nCurrentVertexCount;++j)
					{
						if (vertexes[j].posIndex==vd.posIndex&&
							vertexes[j].normalIndex==vd.normalIndex&&
							vertexes[j].texcoordIndex==vd.texcoordIndex)
						{
							nCurrentVertexIndex = j;
							break;
						}
					}
					if (nCurrentVertexIndex==-1)
					{
						nCurrentVertexIndex = (int)vertexes.size();
						vertexes.push_back(vd);
					}
					indexes.push_back(nCurrentVertexIndex);
				}
			}
		}
	}
	printf("unique vertex count %u index count %u\n",vertexes.size(),indexes.size());
	mIndexCount = (int)indexes.size();
	mIndices = new int[mIndexCount];
	for (int i=0;i<mIndexCount;++i)
	{
		mIndices[i] = indexes[i];
	}
	int vertexCount = (int)vertexes.size();
	mVertexes = new VertexData[vertexCount];
	for (int i=0;i<vertexCount;++i)
	{
		memcpy(mVertexes[i].position, positions[vertexes[i].posIndex - 1].v, sizeof(float) * 3);
		memcpy(mVertexes[i].texcoord, texcoords[vertexes[i].texcoordIndex - 1].v, sizeof(float) * 2);
		memcpy(mVertexes[i].normal, normals[vertexes[i].normalIndex - 1].v, sizeof(float) * 3);
	}
	delete fileContent;
}

void ObjModel::Draw()
{
	glPushMatrix();
	glBegin(GL_TRIANGLES);
	for (int i=0;i<mIndexCount;++i)
	{
		glTexCoord2fv(mVertexes[mIndices[i]].texcoord);
		glNormal3fv(mVertexes[mIndices[i]].normal);
		glVertex3fv(mVertexes[mIndices[i]].position);
	}
	glEnd();
	glPopMatrix();
}