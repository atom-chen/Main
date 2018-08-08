#pragma  once
#include "Vertex.h"

//��������texture�ϣ�ʵ�ʻ���������VBO׼��
class FrameBuffer
{
public:
	FrameBuffer();
	~FrameBuffer();
public:
	//Attch��ɫ������
	void AttachColorBuffer(const char* bufferName, GLenum attachment, const unsigned& width, const unsigned& height);//���֣�����𣩣��󶨵��ĸ����ص㣬����
	//Attch��Ȼ�����
	void AttachDepthBuffer(const char* bufferName, const unsigned& width, const unsigned& height);
public:
	void Finish();//���ý���
	void Begin();
	void End();//���ƽ������л���ԭ���Ļ�����
	GLuint GetBuffer(const char* bufferName);
public:

private:
	GLuint m_Fbo;
	GLint m_PrevFrameBuffer;
	std::map<string, GLuint> m_Buffer;
	std::vector<GLenum> m_DrawBuffer;


	unsigned m_Width = 0;
	unsigned m_Height = 0;
};