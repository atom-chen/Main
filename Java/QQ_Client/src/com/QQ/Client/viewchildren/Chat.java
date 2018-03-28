package com.QQ.Client.viewchildren;
//聊天界面
import java.awt.*;
import javax.swing.*;

import com.QQ.Client.model.QqClientLogin;
import com.QQ.Common.Message;
import com.QQ.Common.MessageType;
import com.QQ.Client.model.*;
import com.QQ.Client.Helper.*;


import java.awt.event.*;
import java.util.Calendar;

public class Chat extends JFrame implements ActionListener{
	JPanel jp1,jp2,jp3;
	JTextArea jta=null;
	JScrollPane jsp=null;
	JTextField jtf=null;
	JButton jb1,jb2;
	//我认为发送者和接收者，自己的线程都是该聊天界面的属性
	String Onwer;
	String Getter;
	QqUserThread qut;
	public Chat(String Onwer,String Getter)
	{
		//获得自己的线程
		qut=UserHashMap.getQqUserThread(Onwer);
		this.Onwer=Onwer;
		this.Getter=Getter;
		//初始化北部
		jp1=new JPanel();
		
		//初始化中部
		jp2=new JPanel();
		jta=new JTextArea();
		jsp=new JScrollPane(jta);
		
		//初始化南部
		jp3=new JPanel(new FlowLayout(FlowLayout.RIGHT));
		jtf=new JTextField(20);
		jb1=new JButton("取消");
		jb1.addActionListener(this);
		jb2=new JButton("发送");
		jb2.addActionListener(this);
		jp3.add(jtf);
		jp3.add(jb1);
		jp3.add(jb2);
		
		this.add(jp1,"North");
		this.add(jta,"Center");
		this.add(jp3,"South");
		
		this.setVisible(true);
		this.setSize(500,500);
		this.setTitle(Onwer+" 在与"+Getter+" 聊天");
		this.setLocation(Toolkit.getDefaultToolkit().getScreenSize().width/2-500, Toolkit.getDefaultToolkit().getScreenSize().height/2-500);
		
	}
	//功能：添加消息到聊天窗口
	//输入：一个消息a
	public void appendTojta(Message m)
	{
		//分析m
		String time=m.getTime();
		String Sender=m.getSender();
		String Getter=m.getGetter();
		String con=m.getCon();
		//添加
		this.jta.append(time+"   "+Sender+"对"+Getter+" 说："+con+"\r\n");
	}
	@Override
	public void actionPerformed(ActionEvent e) {
		//如果点击了发送
		if(e.getSource()==jb2)
		{
			//获取单行文本框内容
			String said=jtf.getText();
			String time=Calendar.getInstance().getTime().toLocaleString();
			//添加到多行文本框
			this.jta.append(time+"   "+this.Onwer+"对"+this.Getter+" 说："+said+"\r\n");
			//然后清空单行文本框内容
			this.jtf.setText("");
			//编辑消息
			Message m=new Message();
			m.setTime(time);
			m.setSender(Onwer);
			m.setGetter(Getter);
			m.setCon(said);
			m.setMessType(MessageType.Message_comm_mes);
			//调用自己线程里的发消息方法
			qut.WriteToServer(m);
		}
		//如果点击了取消
		else if(e.getSource()==jb1)
		{
			this.dispose();
		}
		
	}


}
