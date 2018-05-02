#pragma once
#include "to.h"
#include "OrderContainer.h"

namespace Solar
{
	//内部封装数组类
	template<class T>
	class Array:public OrderContainer<T>
	{
	public:
		Array(const int32_i& lenth);
		~Array();
		void CopyFrom(const Array<T>& from);
	public:
		virtual void push_back(const T& obj);
		virtual T at(const int& index);

		virtual T operator[](const int& index);
		virtual void operator==(const T& other);

		virtual int Size();
	protected:
	private:
		T* m_Arr=nullptr;
		int32_i m_Lenth = INVALID;
	};
}
