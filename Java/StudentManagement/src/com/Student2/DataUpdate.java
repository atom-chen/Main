package com.Student2;

import java.awt.*;
import javax.swing.*;
import java.awt.event.*;
import java.sql.*;
import java.util.Vector;
//功能：实现更新数据
public class DataUpdate extends javax.swing.JDialog implements ActionListener{
	JLabel jla[]=new JLabel[6];
	JTextField jtf[]=new JTextField[6];
	JButton jb[]=new JButton[2];
	
    //rowData储存属性名
	//columnNames储存元组
	Vector<Vector> rowData=null;
	Vector<String> columnNames=null;
	
	Connection ct=null;
	PreparedStatement ps=null;
	ResultSet rs=null;
	//查询语句，将表的模型传进来，再把所选中的元组传进来
	public DataUpdate(Frame owner, String title, boolean modal,DatabaseModel dm,int RowData)
	{
		
		jla[0]=new JLabel("学号");
		jtf[0]=new JTextField(10);
		//给文本框传值
		jtf[0].setText(dm.getValueAt(RowData, 0).toString());
		//学号不能修改
		jtf[0].setEditable(false);
		
		jla[1]=new JLabel("姓名");
		jtf[1]=new JTextField(10);
		jtf[1].setText(dm.getValueAt(RowData, 1).toString());
		
		jla[2]=new JLabel("性别");
		jtf[2]=new JTextField(10);
		jtf[2].setText(dm.getValueAt(RowData, 2).toString());
		
		jla[3]=new JLabel("年龄");
		jtf[3]=new JTextField(10);
		jtf[3].setText(dm.getValueAt(RowData, 3).toString());
		
		jla[4]=new JLabel("籍贯");
		jtf[4]=new JTextField(10);
		jtf[4].setText(dm.getValueAt(RowData, 4).toString());
		
		jla[5]=new JLabel("系名");
		jtf[5]=new JTextField(10);
		jtf[5].setText(dm.getValueAt(RowData, 5).toString());
		
		jb[0]=new JButton("确定");//监听
		jb[0].addActionListener(this);
		jb[1]=new JButton("取消");//监听
		jb[1].addActionListener(this);
		
		this.setLayout(new GridLayout(7,2));
		for(int i=0;i<jtf.length;i++)
		{
			this.add(jla[i]);
			this.add(jtf[i]);
		}
		this.add(jb[0]);
		this.add(jb[1]);
		
		this.setSize(300,300);
		this.setLocation(300, 100);
		this.setVisible(true);
		this.setFocusable(false);
		this.setDefaultCloseOperation(JDialog.DO_NOTHING_ON_CLOSE);
	}

	@Override
	public void actionPerformed(ActionEvent e) {
		//如果点击了确定
		if(e.getSource()==jb[0])
		{
			String num=this.jtf[0].getText();//学号
			String name=this.jtf[1].getText();//姓名
			String sex=this.jtf[2].getText();//性别
			int age=Integer.parseInt(this.jtf[3].getText());//年龄
			String Jg=this.jtf[4].getText();//籍贯
			String dept=this.jtf[5].getText();//系名
			//创建SQL语句
			String sql="update student set stuName=?,stuSex=?,stuAge=?,stuJg=?,stuDept=? where stuId=?";
			System.out.println("用户想要修改");
			//连接数据库
			try {
				Class.forName("com.microsoft.sqlserver.jdbc.SQLServerDriver");
				ct=DriverManager.getConnection("jdbc:sqlserver://127.0.0.1:1433;database=StudentManagement","sa","");
				ps=ct.prepareStatement(sql);
				//给？传值
				ps.setString(1, name);
				ps.setString(2, sex);
				ps.setInt(3, age);
				ps.setString(4, Jg);
				ps.setString(5, dept);
				ps.setString(6,num );
				
				int i=ps.executeUpdate();
				if(i==1)
				{
					System.out.println("成功");
					JOptionPane.showMessageDialog(this, "修改成功！");
				}else if(i==0)
				{
					System.out.println("失败");
					JOptionPane.showMessageDialog(this, "修改失败！");
				}
			} catch (Exception e1) {
				// TODO Auto-generated catch block
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
			this.dispose();
		}else if(e.getSource()==jb[1])
		{
			this.dispose();
		}
		
	}
}
