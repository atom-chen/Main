#include "to.h"

namespace Solar
{
	//�ڲ���װ������
	template<TypeName T>
	class Array
	{
	public:
		Array(const int32_i& lenth);
		~Array();
		void CopyFrom(const Array<T>& from);
	public:
		iterator begin();

	protected:
	private:
		T* m_Arr=nullptr;
		int32_i m_Lenth = INVALID;
	};
}
