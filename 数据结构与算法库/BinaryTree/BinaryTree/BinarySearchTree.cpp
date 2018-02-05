#include "BinarySearchTree.h"

template<class T>
//������
void BinarySearchTree::Put(const T &data)
{
	BinarySearchTree(data, root);
}
template<class T>
void BinarySearchTree::Put(const T &data,N* root)
{
	if (root == nullptr)
	{
		root->data = data;
	}
	else if(root->data>=data){              //root��������
		Put(data, root->left);
	}
	else if (root->data < data)             //rootС��������
	{
		Put(data, root->right);
	}
}

template<class T>
Node& BinarySearchTree::Get(const T &data) const
{
	return Get(data, root);
}
template<class T>
Node& BinarySearchTree::Get(const T &data, Node* root) const
{
	if (root == nullptr || root->data == data)
	{
		return *root;
	}
	else if (root->data > data)      //root��������
	{
		Get(data, root->pLeft);
	}
	else if (root->data < data)     //rootС��������
	{
		Get(data, root->pRight);
	}
}

template<class T>
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
Node& BinarySearchTree::GetMax(Node* root = this->root) const
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
void BinarySearchTree::DeleteMin()
{
	
}

template<class T>
void BinarySearchTree::DeleteMax()
{

}
template<class T>
Node& BinarySearchTree::Delete(const T& data)      //��Ҫɾ���ڵ����������С�ڵ� ����Ҫɾ���Ľڵ�
{
	//���ҵ�Ҫɾ���ڵ�
	Node* pTargetNode = &(Get(data));
	//�ҵ�Ҫɾ���ڵ����������С�ڵ�
	Node* pTargetLeft = GetMin(pTargetNode->pRight);
	
	//����������С�ڵ��滻Ҫɾ���Ľڵ�
	


}



template<class T>
void BinarySearchTree::Clear()
{

}