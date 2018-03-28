package com.myMsServer;
import java.sql.*;

public class Select {

	public static void main(String[] args) {
		//定义对象
		Connection ct=null;
		PreparedStatement ps=null;
		ResultSet rs=null;
		
		
		try {
			//1、加载驱动
			Class.forName("com.microsoft.sqlserver.jdbc.SQLServerDriver");
			//2、得到连接
			ct=DriverManager.getConnection("jdbc:sqlserver://127.0.0.1:1433;databaseName=SHUNPING","sa","");
			//3、创建火箭车
			ps=ct.prepareStatement("select empno,ename,sal from emp where deptno='10'");
			//4、接收查询结果，一行一行输出（查询用executeQuery,增删改用executeUpdate）
			rs=ps.executeQuery();
			//循环取出查询内容
			while(rs.next())
			{
				int num=rs.getInt("empno");
				String name=rs.getString(2);
				float sal=rs.getFloat(3);
				
				System.out.println("工号"+num+" 名字"+name+" 工资"+sal);
				
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
