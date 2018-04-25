#include "DirectionLight.h"


DirectionLight::DirectionLight()
{
	SetAmbientColor(1, 1, 1, 1);
	SetDiffuseColor(1, 1, 1, 1);
	SetSpecularColor(1, 1, 1, 1);
	SetRotate(0, 80, 0);
	m_Type = LIGHT_DIRECTION;
}