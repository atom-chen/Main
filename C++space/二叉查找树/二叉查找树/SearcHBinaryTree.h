#include<iostream>
#include<string>
#include<vector>
using namespace std;
template<class T>
class  SearchTree{
public:
	SearchTree();
	SearchTree& operator=(const SearchTree& their);
	SearchTree(const SearchTree& obj);
public:
	const T& getLeft() const{ 
		return *left 
	};
	void setLeft(const T &obj){ 
		this->left->data = &obj 
	};
	const T& getRight const(){
		return *right;
	}
	void setRighe(const T &obj){
		this->right->data = obj;
	}
	const T& getData const(){
		return *data;
	}
	void setData(const T &obj){
		this->data = obj;
	}
public:
protected:
private:
	SearchTree *left;
	SearchTree *right;
	T *data;
};