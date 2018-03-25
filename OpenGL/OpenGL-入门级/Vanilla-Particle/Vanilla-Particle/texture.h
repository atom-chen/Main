#pragma once
#include <windows.h>
#include <gl/GL.h>
#include <string>
#include <unordered_map>

#define GL_CLAMP_TO_EDGE 0x812F

class Texture
{
protected:
	void Init(const char*imagePath,bool invertY, GLenum wrapMode);
public:
	GLuint mTextureID;//gpu
	std::string mPath;
	static Texture*LoadTexture(const char*imagePath, bool invertY = true,GLenum wrapMode= GL_CLAMP_TO_EDGE);
	static void UnloadTexture(Texture*texture);
	static std::unordered_map<std::string, Texture*> mCachedTextures;
};