#include "Sort.h"
#include "Tools.h"



namespace Solar
{
	template<class T>
	void Sort(OrderContainer<T>& container, int begin, int end)
	{
		if (end > begin)
		{
			int mid = GetMidIndex(container, begin, end);
			Sort(container, begin, mid);
			Sort(container, mid+1, end);
		}
	}

	template<class T>
	int GetMidIndex(OrderContainer<T>& container,int begin,int end)
	{
		T& key = container[begin];
		int index = begin;
		for (int i = index + 1; i < end; i++)
		{
			//把比key小的扔到左边
			if (container[i] <= key)
			{
				index++;
				Swap<T>(container[i], container[index]);
			}
		}
		Swap<T>(container[begin], container[index]);
		return index;
	}
}