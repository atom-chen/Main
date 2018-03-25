#include "displaylist.h"

void DisplayList::Init(std::function<void()>foo)
{
	mDisplayList = glGenLists(1);
	glNewList(mDisplayList, GL_COMPILE);
	foo();
	glEndList();
}

void DisplayList::Draw()
{
	glCallList(mDisplayList);
}