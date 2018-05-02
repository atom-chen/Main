#pragma once
#include "to.h"

namespace Solar
{
	template<class T>
	class  OrderContainer
	{
	public:
		virtual void push_back(const T& obj)=0;
		virtual T& at(const int& index)=0;

		virtual T& operator[](const int& index) = 0;
		virtual void operator==(const T& other) = 0;


		virtual int Size()=0;
	protected:
	private:
	};
}

