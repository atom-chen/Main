#pragma once
#include "ggl.h"

bool Init();
void Draw();

void OnKeyDown(char KeyCode);//按下键盘时调用
void OnKeyUp(char KeyCode);//松开键盘时被调用
void OnMouseMove(float deltaX, float deltaY);//鼠标移动导致旋转时被调用