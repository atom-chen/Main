#include "camera.h"
#include <windows.h>
#include <gl/GL.h>
#include <gl/GLU.h>
#include <stdio.h>


Camera::Camera() :mPos(0.0f, 0.0f, 5.0f),
mViewCenter(0.0f, 0.0f, 4.0f),
mUp(0.0f, 1.0f, 0.0f),
mbMoveLeft(false),
mbMoveRight(false),
mbMoveForward(false),
mbMoveBackward(false)
{
	
}

//rotate view direction
void Camera::RotateView(float angle, float x, float y, float z)
{
	Vector3f viewDirection = mViewCenter - mPos;
	Vector3f newDirection;
	float C = cosf(angle);
	float S = sinf(angle);

	Vector3f tempX(C+x*x*(1-C),x*y*(1-C)-z*S,x*z*(1-C)+y*S);
	newDirection.x = tempX*viewDirection;

	Vector3f tempY(x*y*(1-C)+z*S, C+y*y*(1-C), y*z*(1-C)-x*S);
	newDirection.y = tempY*viewDirection;

	Vector3f tempZ(x*z*(1 - C) - y*S,y*z*(1 - C)+x*S, C + z*z*(1-C));
	newDirection.z = tempZ*viewDirection;

	mViewCenter = mPos + newDirection;
}

void Camera::Yaw(float angle)
{
	RotateView(angle, mUp.x, mUp.y, mUp.z);
}

void Camera::Pitch(float angle)
{
	//right direction vector
	Vector3f viewDirection = mViewCenter - mPos;
	viewDirection.Normalize();
	Vector3f rightDirection = viewDirection^mUp;
	rightDirection.Normalize();
	RotateView(angle, rightDirection.x, rightDirection.y, rightDirection.z);
}

void Camera::Update(float deltaTime)
{
	//update everything
	float moveSpeed = 10.0f;
	float rotateSpeed = 1.0f;
	if (mbMoveLeft)
	{
		//left direction vector
		Vector3f viewDirection = mViewCenter - mPos;
		viewDirection.Normalize();
		Vector3f rightDirection = viewDirection^mUp;
		rightDirection.Normalize();
		mPos = mPos + rightDirection*moveSpeed*deltaTime*-1.0f;
		mViewCenter = mViewCenter + rightDirection*moveSpeed*deltaTime*-1.0f;
	}
	if (mbMoveRight)
	{
		//right direction vector
		Vector3f viewDirection = mViewCenter - mPos;
		viewDirection.Normalize();
		Vector3f rightDirection = viewDirection^mUp;
		rightDirection.Normalize();
		mPos = mPos + rightDirection*moveSpeed*deltaTime;
		mViewCenter = mViewCenter + rightDirection*moveSpeed*deltaTime;
	}
	if (mbMoveForward)
	{
		//left direction vector
		Vector3f forwardDirection=mViewCenter-mPos;
		forwardDirection.Normalize();
		mPos = mPos + forwardDirection*moveSpeed*deltaTime;
		mViewCenter = mViewCenter + forwardDirection*moveSpeed*deltaTime;
	}
	if (mbMoveBackward)
	{
		//right direction vector
		Vector3f backwardDirection= mPos - mViewCenter;
		backwardDirection.Normalize();
		mPos = mPos + backwardDirection*moveSpeed*deltaTime;
		mViewCenter = mViewCenter + backwardDirection*moveSpeed*deltaTime;
	}
	//set model view matrix
	gluLookAt(mPos.x, mPos.y, mPos.z,
		mViewCenter.x, mViewCenter.y, mViewCenter.z,
		mUp.x, mUp.y, mUp.z);
}

void Camera::SwitchTo3D()
{
	glMatrixMode(GL_PROJECTION);//tell the gpu processer that i would select the projection matrix
	glLoadIdentity();
	gluPerspective(50.0f, (float)mViewportWidth / (float)mViewportHeight, 0.1f, 1000.0f);//set some values to projection matrix
	glMatrixMode(GL_MODELVIEW);//tell .... model view matrix
}

void Camera::SwitchTo2D()
{
	glMatrixMode(GL_PROJECTION);//tell the gpu processer that i would select the projection matrix
	glLoadIdentity();
	gluOrtho2D(-mViewportWidth/2,mViewportWidth/2,-mViewportHeight/2,mViewportHeight/2);
	glMatrixMode(GL_MODELVIEW);//tell .... model view matrix
}