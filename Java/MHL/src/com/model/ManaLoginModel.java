package com.model;

import java.util.Vector;

import javax.swing.table.AbstractTableModel;

import com.databasehelper.SQLHelper;
import java.sql.*;

public class ManaLoginModel extends AbstractTableModel{
	Vector<String> column=null;//����
	Vector<Vector> row=null;//Ԫ��
	SQLHelper sqlhelper=null;
	
	//���췽��
	public ManaLoginModel()
	{
		column=new Vector<String>();
		row=new Vector<Vector>();
		sqlhelper=new SQLHelper();
	}
	
	//���ܣ�ˢ�°�ť ���룺��Ҫ��ѯ�Ĺ���Ա����  ���ط�ʽ��ֱ�Ӳ���TableModel
	public void select(String name)
	{
		try {
			String sql="select r.empid,empname,zhiwei,passwd from rszl r,login l where r.empid=l.empid and r.empname="+name;
			//ִ��SQL���
			String parameter[]={};
			ResultSet resultset=sqlhelper.Select(sql, parameter);
			//����ѯ���Ľ����ӵ�vector
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
	//�м���Ԫ��
	public int getRowCount() {
		// TODO Auto-generated method stub
		return row.size();
	}

	@Override
	//�м�������
	public int getColumnCount() {
		// TODO Auto-generated method stub
		return column.size();
	}

	@Override
	public Object getValueAt(int rowIndex, int columnIndex) {
		// TODO Auto-generated method stub
		//�õ�ĳԪ���ĳֵ
		return row.get(rowIndex).get(columnIndex);
	}

	@Override
	//�õ�������
	public String getColumnName(int column) {
		// TODO Auto-generated method stub
		return this.column.get(column);
	}
	
	
}
