package com.UnionFind;
/*
 * id�����ʾ��˫�׽ڵ�
 * sz�����ʾ���������͵�Ȩ
 */

public class Sz_UF {
	//�ڵ㣨�������±���棩
	private int id[];
	//�ڵ����
	private int count;
	//�������ϵĳ�Ա����
	private int sz[];
	public Sz_UF(int size)
	{
		id=new int[size];
		count=size;
		sz=new int[size];
		for(int i=0;i<size;i++)
		{
			id[i]=i;
			sz[i]=1;
		}
		
	}
	//��ȡ�ڵ�Ĺ���
	public int Find(int p)
	{
		//��������Ի������ڵ㣩
		while(id[p]!=p)
		{
			p=id[p];//��ȡ����˫�׽ڵ㣬ֱ�����ĸ��ڵ㣨������
		}
		return p;
	}
	//�ж������ڵ��Ƿ�ͬ��
	public boolean connected(int p,int q)
	{
		return Find(p)==Find(q);
	}
	//�ϲ������ڵ�
	public void Union(int p,int q)
	{
		int p_root=Find(p);
		int q_root=Find(q);
		if(p_root==q_root)
		{
			return;
		}else if(sz[p_root]<sz[q_root]){
			//��С���������
			id[p_root]=q_root;
		}
		else{
			id[q_root]=p_root;
		}
	}

}
