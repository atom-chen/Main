#include "Array.h"

namespace Solar
{
	template<class T>
	Array<T>::Array(const int32_i& lenth)
	{
		BEGTRY
		m_Arr = new T[lenth];
		this->m_Lenth = lenth;
		ENDTRY
	}

	template<class T>
	Array<T>::~Array()
	{
		BEGTRY
		if (m_Arr != nullptr)
		{
			delete[] m_Arr;
			m_Arr = nullptr;
			m_Lenth = INVALID;
		}
		ENDTRY
	}

	template<class T>
	void Array<T>::CopyFrom(const Array<T>& from)
	{
		
	}

};
