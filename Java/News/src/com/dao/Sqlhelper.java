package com.dao;
import java.io.FileInputStream;
import java.io.IOException;
import java.sql.*;
import java.util.Properties;
import javax.management.RuntimeErrorException;
public class Sqlhelper {
	//定义需要的变量
	private Connection ct=null;
	private PreparedStatement ps=null;
	private CallableStatement cs=null;
	private ResultSet rs=null;
	
	private static String driver="";
	private static String url="";
	private static String username="";
	private static String password="";
	
	private static Properties pp=null;
	private static FileInputStream fis=null;

	//加载驱动：一次
	static
	{
		try {
			//从dbinfo.propertis文件中读取oracle配置信息
			pp=new Properties();
			fis=new FileInputStream("dbinfo.properties");
			pp.load(fis);
			driver=pp.getProperty("driver");
			url=pp.getProperty("url");
			username=pp.getProperty("username");
			password=pp.getProperty("password");
	
			//加载驱动
			Class.forName(driver);
		} catch (Exception e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
			System.exit(0);
		}
		finally
		{
			try {
				fis.close();
			} catch (IOException e) {
				// TODO Auto-generated catch block
				e.printStackTrace();
			}
			fis=null;
		}
	}
	//得到连接
	public Connection getConnection()
	{
		try {
			ct=DriverManager.getConnection(url,username,password);
		} catch (SQLException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		}
		return ct;
	}
	//调用存储过程(无返回值)
	public void execpro_nores(String sql,String []parameter)
	{
		ct=getConnection();
		CallableStatement cs=null;
		try {
			cs=ct.prepareCall(sql);
			if(parameter!=null)
			{
				for(int i=0;i<parameter.length;i++)
				{
					cs.setObject(i+1,parameter[i]);
				}
			}
			cs.execute();
		} catch (SQLException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
			throw new RuntimeException(e.getMessage());
		}finally{
			this.close(rs, cs, ct);
		}	
	}
	//调用存储过程（返回结果集）
	public CallableStatement execpro_res(String sql,String[] inparameter,Integer[] outparameter)
	{
		ct=getConnection();
		try {
			cs=ct.prepareCall(sql);
			if(inparameter!=null)
			{
				for(int i=0;i<inparameter.length;i++)
				{
					cs.setObject(i+1, inparameter[i]);
				}
			}
			if(outparameter!=null)
			{
				for(int j=0;j<outparameter.length;j++)
				{
					System.out.println("j="+j);
					System.out.println(outparameter[j]+"  "+(inparameter.length+j+1));
					cs.registerOutParameter((inparameter.length+j+1), outparameter[j]);
				}
			}
			cs.execute();
		} catch (SQLException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
			throw new RuntimeException(e.getMessage());
		}
		
		return cs;
	}
	//存储过程：调用分页
	public CallableStatement fenye(String TableName,String PageSize,String PageNow)
	{
		ct=getConnection();
		try {
			cs=ct.prepareCall("{call fenye(?,?,?,?,?,?)}");
			cs.setString(1, TableName);
			cs.setString(2, PageSize);
			cs.setString(3, PageNow);
			cs.registerOutParameter(4, oracle.jdbc.OracleTypes.NUMBER);
			cs.registerOutParameter(5, oracle.jdbc.OracleTypes.NUMBER);
			cs.registerOutParameter(6, oracle.jdbc.OracleTypes.CURSOR);
			cs.execute();
		} catch (SQLException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
			throw new RuntimeException(e.getMessage());
		}
		return cs;

	}
	
	//先写一个update/delete/insert(不考虑事务)
	public void executeUpdate(String sql,String []parameters)
	{
		ct=getConnection();
		try {
			ps=ct.prepareStatement(sql);
			//给？赋值——但是也支持参数已经写好的sql语句
			if(parameters!=null)
			{
				for(int i=0;i<parameters.length;i++)
				{
					ps.setString(i+1, parameters[i]);
				}
			}
			//执行
			ps.executeUpdate();//——不应该以返回值判断，因为我们不知道它成功了几行 失败了几行
			//应该在catch语句中处理
				
		} catch (SQLException e) {
			e.printStackTrace();//开发阶段
			//抛出异常，可以给调用者调用该函数的函数，一个选择：处理该异常或者放弃处理该异常
			//当认为错了就错了，无所谓，则放弃处理
			//当希望给用户弹出一个提示窗口，则处理该异常
			throw new RuntimeException(e.getMessage());
		}finally{
			this.close(rs,ps,ct);
		}
		
	}
	
	//如果有多个update/delete/insert（需要考虑事务）
	public void executeUpdate(String sql[],String[][] parameters)
	{
		try {
			//获得连接
			ct=getConnection();
			//因为此时用户传入的可能是多个sql语句
			ct.setAutoCommit(false);
			for(int i=0;i<sql.length;i++)
			{
				if(parameters[i]!=null)
				{
					ps=ct.prepareStatement(sql[i]);
					for(int j=0;j<parameters[i].length;j++)
					{
						ps.setString(j+1, parameters[i][j]);
					}
				}
				ps.executeUpdate();
				ct.commit();
			}
		} catch (Exception e) {
			try {
				ct.rollback();
			} catch (SQLException e1) {
				// TODO Auto-generated catch block
				e1.printStackTrace();
			}
			e.printStackTrace();
			throw new RuntimeException(e.getMessage());
			
		}finally{
			this.close(rs, ps, ct);
		}
	}
	//统一的select
	public ResultSet executeQuery(String sql,String[] parameter)
	{
		try {
			 ct=getConnection();
			 ps=ct.prepareStatement(sql);
			if(parameter!=null && !("").equals(parameter))
			{
				for(int i=0;i<parameter.length;i++)
				{
					ps.setString(i+1, parameter[i]);
				}
			}
			rs=ps.executeQuery();
		} catch (Exception e) {
			e.printStackTrace();
			throw new RuntimeException(e.getMessage());
		}
		finally{
			
		}
		return rs;
	}
	public Connection getCt() {
		return ct;
	}


	public PreparedStatement getPs() {
		return ps;
	}



	public ResultSet getRs() {
		return rs;
	}


	//关闭资源的函数
	public void close(ResultSet rs,Statement ps,Connection ct)
	{
		System.out.println(rs);
		System.out.println(ps);
		System.out.println(ct);
		if(rs!=null)
			try {
				rs.close();
			} catch (SQLException e) {
				// TODO Auto-generated catch block
				e.printStackTrace();
			}
		rs=null;
		System.out.println(rs);
		if(ps!=null)
		{
			try {
				ps.close();
			} catch (SQLException e) {
				// TODO Auto-generated catch block
				e.printStackTrace();
			}
		}
		ps=null;
		
		System.out.println(ps);
		if(ct!=null)
		{
			try {
				ct.close();
			} catch (SQLException e) {
				// TODO Auto-generated catch block
				e.printStackTrace();
			}
		}
		ct=null;
		System.out.println(ct);
	}
}







