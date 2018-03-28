package com.myMsServer;
import java.sql.*;

public class Select {

	public static void main(String[] args) {
		//�������
		Connection ct=null;
		PreparedStatement ps=null;
		ResultSet rs=null;
		
		
		try {
			//1����������
			Class.forName("com.microsoft.sqlserver.jdbc.SQLServerDriver");
			//2���õ�����
			ct=DriverManager.getConnection("jdbc:sqlserver://127.0.0.1:1433;databaseName=SHUNPING","sa","");
			//3�����������
			ps=ct.prepareStatement("select empno,ename,sal from emp where deptno='10'");
			//4�����ղ�ѯ�����һ��һ���������ѯ��executeQuery,��ɾ����executeUpdate��
			rs=ps.executeQuery();
			//ѭ��ȡ����ѯ����
			while(rs.next())
			{
				int num=rs.getInt("empno");
				String name=rs.getString(2);
				float sal=rs.getFloat(3);
				
				System.out.println("����"+num+" ����"+name+" ����"+sal);
				
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
