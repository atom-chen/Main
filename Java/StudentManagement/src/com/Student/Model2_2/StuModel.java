package com.Student.Model2_2;

import javax.swing.table.AbstractTableModel;
import java.sql.*;
import java.util.Vector;
//功能：展现学生表
public class StuModel extends AbstractTableModel {
	    DatabaseHelper helper=null;
	    ResultSet rs=null;
	    //rowData储存属性名
		//columnNames储存元组
		Vector<Vector> rowData=null;
		Vector<String> columnNames=null;
		
		//功能：增删改Stu数据库（逻辑层）
		//输入： StuManager(sql语句和数据)→该函数(sql语句和数据)→DatabaseHelper
		//输出：接收DatabaseHelper返回的插入成功与否，返回到StuManager
		public boolean addToData(String sql,String []data)
		{
			helper=new DatabaseHelper();
			//把从DatabaseInsertPane和DaatabaseInsertPane收到的数据传入DatabaseHelper
			boolean b=true;
			//执行操作，取得结果
			b=helper.update(sql, data);
			return b;
		}
		//查询数据库
		public void select(String sql,String data[])
		{
			
			helper=new DatabaseHelper();
			//把SQL语句传给底层，并接受查询结果RS
			rs=helper.select(sql, data);

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
			} catch (SQLException e) {
				// TODO Auto-generated catch block
				e.printStackTrace();
			}
			helper.close();
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
		return this.columnNames.get(column).toString();
	}
	
	
		
}

