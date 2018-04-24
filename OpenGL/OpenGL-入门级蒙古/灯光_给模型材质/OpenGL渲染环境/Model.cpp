#include "Model.h"
#include "Utils.h"


Model::Model()
{

}
bool Model::Init(const char* path)
{
	int nFileSize = 0;
	unsigned char* fileContent = LoadFileContent(path, nFileSize);//�����ļ�������
	if (fileContent == nullptr || nFileSize == 0)
	{
		return false;
	}

	//���ϵĽ�������Ϣ��������ָ���������ô����Щ��Ϣ��֯��һ������
	//����Ӧ�ð�����λ����Ϣ��������Ϣ��������ͼ��Ϣ

	//��ʱ�洢vt,v,vn
	struct FloatData
	{
		float v[3];
	};
	//��ʱ���һ��ָ������ p,t,n���±�
	struct VertexDefine
	{
		int32_t positionIndex;//λ����Ϣ��Index
		int32_t textcoordIndex;//������ͼ��Ϣ��Index
		int32_t normalIndex;//������Ϣ��Index
	};

	vector<FloatData> positions, texcoords, normals;//���ָ���
	vector<int32_t> Indexes;//����ָ���Index
	vector<VertexDefine> VertexList;//��ͼָ���

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
	//���������������Ϣ������Model��Ա
	m_IndexCount = Indexes.size();
	m_Indexes = new unsigned short[m_IndexCount];//ָ������ֵ
	//����ֵ��������ֵ
	for (int32_t i = 0; i < m_IndexCount; i++)
	{
		m_Indexes[i] = Indexes[i];
	}
	int vertexCount = VertexList.size();
	m_Vertexes = new VertexData[vertexCount];//ָ������ֵ
	for (unsigned i = 0; i < vertexCount; i++)
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

void Model::Draw()
{
	//��ģ��������ͼ
	if (m_Texture != NULL)
	{
		glEnable(GL_TEXTURE_2D);
		glBindTexture(GL_TEXTURE_2D, m_Texture);
	}
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

	glEnable(GL_DEPTH_TEST);
	glPushMatrix();
	glTranslatef(0.0f, 0.0f, -5.0f);
	glBegin(GL_TRIANGLES);
	//����Щ���㰴�����λ�����
	for (int32_t i = 0; i < m_IndexCount; i++)
	{
		glTexCoord2fv((m_Vertexes[m_Indexes[i]]).texcoord);//����index��m_Vertexes��ȡ��uv��Ϣ
		glNormal3fv((m_Vertexes[m_Indexes[i]]).nromal);//����index��m_Vertexes��ȡ��������Ϣ
		glVertex3fv((m_Vertexes[m_Indexes[i]]).position);//����Indexȡ����λ����Ϣ
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