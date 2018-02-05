#include "BinarySearchTree.h"

template<class T>
//加入树
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
	else if(root->data>=data){              //root大，往左走
		Put(data, root->left);
	}
	else if (root->data < data)             //root小，往右走
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
	else if (root->data > data)      //root大，往左找
	{
		Get(data, root->pLeft);
	}
	else if (root->data < data)     //root小，往右找
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
template<class T>
Node& BinarySearchTree::Delete(const T& data)      //拿要删除节点的右子树最小节点 代替要删除的节点
{
	//先找到要删除节点
	Node* pTargetNode = &(Get(data));
	//找到要删除节点的右子树最小节点
	Node* pTargetLeft = GetMin(pTargetNode->pRight);
	
	//让右子树最小节点替换要删除的节点
	


}

template<class T>
void BinarySearchTree::DeleteMin()
{

}
template<class T>
void BinarySearchTree::DeleteMax()
{

}

template<class T>
void BinarySearchTree::Clear()
{

}