#pragma once
#include "ggl.h"

//��һ���ļ� ���������ڴ��е�ָ�롢�ļ�����
unsigned char* LoadFileContent(const char* path, int& filesize);

//����BMP�ļ����������ļ���ʼ��ַ��ͼƬ����
unsigned char* DecodeBMP(unsigned char* bmpFileData, int& width, int& height);

//����2D�������
GLuint CreateTexture2D(unsigned char* pixelData, int width, int height, GLenum type);

//��BMP����2D�������
GLuint CreateTexture2DFromBMP(const char* bmpPath);