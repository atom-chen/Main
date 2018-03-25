#pragma once

class List
{
public:
	List() :mNext(nullptr) {}
	List*mNext;
	template<typename T>
	T*Next()
	{
		return (T*)mNext;
	}
	void Push(List*next)
	{
		List*lastNode = this;
		while (lastNode->mNext!=nullptr)
		{
			lastNode = lastNode->Next<List>();
		}
		lastNode->mNext = next;
	}
};

class RenderableObject : public List
{
public:
	virtual void Draw()
	{
		if (mNext!=nullptr)
		{
			Next<RenderableObject>()->Draw();
		}
	}
};