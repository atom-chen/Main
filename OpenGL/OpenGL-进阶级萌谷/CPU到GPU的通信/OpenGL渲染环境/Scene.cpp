#include "Scene.h"
#include "Utils.h"
#include "SkyBox.h"
#include "Model.h"
#include "Ground.h"
#include "Light.h"
#include "Camera.h"
#include "Sprite.h"
#include "ParticleSystem.h"

GLuint vbo;
bool Init()
{
	float data[] = { -0.2f, -0.2f, -0.6f, 1.0f, 0.2f, -0.2f, -0.6f, 1.0f, 0, 0.2f, -0.6f, 1 };
	
	glGenBuffers(1, &vbo);//申请一个vbo,然后传vbo标识变量的地址。OpenGL会去修改这块内存，使之指向某个显存
	glBindBuffer(GL_ARRAY_BUFFER, vbo);//设置为当前vbo
	glBufferData(GL_ARRAY_BUFFER, sizeof(float) * 12, data, GL_STATIC_DRAW);//在当前vbo分配12个float大的内存，存入的值为data指针指向的这块内存，存入显存后就不会修改它了
	glBindBuffer(GL_ARRAY_BUFFER, 0);

	return 1;
}
void SetViewPortSize(float width, float height)
{

}
void Update()
{

}
void Draw()
{

}
void OnKeyDown(char KeyCode)
{

}

void OnKeyUp(char KeyCode)
{
}

void OnMouseMove(float deltaX, float deltaY)
{


}

