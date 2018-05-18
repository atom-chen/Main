#pragma once
#include "ggl.h"


bool Awake();
void Start();

void Update();
void OnDrawBegin();
void Draw3D();
void Draw2D();
void OnDrawOver();

void SetViewPortSize(float width, float height);

void OnKeyDown(char KeyCode);//���¼���ʱ����
void OnKeyUp(char KeyCode);//�ɿ�����ʱ������
void OnMouseMove(float deltaX, float deltaY);//����ƶ�������תʱ������
void OnMouseWheel(int32_t direction);