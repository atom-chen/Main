package com.Student.Model2;

import javax.swing.table.AbstractTableModel;
import java.sql.*;
import java.util.Vector;
//在这里执行对数据库的所有操作
public class DatabaseModel extends AbstractTableModel {
	    //rowData储存属性名
		//columnNames储存元组
		Vector<Vector> rowData=null;
		Vector<String> columnNames=null;
		
		Connection ct=null;
		PreparedStatement ps=null;
		ResultSet rs=null;
		
		//增删改方法，与外界的接口为sql语句和一个字符串数组（代表？）
		public boolean addToData(String sql,String []data)
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
				try {
					ps.close();
				} catch (SQLException e1) {
					// TODO Auto-generated catch block
					e1.printStackTrace();
				}
				try {
					ct.close();
				} catch (SQLException e1) {
					// TODO Auto-generated catch block
					e1.printStackTrace();
				}
			}
			return b;
		}
		//查询方法
		public void init(String sql)
		{
			//如果为空，则默认为显示所有
			if(sql.equals(""))
			{
				sql="select * from student";
			}
			//如果不为空 说明要执行查询
	    	columnNames=new Vector<String>();
			//添加列名
			columnNames.add("学号");
			columnNames.add("姓名");
			columnNames.add("性别");
			columnNames.add("年龄");
			columnNames.add("籍贯");
			columnNames.add("系名");
			//添加属性到集合
			rowData=new Vector<Vector>();
			//添加元组
			try {
				Class.forName("com.microsoft.sqlserver.jdbc.SQLServerDriver");
				ct=DriverManager.getConnection("jdbc:sqlserver://127.0.0.1:1433;database=StudentManagement","sa","zhanmengao");
				
				ps=ct.prepareStatement(sql);
				
				rs=ps.executeQuery();
				while(rs.next())
				{
					Vector<Comparable> person=new Vector<Comparable>();
					person.add(rs.getString(1));
					person.add(rs.getString(2));
					person.add(rs.getString(3));
					person.add(rs.getInt(4));
					person.add(rs.getString(5));
					person.add(rs.getString(6));
					//把一个人的信息添加到元组
					rowData.add(person);
				}				
			} catch (Exception e) {
				e.printStackTrace();
			}finally{
				try {
					rs.close();
				} catch (SQLException e) {
					e.printStackTrace();
				}
				try {
					ps.close();
				} catch (SQLException e) {
					e.printStackTrace();
				}
				try {
					ct.close();
				} catch (SQLException e) {
					e.printStackTrace();
				}
			}
		}
	    DatabaseModel(String sql)
		{
	    	this.init(sql);
		}
	    DatabaseModel()
		{
	    	this.init("");
		}

	//获取几行
	public int getRowCount() {
		// TODO Auto-generated method stub
		return this.rowData.size();
	}

	//获取几列
	public int getColumnCount() {
		// TODO Auto-generated method stub
		return this.columnNames.size();
	}

	//获取坐标（行，列）对应的数值）
	public Object getValueAt(int rowIndex, int columnIndex) {
		// TODO Auto-generated method stub
		return this.rowData.get(rowIndex).get(columnIndex);
	}

	@Override
	//返回属性名
	public String getColumnName(int column) {
		return this.columnNames.get(column);
	}
	
	

}
