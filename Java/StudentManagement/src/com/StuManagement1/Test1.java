package com.StuManagement1;
import java.sql.*;
import java.awt.*;
import java.util.*;
import javax.swing.*;
import java.awt.event.*;

public class Test1 extends JFrame {

	public static void main(String[] args) {
		Test1 t1=new Test1();

	}
	JTable jt=null;
	JScrollPane jsp=null;
	//rowData����������
	//columnNames����Ԫ��
	Vector<Vector> rowData=null;
	Vector<String> columnNames=null;
	
	Connection ct=null;
	PreparedStatement ps=null;
	ResultSet rs=null;
	public Test1()
	{
		columnNames=new Vector<String>();
		//�������
		columnNames.add("ѧ��");
		columnNames.add("����");
		columnNames.add("�Ա�");
		columnNames.add("����");
		columnNames.add("����");
		columnNames.add("ϵ��");
		
		rowData=new Vector<Vector>();
		//���Ԫ��
		try {
			Class.forName("com.microsoft.sqlserver.jdbc.SQLServerDriver");
			ct=DriverManager.getConnection("jdbc:sqlserver://127.0.0.1:1433;database=StudentManagement","sa","");
			ps=ct.prepareStatement("select * from student");
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
		jt=new JTable(rowData,columnNames);
		jsp=new JScrollPane(jt);
		
		this.add(jsp);
		
		this.setSize(500,500);
		
		this.setFocusable(false);
		this.setVisible(true);
		this.setDefaultCloseOperation(JFrame.EXIT_ON_CLOSE);
	
	}
	
}


