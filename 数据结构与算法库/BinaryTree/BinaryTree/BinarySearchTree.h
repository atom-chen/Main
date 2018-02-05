#pragma once
#include "BinaryTree.h"

template<T>
class BinarySearchTree :public BinaryTree<T>{
public:
	void Put(const T &data);
	Node& Get(const T &data) const;
	Node& GetMin() const;
	Node& GetMax() const;
	Node& Delete(const T& data);
	void DeleteMin();
	void DeleteMax();
	void Clear();
private:
	void Put(const T &data,Node* root);
	Node& Get(const T &data,Node* root) const;
};