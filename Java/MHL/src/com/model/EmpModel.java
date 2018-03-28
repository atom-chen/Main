package com.model;
//功能：人事登记表模型
import javax.swing.table.AbstractTableModel;

import com.databasehelper.SQLHelper;

import java.sql.*;
import java.util.*;
public class EmpModel extends AbstractTableModel{
	SQLHelper sh=null;
	ResultSet rs=null;
	//模型表的属性
	Vector<String> column=null;
	//模型表的元组
	Vector<Vector> row=null;
	//构造方法：初始化全局变量
	public EmpModel()
	{
		column=new Vector<String>();
		row=new Vector<Vector>();
		sh=new SQLHelper();
	}
	//功能：刷新按钮
	public void shuaxin(String name)
	{
		//如果为空，则查询所有人
		if((name).equals(""))
		{
			//编辑sql语句
			String sql="select empid 工号,empname 姓名,sex 性别,xl 学历,zhiwei 职位 from rszl where 1=?";
			String[] paramaters={"1"};
			rs=sh.Select(sql, paramaters);
		}
		//若不为空，则查询提供的名字
		else 
		{
			String sql="select empid 工号,empname 姓名,sex 性别,xl 学历,zhiwei 职位 from rszl where empname=?";
			String[] paramaters={name};
			rs=sh.Select(sql, paramaters);
		}
		//对结果集做处理
		try {
			//取出rs的ResultSetMetaData：可获取每一列
			ResultSetMetaData dm=rs.getMetaData();
			//取出属性名
			for(int i=0;i<dm.getColumnCount();i++)
			{
				//把列名添加到集合
				column.add(dm.getColumnName(i+1));
				//取出该列的属性
				System.out.println("读取属性列");
			}
			//按顺序取出元组
			while(rs.next())
			{
				Vector<String> temp=new Vector<String>();
				//该元组有几个属性，就添加多少次
				for(int i=0;i<dm.getColumnCount();i++)
				{
					temp.add(rs.getString(i+1));
				}
				//退出一次for循环 取得一个元组
				row.add(temp);
			}
			//退出while循环时，取出全部元组
		} catch (SQLException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		}finally{
			sh.close();
		}

	}

	//功能：根据id删除用户
	public boolean delete(String empid)
	{
		//编辑sql语句
		String sql="delete from rszl where empid=?";
		String []parameter={empid};
		//与数据库助手交互
		sh=new SQLHelper();
		boolean b=sh.Update(sql, parameter);
		return b;
	}
	//功能：添加按钮,需要输入14个String内容
	public boolean insert(String a,String b,String c,String d,String e,String f,String g,String h,String i,String j,String k,String l,String m,String n)
	{
		boolean boo=true;
		//编辑sql语句
		String sql="insert into rszl values(?,?,?,?,?,?,?,?,?,?,?,?,?,?)";
		//编辑参数组
		String []parameter={a,b,c,d,e,f,g,h,i,j,k,l,m,n};
		//执行Helper的查询方法
		sh=new SQLHelper();
		boo=sh.Update(sql, parameter);
		return boo;
	}
	//功能：修改按钮，需要输入14个String内容
	public boolean update(String a,String b,String c,String d,String e,String f,String g,String h,String i,String j,String k,String l,String m,String n)
	{
		boolean boo=true;
		//编辑sql语句
		String sql="update rszl set ";
		//编辑参数组
		String []parameter={a,b,c,d,e,f,g,h,i,j,k,l,m,n};
		//执行Helper的查询方法
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
