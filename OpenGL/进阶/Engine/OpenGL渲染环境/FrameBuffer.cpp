#include "FrameBuffer.h"
#include "Utils.h"
#include "Resource.h"
#include "SceneManager.h"



FrameBuffer::FrameBuffer()
{

}
FrameBuffer::~FrameBuffer()
{
	glDeleteFramebuffers(1, &m_Fbo);
}
void FrameBuffer::AttachColorBuffer(const char* bufferName, GLenum attachment, const unsigned& width, const unsigned& height)
{
	if (m_Fbo <= _INVALID_ID_)
	{
		glGenFramebuffers(1, &m_Fbo);
	}
	GLuint colorBuffer;
	glBindFramebuffer(GL_FRAMEBUFFER, m_Fbo);

	//创建一个空texture并为它赋值
	colorBuffer = ResourceManager::CreateTexture2D(nullptr, width, height, GL_RGBA, GL_UNSIGNED_BYTE);
	//追加颜色缓冲区到fbo
	glFramebufferTexture2D(GL_FRAMEBUFFER, attachment, GL_TEXTURE_2D, colorBuffer, 0);

	m_DrawBuffer.push_back(attachment);
	//名字只作为标识
	m_Buffer.insert(std::pair<string, GLuint>(bufferName, colorBuffer));
	glBindFramebuffer(GL_FRAMEBUFFER, _INVALID_ID_);
}

void FrameBuffer::AttachDepthBuffer(const char* bufferName, const unsigned& width, const unsigned& height)//Attch深度缓冲区
{
	if (m_Fbo <= _INVALID_ID_)
	{
		glGenFramebuffers(1, &m_Fbo);
	}
	glBindFramebuffer(GL_FRAMEBUFFER, m_Fbo);

	GLuint depthMap = ResourceManager::CreateTexture2D(nullptr, width, height, GL_DEPTH_COMPONENT, GL_FLOAT);
	glFramebufferTexture2D(GL_FRAMEBUFFER, GL_DEPTH_ATTACHMENT, GL_TEXTURE_2D, depthMap, 0);

	//将缓冲区添加到集合，方便取出
	m_Buffer.insert(std::pair<string, GLuint>(bufferName, depthMap));
	glBindFramebuffer(GL_FRAMEBUFFER, _INVALID_ID_);
}
void FrameBuffer::Finish()//设置结束
{
	const unsigned count = m_DrawBuffer.size();
	if (count > 0)
	{
		GLenum *buffers=new GLenum[count];
		for (int i = 0; i < count; i++)
		{
			buffers[i] = m_DrawBuffer[i];
		}
		glBindFramebuffer(GL_FRAMEBUFFER, m_Fbo);
		glDrawBuffers(count, buffers);
		glBindFramebuffer(GL_DRAW_FRAMEBUFFER, 0);
		if (buffers != nullptr)
		{
			delete[] buffers;
			buffers = nullptr;
		}
	}
}
void FrameBuffer::Begin()
{
	//拿到当前的FrameBuffer
	glGetIntegerv(GL_FRAMEBUFFER_BINDING, &m_PrevFrameBuffer);
	glBindFramebuffer(GL_FRAMEBUFFER, m_Fbo);
	glClearColor(0, 0, 0, 0);
	glClear(GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT);
}

void FrameBuffer::End()
{
	SceneManager::DrawCommit();
	glBindFramebuffer(GL_FRAMEBUFFER, _INVALID_ID_);
}

GLuint FrameBuffer::GetBuffer(const char* bufferName)
{
	auto it = m_Buffer.find(bufferName);
	if (it != m_Buffer.end())
	{
		return it->second;
	}
	else
	{
		return _INVALID_ID_;
	}
}