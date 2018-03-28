package com.QQ.Client.viewchildren;
import javax.swing.*;

import com.QQ.Client.view1.QQ_Main;

import java.awt.*;
import java.awt.event.*;
//黑名单卡片

public class Heimingdan extends JPanel{
	int count=10;
	JButton jb1,jb2,jb3;
	JScrollPane jsp=null;
	JLabel[] jla=new JLabel[count];
	JPanel jp1,jp2;
	//构造好友列表
	public Heimingdan(QQ_Main qm)
	{
		this.setLayout(new BorderLayout());
		//初始化北部
		jp2=new JPanel(new GridLayout(3,1));
		jb1=new JButton("我的好友");
		jb1.addActionListener(qm);
		jb1.setActionCommand("hy");//委托主界面切换到好友
		
		jb2=new JButton("陌生人");
		jb2.addActionListener(qm);
		jb2.setActionCommand("msr");//委托主界面切换到陌生人
		
		jb3=new JButton("黑名单");
		jp2.add(jb1);
		jp2.add(jb2);
		jp2.add(jb3);
		this.add(jp2,"North");
		
		//初始化中部
		jp1=new JPanel(new GridLayout(jla.length,1,8,8));
		for(int i=0;i<jla.length;i++)
		{
			//初始化好友
			jla[i]=new JLabel((i+1)+"",new ImageIcon("image/mm.jpg"), JLabel.LEFT);
			jla[i].addMouseListener(qm);
			//添加好友到jp1
			jp1.add(jla[i]);
		}
		jsp=new JScrollPane(jp1);
		this.add(jsp,"Center");
	}

}
