#include "BinarySearchTree.h"

//加入树
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
	else if(root->data>=data)         //root大，往左走
	{              
		Put(data, root->pLeft);
	}
	else if          //root小，往右走
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
	else if (*(root->data) > data)      //root大，往左找
	{
		Get(data, root->pLeft);
	}
	else if (*(root->data) < data)     //root小，往右找
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
/*功能描述：用二叉搜索树的右孩子替代它
参数：
返回值：*/
template<class T>
void BinarySearchTree<T>::DeleteMin()
{
	//找到它的最小节点
	Node<T>& target = GetMin(this->root);
	//让它的右孩子代替它
	target.data = target.pRight->data;
	if (target.pRight != nullptr)
	{
		delete target.pRight;
	}
}

template<class T>
void BinarySearchTree<T>::DeleteMax()
{
	//找到它的最大节点
	Node<T>& target = GetMin(this->root);
	//让它的左孩子代替它
	target.data = target.pLeft->data;
	if (target.pLeft != nullptr)
	{
		delete target.pLeft;
	}
};

template<class T>
Node& BinarySearchTree<T>::Delete(const T& data)      //拿要删除节点的右子树最小节点 代替要删除的节点
{

	


}



template<class T>
void BinarySearchTree<T>::Clear()
{

}