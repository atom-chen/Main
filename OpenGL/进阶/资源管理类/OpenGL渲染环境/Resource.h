#pragma once
#include "ggl.h"
#include "Utils.h"
#include "Vertex.h"

using mType = std::pair < string, GLuint > ;
//һ��v���ݵ����ݽṹ
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

//һ��f���ݵ����ݽṹ
struct VertexDefine
{
	int32_t positionIndex;//λ����Ϣ��Index
	int32_t textcoordIndex;//������ͼ��Ϣ��Index
	int32_t normalIndex;//������Ϣ��Index
};

struct ObjModel 
{
	vector<FloatData> positions, texcoords, normals;//��¼��ʵ����
	vector<VertexDefine> VertexList;//��¼����ָ��
	vector<int32_t> Indexes;//��ʾ���л���ָ�������λ�á�
};


class ResourceManager
{
friend class ElementBuffer;
friend class FrameBuffer;
private:
	ResourceManager(){};
public:
	static GLuint GetPic(const char* bmpPath);//��ȡ��res�ж������ļ�
	static GLuint GetProgram(const char* vertexShaderPath, const char* fragmentShaderPath);//��ȡGPU����
	static bool GetModel(const char* path,VertexBuffer& vbo);//��ȡģ��

	//����GPU BufferObject
	static GLuint CreateBufferObject(GLenum bufferType, const GLsizeiptr& size, const GLenum& usage, void* data = nullptr);

private:
	//����2D�������
	static GLuint CreateTexture2D(unsigned char* pixelData, int width, int height, GLenum type);

	//����һ����������
	static GLuint CreateProcedureTexture(const int32_t& size, ALPHA_TYPE type = ALPHA_TYPE::ALPHA_LINNER);

	//��BMP����2D�������
	static GLuint CreateTexture2DFromBMP(const char* bmpPath);

	//��PNG��������
	static GLuint CreateTexture2DFromPNG(const char* bmpPath, bool invertY = 1);

	//����һ����ʾ�б�
	static GLuint CreateDisplayList(std::function<void()> foo);

	//����shader
	static GLuint CompileShader(GLenum shaderType, const char* shaderCode);

	//����GPU����
	static GLuint CreateProgram(GLuint vertexShader, GLuint fragmentShader);

	//��һ���ļ� ���������ڴ��е�ָ�롢�ļ�����
	static unsigned char* LoadFileContent(const char* path, int& filesize);

protected:
private:
	//texture����
	static std::map<string, GLuint> m_mTexture;//����map<�ļ���������>
	//GPU�������
	static std::map<string, GLuint> m_mProgram;//Program map<vert+frag,����ID>
	//ObjModel����
	static std::map<string, ObjModel> m_mModel;
};
