package com.myMsServer;
import java.sql.*;

public class Backups {

	public static void main(String[] args) {
		//定义对象
		Connection ct=null;
		PreparedStatement ps=null;
		
		
		try {
			//1、加载驱动
			Class.forName("com.microsoft.sqlserver.jdbc.SQLServerDriver");
			//2、得到连接
			ct=DriverManager.getConnection("jdbc:sqlserver://127.0.0.1:1433;databaseName=SHUNPING","sa","");
			//3、创建火箭车
			ps=ct.prepareStatement("create table aaa(num int primary key,name nvarchar(50))");
			//4、执行语句(ddl语句：execute)
			boolean b=ps.execute();
			if(!b)
			{
				System.out.println("备份成功！");
			}else 
			{
				System.out.println("备份失败");
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
