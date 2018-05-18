#pragma once
#include "vector3f.h"

class Camera
{
protected:
	void RotateView(float angle, float x, float y, float z);
public:
	Camera();
	Vector3f mPos,mViewCenter,mUp;
	bool mbMoveLeft, mbMoveRight,mbMoveForward,mbMoveBackward;
	void Update(float deltaTime);
};