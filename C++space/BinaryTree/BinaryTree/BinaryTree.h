#pragma once

template<class T>
struct Node{
	Node* pLeft;
	Node* pRight;
	Node* pPerent;
	T* data;
	unsigned size;     //�Ե�ǰ�ڵ�Ϊ���������ڵ�����
};

template<class T>
class BinaryTree{
public:
	unsigned size(){ return size; };
protected:
	Node<T>* root;
};