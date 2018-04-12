#pragma once
#include "ggl.h"
#include "Utils.h"
#include "Vertex.h"

using mType = std::pair < string, GLuint > ;
//一行v数据的数据结构
struct FloatData
{
	float v[3];
};
enum  ALPHA_TYPE
{
	ALPHA_INVALID = -1,
	ALPHA_LINNER = 0,
	ALPHA_GAUSSIAN = 1,
};
#pragma region 模型
//一行f数据的数据结构
struct VertexDefine
{
	int32_t positionIndex;//位置信息的Index
	int32_t textcoordIndex;//纹理贴图信息的Index
	int32_t normalIndex;//法线信息的Index
};

struct ObjModel
{
	vector<FloatData> positions, texcoords, normals;//记录真实数据
	vector<VertexDefine> VertexList;//记录绘制指令
	vector<int32_t> Indexes;//表示所有绘制指令的索引位置。
};
#pragma endregion 模型

#pragma region Texture
struct Texture
{
	GLuint texture=_INVALID_ID_;
	int CiteCount = 0;
public:
	bool operator<(const Texture& other)
	{
		if (this->texture <= other.texture)
		{
			return 1;
		}
		else
		{
			return 0;
		}
	}
};
#pragma endregion Texture

#pragma region Program
struct Program
{
	GLuint program = _INVALID_ID_;
	int CiteCount = 0;
public:
	bool operator<(const Program& other)
	{
		if (this->program <= other.program)
		{
			return 1;
		}
		else
		{
			return 0;
		}
	}
};
#pragma endregion Program



class ResourceManager
{
friend class ElementBuffer;
friend class FrameBuffer;
private:
	ResourceManager(){};
public:
	static GLuint GetPic(const char* bmpPath);//获取从res中读到的文件
	static void RemovePic(GLuint texture);
	static GLuint GetProgram(const char* vertexShaderPath, const char* fragmentShaderPath);//获取GPU程序
	static bool GetModel(const char* path,VertexBuffer& vbo);//获取模型
	static void RemoveProgram(GLuint program);

	//创建GPU BufferObject
	static GLuint CreateBufferObject(GLenum bufferType, const GLsizeiptr& size, const GLenum& usage, void* data = nullptr);
	static void DeleteBufferObject(GLuint *buffer, int count = 1);

private:
	//创建2D纹理对象
	static GLuint CreateTexture2D(unsigned char* pixelData, int width, int height, GLenum format, GLenum type);

	//生成一个程序纹理
	static GLuint CreateProcedureTexture(const int32_t& size, ALPHA_TYPE type = ALPHA_TYPE::ALPHA_LINNER);

	//从BMP创建2D纹理对象
	static GLuint CreateTexture2DFromBMP(const char* bmpPath);

	//从PNG创建纹理
	static GLuint CreateTexture2DFromPNG(const char* bmpPath, bool invertY = 1);

	//创建2D纹理对象
	static GLuint CreateTexture2D(const char* fileName);

	//创建一个显示列表
	static GLuint CreateDisplayList(std::function<void()> foo);

	//编译shader
	static GLuint CompileShader(GLenum shaderType, const char* shaderCode);

	//创建GPU程序
	static GLuint CreateProgram(GLuint vertexShader, GLuint fragmentShader);

	//读一个文件 返回其在内存中的指针、文件长度
	static unsigned char* LoadFileContent(const char* path, int& filesize);

protected:
private:
	//texture管理
	static std::map<string, Texture> m_mTexture;//纹理map<文件名，纹理>
	//GPU程序管理
	static std::map<string, Program> m_mProgram;//Program map<vert+frag,程序ID>
	//ObjModel管理
	static std::map<string, ObjModel> m_mModel;
};
