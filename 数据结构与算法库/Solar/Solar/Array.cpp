#include "Array.h"

namespace Solar
{
	template<TypeName T>
	Array<T>::Array(const int32_i& lenth)
	{
		BEGTRY
		m_Arr = new T[lenth];
		this->m_Lenth = lenth;
		ENDTRY
	}

	template<TypeName T>
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

	template<TypeName T>
	void Array<T>::CopyFrom(const Array<T>& from)
	{
		
	}

	template<TypeName T>
	iterator Array<T>::begin()
	{
		BEGTRY
		return begin(this->m_Arr);
		ENDTRY
		return  nullptr;
	}
};
