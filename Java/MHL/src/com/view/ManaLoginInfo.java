package com.view;
import java.awt.*;
import javax.swing.*;
import javax.swing.border.Border;

import com.Toolkit.MyFont;

/*
 * 人事管理子页面的view部分
 */
public class ManaLoginInfo extends JPanel{
	//北部
	JPanel jp1;
	JLabel jp1_jl;
	JTextField jp1_jtf;
	JButton jp1_jb;
	//中部
	JTable jta;
	JScrollPane jsp;
	//南部
	JPanel jp2,jp3,jp4;
	JLabel jp3_jl;
	JButton jp4_jb1,jp4_jb2,jp4_jb3;
	//构造界面
	public ManaLoginInfo()
	{
		//初始化北部
		jp1=new JPanel();
		jp1_jl=new JLabel("请输入员工号");
		jp1.add(jp1_jl);
		jp1_jtf=new JTextField(10);
		jp1.add(jp1_jtf);
		jp1_jb=new JButton("查找");
		jp1.add(jp1_jb);
		
		//初始化中部
		jta=new JTable();
		jsp=new JScrollPane(jta);
		
		//初始化南部
		jp2=new JPanel(new BorderLayout());
		jp3=new JPanel();
		jp3_jl=new JLabel("共有x条记录");
		jp3_jl.setFont(MyFont.f1);
		jp3.add(jp3_jl);
		jp2.add(jp3,BorderLayout.WEST);
		jp4=new JPanel(new FlowLayout(FlowLayout.TRAILING));
		jp4_jb1=new JButton("刷新记录");
		jp4_jb1.setFont(MyFont.f3);
		jp4.add(jp4_jb1);
		jp4_jb2=new JButton("修改密码");
		jp4_jb2.setFont(MyFont.f3);
		jp4.add(jp4_jb2);
		jp2.add(jp4,BorderLayout.EAST);
		
		this.setLayout(new BorderLayout());
		this.add(jp1,BorderLayout.NORTH);
		this.add(jsp,BorderLayout.CENTER);
		this.add(jp2,BorderLayout.SOUTH);
		this.setVisible(true);
		
	}

	
}







