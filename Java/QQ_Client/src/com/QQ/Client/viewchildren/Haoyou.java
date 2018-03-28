package com.QQ.Client.viewchildren;
import javax.imageio.ImageIO;
//好友卡片
import javax.swing.*;

import com.QQ.Client.view1.QQ_Main;
import com.QQ.Common.Message;

import java.awt.*;
import java.awt.event.*;

public class Haoyou extends JPanel{
	int count=50;
	JButton jb1,jb2,jb3;
	JScrollPane jsp=null;
	JLabel[] jla=new JLabel[count];
	JPanel jp1,jp2;
	//构造好友列表
	public Haoyou(QQ_Main qm)
	{
		this.setLayout(new BorderLayout());
		//初始化北部
		jb1=new JButton("我的好友");
		this.add(jb1,"North");
		
		//初始化中部
		jp1=new JPanel(new GridLayout(jla.length,1,8,8));
		for(int i=0;i<jla.length;i++)
		{
			//初始化好友
			jla[i]=new JLabel((i+1)+"",new ImageIcon("image/mm.jpg"), JLabel.LEFT);
			//设置不在线好友是灰色的
			jla[i].setEnabled(false);
			//如果是自己，则不添加到好友列表
			if(jla[i].getText().equals(qm.Onwer))
			{
				continue;
			}
			//给每个好友注册监听
			jla[i].addMouseListener(qm);
			//添加好友到jp1
			jp1.add(jla[i]);
		}
		jsp=new JScrollPane(jp1);
		this.add(jsp,"Center");
		
		//初始化南部
		jp2=new JPanel(new GridLayout(2,1));
		jb2=new JButton("陌生人");
		jb2.addActionListener(qm);
		jb2.setActionCommand("msr");//委托主界面切换到陌生人
		jb3=new JButton("黑名单");
		jb3.addActionListener(qm);
		jb3.setActionCommand("hmd");//委托主界面切换到黑名单
		jp2.add(jb2);
		jp2.add(jb3);
		this.add(jp2,"South");
	}
	
	//功能：更新在线好友消息
	public void UpdateFriendsList(Message m)
	{
		//以空格  切割消息内容
		String OnLineFriend[]=m.getCon().split(" ");
		//按消息内容激活在线好友
		for(int i=0;i<OnLineFriend.length;i++)
		{
			//如果是第1个，就应该点亮第0个标签，以此类推
			this.jla[Integer.parseInt(OnLineFriend[i])-1].setEnabled(true);
		}
	}
	

	

}
