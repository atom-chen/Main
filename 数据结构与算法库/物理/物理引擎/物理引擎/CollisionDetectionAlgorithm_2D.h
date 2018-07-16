#pragma once
#include "BoxCollider.h"
#include "SphereCollider.h"
#include "CapsuleCollider.h"
#include "AABB.h"
#include "Ray.h"
#include "Plane.h"
#include "Triangle.h"

//检测算法定义
bool OnCollider(SphereCollider2D sp1,SphereCollider2D sp2);          //两个圆

bool OnCollider(AABBCollider2D aabb1, AABBCollider2D aabb2);        //AABB与AABB

bool OnCollider(Ray ray, Plane plane);                             //射线与平面

bool OnCollider(Ray ray, Triangle2D triangle);                     //射线与三角形