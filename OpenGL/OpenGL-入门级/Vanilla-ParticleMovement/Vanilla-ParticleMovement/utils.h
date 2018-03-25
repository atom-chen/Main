#pragma once
#include <windows.h>
#include <gl/GL.h>
#include <stdio.h>
#include <functional>

unsigned char* LoadFileContent(const char*filePath);
GLuint CaptureScreen(int width, int height, std::function<void()> foo);
void SaveScreenPixelToFile(int width, int height, std::function<void()> foo, const char*filePath);
GLuint CreateTexture(int size);
float randf();//0.0~1.0
float srandf();//-1.0~1.0