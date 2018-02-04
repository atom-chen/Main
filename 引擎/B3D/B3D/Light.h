#pragma once
#include "B3DClass.h"
#include "Color.h"
#include "Vector.h"

namespace B3D
{
	enum  LightType
	{
		LIGHT_AMBIENT,//环境光
		LIGHT_DIRECTION,//平行光
		LIGHT_POINT//点光源
	};
	class Light
	{
	public:
		friend class LightManager;
	public:
		Light(LightType type = LIGHT_DIRECTION) :m_LightOn(1), m_Power(1), m_ShadowFactor(0)
		{
			this->m_Type = type;
			m_Ambiient = new Color(255, 255, 255, 255);
			m_Diffuse = new Color(0, 0, 0, 255);
			m_Specular = new Color(0, 0, 0, 255);

			m_Position = new Vector4(0,0,0,0);
			m_Direction = new Vector4(0, 0, 0, 0);
			m_PositionInCamera = new Vector4(0, 0, 0, 0);
			m_DirectionInCamera = new Vector4(0, 0, 0, 0);
		}

		~Light()
		{
			DeletePtr<Color>(m_Ambiient);
			DeletePtr<Color>(m_Diffuse);
			DeletePtr<Color>(m_Specular);
			DeletePtr<Vector4>(m_Position);
			DeletePtr<Vector4>(m_Direction);
			DeletePtr<Vector4>(m_PositionInCamera);
			DeletePtr<Vector4>(m_DirectionInCamera);
		}
		
	private:
		int m_Id;
		bool m_LightOn;
		LightType m_Type;
		float m_Power;//强度
		float m_ShadowFactor;//背面强度
		Color *m_Ambiient;//环境色（只有环境光有效）
		Color *m_Diffuse;//漫反射颜色
		Color *m_Specular;//高光颜色
		
		Vector4 *m_Position;//世界坐标系位置
		Vector4 *m_Direction;//方向（点光源无效）
		
		Vector4 *m_PositionInCamera;
		Vector4 *m_DirectionInCamera;

		float m_Kc, m_Kl, m_Kq;

		float m_SpotInner;//聚光灯内锥角
		float m_SpotOuter;//聚光灯外锥角
		float m_Pf;//聚光灯指数因子
	};
	class LightManager
	{
	public:
		LightManager& Instance();
		int CreateLight(const LightType& lightType);
		Light* GetLight(int id);
		void ClearLight();
	private:
		static map<int, Light *> m_Lights;
		LightManager *m_Instance;
		LightManager();
		LightManager(const LightManager& light);
		void operator=(LightManager& manager);
	};
	map<int, Light *> m_Light();
}