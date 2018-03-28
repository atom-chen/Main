package com.databasehelper;
//�ڲ������������ݿ�Ľ�������
import java.sql.*;
public class SQLHelper {
	//�����Ա����
	Connection ct=null;
	PreparedStatement ps=null;
	ResultSet rs=null;
	String driver="com.microsoft.sqlserver.jdbc.SQLServerDriver";
	String url="jdbc:sqlserver://127.0.0.1:1433;database=restaurant";
	String id="sa";
	String password="";
	//���췽������������
	public SQLHelper()
	{
		try {
			Class.forName(driver);
			ct=DriverManager.getConnection(url,id,password);
		} catch (Exception e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		}
		
	}
	//��ѯ����
	//���룺SQL����Լ�"��"�Ĳ�����
	//����������ResultSet
	public ResultSet Select(String sql,String[] parameter)
	{
		try {
			ps=ct.prepareStatement(sql);
			for(int i=0;i<parameter.length;i++)
			{
				ps.setString(i+1, parameter[i]);
			}
			rs=ps.executeQuery();
		} catch (SQLException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		}
		return rs;
	}
	//����:�ر�����
	public void close()
	{
		try {
			if(rs!=null) rs.close();
		} catch (SQLException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		}
		if(ps!=null)
			try {
				ps.close();
			} catch (SQLException e1) {
				// TODO Auto-generated catch block
				e1.printStackTrace();
			}
		if(ct!=null)
			try {
				ct.close();
			} catch (SQLException e) {
				// TODO Auto-generated catch block
				e.printStackTrace();
			}
	}
	//���ܣ���ɾ��
	public boolean Update(String sql,String[] parameter)
	{
		boolean b=true;
	
		try {
			//���������
			ps=ct.prepareStatement(sql);
			//������ֵ
			for(int i=0;i<parameter.length;i++)
			{
				ps.setString(i+1, parameter[i]);
			}
			//ִ�л����
			if(ps.executeUpdate()==0)
			{
				b=false;
				System.out.println("��������Ϊ0");
			}
		} catch (SQLException e) {
			// TODO Auto-generated catch block
			b=false;
			e.printStackTrace();
		}finally{
			this.close();
		}
		return b;
	}
	
	
	
	
	
	
	
	
	
	
	
}
