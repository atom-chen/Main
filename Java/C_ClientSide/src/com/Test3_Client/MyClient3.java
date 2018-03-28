package com.Test3_Client;
import javax.swing.*;
import java.net.*;
import java.awt.event.ActionEvent;
import java.awt.event.ActionListener;
import java.io.*;

//通讯的客户端界面
public class MyClient3 extends JFrame implements ActionListener{
	JTextArea jta=null;
	JScrollPane jsp=null;
	JPanel jp=null;
	JTextField jtf=null;
	JButton jb=null;
	
	Socket s=null;
	PrintWriter pw=null;
	public static void main(String[] args) {
		// TODO Auto-generated method stub
		new MyClient3();
	}
	public MyClient3()
	{
		//构造界面
		//构造界面
		this.jta=new JTextArea();
		this.jsp=new JScrollPane(jta);
		
		this.jp=new JPanel();
		this.jtf=new JTextField(10);
		jb=new JButton("Send");
		jb.addActionListener(this);
		jp.add(jtf);
		jp.add(jb);
		
		this.add(jsp);
		this.add(jp,"South");
		this.setSize(300,300);
		this.setTitle("我是客户端");;
		this.setVisible(true);
		this.setDefaultCloseOperation(JFrame.EXIT_ON_CLOSE);
		this.setResizable(false);
		
		//连接服务器
		try {
			s=new Socket("127.0.0.1",8888);
			//输出流
			pw=new PrintWriter(s.getOutputStream(),true);
			//输入流
			BufferedReader br=new BufferedReader(new InputStreamReader(s.getInputStream()));
			
			//实现循环接收服务器发来的信息
			while(true)
			{
				//把聊天内容添加到多行文本框
				jta.append("服务器说："+br.readLine()+"\r\n");
			}
		} catch (IOException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		}
		

	}
	@Override
	public void actionPerformed(ActionEvent e) {
		//按下发送按钮
		if(e.getSource()==jb)
		{
			//读取单行文本框内容
			String info=this.jtf.getText();
			//将文本框信息发送到客户端
			this.pw.println(info);
			//添加发送的内容到多行文本框
			jta.append("客户端说："+info+"\r\n");
			//成功发送后清空单行文本框
			this.jtf.setText("");
		}
		
	}
}
