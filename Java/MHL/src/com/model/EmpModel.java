package com.model;
//���ܣ����µǼǱ�ģ��
import javax.swing.table.AbstractTableModel;

import com.databasehelper.SQLHelper;

import java.sql.*;
import java.util.*;
public class EmpModel extends AbstractTableModel{
	SQLHelper sh=null;
	ResultSet rs=null;
	//ģ�ͱ������
	Vector<String> column=null;
	//ģ�ͱ��Ԫ��
	Vector<Vector> row=null;
	//���췽������ʼ��ȫ�ֱ���
	public EmpModel()
	{
		column=new Vector<String>();
		row=new Vector<Vector>();
		sh=new SQLHelper();
	}
	//���ܣ�ˢ�°�ť
	public void shuaxin(String name)
	{
		//���Ϊ�գ����ѯ������
		if((name).equals(""))
		{
			//�༭sql���
			String sql="select empid ����,empname ����,sex �Ա�,xl ѧ��,zhiwei ְλ from rszl where 1=?";
			String[] paramaters={"1"};
			rs=sh.Select(sql, paramaters);
		}
		//����Ϊ�գ����ѯ�ṩ������
		else 
		{
			String sql="select empid ����,empname ����,sex �Ա�,xl ѧ��,zhiwei ְλ from rszl where empname=?";
			String[] paramaters={name};
			rs=sh.Select(sql, paramaters);
		}
		//�Խ����������
		try {
			//ȡ��rs��ResultSetMetaData���ɻ�ȡÿһ��
			ResultSetMetaData dm=rs.getMetaData();
			//ȡ��������
			for(int i=0;i<dm.getColumnCount();i++)
			{
				//��������ӵ�����
				column.add(dm.getColumnName(i+1));
				//ȡ�����е�����
				System.out.println("��ȡ������");
			}
			//��˳��ȡ��Ԫ��
			while(rs.next())
			{
				Vector<String> temp=new Vector<String>();
				//��Ԫ���м������ԣ�����Ӷ��ٴ�
				for(int i=0;i<dm.getColumnCount();i++)
				{
					temp.add(rs.getString(i+1));
				}
				//�˳�һ��forѭ�� ȡ��һ��Ԫ��
				row.add(temp);
			}
			//�˳�whileѭ��ʱ��ȡ��ȫ��Ԫ��
		} catch (SQLException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		}finally{
			sh.close();
		}

	}

	//���ܣ�����idɾ���û�
	public boolean delete(String empid)
	{
		//�༭sql���
		String sql="delete from rszl where empid=?";
		String []parameter={empid};
		//�����ݿ����ֽ���
		sh=new SQLHelper();
		boolean b=sh.Update(sql, parameter);
		return b;
	}
	//���ܣ���Ӱ�ť,��Ҫ����14��String����
	public boolean insert(String a,String b,String c,String d,String e,String f,String g,String h,String i,String j,String k,String l,String m,String n)
	{
		boolean boo=true;
		//�༭sql���
		String sql="insert into rszl values(?,?,?,?,?,?,?,?,?,?,?,?,?,?)";
		//�༭������
		String []parameter={a,b,c,d,e,f,g,h,i,j,k,l,m,n};
		//ִ��Helper�Ĳ�ѯ����
		sh=new SQLHelper();
		boo=sh.Update(sql, parameter);
		return boo;
	}
	//���ܣ��޸İ�ť����Ҫ����14��String����
	public boolean update(String a,String b,String c,String d,String e,String f,String g,String h,String i,String j,String k,String l,String m,String n)
	{
		boolean boo=true;
		//�༭sql���
		String sql="update rszl set ";
		//�༭������
		String []parameter={a,b,c,d,e,f,g,h,i,j,k,l,m,n};
		//ִ��Helper�Ĳ�ѯ����
		sh=new SQLHelper();
		boo=sh.Update(sql, parameter);
		return boo;
	}

	@Override
	public int getRowCount() {
		// TODO Auto-generated method stub
		return row.size();
	}

	@Override
	public int getColumnCount() {
		// TODO Auto-generated method stub
		return column.size();
	}

	@Override
	public Object getValueAt(int rowIndex, int columnIndex) {
		// TODO Auto-generated method stub
		return row.get(rowIndex).get(columnIndex);
	}


	@Override
	public String getColumnName(int i) {
		// TODO Auto-generated method stub
		return this.column.get(i);
	}

}
