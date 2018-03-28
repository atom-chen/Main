package com.databasehelper;
//内部：处理与数据库的交互部分
import java.sql.*;
public class SQLHelper {
	//定义成员变量
	Connection ct=null;
	PreparedStatement ps=null;
	ResultSet rs=null;
	String driver="com.microsoft.sqlserver.jdbc.SQLServerDriver";
	String url="jdbc:sqlserver://127.0.0.1:1433;database=restaurant";
	String id="sa";
	String password="";
	//构造方法：建立连接
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
	//查询方法
	//输入：SQL语句以及"？"的参数集
	//输出：结果集ResultSet
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
	//功能:关闭连接
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
	//功能：增删改
	public boolean Update(String sql,String[] parameter)
	{
		boolean b=true;
	
		try {
			//创建火箭车
			ps=ct.prepareStatement(sql);
			//给？赋值
			for(int i=0;i<parameter.length;i++)
			{
				ps.setString(i+1, parameter[i]);
			}
			//执行火箭车
			if(ps.executeUpdate()==0)
			{
				b=false;
				System.out.println("火箭车结果为0");
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
