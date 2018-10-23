#pragma  once
#include "Vertex.h"

//画东西在texture上，实际绘制数据由VBO准备
class FrameBuffer
{
public:
	FrameBuffer();
	~FrameBuffer();
public:
	//Attch颜色缓冲区
	void AttachColorBuffer(const char* bufferName, GLenum attachment, const unsigned& width, const unsigned& height);//名字（随便起），绑定到哪个挂载点，宽，高
	//Attch深度缓冲区
	void AttachDepthBuffer(const char* bufferName, const unsigned& width, const unsigned& height);
public:
	void Finish();//设置结束
	void Begin();
	void End();//绘制结束，切换回原来的缓冲区
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