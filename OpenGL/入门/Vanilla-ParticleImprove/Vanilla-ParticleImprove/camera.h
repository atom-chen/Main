#pragma once
#include "vector3f.h"

class Camera
{
protected:
	void RotateView(float angle, float x, float y, float z);
public:
	Camera();
	Vector3f mPos,mViewCenter,mUp;
	int mViewportWidth, mViewportHeight;
	bool mbMoveLeft, mbMoveRight,mbMoveForward,mbMoveBackward;
	void Yaw(float angle);
	void Pitch(float angle);
	void Update(float deltaTime);
	void SwitchTo3D();
	void SwitchTo2D();
};