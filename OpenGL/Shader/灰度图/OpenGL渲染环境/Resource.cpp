
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
		//去读文件
		GLuint texture = CreateTexture2D(bmpPath);
		printf("加载图片%s\r\n", bmpPath);
		Texture textureStrc;
		textureStrc.CiteCount++;
		textureStrc.texture = texture;
		m_mTexture.insert(std::pair<string, Texture>(name, textureStrc));
		return texture;
	}
}

void ResourceManager::RemovePic(GLuint texture)
{
	//引用计数
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
		printf("%s读取成功\r\n", fragmentShaderPath);

		const char* fragmentShaderCode = (char*)LoadFileContent(fragmentShaderPath, nFileSize);
		ASSERT_PTR_BOOL(fragmentShaderCode);
		GLuint fsShader = CompileShader(GL_FRAGMENT_SHADER, fragmentShaderCode);
		delete fragmentShaderCode;
		ASSERT_INT_BOOL(fsShader);
		printf("%s读取成功\r\n", fragmentShaderPath);

		GLuint program = CreateProgram(vsShader, fsShader);
		printf("创建程序%s,%s\r\n", vertexShaderPath,fragmentShaderPath);
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
		//根据上面读到的信息，构造VertexBuffer
		vbo.Init(obj.Indexes.size());
		for (int32_t i = 0; i < obj.Indexes.size(); i++)
		{
			if (obj.Indexes[i] < obj.VertexList.size())
			{
				VertexDefine vertex = obj.VertexList.at(obj.Indexes[i]);      //取出绘制指令
				//位置信息
				if (vertex.positionIndex - 1 > obj.positions.size())
				{
					continue;
				}
				FloatData data = obj.positions[vertex.positionIndex - 1];  //OBJ文件里面索引信息从1开始
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
		unsigned char* fileContent = LoadFileContent(path, nFileSize);//读到文件字节流
		ASSERT_PTR_BOOL(fileContent);

		//以上的仅仅是信息，而绘制指令教我们怎么把那些信息组织成一个顶点
		//顶点应该包含：位置信息，法线信息，纹理贴图信息
		vector<FloatData> positions;
		vector<FloatData> texcoords;
		vector<FloatData> normals;
		vector<VertexDefine> VertexList;
		vector<int32_t> Indexes;
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
		if (fileContent != nullptr)
		{
			delete fileContent;
		}
		//根据上面读到的信息，构造VertexBuffer
		//根据上面读到的信息，构造VertexBuffer
		vbo.Init(Indexes.size());
		for (int32_t i = 0; i < Indexes.size(); i++)
		{
			if (Indexes[i] < VertexList.size())
			{
				VertexDefine vertex = VertexList.at(Indexes[i]);      //取出绘制指令
				//位置信息
				if (vertex.positionIndex - 1 > positions.size())
				{
					continue;
				}
				FloatData data = positions[vertex.positionIndex - 1];  //OBJ文件里面索引信息从1开始
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

		printf("加载模型%s\r\n", path);
		//存储到map
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
std::map<string, Texture> ResourceManager::m_mTexture;//纹理map<文件名，纹理>
//GPU程序管理
std::map<string, Program> ResourceManager::m_mProgram;//Program map<vert+frag,程序ID>
//ObjModel管理
std::map<string, ObjModel> ResourceManager::m_mModel;

///---------------------------Private Begin-----------------------
//----------------------------------------------------------------

unsigned char* ResourceManager::LoadFileContent(const char* path, int& filesize)
{
	unsigned char* fileContent = nullptr;
	filesize = 0;
	FILE* pFile = fopen(path, "rb");
	//按二进制打开文件，只读
	if (pFile)
	{
		fseek(pFile, 0, SEEK_END);//移动到文件尾
		int nLen = ftell(pFile);
		if (nLen > 0)
		{
			rewind(pFile);//移到文件头部
			fileContent = new unsigned char[nLen + 1];
			fread(fileContent, sizeof(unsigned char), nLen, pFile);
			fileContent[nLen] = '\0';
			filesize = nLen;
		}
		fclose(pFile);
	}
	return fileContent;
}

// GL_CLAMP：大于1.0以上的纹理坐标，会取1.0位置的纹理颜色
//GL_REPEAT:大于1.0以上的纹理坐标，会取坐标-1.0位置的纹理颜色，循环往复
//GL_CLAMP_TO_EDGE：天空盒无缝相接
GLuint ResourceManager::CreateTexture2D(unsigned char* pixelData, int width, int height, GLenum format, GLenum type)
{
	GLuint texture;//该纹理在OpenGL中的唯一标识符
	glGenTextures(1, &texture);//申请一个纹理
	glBindTexture(GL_TEXTURE_2D, texture);//绑定一个2D纹理到texture上

	//设置参数
	glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MAG_FILTER, GL_LINEAR);//举例：当128*128的纹理映射到256*256的物体上时（纹理扩大），使用线性过滤
	glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MIN_FILTER, GL_LINEAR);//举例：当128*128的纹理映射到64*64的物体上时（纹理缩小），使用线性过滤

	glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_WRAP_S, GL_CLAMP_TO_EDGE);//举例：当在uv为0.5的图形上输入1.0时，去纹理的边界上取(0.5的地方取)
	glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_WRAP_T, GL_CLAMP_TO_EDGE);//举例：当在uv为0.5的图形上输入1.0时，去纹理的边界上取(0.5的地方取)

	//拷贝数据到显卡（啥数据，MipMapLevel级别（用不同级别的像素数据为多边形着色），纹理数据在显卡上的像素格式，宽，高，boder必须写0，纹理数据在内存中的格式，每个像素数据的分量是什么类型，像素数据在内存哪里）
	glTexImage2D(GL_TEXTURE_2D, 0, format, width, height, 0, format, type, pixelData);

	glBindTexture(GL_TEXTURE_2D, _INVALID_ID_);//把当前纹理设置为0号纹理

	return texture;
}


GLuint ResourceManager::CreateTexture2DFromBMP(const char* bmpPath)
{
	//读文件
	int32_t fileLen = 0;
	unsigned char* pileContent = LoadFileContent(bmpPath, fileLen);
	if (pileContent == nullptr)
	{
		return 0;
	}
	//解码BMP
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
	//1 获取图片格式
	FREE_IMAGE_FORMAT fifmt = FreeImage_GetFileType(fileName, 0);

	//2 加载图片
	FIBITMAP    *dib = FreeImage_Load(fifmt, fileName, 0);

	//3 转化为rgb 24色
	dib = FreeImage_ConvertTo24Bits(dib);

	//4 获取数据指针
	BYTE    *pixels = (BYTE*)FreeImage_GetBits(dib);

	int     width = FreeImage_GetWidth(dib);
	int     height = FreeImage_GetHeight(dib);

	/**
	*   产生一个纹理Id,可以认为是纹理句柄，后面的操作将书用这个纹理id
	*/
	glGenTextures(1, &texId);

	/**
	*   使用这个纹理id,或者叫绑定(关联)
	*/
	glBindTexture(GL_TEXTURE_2D, texId);
	/**
	*   指定纹理的放大,缩小滤波，使用线性方式，即当图片放大的时候插值方式
	*/
	//设置参数
	glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MAG_FILTER, GL_LINEAR);//举例：当128*128的纹理映射到256*256的物体上时（纹理扩大），使用线性过滤
	glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MIN_FILTER, GL_LINEAR);//举例：当128*128的纹理映射到64*64的物体上时（纹理缩小），使用线性过滤

	glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_WRAP_S, GL_CLAMP_TO_EDGE);//举例：当在uv为0.5的图形上输入1.0时，去纹理的边界上取(0.5的地方取)
	glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_WRAP_T, GL_CLAMP_TO_EDGE);//举例：当在uv为0.5的图形上输入1.0时，去纹理的边界上取(0.5的地方取)
	/**
	*   将图片的rgb数据上传给opengl.
	*/
	glTexImage2D(
		GL_TEXTURE_2D,      //! 指定是二维图片
		0,                  //! 指定为第一级别，纹理可以做mipmap,即lod,离近的就采用级别大的，远则使用较小的纹理
		GL_RGB,             //! 纹理的使用的存储格式
		width,              //! 宽度，老一点的显卡，不支持不规则的纹理，即宽度和高度不是2^n。
		height,             //! 宽度，老一点的显卡，不支持不规则的纹理，即宽度和高度不是2^n。
		0,                  //! 是否的边
		GL_BGR,             //! 数据的格式，bmp中，windows,操作系统中存储的数据是bgr格式
		GL_UNSIGNED_BYTE,   //! 数据是8bit数据
		pixels
		);
	/**
	*   释放内存
	*/
	FreeImage_Unload(dib);

	return  texId;
}

GLuint ResourceManager::CreateProcedureTexture(const int32_t& lenth, ALPHA_TYPE type)
{
	unsigned char  *imageData = new unsigned char[lenth*lenth * 4];//rgba

	//计算中心点坐标
	float halfSize = (float)lenth / 2.0f;
	float maxDistance = sqrtf(halfSize*halfSize * 2);             //最远点到中心点的距离
	float centerX = halfSize;
	float centerY = halfSize;

	for (int32_t y = 0; y < lenth; y++)
	{
		for (int32_t x = 0; x < lenth; x++)
		{
			//加入线性渐变：根据到中心点的距离
			float deltaX = (float)x - centerX;
			float deltaY = (float)y - centerY;
			float distance = sqrtf(deltaX*deltaX + deltaY*deltaY);                               //内积公式
			float alpha = INVALID;
			//粒子alhpa渐变方式
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
			//设置RGBA
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
	//创建一个shader对象
	GLuint shader = glCreateShader(shaderType);

	//把shader源码放到GPU
	glShaderSource(shader, 1, &shaderCode, nullptr);//shader,有一句代码（因为就包含在shaderCode里面，实际上是包含了整个文件），代码在哪里，

	//编译shader
	glCompileShader(shader);

	GLint compileResule = GL_TRUE;
	//查看shader的编译状态
	glGetShaderiv(shader, GL_COMPILE_STATUS, &compileResule);
	if (compileResule == GL_FALSE)
	{
		char szLog[1024] = { 0 };
		GLsizei logLen = 0;
		//拿到错误日志
		glGetShaderInfoLog(shader, 1024, &logLen, szLog);   //shader，错误日志最多多少字符，做输出拿到实际多少个字符，日志写在哪

		printf("Comoile Shaderr fail error log:%s\nshader code:\n%s\n", szLog, shaderCode);
		glDeleteShader(shader);
		shader = 0;
	}
	return shader;
}

GLuint ResourceManager::CreateProgram(GLuint vertexShader, GLuint fragmentShader)
{
	GLuint program = glCreateProgram();
	//将shader绑定到程序上
	glAttachShader(program, vertexShader);
	glAttachShader(program, fragmentShader);

	glLinkProgram(program);
	//解绑定
	glDetachShader(program, vertexShader);
	glDetachShader(program, fragmentShader);

	GLint linkResult;
	//检查程序链接是否OK
	glGetProgramiv(program, GL_LINK_STATUS, &linkResult);
	if (linkResult == GL_FALSE)
	{
		char szLog[1024] = { 0 };
		GLsizei logLen = 0;
		//拿到错误日志
		glGetProgramInfoLog(program, 1024, &logLen, szLog);   //shader，错误日志最多多少字符，做输出拿到实际多少个字符，日志写在哪

		printf("Comoile Shaderr fail error log:%s\nshader code:\n%s\n", szLog, program);
		glDeleteShader(
			program);
		program = 0;
	}


	return program;
}
//----------------------------------------------------------------
///---------------------------Private End--------------------------