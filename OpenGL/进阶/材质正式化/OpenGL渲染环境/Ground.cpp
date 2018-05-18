#include "Ground.h"

Ground::Ground()
{
	m_Material.SetAmbientMaterial(0.1f, 0.1f, 0.1f,1);
	m_Material.SetDiffuseMaterial(0.6f, 0.6f, 0.6f, 1);
	m_Material.SetSepcularMaterial(0, 0, 0, 0);
}
bool Ground::Init()
{
	m_VertexBuf.Init(4);

	//起始坐标
	int32_t zStart = 100;
	int32_t xStart = -100;
	int32_t length = 200;

	m_VertexBuf.SetPosition(0, xStart, height, zStart);
	m_VertexBuf.SetPosition(1, xStart + length, height, zStart);
	m_VertexBuf.SetPosition(2, xStart, height, zStart - length);
	m_VertexBuf.SetPosition(3, xStart + length, height, zStart - length);

	m_VertexBuf.SetNormal(0, 0, 1, 0);
	m_VertexBuf.SetNormal(1, 0, 1, 0);
	m_VertexBuf.SetNormal(2, 0, 1, 0);
	m_VertexBuf.SetNormal(3, 0, 1, 0);
	m_VertexBuf.SetColor(0, 0.4f, 0.4f, 0.4f);
	m_VertexBuf.SetColor(1, 0.4f, 0.4f, 0.4f);
	m_VertexBuf.SetColor(2, 0.4f, 0.4f, 0.4f);
	m_VertexBuf.SetColor(3, 0.4f, 0.4f, 0.4f);

	m_Shader.Init("res/ground.vert", "res/ground.frag");
	SetAmbientMaterial(m_Material.GetAmbientMaterial());
	SetDiffuseMaterial(m_Material.GetDiffuseMaterial());
	return 1;
}
bool Ground::Init(const char* picName)
{
	m_VertexBuf.Init(4);

	//起始坐标
	int32_t size = 100;
	int32_t rept = 100;

	m_VertexBuf.SetPosition(0, -size, -0.5f, size);
	m_VertexBuf.SetTexcoord(0, 0, 0);
	m_VertexBuf.SetPosition(1, size, -0.5f, size);
	m_VertexBuf.SetTexcoord(1, rept,0 );
	m_VertexBuf.SetPosition(2, -size, height, -size);
	m_VertexBuf.SetTexcoord(2, rept, rept);
	m_VertexBuf.SetPosition(3, size, height, -size);
	m_VertexBuf.SetTexcoord(3, 0, rept);


	m_VertexBuf.SetNormal(0, 0, 1, 0);
	m_VertexBuf.SetNormal(1, 0, 1, 0);
	m_VertexBuf.SetNormal(2, 0, 1, 0);
	m_VertexBuf.SetNormal(3, 0, 1, 0);
	m_VertexBuf.SetColor(0, 1, 1, 1,1);
	m_VertexBuf.SetColor(1, 1, 1, 1,1);
	m_VertexBuf.SetColor(2, 1, 1, 1,1);
	m_VertexBuf.SetColor(3, 1, 1, 1,1);



	m_Shader.Init("res/ground.vert", "res/ground.frag");
	SetAmbientMaterial(m_Material.GetAmbientMaterial());
	SetDiffuseMaterial(m_Material.GetDiffuseMaterial());
	SetTexture2D(picName);
	return 1;
}

void Ground::Draw(glm::mat4& viewMatrix,glm::mat4 &ProjectionMatrix)
{
	glEnable(GL_DEPTH_TEST);
	//m_VertexBuf.Begin();
	//{
	//	m_Shader.Begin();
	//	{
	//		m_Shader.Bind(glm::value_ptr(m_ModelMatrix), glm::value_ptr(viewMatrix), glm::value_ptr(ProjectionMatrix));

	//		for (int32_t i = 0; i < 400; i++)
	//		{
	//			glDrawArrays(GL_TRIANGLE_STRIP, i * 4, 4);
	//		}
	//	}
	//	m_Shader.End();
	//}
	BEGIN
		glDrawArrays(GL_TRIANGLE_STRIP, 0, m_VertexBuf.GetLenth());
	END
}

void Ground::SetAmbientMaterial(float r, float g, float b, float a)
{
	m_Material.SetAmbientMaterial(r, g, b, a);
	m_Shader.SetVec4("U_AmbientMaterial", r, g, b, a);
}
void Ground::SetDiffuseMaterial(float r, float g, float b, float a)
{
	m_Material.SetDiffuseMaterial(r, g, b, a);
	m_Shader.SetVec4("U_DiffuseMaterial", r, g, b, a);
}
void Ground::Destory()
{
	m_Shader.Destory();
}
void Ground::SetLight_1(const Light& light1)
{
	switch (light1.GetType())
	{
	case 0:
		break;
	case 1://方向光
		m_Shader.SetVec4("U_Light1_Pos", light1.GetDirection());
		m_Shader.SetVec4("U_Light1_Ambient", light1.GetAmbientColor());
		m_Shader.SetVec4("U_Light1_Diffuse", light1.GetDiffuseColor());
		m_Shader.SetVec4("U_Light1_Specular", light1.GetSpecularColor());
		m_Shader.SetVec4("U_Light1_Opt", 1, 0, 0, 32);
		break;
	case 2://点光源
		m_Shader.SetVec4("U_Light1_Pos", light1.GetPosition());
		m_Shader.SetVec4("U_Light1_Ambient", light1.GetAmbientColor());
		m_Shader.SetVec4("U_Light1_Diffuse", light1.GetDiffuseColor());
		m_Shader.SetVec4("U_Light1_Specular", light1.GetSpecularColor());
		m_Shader.SetFloat("U_Light1_Constant", light1.GetConstAttenuation());
		m_Shader.SetFloat("U_Light1_Linear", light1.GetLinearAttenuation());
		m_Shader.SetFloat("U_Light1_Quadric", light1.GetLinearAttenuation());
		m_Shader.SetVec4("U_Light1_Opt", 2, 0, 0, 32);
		break;
	default:
		break;
	}
}



void Ground::SetTexture2D(const char* picName)
{
	m_Shader.SetTexture2D(picName);
}
void Ground::SetTexture2D(GLuint texture)
{
	m_Shader.SetTexture2D(texture);
}