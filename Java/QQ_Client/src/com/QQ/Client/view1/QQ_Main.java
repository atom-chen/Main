package com.QQ.Client.view1;
import com.QQ.Client.model.ChatViewHashMap;
import com.QQ.Client.model.QqClientLogin;
//QQ主界面
import com.QQ.Client.viewchildren.*;
import com.QQ.Common.Message;

import javax.swing.*;
import java.awt.*;
import java.awt.event.*;

public class QQ_Main extends JFrame implements ActionListener,MouseListener{
	//主界面是卡片布局
	CardLayout card=null;
	JPanel jp=null;
	//主界面有3个JPanel
	Haoyou hy=null;
	Moshengren msr=null;
	Heimingdan hmd=null;
	//我认为使用者自己也是窗体的属性
	public String Onwer;
	public QQ_Main(String Onwer)
	{
		this.Onwer=Onwer;
		this.card=new CardLayout();
		jp=new JPanel(card);
		//初始化3个卡片
		hy=new Haoyou(this);
		msr=new Moshengren(this);
		hmd=new Heimingdan(this);
			
		//添加到卡片
		jp.add(hy,"1");
		jp.add(msr,"2");
		jp.add(hmd,"3");
		this.add(jp);
		
		this.setSize(200,500);
		this.setLocation(Toolkit.getDefaultToolkit().getScreenSize().width-400, Toolkit.getDefaultToolkit().getScreenSize().height-1000);
		this.setTitle(Onwer);
		this.setVisible(true);
		this.setDefaultCloseOperation(JFrame.EXIT_ON_CLOSE);
	}
	//功能：把发来的在线好友消息传给好友列表
	public void Tohaoyou(Message m)
	{
		hy.UpdateFriendsList(m);
	}
	@Override
	public void actionPerformed(ActionEvent e) {
		//如果点击了好友
		if(e.getActionCommand().equals("hy"))
		{
			this.card.show(jp, "1");
		}
		//如果点击了陌生人
	    else if(e.getActionCommand().equals("msr"))
		{
			//切换到陌生人选项卡
			this.card.show(this.jp, "2");
		}
	    else if(e.getActionCommand().equals("hmd"))
	    {
	    	this.card.show(jp, "3");
	    }
		
	}
	@Override
	public void mouseClicked(MouseEvent e) {
		// TODO Auto-generated method stub
		
	}
	@Override
	public void mousePressed(MouseEvent e) {
		//如果双击某个人
		if(e.getClickCount()==2)
		{
			//获取点击的人的名称
			JLabel jl=(JLabel)e.getSource();
			//如果该用户不在线
			if(!(jl.isEnabled()))
			{
				JOptionPane.showMessageDialog(null, "该好友不在线！不能发送");
				
			}
			else
			{
				System.out.println("您想要与"+jl.getText()+"号聊天");
				//弹出聊天窗口   把使用者自己和自己的链接，和想要聊天的人传进去
				Chat c=new Chat(this.Onwer,jl.getText());
				//把聊天界面添加到集合
				ChatViewHashMap.addToChatMap(this.Onwer+" "+jl.getText(), c);
			}
		}
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











