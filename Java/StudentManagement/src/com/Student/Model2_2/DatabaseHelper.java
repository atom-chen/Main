package com.Student.Model2_2;

import java.sql.*;
import java.util.Vector;


//���ܣ��������ݿ� ִ��SQL���
public class DatabaseHelper {
	Connection ct=null;
	PreparedStatement ps=null;
	ResultSet rs=null;
	//��ɾ�Ĳ���  ���룺ִ�и��µ�SQL��䡢��������  ���أ�����ֵ���ɹ�����
	public boolean update(String sql,String data[])
	{
		boolean b=true;
		//�������ݿ�
		try {
			Class.forName("com.microsoft.sqlserver.jdbc.SQLServerDriver");
			ct=DriverManager.getConnection("jdbc:sqlserver://127.0.0.1:1433;database=StudentManagement","sa","");	
			ps=ct.prepareStatement(sql);
			//������ֵ
			for(int i=0;i<data.length;i++)
			{
				//��ֵ
				ps.setString(i+1, data[i]);
			}		
			//ִ�л����
			int i=ps.executeUpdate();
			if(i==1)
			{
				System.out.println("�ɹ�");
			}else if(i==0)
			{
				b=false;
				System.out.println("ʧ��");
			}
		} catch (Exception e1) {
			// TODO Auto-generated catch block
			b=false;
			e1.printStackTrace();
		}finally{
			this.close();
		}
		return b;
	}
	//��ѯ����  ���룺ִ�в�ѯ��SQL��䣬��ѯ������   �������ѯ���rs
	public ResultSet select(String sql,String[] data)
	{
		try {
			Class.forName("com.microsoft.sqlserver.jdbc.SQLServerDriver");
			ct=DriverManager.getConnection("jdbc:sqlserver://127.0.0.1:1433;database=StudentManagement","sa","");
			
			ps=ct.prepareStatement(sql);
			//������ֵ
			for(int i=0;i<data.length;i++)
			{
				ps.setString(i+1, data[i]);
			}
			rs=ps.executeQuery();
		
		} catch (Exception e) {
			e.printStackTrace();
		}finally{
			//���ﻹ���ܹر��������ӣ���Ϊ��Ҫ�����ݴ���ȥ
			return rs;
		}
	}
	
	//���ܣ��ر����ݿ� ���������
	public void close()
	{
		try {
			if(rs!=null)
				rs.close();
		} catch (SQLException e) {
			e.printStackTrace();
		}
		try {
			if(ps!=null)
				ps.close();
		} catch (SQLException e) {
			e.printStackTrace();
		}
		try {
			if(ct!=null)
				ct.close();
		} catch (SQLException e) {
			e.printStackTrace();
		}

	}
}

