package com.Test3_Server;
//ͨѶ�ķ���˽���
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
		//�������
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
		this.setTitle("���Ƿ�����");
		this.setDefaultCloseOperation(JFrame.EXIT_ON_CLOSE);
		this.setResizable(false);
		
		try {
			//����8888�˿�
			ServerSocket ss=new ServerSocket(8888);
			//�ɹ����Ӻ��ȡSocket
			s=ss.accept();
			
			pw=new PrintWriter(s.getOutputStream(),true);
			BufferedReader br=new BufferedReader(new InputStreamReader(s.getInputStream()));
			//ʵ��ѭ�����տͻ��˷�������Ϣ
			while(true)
			{
				//������������ӵ������ı���
				jta.append("�ͻ���˵��"+br.readLine()+"\r\n");
			}
			
			
			
		} catch (IOException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		}
		
	
	}
	@Override
	public void actionPerformed(ActionEvent e) {
		//���·��Ͱ�ť
		if(e.getSource()==jb)
		{
			//��ȡ�����ı�������
			String info=this.jtf.getText();
			//���ı�����Ϣ���͵��ͻ���
			this.pw.println(info);
			//��ӷ��͵����ݵ������ı���
			jta.append("������˵��"+info+"\r\n");
			//�ɹ����ͺ���յ����ı���
			this.jtf.setText("");
		}
		
	}
}
