#include "FrameBuffer.h"
#include "Utils.h"
#include "Resource.h"



FrameBuffer::FrameBuffer()
{
	glGenFramebuffers(1, &m_Fbo);
}
FrameBuffer::~FrameBuffer()
{
	glDeleteFramebuffers(1, &this->m_Fbo);
}

void FrameBuffer::AttachColorBuffer(const char* bufferName, GLenum attachment,const unsigned& width, const unsigned& height)
{
	GLuint colorBuffer;
	glBindFramebuffer(GL_FRAMEBUFFER, m_Fbo);

	//����һ����texture��Ϊ����ֵ
	colorBuffer = ResourceManager::CreateTexture2D(nullptr, width, height, GL_RGBA, GL_UNSIGNED_BYTE);
	//׷����ɫ��������fbo
	glFramebufferTexture2D(GL_FRAMEBUFFER, attachment, GL_TEXTURE_2D, colorBuffer, 0);

	m_DrawBuffer.push_back(attachment);
	//����ֻ��Ϊ��ʶ
	m_Buffer.insert(std::pair<string, GLuint>(bufferName, colorBuffer));
	glBindFramebuffer(GL_FRAMEBUFFER, _INVALID_ID_);
}

void FrameBuffer::AttachDepthBuffer(const char* bufferName, const unsigned& width, const unsigned& height)//Attch��Ȼ�����
{
	glBindFramebuffer(GL_FRAMEBUFFER, m_Fbo);

	GLuint depthMap = ResourceManager::CreateTexture2D(nullptr, width, height, GL_DEPTH_COMPONENT, GL_FLOAT);
	glFramebufferTexture2D(GL_FRAMEBUFFER, GL_DEPTH_ATTACHMENT, GL_TEXTURE_2D, depthMap, 0);

	//����������ӵ����ϣ�����ȡ��
	m_Buffer.insert(std::pair<string, GLuint>(bufferName, depthMap));
	glBindFramebuffer(GL_FRAMEBUFFER, _INVALID_ID_);
}
void FrameBuffer::Finish()//���ý���
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
	//�õ���ǰ��FrameBuffer
	glGetIntegerv(GL_FRAMEBUFFER_BINDING, &m_PrevFrameBuffer);
	glBindFramebuffer(GL_FRAMEBUFFER, m_Fbo);
	glClearColor(0, 0, 0, 0);
	glClear(GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT);
}

void FrameBuffer::End()
{
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