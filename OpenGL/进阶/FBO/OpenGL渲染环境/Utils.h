#pragma once
#include "ggl.h"


//����BMP�ļ����������ļ���ʼ��ַ��ͼƬ����
unsigned char* DecodeBMP(unsigned char* bmpFileData, int& width, int& height);

//��ȡһ֡�����ĵ�ʱ��
float GetFrameTime();

