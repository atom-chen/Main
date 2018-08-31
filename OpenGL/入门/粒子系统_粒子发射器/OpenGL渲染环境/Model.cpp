#include "Model.h"
#include "Utils.h"


Model::Model()
{

}
bool Model::Init(const char* path)
{
	int nFileSize = 0;
	unsigned char* fileContent = LoadFileContent(path, nFileSize);//读到文件二进制
	if (fileContent == nullptr || nFileSize == 0)
	{
		return false;
	}

	//以上的仅仅是信息，而绘制指令教我们怎么把那些信息组织成一个顶点
	//顶点应该包含：位置信息，法线信息，纹理贴图信息

	//临时存储vt,v,vn
	struct FloatData
	{
		float v[3];
	};
	//临时存放一条指令数据 p,t,n的下标
	struct VertexDefine
	{
		int positionIndex;//位置信息的Index
		int textcoordIndex;//纹理贴图信息的Index
		int normalIndex;//法线信息的Index
	};

	vector<FloatData> positions, texcoords, normals;//点的指令集合
	vector<int> Indexes;//绘制指令的Index
	vector<VertexDefine> VertexList;//绘图指令集合

	std::stringstream ssFileContent((char *)fileContent);//用字符流去读
	char buffer[256];//缓存
	while (!ssFileContent.eof())
	{
		memset(buffer, 0, 256);
		ssFileContent.getline(buffer, 256);//读到buffer，同时ss指针后移
		//如果行不为空
		if (strlen(buffer) > 0)
		{
			//如果是v开头
			if (buffer[0] == 'v')
			{
				std::stringstream ssBuffer(buffer);//利用stringstream遇到空格就中断流的特性
				char temp[256];
				//如果是vt
				if (buffer[1] == 't')
				{
					ssBuffer >> temp;//把字母vt流出去，剩下2个数字信息（表示uv）
					FloatData data;
					ssBuffer >> data.v[0];//u
					ssBuffer >> data.v[1];//v
					texcoords.push_back(data);
				}
				else if (buffer[1] == 'n')
				{
					ssBuffer >> temp;
					FloatData data;
					ssBuffer >> data.v[0];//x
					ssBuffer >> data.v[1];//y
					ssBuffer >> data.v[2];//z
					normals.push_back(data);
				}
				else
				{
					ssBuffer >> temp;
					FloatData data;
					ssBuffer >> data.v[0];//x
					ssBuffer >> data.v[1];//y
					ssBuffer >> data.v[2];//z
					positions.push_back(data);
				}
			}
			else if (buffer[0] == 'f')
			{
				std::stringstream ssBuffer(buffer);
				char temp[256];
				ssBuffer >> temp;
				string vertexString;//存储形如1/1/1的信息

				for (unsigned i = 0; i < 3; i++)
				{
					ssBuffer >> vertexString;
					//第一对信息：位置信息
					size_t pos = vertexString.find_first_of('/');
					string posIndexStr = vertexString.substr(0, pos);

					//第二对信息：纹理信息
					size_t pos2 = vertexString.find_first_of('/', pos + 1);
					string texcoordIndexStr = vertexString.substr(pos + 1, pos2 - 1 - pos);

					//第三对信息：法线信息
					string normalIndexStr = vertexString.substr(pos2 + 1, normalIndexStr.length() - pos2 - 1);


					VertexDefine vertex;
					vertex.positionIndex = atoi(posIndexStr.c_str());
					vertex.textcoordIndex = atoi(texcoordIndexStr.c_str());
					vertex.normalIndex = atoi(normalIndexStr.c_str());

					int index = INVALID;
					//如果集合里有相同的，则不添加
					for (unsigned i = 0; i < VertexList.size(); i++)
					{
						if (VertexList[i].positionIndex == vertex.positionIndex && VertexList[i].normalIndex == vertex.normalIndex && VertexList[i].textcoordIndex == vertex.textcoordIndex)
						{
							index = i;//如果有相同的，就认为是
							break;
						}
					}
					//如果没有相同的
					if (index == INVALID)
					{
						index = VertexList.size();
						VertexList.push_back(vertex);//如果是第一次出现，则保存该指令。否则仅保存其索引值
					}
					Indexes.push_back(index);//保存该指令的索引值
				}

			}
		}
	}
	//根据上面读到的信息，构造Model成员
	m_IndexCount = Indexes.size();
	m_Indexes = new unsigned short[m_IndexCount];//指令索引值
	//拷贝值顶点索引值
	for (int i = 0; i < m_IndexCount; i++)
	{
		m_Indexes[i] = Indexes[i];
	}
	int vertexCount = VertexList.size();
	m_Vertexes = new VertexData[vertexCount];//指令数据值
	for (int i = 0; i < vertexCount; i++)
	{
		memcpy(m_Vertexes[i].position, positions[VertexList[i].positionIndex - 1].v, sizeof(float) * 3);//position Info
		memcpy(m_Vertexes[i].nromal, normals[VertexList[i].normalIndex - 1].v, sizeof(float) * 3);//normal Info
		memcpy(m_Vertexes[i].texcoord, texcoords[VertexList[i].textcoordIndex - 1].v, sizeof(float) * 2);//texcoord Info
	}

	if (fileContent != nullptr)
	{
		delete fileContent;
	}
	return 1;
}
void Model::Update(float frameTime)
{
	if (m_IsMoveToRight)
	{
		float delta = frameTime*m_MoveSpeed;
		m_Position.x += delta;
	}
	if (m_IsMoveToLeft)
	{
		float delta = frameTime*m_MoveSpeed;
		m_Position.x -= delta;
	}
	if (m_IsMoveToTop)
	{
		float delta = frameTime*m_MoveSpeed;
		m_Position.z += delta;
	}
	if (m_IsMoveToBottom)
	{
		float delta = frameTime*m_MoveSpeed;
		m_Position.z -= delta;
	}
}

void Model::Draw()
{
	//打开模型灯光材质
	glEnable(GL_LIGHTING);
	if (m_AmbientMaterial != nullptr)
	{
		glMaterialfv(GL_FRONT, GL_AMBIENT, m_AmbientMaterial);
	}
	if (m_DiffuseMaterial != nullptr)
	{
		glMaterialfv(GL_FRONT, GL_DIFFUSE, m_DiffuseMaterial);
	}
	if (m_SpecularMaterial != nullptr)
	{
		glMaterialfv(GL_FRONT, GL_SPECULAR, m_SpecularMaterial);
	}
	glEnable(GL_TEXTURE_2D);
	glBindTexture(GL_TEXTURE_2D, m_Texture);

	//关闭纹理
	//glDisable(GL_TEXTURE_2D);
	//glMaterialf(GL_FRONT, GL_SHININESS, 64);


	glEnable(GL_DEPTH_TEST);
	glPushMatrix();
	glTranslatef(m_Position.x,m_Position.y,m_Position.z);
	glBegin(GL_TRIANGLES);
	//把这些顶点按三角形画出来
	for (int i = 0; i < m_IndexCount; i++)
	{
		glTexCoord2fv((m_Vertexes[m_Indexes[i]]).texcoord);//根据index从m_Vertexes中取出uv信息
		glNormal3fv((m_Vertexes[m_Indexes[i]]).nromal);//根据index从m_Vertexes中取出法线信息
		glVertex3fv((m_Vertexes[m_Indexes[i]]).position);//根据Index取出其位置信息
	}
	glEnd();
	glPopMatrix();
}

void Model::SetTexture(const char* bmpPath)
{
	m_Texture = CreateTexture2DFromBMP(bmpPath);
}

void Model::SetAmbientMaterialColor(float r, float g, float b, float a)
{
	m_AmbientMaterial[0] = r;
	m_AmbientMaterial[1] = g;
	m_AmbientMaterial[2] = b;
	m_AmbientMaterial[3] = a;
}

void Model::SetDiffuseMaterialColor(float r, float g, float b, float a)
{
	m_DiffuseMaterial[0] = r;
	m_DiffuseMaterial[1] = g;
	m_DiffuseMaterial[2] = b;
	m_DiffuseMaterial[3] = a;
}

void Model::SetSpecularMaterialColor(float r, float g, float b, float a)
{
	m_SpecularMaterial[0] = r;
	m_SpecularMaterial[1] = g;
	m_SpecularMaterial[2] = b;
	m_SpecularMaterial[3] = a;
}

void Model::MoveToLeft(bool isMove)
{
	m_IsMoveToLeft = isMove;
}
void Model::MoveToRight(bool isMove)
{
	m_IsMoveToRight = isMove;
}
void Model::MoveToTop(bool isMove)
{
	m_IsMoveToTop = isMove;
}
void Model::MoveToBottom(bool isMove)
{
	m_IsMoveToBottom = isMove;
}
void Model::SetMoveSpeed(float speed)
{
	this->m_MoveSpeed = speed;
}