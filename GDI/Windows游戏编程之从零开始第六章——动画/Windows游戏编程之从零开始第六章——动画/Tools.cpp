
template<class T>
void QuikSort(T *array, unsigned lo, unsigned hi)
{
	if (lo >= hi)
	{
		return;
	}
	unsigned i = Sort<T>(array, lo, hi);
	QuikSort<T>(array, lo, i-1);
	QuikSort<T>(array, i + 1, hi);

	
}
template<class T>
void exChange(T &obj1, T &obj2)
{
	T temp = obj1;
	obj1 = obj2;
	obj2 = temp;
}
template<class T>
unsigned Sort(T *array, unsigned lo, unsigned hi)
{
	unsigned key = lo;
	T value = array[lo];
	unsigned i = key + 1;          //记录当前指向第几个数
	//比value小的往左走
	for (unsigned j = key; j <= hi; j++)
	{
		if (array[j] < value)
		{
			exChange<T>(array[i], array[j]);
			i++;
		}
	}
	exChange<T>(array[key], array[i]);
	return i;
}