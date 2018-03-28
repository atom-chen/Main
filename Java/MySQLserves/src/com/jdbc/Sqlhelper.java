package com.jdbc;
import java.io.FileInputStream;
import java.io.IOException;
import java.sql.*;
import java.util.Properties;

import javax.management.RuntimeErrorException;
public class Sqlhelper {
	//������Ҫ�ı���
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

	//����������һ��
	static
	{
		try {
			//��dbinfo.propertis�ļ��ж�ȡoracle������Ϣ
			pp=new Properties();
			fis=new FileInputStream("dbinfo.properties");
			pp.load(fis);
			driver=pp.getProperty("driver");
			url=pp.getProperty("url");
			username=pp.getProperty("username");
			password=pp.getProperty("password");
	
			//��������
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
	//�õ�����
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
	//���ô洢����(�޷���ֵ)
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
	//���ô洢���̣����ؽ������
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
	//�洢���̣����÷�ҳ
//	public CallableStatement fenye(String TableName,String PageSize,String PageNow)
//	{
//		ct=getConnection();
//		try {
//			cs=ct.prepareCall("{call fenye(?,?,?,?,?,?)}");
//			cs.setString(1, TableName);
//			cs.setString(2, PageSize);
//			cs.setString(3, PageNow);
//			cs.registerOutParameter(4, oracle.jdbc.OracleTypes.NUMBER);
//			cs.registerOutParameter(5, oracle.jdbc.OracleTypes.NUMBER);
//			cs.registerOutParameter(6, oracle.jdbc.OracleTypes.CURSOR);
//			cs.execute();
//		} catch (SQLException e) {
//			// TODO Auto-generated catch block
//			e.printStackTrace();
//			throw new RuntimeException(e.getMessage());
//		}
//		return cs;
//
//	}
	
	//��дһ��update/delete/insert(����������)
	public void executeUpdate(String sql,String []parameters)
	{
		ct=getConnection();
		try {
			ps=ct.prepareStatement(sql);
			//������ֵ��������Ҳ֧�ֲ����Ѿ�д�õ�sql���
			if(parameters!=null)
			{
				for(int i=0;i<parameters.length;i++)
				{
					ps.setString(i+1, parameters[i]);
				}
			}
			//ִ��
			ps.executeUpdate();//������Ӧ���Է���ֵ�жϣ���Ϊ���ǲ�֪�����ɹ��˼��� ʧ���˼���
			//Ӧ����catch����д���
				
		} catch (SQLException e) {
			e.printStackTrace();//�����׶�
			//�׳��쳣�����Ը������ߵ��øú����ĺ�����һ��ѡ�񣺴�����쳣���߷���������쳣
			//����Ϊ���˾ʹ��ˣ�����ν�����������
			//��ϣ�����û�����һ����ʾ���ڣ�������쳣
			throw new RuntimeException(e.getMessage());
		}finally{
			this.close(rs,ps,ct);
		}
		
	}
	
	//����ж��update/delete/insert����Ҫ��������
	public void executeUpdate(String sql[],String[][] parameters)
	{
		try {
			//�������
			ct=getConnection();
			//��Ϊ��ʱ�û�����Ŀ����Ƕ��sql���
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
	//ͳһ��select
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


	//�ر���Դ�ĺ���
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







