#include "Model.h"
#include "Utils.h"


Model::Model()
{

}
bool Model::Init(const char* path, const char* vertexShader, const char* fragmentShader)
{
	int nFileSize = 0;
	unsigned char* fileContent = LoadFileContent(path, nFileSize);//�����ļ�������
	ASSERT_PTR_BOOL(fileContent);

	//���ϵĽ�������Ϣ��������ָ���������ô����Щ��Ϣ��֯��һ������
	//����Ӧ�ð�����λ����Ϣ��������Ϣ��������ͼ��Ϣ

	//һ��v���ݵ����ݽṹ
	struct FloatData
	{
		float v[3];
	};

	//һ��f���ݵ����ݽṹ
	struct VertexDefine
	{
		int32_t positionIndex;//λ����Ϣ��Index
		int32_t textcoordIndex;//������ͼ��Ϣ��Index
		int32_t normalIndex;//������Ϣ��Index
	};

	vector<FloatData> positions, texcoords, normals;//��¼��ʵ����
	vector<VertexDefine> VertexList;//��¼����ָ��

	vector<int32_t> Indexes;//��ʾ���л���ָ�������λ�á�
	

	std::stringstream ssFileContent((char *)fileContent);//���ַ���ȥ��
	char buffer[256];//����
	while (!ssFileContent.eof())
	{
		memset(buffer, 0, 256);
		ssFileContent.getline(buffer, 256);//����buffer��ͬʱssָ�����
		//����в�Ϊ��
		if (strlen(buffer) > 0)
		{
			//�����v��ͷ
			if (buffer[0] == 'v')
			{
				std::stringstream ssBuffer(buffer);//����stringstream�����ո���ж���������
				char temp[256];
				//�����vt
				if (buffer[1] == 't')
				{
					ssBuffer >> temp;//����ĸvt����ȥ��ʣ��2��������Ϣ����ʾuv��
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
				string vertexString;//�洢����1/1/1����Ϣ
				
				//һ�ν���һ��f���ݣ�һ���棩
				for (unsigned i = 0; i < 3; i++)
				{
					ssBuffer >> vertexString;
					//��һ����Ϣ��λ����Ϣ
					size_t pos = vertexString.find_first_of('/');
					string posIndexStr = vertexString.substr(0, pos);

					//�ڶ�����Ϣ��������Ϣ
					size_t pos2 = vertexString.find_first_of('/', pos + 1);
					string texcoordIndexStr = vertexString.substr(pos + 1, pos2 - 1 - pos);

					//��������Ϣ��������Ϣ
					string normalIndexStr = vertexString.substr(pos2 + 1, normalIndexStr.length() - pos2 - 1);


					VertexDefine vertex;
					vertex.positionIndex = atoi(posIndexStr.c_str());
					vertex.textcoordIndex = atoi(texcoordIndexStr.c_str());
					vertex.normalIndex = atoi(normalIndexStr.c_str());

					int32_t index = INVALID;
					//�������������ͬ�ģ������
					for (unsigned i = 0; i < VertexList.size(); i++)
					{
						if (VertexList[i].positionIndex == vertex.positionIndex && VertexList[i].normalIndex == vertex.normalIndex && VertexList[i].textcoordIndex == vertex.textcoordIndex)
						{
							index = i;//�������ͬ�ģ�����Ϊ��
							break;
						}
					}
					//���û����ͬ��
					if (index == INVALID)
					{
						index = VertexList.size();
						VertexList.push_back(vertex);//����ǵ�һ�γ��֣��򱣴��ָ����������������ֵ
					}
					Indexes.push_back(index);//�����ָ�������ֵ
				}

			}
		}
	}
	//���������������Ϣ������VertexBuffer
	m_VertexBuf.Init(Indexes.size());
	for (int32_t i = 0; i < Indexes.size();i++)
	{
		if (Indexes[i] < VertexList.size())
		{
			VertexDefine vertex=VertexList.at(Indexes[i]);      //ȡ������ָ��
			//λ����Ϣ
			if (vertex.positionIndex-1>positions.size())
			{
				continue;
			}
			FloatData data = positions[vertex.positionIndex-1];  //OBJ�ļ�����������Ϣ��1��ʼ
			m_VertexBuf.SetPosition(i,data.v[0], data.v[1], data.v[2]);

			//uv
			if (vertex.textcoordIndex-1>texcoords.size())
			{
				continue;
			}
			data = texcoords[vertex.textcoordIndex-1];
			m_VertexBuf.SetNormal(i, data.v[0], data.v[1], data.v[2]);

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
			m_Shader.Init(vertexShader, "res/obj.frag");
		}
	}
	else
	{
		m_Shader.Init("res/obj.vert", "res/obj.frag");
	}
	m_Shader.SetVec4("U_LightPos", 0, 1, 1, 0);
	m_Shader.SetVec4("U_LightAmbient", 1, 1, 1, 1);
	m_Shader.SetVec4("U_LightDiffuse", 1, 1, 1, 1);

	m_Shader.SetVec4("U_AmbientMaterial", 0.1f, 0.1f, 0.1f, 1);
	m_Shader.SetVec4("U_DiffuseMaterial", 0.6f, 0.6f, 0.6f, 1);
	return 1;
}
void Model::Update(float frameTime)
{
	if (m_IsMoveToRight)
	{
		float delta = frameTime*m_MoveSpeed;

	}
	if (m_IsMoveToLeft)
	{
		float delta = frameTime*m_MoveSpeed;

	}
	if (m_IsMoveToTop)
	{
		float delta = frameTime*m_MoveSpeed;

	}
	if (m_IsMoveToBottom)
	{
		float delta = frameTime*m_MoveSpeed;
	}
}

void Model::Draw(glm::mat4& viewMatrix, glm::mat4 &ProjectionMatrix)
{
	//��ģ�͵ƹ����
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
	glm::mat4 ITMatrix = glm::inverseTranspose(this->m_ModelMatrix);
	glEnable(GL_DEPTH_TEST);
	BEGIN
	glUniformMatrix4fv(m_Shader.GetITModelMatrixLocation(), 1, GL_FALSE, glm::value_ptr(ITMatrix));
	DRAW
	END
}
void Model::SetPosition(float x, float y, float z)
{
	m_ModelMatrix = glm::translate(x, y, z);
}
void Model::SetTexture2D(const char* path)
{
	m_Shader.SetTexture2D(path);
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