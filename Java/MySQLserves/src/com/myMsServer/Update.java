package com.myMsServer;
import java.sql.*;


public class Update {

	public static void main(String[] args) {
		//�������ӡ������
		Connection ct=null;
		PreparedStatement ps=null;
			
		try {
			//1����������
			Class.forName("com.microsoft.sqlserver.jdbc.SQLServerDriver");
			//2���õ�����
			ct=DriverManager.getConnection("jdbc:sqlserver://127.0.0.1:1433;databaseName=SHUNPING","sa","");
			//3�����������
			ps=ct.prepareStatement("insert into dept values(?,?,?)");
			//������ֵ
			ps.setInt(1,50);
			ps.setString(2, "����");
			ps.setString(3, "ʯ��ɽ");
			//4��ִ�л����
			int i=ps.executeUpdate();
			if(i==1)
			{
				//����1������ɹ�
				System.out.println("�����ɹ�");
			}else{
				//����0�����ʧ��
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
