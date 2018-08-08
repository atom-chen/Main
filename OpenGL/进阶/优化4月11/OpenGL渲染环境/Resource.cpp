#include "SOIL.h"
#include "Resource.h"
#include "freeImage/FreeImage.h"


///---------------------------Public Begin-----------------------
//----------------------------------------------------------------
GLuint ResourceManager::GetPic(const char* bmpPath)
{
	string name(bmpPath);
	auto it = m_mTexture.find(name);
	if (it != m_mTexture.end())
	{
		it->second.CiteCount++;
		return it->second.texture;
	}
	else
	{
		//ȥ���ļ�
		GLuint texture = CreateTexture2D(bmpPath);
		printf("����ͼƬ%s\r\n", bmpPath);
		Texture textureStrc;
		textureStrc.CiteCount++;
		textureStrc.texture = texture;
		m_mTexture.insert(std::pair<string, Texture>(name, textureStrc));
		return texture;
	}
}

void ResourceManager::RemovePic(GLuint texture)
{
	//���ü���
	for (auto it = m_mTexture.begin(); it != m_mTexture.end();it++)
	{
		if (it->second.texture = texture)
		{
			it->second.CiteCount--;
			if (it->second.CiteCount <= 0)
			{
				glDeleteTextures(1, &(it->second.texture));
				m_mTexture.erase(it);
				return;
			}
		}
	}
}

GLuint ResourceManager::GetProgram(const char* vertexShaderPath, const char* fragmentShaderPath)
{
	string name = string(vertexShaderPath) +","+string(fragmentShaderPath);
	auto it = m_mProgram.find(name);
	if (it != m_mProgram.end())
	{
		it->second.CiteCount++;
		return it->second.program;
	}
	else
	{
		int32_t nFileSize = 0;
		const char* vertexShaderCode = (char*)LoadFileContent(vertexShaderPath, nFileSize);
		ASSERT_PTR_BOOL(vertexShaderCode);
		GLuint vsShader = CompileShader(GL_VERTEX_SHADER, vertexShaderCode);
		delete vertexShaderCode;
		ASSERT_INT_BOOL(vsShader);
		printf("%s��ȡ�ɹ�\r\n", fragmentShaderPath);

		const char* fragmentShaderCode = (char*)LoadFileContent(fragmentShaderPath, nFileSize);
		ASSERT_PTR_BOOL(fragmentShaderCode);
		GLuint fsShader = CompileShader(GL_FRAGMENT_SHADER, fragmentShaderCode);
		delete fragmentShaderCode;
		ASSERT_INT_BOOL(fsShader);
		printf("%s��ȡ�ɹ�\r\n", fragmentShaderPath);

		GLuint program = CreateProgram(vsShader, fsShader);
		printf("��������%s,%s\r\n", vertexShaderPath,fragmentShaderPath);
		Program programStru;
		programStru.program = program;
		programStru.CiteCount++;
		m_mProgram.insert(std::pair<string, Program>(name, programStru));
		return program;
	}
}

void ResourceManager::RemoveProgram(GLuint program)
{
	for (auto it = m_mProgram.begin(); it != m_mProgram.end();)
	{
		if (it->second.program == program)
		{
			it->second.CiteCount--;
			if (it->second.CiteCount <= 0)
			{
				it = m_mProgram.erase(it);
				return;
			}
		}
		it++;
	}
}
bool ResourceManager::GetModel(const char* path, VertexBuffer &vbo)
{
	string name(path);
	auto it = m_mModel.find(name);
	if (it != m_mModel.end())
	{
		auto obj = it->second; 
		//���������������Ϣ������VertexBuffer
		vbo.Init(obj.Indexes.size());
		for (int32_t i = 0; i < obj.Indexes.size(); i++)
		{
			if (obj.Indexes[i] < obj.VertexList.size())
			{
				VertexDefine vertex = obj.VertexList.at(obj.Indexes[i]);      //ȡ������ָ��
				//λ����Ϣ
				if (vertex.positionIndex - 1 > obj.positions.size())
				{
					continue;
				}
				FloatData data = obj.positions[vertex.positionIndex - 1];  //OBJ�ļ�����������Ϣ��1��ʼ
				vbo.SetPosition(i, data.v[0], data.v[1], data.v[2]);

				//uv
				if (vertex.textcoordIndex - 1>obj.texcoords.size())
				{
					continue;
				}
				data = obj.texcoords[vertex.textcoordIndex - 1];
				vbo.SetTexcoord(i, data.v[0], data.v[1], data.v[2]);

				//normal
				if (vertex.normalIndex - 1 > obj.normals.size())
				{
					continue;
				}
				data = obj.normals[vertex.normalIndex - 1];
				vbo.SetNormal(i, data.v[0], data.v[1], data.v[2]);
				vbo.SetColor(i, 1, 1, 1, 1);
			}
		}
		return 1;
	}
	else
	{
		int nFileSize = 0;
		unsigned char* fileContent = LoadFileContent(path, nFileSize);//�����ļ��ֽ���
		ASSERT_PTR_BOOL(fileContent);

		//���ϵĽ�������Ϣ��������ָ���������ô����Щ��Ϣ��֯��һ������
		//����Ӧ�ð�����λ����Ϣ��������Ϣ��������ͼ��Ϣ
		vector<FloatData> positions;
		vector<FloatData> texcoords;
		vector<FloatData> normals;
		vector<VertexDefine> VertexList;
		vector<int32_t> Indexes;
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
		if (fileContent != nullptr)
		{
			delete fileContent;
		}
		//���������������Ϣ������VertexBuffer
		//���������������Ϣ������VertexBuffer
		vbo.Init(Indexes.size());
		for (int32_t i = 0; i < Indexes.size(); i++)
		{
			if (Indexes[i] < VertexList.size())
			{
				VertexDefine vertex = VertexList.at(Indexes[i]);      //ȡ������ָ��
				//λ����Ϣ
				if (vertex.positionIndex - 1 > positions.size())
				{
					continue;
				}
				FloatData data = positions[vertex.positionIndex - 1];  //OBJ�ļ�����������Ϣ��1��ʼ
				vbo.SetPosition(i, data.v[0], data.v[1], data.v[2]);

				//uv
				if (vertex.textcoordIndex - 1>texcoords.size())
				{
					continue;
				}
				data = texcoords[vertex.textcoordIndex - 1];
				vbo.SetTexcoord(i, data.v[0], data.v[1], data.v[2]);

				//normal
				if (vertex.normalIndex - 1 > normals.size())
				{
					continue;
				}
				data = normals[vertex.normalIndex - 1];
				vbo.SetNormal(i, data.v[0], data.v[1], data.v[2]);
			}
		}

		printf("����ģ��%s\r\n", path);
		//�洢��map
		ObjModel obj;
		obj.Indexes.assign(Indexes.begin(), Indexes.end());
		obj.normals.assign(normals.begin(), normals.end());
		obj.texcoords.assign(texcoords.begin(), texcoords.end());
		obj.VertexList.assign(VertexList.begin(), VertexList.end());
		obj.positions.assign(positions.begin(), positions.end());
		m_mModel.insert(std::pair<string,ObjModel>(name, obj));
		return 1;
	}
}
void ResourceManager::RemoveModel(const char* path)
{

}

//----------------------------------------------------------------
///---------------------------Public End--------------------------
std::map<string, Texture> ResourceManager::m_mTexture;//����map<�ļ���������>
//GPU�������
std::map<string, Program> ResourceManager::m_mProgram;//Program map<vert+frag,����ID>
//ObjModel����
std::map<string, ObjModel> ResourceManager::m_mModel;

///---------------------------Private Begin-----------------------
//----------------------------------------------------------------

unsigned char* ResourceManager::LoadFileContent(const char* path, int& filesize)
{
	unsigned char* fileContent = nullptr;
	filesize = 0;
	FILE* pFile = fopen(path, "rb");
	//�������ƴ��ļ���ֻ��
	if (pFile)
	{
		fseek(pFile, 0, SEEK_END);//�ƶ����ļ�β
		int nLen = ftell(pFile);
		if (nLen > 0)
		{
			rewind(pFile);//�Ƶ��ļ�ͷ��
			fileContent = new unsigned char[nLen + 1];
			fread(fileContent, sizeof(unsigned char), nLen, pFile);
			fileContent[nLen] = '\0';
			filesize = nLen;
		}
		fclose(pFile);
	}
	return fileContent;
}

// GL_CLAMP������1.0���ϵ��������꣬��ȡ1.0λ�õ�������ɫ
//GL_REPEAT:����1.0���ϵ��������꣬��ȡ����-1.0λ�õ�������ɫ��ѭ������
//GL_CLAMP_TO_EDGE����պ��޷����
GLuint ResourceManager::CreateTexture2D(unsigned char* pixelData, int width, int height, GLenum format, GLenum type)
{
	GLuint texture;//��������OpenGL�е�Ψһ��ʶ��
	glGenTextures(1, &texture);//����һ������
	glBindTexture(GL_TEXTURE_2D, texture);//��һ��2D����texture��

	//���ò���
	glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MAG_FILTER, GL_LINEAR);//��������128*128������ӳ�䵽256*256��������ʱ���������󣩣�ʹ�����Թ���
	glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MIN_FILTER, GL_LINEAR);//��������128*128������ӳ�䵽64*64��������ʱ��������С����ʹ�����Թ���

	glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_WRAP_S, GL_CLAMP_TO_EDGE);//����������uvΪ0.5��ͼ��������1.0ʱ��ȥ����ı߽���ȡ(0.5�ĵط�ȡ)
	glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_WRAP_T, GL_CLAMP_TO_EDGE);//����������uvΪ0.5��ͼ��������1.0ʱ��ȥ����ı߽���ȡ(0.5�ĵط�ȡ)

	//�������ݵ��Կ���ɶ���ݣ�MipMapLevel�����ò�ͬ�������������Ϊ�������ɫ���������������Կ��ϵ����ظ�ʽ�����ߣ�boder����д0�������������ڴ��еĸ�ʽ��ÿ���������ݵķ�����ʲô���ͣ������������ڴ����
	glTexImage2D(GL_TEXTURE_2D, 0, format, width, height, 0, format, type, pixelData);

	glBindTexture(GL_TEXTURE_2D, _INVALID_ID_);//�ѵ�ǰ��������Ϊ0������

	return texture;
}


GLuint ResourceManager::CreateTexture2DFromBMP(const char* bmpPath)
{
	//���ļ�
	int32_t fileLen = 0;
	unsigned char* pileContent = LoadFileContent(bmpPath, fileLen);
	if (pileContent == nullptr)
	{
		return 0;
	}
	//����BMP
	int width = 0, height = 0;
	unsigned char* pixelData = DecodeBMP(pileContent, width, height);
	if (width == 0 || height == 0 || pixelData == nullptr)
	{
		delete pileContent;
		return 0;
	}
	GLuint texture = CreateTexture2D(pixelData, width, height, GL_RGB,GL_UNSIGNED_BYTE);
	if (pileContent != nullptr)
	{
		delete pileContent;
	}
	return texture;
}

GLuint ResourceManager::CreateTexture2DFromPNG(const char* bmpPath, bool invertY)
{
	int32_t nFileSize = 0;
	unsigned char* fileContent = LoadFileContent(bmpPath, nFileSize);
	if (fileContent == nullptr)
	{
		return 0;
	}
	unsigned int flags = SOIL_FLAG_POWER_OF_TWO;
	if (invertY)
	{
		flags |= SOIL_FLAG_INVERT_Y;
	}
	GLuint texture = SOIL_load_OGL_texture_from_memory(fileContent, nFileSize, 0, 0, flags);
	delete fileContent;
	return texture;
}
GLuint ResourceManager::CreateTexture2D(const char* fileName)
{
	unsigned    texId = 0;
	//1 ��ȡͼƬ��ʽ
	FREE_IMAGE_FORMAT fifmt = FreeImage_GetFileType(fileName, 0);

	//2 ����ͼƬ
	FIBITMAP    *dib = FreeImage_Load(fifmt, fileName, 0);

	//3 ת��Ϊrgb 24ɫ
	dib = FreeImage_ConvertTo24Bits(dib);

	//4 ��ȡ����ָ��
	BYTE    *pixels = (BYTE*)FreeImage_GetBits(dib);

	int     width = FreeImage_GetWidth(dib);
	int     height = FreeImage_GetHeight(dib);

	/**
	*   ����һ������Id,������Ϊ��������������Ĳ����������������id
	*/
	glGenTextures(1, &texId);

	/**
	*   ʹ���������id,���߽а�(����)
	*/
	glBindTexture(GL_TEXTURE_2D, texId);
	/**
	*   ָ������ķŴ�,��С�˲���ʹ�����Է�ʽ������ͼƬ�Ŵ��ʱ���ֵ��ʽ
	*/
	glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MAG_FILTER, GL_LINEAR);

	glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MIN_FILTER, GL_LINEAR);
	/**
	*   ��ͼƬ��rgb�����ϴ���opengl.
	*/
	glTexImage2D(
		GL_TEXTURE_2D,      //! ָ���Ƕ�άͼƬ
		0,                  //! ָ��Ϊ��һ�������������mipmap,��lod,����ľͲ��ü����ģ�Զ��ʹ�ý�С������
		GL_RGB,             //! �����ʹ�õĴ洢��ʽ
		width,              //! ��ȣ���һ����Կ�����֧�ֲ��������������Ⱥ͸߶Ȳ���2^n��
		height,             //! ��ȣ���һ����Կ�����֧�ֲ��������������Ⱥ͸߶Ȳ���2^n��
		0,                  //! �Ƿ�ı�
		GL_BGR,             //! ���ݵĸ�ʽ��bmp�У�windows,����ϵͳ�д洢��������bgr��ʽ
		GL_UNSIGNED_BYTE,   //! ������8bit����
		pixels
		);
	/**
	*   �ͷ��ڴ�
	*/
	FreeImage_Unload(dib);

	return  texId;
}

GLuint ResourceManager::CreateProcedureTexture(const int32_t& lenth, ALPHA_TYPE type)
{
	unsigned char  *imageData = new unsigned char[lenth*lenth * 4];//rgba

	//�������ĵ�����
	float halfSize = (float)lenth / 2.0f;
	float maxDistance = sqrtf(halfSize*halfSize * 2);             //��Զ�㵽���ĵ�ľ���
	float centerX = halfSize;
	float centerY = halfSize;

	for (int32_t y = 0; y < lenth; y++)
	{
		for (int32_t x = 0; x < lenth; x++)
		{
			//�������Խ��䣺���ݵ����ĵ�ľ���
			float deltaX = (float)x - centerX;
			float deltaY = (float)y - centerY;
			float distance = sqrtf(deltaX*deltaX + deltaY*deltaY);                               //�ڻ���ʽ
			float alpha = INVALID;
			//����alhpa���䷽ʽ
			switch (type)
			{
			case ALPHA_LINNER:
				alpha = (1.0f - distance / maxDistance);
				break;
			case ALPHA_GAUSSIAN:
				alpha = powf(1.0f - (distance / maxDistance), 10);
				break;
			default:
				alpha = 1;
				break;
			}
			alpha = alpha>1 ? 1 : alpha;
			//����RGBA
			int32_t index = (lenth*y + x) * 4;
			imageData[index + 0] = 150;
			imageData[index + 1] = 200;
			imageData[index + 2] = 128;
			imageData[index + 3] = static_cast<unsigned char>(alpha * 255);
		}
	}
	GLuint texture = CreateTexture2D(imageData, lenth, lenth, GL_RGBA, GL_UNSIGNED_BYTE);
	delete imageData;
	return texture;
}

GLuint ResourceManager::CreateDisplayList(std::function<void()> foo)
{
	GLuint displayList = glGenLists(1);
	glNewList(displayList, GL_COMPILE);
	foo();
	glEndList();
	return displayList;
}

GLuint ResourceManager::CreateBufferObject(GLenum bufferType, const GLsizeiptr& size, const GLenum& usage, void* data)
{
	GLuint object;
	glGenBuffers(1, &object);
	glBindBuffer(bufferType, object);
	glBufferData(bufferType, size, data, usage);
	glBindBuffer(bufferType, 0);
	return object;
}
void ResourceManager::DeleteBufferObject(GLuint *buffer,int count)
{
	glDeleteBuffers(count, buffer);
}


GLuint ResourceManager::CompileShader(GLenum shaderType, const char* shaderCode)
{
	//����һ��shader����
	GLuint shader = glCreateShader(shaderType);

	//��shaderԴ��ŵ�GPU
	glShaderSource(shader, 1, &shaderCode, nullptr);//shader,��һ����루��Ϊ�Ͱ�����shaderCode���棬ʵ�����ǰ����������ļ��������������

	//����shader
	glCompileShader(shader);

	GLint compileResule = GL_TRUE;
	//�鿴shader�ı���״̬
	glGetShaderiv(shader, GL_COMPILE_STATUS, &compileResule);
	if (compileResule == GL_FALSE)
	{
		char szLog[1024] = { 0 };
		GLsizei logLen = 0;
		//�õ�������־
		glGetShaderInfoLog(shader, 1024, &logLen, szLog);   //shader��������־�������ַ���������õ�ʵ�ʶ��ٸ��ַ�����־д����

		printf("Comoile Shaderr fail error log:%s\nshader code:\n%s\n", szLog, shaderCode);
		glDeleteShader(shader);
		shader = 0;
	}
	return shader;
}

GLuint ResourceManager::CreateProgram(GLuint vertexShader, GLuint fragmentShader)
{
	GLuint program = glCreateProgram();
	//��shader�󶨵�������
	glAttachShader(program, vertexShader);
	glAttachShader(program, fragmentShader);

	glLinkProgram(program);
	//���
	glDetachShader(program, vertexShader);
	glDetachShader(program, fragmentShader);

	GLint linkResult;
	//�����������Ƿ�OK
	glGetProgramiv(program, GL_LINK_STATUS, &linkResult);
	if (linkResult == GL_FALSE)
	{
		char szLog[1024] = { 0 };
		GLsizei logLen = 0;
		//�õ�������־
		glGetProgramInfoLog(program, 1024, &logLen, szLog);   //shader��������־�������ַ���������õ�ʵ�ʶ��ٸ��ַ�����־д����

		printf("Comoile Shaderr fail error log:%s\nshader code:\n%s\n", szLog, program);
		glDeleteShader(program);
		program = 0;
	}
	return program;
}
//----------------------------------------------------------------
///---------------------------Private End--------------------------