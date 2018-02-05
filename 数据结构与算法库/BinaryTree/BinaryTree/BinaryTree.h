#pragma once

template<class T>
struct Node{
	Node* pLeft;
	Node* pRight;
	Node* pPerent;
	T* data;
	unsigned size;     //以当前节点为根的子树节点数量
};

template<class T>
class BinaryTree{
public:
	unsigned size(){ return size; };
protected:
	Node<T>* root;
};