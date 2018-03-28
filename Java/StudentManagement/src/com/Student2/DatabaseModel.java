package com.Student2;

import javax.swing.table.AbstractTableModel;
import java.sql.*;
import java.util.Vector;

public class DatabaseModel extends AbstractTableModel {
	    //rowData储存属性名
		//columnNames储存元组
		Vector<Vector> rowData=null;
		Vector<String> columnNames=null;
		
		Connection ct=null;
		PreparedStatement ps=null;
		ResultSet rs=null;
		
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
				ct=DriverManager.getConnection("jdbc:sqlserver://127.0.0.1:1433;database=StudentManagement","sa","");
				
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
