#pragma once
#include "ggl.h"


bool Init();
void SetViewPortSize(float width, float height);
void Update();
void Draw();

void OnKeyDown(char KeyCode);//���¼���ʱ����
void OnKeyUp(char KeyCode);//�ɿ�����ʱ������
void OnMouseMove(float deltaX, float deltaY);//����ƶ�������תʱ������