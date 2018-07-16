#pragma once
#include "BoxCollider.h"
#include "SphereCollider.h"
#include "CapsuleCollider.h"
#include "AABB.h"
#include "Ray.h"
#include "Plane.h"
#include "Triangle.h"

//����㷨����
bool OnCollider(SphereCollider2D sp1,SphereCollider2D sp2);          //����Բ

bool OnCollider(AABBCollider2D aabb1, AABBCollider2D aabb2);        //AABB��AABB

bool OnCollider(Ray ray, Plane plane);                             //������ƽ��

bool OnCollider(Ray ray, Triangle2D triangle);                     //������������