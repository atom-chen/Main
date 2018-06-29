#include "Tools.h"

//��tga�ļ���ȡ���Դ�
GLuint LoadTGATexture(const char* filePath, GLenum minFilter, GLenum magFilter, GLenum wrapMode)
{
	GLuint texture;
	glGenTextures(1, &texture);
	glBindTexture(GL_TEXTURE_2D, texture);
	GLbyte *pTGA;
	int nWidth, nHeight, nComponts;
	GLenum eFormat;
	pTGA = gltReadTGABits(filePath, &nWidth, &nHeight, &nComponts, &eFormat);
	if (pTGA == nullptr)
	{
		return -1;
	}
	//������
	glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_WRAP_S, wrapMode);
	glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_WRAP_T, wrapMode);
	//��������
	glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MIN_FILTER, minFilter);
	glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MAG_FILTER, magFilter);

	free(pTGA);

	if (minFilter == GL_LINEAR_MIPMAP_LINEAR || minFilter == GL_LINEAR_MIPMAP_NEAREST
		|| minFilter == GL_NEAREST_MIPMAP_LINEAR || minFilter = GL_LINEAR_MIPMAP_NEAREST)
	{
		glGenerateMipmap(GL_TEXTURE_2D);
	}
}