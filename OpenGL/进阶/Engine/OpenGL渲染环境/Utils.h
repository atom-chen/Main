#pragma once
#include "ggl.h"


//����BMP�ļ����������ļ���ʼ��ַ��ͼƬ����
unsigned char* DecodeBMP(unsigned char* bmpFileData, int& width, int& height);


//��һ���ļ� ���������ڴ��е�ָ�롢�ļ�����
bool LoadFileContent(const char* path, int& filesize,char* content);