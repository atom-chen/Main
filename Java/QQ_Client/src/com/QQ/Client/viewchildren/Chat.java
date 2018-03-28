package com.QQ.Client.viewchildren;
//�������
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
	//����Ϊ�����ߺͽ����ߣ��Լ����̶߳��Ǹ�������������
	String Onwer;
	String Getter;
	QqUserThread qut;
	public Chat(String Onwer,String Getter)
	{
		//����Լ����߳�
		qut=UserHashMap.getQqUserThread(Onwer);
		this.Onwer=Onwer;
		this.Getter=Getter;
		//��ʼ������
		jp1=new JPanel();
		
		//��ʼ���в�
		jp2=new JPanel();
		jta=new JTextArea();
		jsp=new JScrollPane(jta);
		
		//��ʼ���ϲ�
		jp3=new JPanel(new FlowLayout(FlowLayout.RIGHT));
		jtf=new JTextField(20);
		jb1=new JButton("ȡ��");
		jb1.addActionListener(this);
		jb2=new JButton("����");
		jb2.addActionListener(this);
		jp3.add(jtf);
		jp3.add(jb1);
		jp3.add(jb2);
		
		this.add(jp1,"North");
		this.add(jta,"Center");
		this.add(jp3,"South");
		
		this.setVisible(true);
		this.setSize(500,500);
		this.setTitle(Onwer+" ����"+Getter+" ����");
		this.setLocation(Toolkit.getDefaultToolkit().getScreenSize().width/2-500, Toolkit.getDefaultToolkit().getScreenSize().height/2-500);
		
	}
	//���ܣ������Ϣ�����촰��
	//���룺һ����Ϣa
	public void appendTojta(Message m)
	{
		//����m
		String time=m.getTime();
		String Sender=m.getSender();
		String Getter=m.getGetter();
		String con=m.getCon();
		//���
		this.jta.append(time+"   "+Sender+"��"+Getter+" ˵��"+con+"\r\n");
	}
	@Override
	public void actionPerformed(ActionEvent e) {
		//�������˷���
		if(e.getSource()==jb2)
		{
			//��ȡ�����ı�������
			String said=jtf.getText();
			String time=Calendar.getInstance().getTime().toLocaleString();
			//��ӵ������ı���
			this.jta.append(time+"   "+this.Onwer+"��"+this.Getter+" ˵��"+said+"\r\n");
			//Ȼ����յ����ı�������
			this.jtf.setText("");
			//�༭��Ϣ
			Message m=new Message();
			m.setTime(time);
			m.setSender(Onwer);
			m.setGetter(Getter);
			m.setCon(said);
			m.setMessType(MessageType.Message_comm_mes);
			//�����Լ��߳���ķ���Ϣ����
			qut.WriteToServer(m);
		}
		//��������ȡ��
		else if(e.getSource()==jb1)
		{
			this.dispose();
		}
		
	}


}
