#pragma once
#include "ggl.h"

class  Plane
{
public:
	vec3    _normal;
	float           _distance;
public:
	Plane()
	{
		_normal = vec3(0, 0, 0);
		_distance = 0.0f;
	}
	Plane(const Plane& right)
	{
		_normal = right._normal;
		_distance = right._distance;
	}
	/** Construct a plane through a normal, and a distance to move the plane along the normal.*/
	Plane(const vec3& rkNormal, float fConstant)
	{
		_normal = rkNormal;
		_distance = -fConstant;
	}
	/** Construct a plane using the 4 constants directly **/
	Plane(float x, float y, float z, float o)
	{
		_normal = vec3(x, y, z);
		float invLen = 1.0f / (_normal).length();
		_normal *= invLen;
		_distance = o * invLen;
	}
	Plane(const vec3& rkNormal, const vec3& rkPoint)
	{
		this->redefine(rkNormal, rkPoint);
	}
	Plane(const vec3& rkPoint0, const vec3& rkPoint1, const vec3& rkPoint2)
	{
		this->redefine(rkPoint0, rkPoint1, rkPoint2);
	}
	/**
	*   µ½µãµÄ¾àÀë
	*/
	float distance(const vec3& pos) const
	{
		return  glm::dot(_normal, pos) + _distance;
	}

	/** The "positive side" of the plane is the half space to which the
	plane normal points. The "negative side" is the other half
	space. The flag "no side" indicates the plane itself.
	*/
	enum Side
	{
		NO_SIDE,
		POSITIVE_SIDE,
		NEGATIVE_SIDE,
		BOTH_SIDE
	};

	Side getSide(const vec3& rkPoint) const
	{
		float fDistance = getDistance(rkPoint);

		if (fDistance < 0.0)
			return Plane::NEGATIVE_SIDE;

		if (fDistance > 0.0)
			return Plane::POSITIVE_SIDE;

		return Plane::NO_SIDE;
	}


	Side getSide(const vec3& centre, const vec3& halfSize) const
	{
		// Calculate the distance between box centre and the plane
		float dist = getDistance(centre);

		// Calculate the maximise allows absolute distance for
		// the distance between box centre and plane
		float maxAbsDist = abs(dot(_normal,halfSize));

		if (dist < -maxAbsDist)
			return Plane::NEGATIVE_SIDE;

		if (dist > +maxAbsDist)
			return Plane::POSITIVE_SIDE;

		return Plane::BOTH_SIDE;
	}

	float getDistance(const vec3& rkPoint) const
	{
		return dot(_normal,rkPoint) + _distance;
	}

	void redefine(const vec3& rkPoint0, const vec3& rkPoint1,
		const vec3& rkPoint2)
	{
		vec3 kEdge1 = rkPoint1 - rkPoint0;
		vec3 kEdge2 = rkPoint2 - rkPoint0;
		_normal = cross(kEdge1, kEdge2);
		_normal =glm::normalize(_normal);
		_distance = -dot(_normal, rkPoint0);
	}

	/** Redefine this plane based on a normal and a point. */
	void redefine(const vec3& rkNormal, const vec3& rkPoint)
	{
		_normal = rkNormal;
		_distance = -dot(rkNormal, rkPoint);
	}


	// 	    tvec3<T> projectVector(const tvec3<T>& p) const
	//         {
	//             matrix3 xform;
	//             xform[0][0] = 1.0f - _normal.x * _normal.x;
	//             xform[0][1] = -_normal.x * _normal.y;
	//             xform[0][2] = -_normal.x * _normal.z;
	//             xform[1][0] = -_normal.y * _normal.x;
	//             xform[1][1] = 1.0f - _normal.y * _normal.y;
	//             xform[1][2] = -_normal.y * _normal.z;
	//             xform[2][0] = -_normal.z * _normal.x;
	//             xform[2][1] = -_normal.z * _normal.y;
	//             xform[2][2] = 1.0f - _normal.z * _normal.z;
	//             return xform * p;
	//         }

	/** Normalises the plane.
	@remarks
	This method normalises the plane's normal and the length scale of d
	is as well.
	@note
	This function will not crash for zero-sized vectors, but there
	will be no changes made to their components.
	@returns The previous length of the plane's normal.
	*/
	float normalise(void)
	{
		float fLength = _normal.length();

		// Will also work for zero-sized vectors, but will change nothing
		if (fLength > 1e-08f)
		{
			float fInvLength = 1.0f / fLength;
			_normal *= fInvLength;
			_distance *= fInvLength;
		}

		return fLength;
	}



	/// Comparison operator
	bool operator==(const Plane& right) const
	{
		return (right._distance == _distance && right._normal == _normal);
	}
	bool operator!=(const Plane& right) const
	{
		return (right._distance != _distance && right._normal != _normal);
	}
};