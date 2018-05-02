package com.mysql;
import java.sql.*;

public class TestMysql {
	
	public static void main(String args[]){
		Connection ct=null;
		PreparedStatement ps=null;
		ResultSet rs=null;
		try {
			Class.forName("com.mysql.jdbc.Driver");
			ct=DriverManager.getConnection("jdbc:mysql://192.168.2.102:3306/test","root","root");
			System.out.println(ct);
			ps=ct.prepareStatement("select * from test");
			rs=ps.executeQuery();
			while(rs.next())
			{
				System.out.println(rs.getString(2));
			}
		} catch (Exception e) {
			e.printStackTrace();
		}finally{
			try {
				ct.close();
				System.out.println(ct);
			} catch (SQLException e) {
				// TODO Auto-generated catch block
				e.printStackTrace();
			}finally{
				System.out.println(ct);
			}
			try {
				ps.close();
			} catch (SQLException e) {
				// TODO Auto-generated catch block
				e.printStackTrace();
			}
			try {
				rs.close();
			} catch (SQLException e) {
				// TODO Auto-generated catch block
				e.printStackTrace();
			}
		}
		
	}

}
