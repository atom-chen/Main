#pragma once
#include "VertexData.h"

class ObjModel
{
public:
	int *mIndices;
	VectexData *m_Vertexes;
	void Init(const char* objModel);
	void Draw();
protected:
private:
};