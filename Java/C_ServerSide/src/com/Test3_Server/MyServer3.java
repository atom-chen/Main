package com.Test3_Server;
//通讯的服务端界面
import javax.swing.*;
import java.net.*;
import java.awt.event.ActionEvent;
import java.awt.event.ActionListener;
import java.io.*;
public class MyServer3 extends JFrame implements ActionListener{
	JTextArea jta=null;
	JScrollPane jsp=null;
	JPanel jp=null;
	JTextField jtf=null;
	JButton jb=null;
	
	PrintWriter pw=null;
	Socket s=null;
	public static void main(String[] args) {
		new MyServer3();

	}
	public MyServer3()
	{
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
		this.setVisible(true);
		this.setTitle("我是服务器");
		this.setDefaultCloseOperation(JFrame.EXIT_ON_CLOSE);
		this.setResizable(false);
		
		try {
			//监听8888端口
			ServerSocket ss=new ServerSocket(8888);
			//成功连接后获取Socket
			s=ss.accept();
			
			pw=new PrintWriter(s.getOutputStream(),true);
			BufferedReader br=new BufferedReader(new InputStreamReader(s.getInputStream()));
			//实现循环接收客户端发来的信息
			while(true)
			{
				//把聊天内容添加到多行文本框
				jta.append("客户端说："+br.readLine()+"\r\n");
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
			jta.append("服务器说："+info+"\r\n");
			//成功发送后清空单行文本框
			this.jtf.setText("");
		}
		
	}
}
