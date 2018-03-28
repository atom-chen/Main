package com.myMsServer;
import java.sql.*;


public class Update {

	public static void main(String[] args) {
		//定义连接、火箭车
		Connection ct=null;
		PreparedStatement ps=null;
			
		try {
			//1、加载驱动
			Class.forName("com.microsoft.sqlserver.jdbc.SQLServerDriver");
			//2、得到连接
			ct=DriverManager.getConnection("jdbc:sqlserver://127.0.0.1:1433;databaseName=SHUNPING","sa","");
			//3、创建火箭车
			ps=ct.prepareStatement("insert into dept values(?,?,?)");
			//给？赋值
			ps.setInt(1,50);
			ps.setString(2, "北京");
			ps.setString(3, "石景山");
			//4、执行火箭车
			int i=ps.executeUpdate();
			if(i==1)
			{
				//返回1则操作成功
				System.out.println("操作成功");
			}else{
				//返回0则操作失败
				System.out.println("操作失败");
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
