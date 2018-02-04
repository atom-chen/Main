class A{
public:
	int a;
private:

};

class B :private A{
public:
	void TT()
	{
		a = 10;
	}
};
