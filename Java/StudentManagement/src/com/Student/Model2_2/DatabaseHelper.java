package com.Student.Model2_2;

import java.sql.*;
import java.util.Vector;


//功能：连接数据库 执行SQL语句
public class DatabaseHelper {
	Connection ct=null;
	PreparedStatement ps=null;
	ResultSet rs=null;
	//增删改操作  输入：执行更新的SQL语句、数据数组  返回：布尔值（成功？）
	public boolean update(String sql,String data[])
	{
		boolean b=true;
		//连接数据库
		try {
			Class.forName("com.microsoft.sqlserver.jdbc.SQLServerDriver");
			ct=DriverManager.getConnection("jdbc:sqlserver://127.0.0.1:1433;database=StudentManagement","sa","");	
			ps=ct.prepareStatement(sql);
			//给？赋值
			for(int i=0;i<data.length;i++)
			{
				//赋值
				ps.setString(i+1, data[i]);
			}		
			//执行火箭车
			int i=ps.executeUpdate();
			if(i==1)
			{
				System.out.println("成功");
			}else if(i==0)
			{
				b=false;
				System.out.println("失败");
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
	//查询操作  输入：执行查询的SQL语句，查询的条件   输出：查询结果rs
	public ResultSet select(String sql,String[] data)
	{
		try {
			Class.forName("com.microsoft.sqlserver.jdbc.SQLServerDriver");
			ct=DriverManager.getConnection("jdbc:sqlserver://127.0.0.1:1433;database=StudentManagement","sa","");
			
			ps=ct.prepareStatement(sql);
			//给？赋值
			for(int i=0;i<data.length;i++)
			{
				ps.setString(i+1, data[i]);
			}
			rs=ps.executeQuery();
		
		} catch (Exception e) {
			e.printStackTrace();
		}finally{
			//这里还不能关闭数据连接，因为还要把数据传出去
			return rs;
		}
	}
	
	//功能：关闭数据库 无输入输出
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

