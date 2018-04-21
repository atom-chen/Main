#pragma once
#include "ggl.h"
#include "Utils.h"
#include "ElementBuffer.h"
#include "VertexBuffer.h"
#include "FrameBuffer.h"

using mType = std::pair < string, GLuint > ;

enum  ALPHA_TYPE
{
	ALPHA_INVALID = -1,
	ALPHA_LINNER = 0,
	ALPHA_GAUSSIAN = 1,
};
#pragma region ģ��
//һ��f���ݵ����ݽṹ
//һ��v���ݵ����ݽṹ
struct FloatData
{
	float v[3];
};
struct VertexDefine
{
	int positionIndex;//λ����Ϣ��Index
	int textcoordIndex;//������ͼ��Ϣ��Index
	int normalIndex;//������Ϣ��Index
};

struct ObjModel
{
	vector<FloatData> positions, texcoords, normals;//��¼��ʵ����
	vector<VertexDefine> VertexList;//��¼����ָ��
	vector<int> Indexes;//��ʾ���л���ָ�������λ�á�
	int CiteCount = 0;//���ü���
};
#pragma endregion ģ��

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

#pragma region Buffer
struct BufferObject
{
	GLuint Buffer = _INVALID_ID_;
	int CiteCount = 0;
};
#pragma endregion Buffer



class ResourceManager
{
friend class VertexBuffer;
friend class ElementBuffer;
friend class FrameBuffer;
private:
	ResourceManager(){};
public:
	static GLuint GetPic(const char* bmpPath, bool isRepeat=0);//��ȡ��res�ж������ļ�
	static void RemovePic(GLuint texture);

	static GLuint GetTextureCube(const char* front,const char* back,const char* top,const char* bottom,const char* left,const char* right);//��ȡ��res�ж������ļ�
	static void RemoveTextureCube(GLuint texture);

	static bool GetModel(const char* path,VertexBuffer& vbo);//��ȡģ��
	static void RemoveModel(string path);

	static GLuint GetProgram(const char* vertexShaderPath, const char* fragmentShaderPath);//��ȡGPU����
	static void RemoveProgram(GLuint program);

	//��һ���ļ� ���������ڴ��е�ָ�롢�ļ�����
	static unsigned char* LoadFileContent(const char* path, int& filesize);

	//����GPU BufferObject
	static GLuint CreateBufferObject(GLenum bufferType, const GLsizeiptr& size, const GLenum& usage, void* data = nullptr);
	static void RemoveBufferObject(GLuint buffer);
private:

	//����2D�������
	static GLuint CreateTexture2D(unsigned char* pixelData, int width, int height, GLenum format, GLenum type);

	//����һ����������

	static GLuint CreateProcedureTexture(const int& size, ALPHA_TYPE type = ALPHA_TYPE::ALPHA_LINNER);

	//��BMP����2D�������
	static GLuint CreateTexture2DFromBMP(const char* bmpPath);

	//��PNG��������
	static GLuint CreateTexture2DFromPNG(const char* bmpPath, bool invertY = 1);

	//����2D�������
	static GLuint CreateTexture2D(const char* fileName, bool isRepeat);

	//����һ��CubeMap
	static GLuint CreateCubeMap(const char* front, const char* back, const char* top, const char* bottom, const char* left, const char* right);

	//����һ����ʾ�б�
	static GLuint CreateDisplayList(std::function<void()> foo);

	//����shader
	static GLuint CompileShader(GLenum shaderType, const char* shaderCode);

	//����GPU����
	static GLuint CreateProgram(GLuint vertexShader, GLuint fragmentShader);



protected:
private:
	//texture����
	static std::map<string, Texture> m_mTexture;//����map<�ļ���������>
	//textureCube����
	static std::map<string, Texture> m_mTextureCube;
	//GPU�������
	static std::map<string, Program> m_mProgram;//Program map<vert+frag,����ID>
	//ObjModel����
	static std::map<string, ObjModel> m_mModel;
	//����������
	static std::vector<BufferObject> m_mBuffer;
};
