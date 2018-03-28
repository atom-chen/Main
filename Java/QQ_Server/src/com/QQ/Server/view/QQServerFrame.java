package com.QQ.Server.view;
//服务器的主界面
import javax.swing.*;

import com.QQ.Server.Helper.ServerConHelper;
import com.QQ.Server.model.*;

import java.awt.*;
import java.awt.event.*;
public class QQServerFrame extends JFrame implements ActionListener{
	JPanel jp;
	JButton jb1,jb2;
	ServerConHelper  sch=null;
	public static void main(String[] args) {
		// TODO Auto-generated method stub
		new QQServerFrame();
	}
	public QQServerFrame()
	{
		jp=new JPanel();
		jb1=new JButton("开启服务");
		jb1.addActionListener(this);
		jb2=new JButton("关闭服务");
		jb2.addActionListener(this);
		jp.add(jb1);
		jp.add(jb2);
		this.add(jp,"North");
		
		this.setSize(500,500);
		this.setVisible(true);
		this.setDefaultCloseOperation(JFrame.EXIT_ON_CLOSE);
		
	}
	@Override
	public void actionPerformed(ActionEvent e) {
		// TODO Auto-generated method stub
		if(e.getSource()==jb1)
		{
			//管理员想要开启服务
			System.out.println("开启服务器");
			sch=new ServerConHelper();
		}else if(e.getSource()==jb2)
		{
			//管理员想要关闭服务
			sch.close();
		}
	}

}
