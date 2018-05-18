#pragma once
#include "Plane.hpp"

class   Frustum
{
public:
	enum
	{
		FRUSTUM_LEFT = 0,
		FRUSTUM_RIGHT = 1,
		FRUSTUM_TOP = 2,
		FRUSTUM_BOTTOM = 3,
		FRUSTUM_FAR = 4,
		FRUSTUM_NEAR = 5,
	};
public:
	/**
	*   project * modleview
	*/
	void    loadFrustum(const glm::mat4 &mvp)
	{
		const float*  dataPtr = glm::value_ptr(mvp);
		_planes[FRUSTUM_LEFT] = Plane(dataPtr[12] - dataPtr[0], dataPtr[13] - dataPtr[1], dataPtr[14] - dataPtr[2], dataPtr[15] - dataPtr[3]);
		_planes[FRUSTUM_RIGHT] = Plane(dataPtr[12] + dataPtr[0], dataPtr[13] + dataPtr[1], dataPtr[14] + dataPtr[2], dataPtr[15] + dataPtr[3]);

		_planes[FRUSTUM_TOP] = Plane(dataPtr[12] - dataPtr[4], dataPtr[13] - dataPtr[5], dataPtr[14] - dataPtr[6], dataPtr[15] - dataPtr[7]);
		_planes[FRUSTUM_BOTTOM] = Plane(dataPtr[12] + dataPtr[4], dataPtr[13] + dataPtr[5], dataPtr[14] + dataPtr[6], dataPtr[15] + dataPtr[7]);

		_planes[FRUSTUM_FAR] = Plane(dataPtr[12] - dataPtr[8], dataPtr[13] - dataPtr[9], dataPtr[14] - dataPtr[10], dataPtr[15] - dataPtr[11]);
		_planes[FRUSTUM_NEAR] = Plane(dataPtr[12] + dataPtr[8], dataPtr[13] + dataPtr[9], dataPtr[14] + dataPtr[10], dataPtr[15] + dataPtr[11]);
	}

	bool  pointInFrustum(const vec3 &pos) const
	{
		for (int i = 0; i < 6; i++)
		{
			if (_planes[i].distance(pos) <= 0)
				return false;
		}
		return true;
	}

	bool  sphereInFrustum(const vec3 &pos, const float radius) const
	{
		for (int i = 0; i < 6; i++)
		{
			if (_planes[i].distance(pos) <= -radius)
				return false;
		}
		return true;
	}

	bool cubeInFrustum(float minX, float maxX, float minY, float maxY, float minZ, float maxZ) const
	{
		for (int i = 0; i < 6; i++)
		{
			if (_planes[i].distance(vec3(minX, minY, minZ)) > 0) continue;
			if (_planes[i].distance(vec3(minX, minY, maxZ)) > 0) continue;
			if (_planes[i].distance(vec3(minX, maxY, minZ)) > 0) continue;
			if (_planes[i].distance(vec3(minX, maxY, maxZ)) > 0) continue;
			if (_planes[i].distance(vec3(maxX, minY, minZ)) > 0) continue;
			if (_planes[i].distance(vec3(maxX, minY, maxZ)) > 0) continue;
			if (_planes[i].distance(vec3(maxX, maxY, minZ)) > 0) continue;
			if (_planes[i].distance(vec3(maxX, maxY, maxZ)) > 0) continue;
			return false;
		}
		return true;
	}

	const Plane &getPlane(const int plane) const
	{
		return _planes[plane];
	}
protected:
	Plane    _planes[6];
};