#include "Light.h"

namespace B3D
{
	int LightManager::CreateLight(const LightType& lightType)
	{
		if (this->m_Lights.size() < MaxLights)
		{
			Light *light=new Light(lightType);
			int key = 0;
			while (true)
			{
				auto it = this->m_Lights.find(key);
				if (it != this->m_Lights.end())
				{
					pair<map<int,Light*>::iterator,bool> p=m_Lights.insert(pair<int, Light*>(key, light));
					if (p.second == true)
					{
						return key;
					}
					else
					{
						return -1;
					}
				}
				else
				{
					key++;
				}
			}
		}
		return -1;
	}
	Light* LightManager::GetLight(int id)
	{
		auto it = m_Lights.find(id);
		if (it != m_Lights.end())
		{
			return (it->second);
		}
		else
		{
			return nullptr;
		}
	}
	void LightManager::ClearLight()
	{
		for (auto it = m_Lights.begin(); it != m_Lights.end();)
		{
			it->second->~Light();
			DeletePtr(it->second);
			it++;
		}
		m_Lights.clear();
	}
	LightManager& LightManager::Instance()
	{
		if (this->m_Instance == nullptr)
		{
			m_Instance = new LightManager();
		}
		return *m_Instance;
	}
}
