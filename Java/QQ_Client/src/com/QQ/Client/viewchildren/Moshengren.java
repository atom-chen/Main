package com.QQ.Client.viewchildren;
import javax.swing.*;

import com.QQ.Client.view1.QQ_Main;

import java.awt.*;
import java.awt.event.*;

//陌生人卡片
public class Moshengren extends JPanel implements MouseListener{
	int count=20;
	JButton jb1,jb2,jb3;
	JScrollPane jsp=null;
	JLabel[] jla=new JLabel[count];
	JPanel jp1,jp2;
	//构造陌生人列表
	public Moshengren(QQ_Main qm)
	{
		this.setLayout(new BorderLayout());
		//初始化北部
		jp2=new JPanel(new GridLayout(2,1));
		jb1=new JButton("我的好友");
		jb1.addActionListener(qm);//委托主界面处理
		jb1.setActionCommand("hy");
		
		jb2=new JButton("陌生人");
		jp2.add(jb1);
		jp2.add(jb2);
		this.add(jp2,"North");
		
		//初始化中部
		jp1=new JPanel(new GridLayout(jla.length,1,8,8));
		for(int i=0;i<jla.length;i++)
		{
			//初始化陌生人
			jla[i]=new JLabel((i+1)+"",new ImageIcon("image/mm.jpg"), JLabel.LEFT);
			jla[i].addMouseListener(qm);
			//添加陌生人
			jp1.add(jla[i]);
		}
		jsp=new JScrollPane(jp1);
		this.add(jsp,"Center");
		
		//初始化南部
		jb3=new JButton("黑名单");
		jb3.addActionListener(qm);
		jb3.setActionCommand("hmd");//委托主界面处理切换到黑名单
		this.add(jb3,"South");
		this.setSize(300,500);
		this.setVisible(true);
	}
	@Override
	public void mouseClicked(MouseEvent e) {
		// TODO Auto-generated method stub
		
	}
	@Override
	public void mousePressed(MouseEvent e) {
		// TODO Auto-generated method stub
		
	}
	@Override
	public void mouseReleased(MouseEvent e) {
		// TODO Auto-generated method stub
		
	}
	@Override
	//鼠标进入改变颜色
	public void mouseEntered(MouseEvent e) {
		//获取点击到的JLabel
		JLabel jl=(JLabel) e.getSource();
		//改变颜色
		jl.setForeground(Color.red);
		
	}
	//鼠标离开改变颜色
	public void mouseExited(MouseEvent e) {
		// TODO Auto-generated method stub
		//获取点击到的JLabel
		JLabel jl=(JLabel) e.getSource();
		//改变颜色
		jl.setForeground(Color.black);
	}

}
