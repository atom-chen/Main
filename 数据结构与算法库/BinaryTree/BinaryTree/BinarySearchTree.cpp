#include "BinarySearchTree.h"

//������
void BinarySearchTree::Put(const int &data)
{
	Put(data, root);
}

void Put(const int &data,Node* root)
{
	if (root == nullptr)
	{
		root->data = data;
	}
	else if(root->data>=data)         //root��������
	{              
		Put(data, root->pLeft);
	}
	else if          //rootС��������
	{
		Put(data, root->pRight);
	}
}


Node& BinarySearchTree::Get(const int &data) const
{
	return Get(data, root);
}


Node& Get(const int &data, Node* root)
{
	if (root == nullptr || *(root->data) == data)
	{
		return *root;
	}
	else if (*(root->data) > data)      //root��������
	{
		Get(data, root->pLeft);
	}
	else if (*(root->data) < data)     //rootС��������
	{
		Get(data, root->pRight);
	}
}


Node& BinarySearchTree::GetMin(Node* root=this->root) const
{
	Node* pCur = root;
	while (pCur->pLeft != nullptr)
	{
		pCur = pCur->pLeft;
	}
	return *pCur;
}

template<class T>
Node& BinarySearchTree<T>::GetMax(Node* root = this->root)
{
	Node* pCur = root;
	while (pCur->pRight != nullptr)
	{
		pCur = pCur->pRight;
	}
	return *pCur;
}
/*�����������ö������������Һ��������
������
����ֵ��*/
template<class T>
void BinarySearchTree<T>::DeleteMin()
{
	//�ҵ�������С�ڵ�
	Node<T>& target = GetMin(this->root);
	//�������Һ��Ӵ�����
	target.data = target.pRight->data;
	if (target.pRight != nullptr)
	{
		delete target.pRight;
	}
}

template<class T>
void BinarySearchTree<T>::DeleteMax()
{
	//�ҵ��������ڵ�
	Node<T>& target = GetMin(this->root);
	//���������Ӵ�����
	target.data = target.pLeft->data;
	if (target.pLeft != nullptr)
	{
		delete target.pLeft;
	}
};

template<class T>
Node& BinarySearchTree<T>::Delete(const T& data)      //��Ҫɾ���ڵ����������С�ڵ� ����Ҫɾ���Ľڵ�
{

	


}



template<class T>
void BinarySearchTree<T>::Clear()
{

}