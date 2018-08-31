#include "Model.h"
#include "Utils.h"


Model::Model() :m_Position(0, 0, 0), m_Scale(1,1,1)
{
	m_ModelMatrix = glm::translate(m_Position)*glm::scale(m_Scale);
	for (int i = 0; i < 4; i++)
	{
		if (i != 3)
		{
			m_AmbientMaterial[i] = 0.1f;
			m_DiffuseMaterial[i] = 0.6f;
			m_SpecularMaterial[i] = 1;
		}
		else
		{
			m_AmbientMaterial[i] = 1;
			m_DiffuseMaterial[i] = 1.0f;
			m_SpecularMaterial[i] = 1;
		}
	}
}
bool Model::Init(const char* path, const char* vertexShader, const char* fragmentShader)
{
	int nFileSize = 0;
	unsigned char* fileContent = LoadFileContent(path, nFileSize);//读到文件二进制
	ASSERT_PTR_BOOL(fileContent);

	//以上的仅仅是信息，而绘制指令教我们怎么把那些信息组织成一个顶点
	//顶点应该包含：位置信息，法线信息，纹理贴图信息

	//一行v数据的数据结构
	struct FloatData
	{
		float v[3];
	};

	//一行f数据的数据结构
	struct VertexDefine
	{
		int32_t positionIndex;//位置信息的Index
		int32_t textcoordIndex;//纹理贴图信息的Index
		int32_t normalIndex;//法线信息的Index
	};

	vector<FloatData> positions, texcoords, normals;//记录真实数据
	vector<VertexDefine> VertexList;//记录绘制指令

	vector<int32_t> Indexes;//表示所有绘制指令的索引位置。
	

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
				
				//一次解析一行f数据（一个面）
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

					int32_t index = INVALID;
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
	//根据上面读到的信息，构造VertexBuffer
	m_VertexBuf.Init(Indexes.size());
	for (int32_t i = 0; i < Indexes.size();i++)
	{
		if (Indexes[i] < VertexList.size())
		{
			VertexDefine vertex=VertexList.at(Indexes[i]);      //取出绘制指令
			//位置信息
			if (vertex.positionIndex-1>positions.size())
			{
				continue;
			}
			FloatData data = positions[vertex.positionIndex-1];  //OBJ文件里面索引信息从1开始
			m_VertexBuf.SetPosition(i,data.v[0], data.v[1], data.v[2]);

			//uv
			if (vertex.textcoordIndex-1>texcoords.size())
			{
				continue;
			}
			data = texcoords[vertex.textcoordIndex-1];
			m_VertexBuf.SetTexcoord(i, data.v[0], data.v[1], data.v[2]);

			//normal
			if (vertex.normalIndex-1 > normals.size())
			{
				continue;
			}
			data = normals[vertex.normalIndex-1];
			m_VertexBuf.SetNormal(i, data.v[0], data.v[1], data.v[2]);
		}
	}

	if (fileContent != nullptr)
	{
		delete fileContent;
	}
	if (vertexShader != nullptr)
	{
		if (fragmentShader != nullptr)
		{
			m_Shader.Init(vertexShader, fragmentShader);
		}
		else
		{
			m_Shader.Init(vertexShader, "obj.frag");
		}
	}
	else
	{
		m_Shader.Init("obj.vert", "obj.frag");
	}
	m_Shader.SetVec4("U_LightPos", 0, 1, 1, 0);
	m_Shader.SetVec4("U_LightAmbient", 1, 1, 1, 1);
	m_Shader.SetVec4("U_LightDiffuse", 1, 1, 1, 1);
	m_Shader.SetVec4("U_LightSpecular", 1, 1, 1, 1);
	this->SetAmbientMaterial(m_AmbientMaterial[0], m_AmbientMaterial[1], m_AmbientMaterial[2], m_AmbientMaterial[3]);
	this->SetDiffuseMaterial(m_DiffuseMaterial[0], m_DiffuseMaterial[1], m_DiffuseMaterial[2], m_DiffuseMaterial[3]);
	this->SetSpecularMaterial(m_SpecularMaterial[0], m_SpecularMaterial[1], m_SpecularMaterial[2], m_SpecularMaterial[3]);
	m_Shader.SetVec4("U_CameraPos", 0, 0,  0,1);
	m_Shader.SetVec4("U_LightOpt", 32, 0, 0, 1);
	return 1;
}
void Model::Update(const float& frameTime, const vec3& cameraPos)
{
	if (m_IsMoveToRight)
	{
		float delta = frameTime*m_MoveSpeed;
		SetPosition(m_Position.x + delta, m_Position.y, m_Position.z);
	}
	if (m_IsMoveToLeft)
	{
		float delta = frameTime*m_MoveSpeed;
		SetPosition(m_Position.x - delta, m_Position.y, m_Position.z);
	}
	if (m_IsMoveToTop)
	{
		float delta = frameTime*m_MoveSpeed;
		SetPosition(m_Position.x, m_Position.y, m_Position.z-delta);
	}
	if (m_IsMoveToBottom)
	{
		float delta = frameTime*m_MoveSpeed;
		SetPosition(m_Position.x, m_Position.y, m_Position.z + delta);
	}
	m_Shader.SetVec4("U_CameraPos", cameraPos.x, cameraPos.y, cameraPos.z, 1);
}

void Model::Draw(glm::mat4& viewMatrix, glm::mat4 &ProjectionMatrix)
{
	glm::mat4 ITMatrix = glm::inverseTranspose(this->m_ModelMatrix);
	glEnable(GL_DEPTH_TEST);
	BEGIN
	glUniformMatrix4fv(m_Shader.GetITModelMatrixLocation(), 1, GL_FALSE, glm::value_ptr(ITMatrix));
	DRAW
	END
}

void Model::SetPosition(float x, float y, float z)
{
	m_Position.x = x;
	m_Position.y = y;
	m_Position.z = z;
	m_ModelMatrix = glm::translate(x, y, z)*glm::scale(m_Scale.x, m_Scale.y, m_Scale.z);
}
void Model::SetScale(float x, float y, float z)
{
	m_Scale.x = x;
	m_Scale.y = y;
	m_Scale.z = z;
	m_ModelMatrix = glm::translate(m_Position.x, m_Position.y, m_Position.z)* glm::scale(x, y, z);
}


void Model::SetTexture2D(const char* path,const char* nameInShader)
{
	m_Shader.SetTexture2D(path,nameInShader);
}


void Model::SetAmbientMaterial(float r, float g, float b, float a)
{
	m_AmbientMaterial[0] = r;
	m_AmbientMaterial[1] = g;
	m_AmbientMaterial[2] = b;
	m_AmbientMaterial[3] = a;
	m_Shader.SetVec4("U_AmbientMaterial", r,g,b,a);
}

void Model::SetDiffuseMaterial(float r, float g, float b, float a)
{
	m_DiffuseMaterial[0] = r;
	m_DiffuseMaterial[1] = g;
	m_DiffuseMaterial[2] = b;
	m_DiffuseMaterial[3] = a;
	m_Shader.SetVec4("U_DiffuseMaterial",r,g,b,a);
}

void Model::SetSpecularMaterial(float r, float g, float b, float a)
{
	m_SpecularMaterial[0] = r;
	m_SpecularMaterial[1] = g;
	m_SpecularMaterial[2] = b;
	m_SpecularMaterial[3] = a;
	m_Shader.SetVec4("U_SpecularMaterial", r,g,b,a);
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