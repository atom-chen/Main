
class BinatySearchTree{
	template<class T>
	class Node{
	private:
		Node *left;
		Node *right;
		Node *parent;
		T *data;
	public:
		const Node& getLeft() const{ return *(this->left) };
		const Node& getRight() const{ return *(this->right) };
		const Node& getParent() const{ return *(this->parent) };
		const T& getData() const{ return *(this->data) };
	public:
		void setLeft(Node *left){ this->left = left };
		void setRight(Node *right){ this->right = right };
		void setParent(Node *parent){ this->parent = parent };
		void setData(T *data){ this->data = data };
	};
};
