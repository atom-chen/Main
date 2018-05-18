#include "Vertex.h"
#include "Utils.h"
#include "Resource.h"



FrameBuffer::FrameBuffer()
{

}
FrameBuffer::~FrameBuffer()
{
	glDeleteFramebuffers(1, &this->m_Fbo);
}

void FrameBuffer::Init(const unsigned& width, const unsigned& height)
{
	this->m_Width = width;
	this->m_Height = height;
	glGenFramebuffers(1, &m_Fbo);
	glBindFramebuffer(GL_FRAMEBUFFER, m_Fbo);

	glGenRenderbuffers(1, &m_DepthBuffer);//��Ȼ�����
	glBindRenderbuffer(GL_RENDERBUFFER, m_DepthBuffer);
	glRenderbufferStorage(GL_RENDERBUFFER, GL_DEPTH_COMPONENT16, width, height);
	glFramebufferRenderbuffer(GL_FRAMEBUFFER, GL_DEPTH_ATTACHMENT, GL_RENDERBUFFER, m_DepthBuffer);
	
	m_Texture = ResourceManager::CreateTexture2D(NULL, width, height, GL_RGBA);//�հ���ͼ
	glFramebufferTexture2D(GL_FRAMEBUFFER, GL_COLOR_ATTACHMENT0, GL_TEXTURE_2D, m_Texture, 0);

	glBindFramebuffer(GL_FRAMEBUFFER, 0);
}

void FrameBuffer::Begin()
{
	glBindFramebuffer(GL_FRAMEBUFFER, m_Fbo);
}

void FrameBuffer::End()
{
	glBindFramebuffer(GL_FRAMEBUFFER, _INVALID_ID_);
}