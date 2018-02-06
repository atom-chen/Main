#pragma once
#include "BinaryTree.h"

class BinarySearchTree :public BinaryTree{
public:
	void Put(const int &data);
	Node& Get(const int &data) const;
	Node& GetMin() const;
	Node& GetMax() const;
	Node& Delete(const int& data);
	void DeleteMin();
	void DeleteMax();
	void Clear();
private:
	void Put(const int &data,Node* root);
	Node& Get(const int &data,Node* root) const;
	Node& GetMin(Node* root) const;
};