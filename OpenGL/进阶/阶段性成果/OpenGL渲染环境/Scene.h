#pragma once
#include "ggl.h"


bool Awake();
void Start();

void Update();
void OnDrawBegin();
void Draw();
void OnDrawOver();

void OnKeyDown(char KeyCode);//���¼���ʱ����
void OnKeyUp(char KeyCode);//�ɿ�����ʱ������
void OnMouseMove(float deltaX, float deltaY);//����ƶ�������תʱ������