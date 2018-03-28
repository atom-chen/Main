package com.model;

import java.util.Vector;

import javax.swing.table.AbstractTableModel;

import com.databasehelper.SQLHelper;
import java.sql.*;

public class ManaLoginModel extends AbstractTableModel{
	Vector<String> column=null;//属性
	Vector<Vector> row=null;//元组
	SQLHelper sqlhelper=null;
	
	//构造方法
	public ManaLoginModel()
	{
		column=new Vector<String>();
		row=new Vector<Vector>();
		sqlhelper=new SQLHelper();
	}
	
	//功能；刷新按钮 输入：所要查询的管理员姓名  返回方式：直接操作TableModel
	public void select(String name)
	{
		try {
			String sql="select r.empid,empname,zhiwei,passwd from rszl r,login l where r.empid=l.empid and r.empname="+name;
			//执行SQL语句
			String parameter[]={};
			ResultSet resultset=sqlhelper.Select(sql, parameter);
			//将查询到的结果添加到vector
			while(resultset.next())
			{
				
			}
		} catch (Exception e) {
			e.printStackTrace();
		}
		finally{
			sqlhelper.close();
		}
		
		
	}

	@Override
	//有几个元组
	public int getRowCount() {
		// TODO Auto-generated method stub
		return row.size();
	}

	@Override
	//有几个属性
	public int getColumnCount() {
		// TODO Auto-generated method stub
		return column.size();
	}

	@Override
	public Object getValueAt(int rowIndex, int columnIndex) {
		// TODO Auto-generated method stub
		//拿到某元组的某值
		return row.get(rowIndex).get(columnIndex);
	}

	@Override
	//拿到属性名
	public String getColumnName(int column) {
		// TODO Auto-generated method stub
		return this.column.get(column);
	}
	
	
}
