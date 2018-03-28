package com.myMsServer;
import java.sql.*;

public class Backups {

	public static void main(String[] args) {
		//�������
		Connection ct=null;
		PreparedStatement ps=null;
		
		
		try {
			//1����������
			Class.forName("com.microsoft.sqlserver.jdbc.SQLServerDriver");
			//2���õ�����
			ct=DriverManager.getConnection("jdbc:sqlserver://127.0.0.1:1433;databaseName=SHUNPING","sa","");
			//3�����������
			ps=ct.prepareStatement("create table aaa(num int primary key,name nvarchar(50))");
			//4��ִ�����(ddl��䣺execute)
			boolean b=ps.execute();
			if(!b)
			{
				System.out.println("���ݳɹ���");
			}else 
			{
				System.out.println("����ʧ��");
			}
			
			
		} catch (Exception e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		}finally{
			try {
				ps.close();
			} catch (SQLException e) {
				// TODO Auto-generated catch block
				e.printStackTrace();
			}
			try {
				ct.close();
			} catch (SQLException e) {
				// TODO Auto-generated catch block
				e.printStackTrace();
			}
		}
		
	}

}
