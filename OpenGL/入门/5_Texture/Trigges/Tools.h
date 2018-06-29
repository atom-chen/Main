#pragma  once
#include "main.h"

bool LoadTGATexture(const char* filePath, GLenum minFilter = GL_LINEAR, GLenum magFilter = GL_LINEAR, GLenum wrapMode = GL_CLAMP_TO_EDGE);

void MakePyramid(GLBatch& pyramidBatch);