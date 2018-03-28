package com.Student.Model2_2;
//功能：实现主面板
import java.sql.*;
import java.util.*;
import javax.swing.*;
import java.awt.*;
import java.awt.event.*;


public class StuManager extends JFrame implements ActionListener{
	Connection ct=null;
	PreparedStatement ps=null;
	ResultSet rs=null;
	public static void main(String[] args) {
		StuManager sm2=new StuManager();
	}
	//由于我们希望整个界面所用的数据库模型，都来自同一个对象，故作为全局变量
	StuModel db=null;
	//分析：界面是border布局
	//北部
	JLabel jla1=null;
	JTextField jtf=null;
	JButton jb1=null;
	JPanel jp1=null;
	
	//南部
	JButton jb[]=new JButton[3];
	JPanel jp2=null;
	
	//中部
	JTable jt=null;
	JScrollPane jsp=null;
	
	//开始构造界面
	public StuManager()
	{
		db=new StuModel();
		String []data={"1"};
		db.select("select * from student where 1=?", data);
		
		jla1=new JLabel("请输入名字");
		jtf=new JTextField(10);
		jb1=new JButton("查询");//被监听
		jb1.addActionListener(this);
		
		jp1=new JPanel();
		jp1.add(jla1);
		jp1.add(jtf);
		jp1.add(jb1);
		
		jb[0]=new JButton("添加");//被监听
		jb[0].addActionListener(this);
		jb[1]=new JButton("修改");//被监听
		jb[1].addActionListener(this);
		jb[2]=new JButton("删除");//被监听
		jb[2].addActionListener(this);
		jp2=new JPanel();
		for(int i=0;i<jb.length;i++)
		{
			jp2.add(jb[i]);
		}
		
//		db=new StuModel("");
		jt=new JTable(db);
		jsp=new JScrollPane(jt);
		
		this.add(jp1,BorderLayout.NORTH);
		this.add(jsp);
		this.add(jp2,BorderLayout.SOUTH);
		
		this.setSize(500,500);
		
		this.setFocusable(false);
		this.setVisible(true);
		this.setDefaultCloseOperation(JFrame.EXIT_ON_CLOSE);
	}
	//最终数据都返回到主面板
	@Override
	public void actionPerformed(ActionEvent e) {
		//查询
		if(e.getSource()==jb1)
		{
			System.out.println("用户想要查询");
			String name=this.jtf.getText();
			//定义SQL语句
			String sql="select * from student where stuName=?";	
			//定义要查询的学生姓名
			String data[]={name};
			db=new StuModel();
			//把查询要求传递给学生表模型，并接受新的学生表模型
			db.select(sql,data);
			this.jt.setModel(db);
		}
		//添加
		else if(e.getSource()==jb[0])
		{
			System.out.println("用户想要添加");
			//弹出添加JDialog
			DatabaseInsertPane du=new DatabaseInsertPane(this,"添加学生", true);
			
			//更新数据模型
			System.out.println("插入");
			db=new StuModel();
			String[] d1={"1"};
			db.select("select * from student where 1=?", d1);
			this.jt.setModel(db);
		}
		//修改
		else if(e.getSource()==jb[1])
		{
			System.out.println("用户想要修改");
			//给修改类传数据
			//获取当前选中行
			int RowData=this.jt.getSelectedRow();
			if(RowData==-1)
			{
				JOptionPane.showMessageDialog(this, "请选择一行");
				return;
			}
			//如果操作检查通过，则打开修改界面
			DatabaseUpdatePane du=new DatabaseUpdatePane(this, "修改",true,db, RowData);
			System.out.println("修改");
			//更新当前显示
			db=new StuModel();
			String[] d1={"1"};
			db.select("select * from student where 1=?", d1);
			this.jt.setModel(db);
		}
		//删除
		else if(e.getSource()==jb[2])
		{
			//获得要删除列的索引
			int num=this.jt.getSelectedRow();
			if(num==-1)
			{
				//弹出提示信息
				JOptionPane.showMessageDialog(this, "请选择一行");
				return;
			}
			//获取该元组的第一个属性
			String stuID=db.getValueAt(num, 0).toString();
			String sql="delete from student where stuID=?";
			String data[]={stuID};

			boolean b=db.addToData(sql, data);
			if(b==false)
			{
				JOptionPane.showMessageDialog(this, "删除失败！请刷新！");
				return;
			}
			//刷新数据库
			System.out.println("删除成功");
			db=new StuModel();
			String[] d1={"1"};
			db.select("select * from student where 1=?", d1);
			this.jt.setModel(db);
		}
}
}









