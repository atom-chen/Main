package com.Test3_Client;
import javax.swing.*;
import java.net.*;
import java.awt.event.ActionEvent;
import java.awt.event.ActionListener;
import java.io.*;

//ͨѶ�Ŀͻ��˽���
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
		//�������
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
		this.setTitle("���ǿͻ���");;
		this.setVisible(true);
		this.setDefaultCloseOperation(JFrame.EXIT_ON_CLOSE);
		this.setResizable(false);
		
		//���ӷ�����
		try {
			s=new Socket("127.0.0.1",8888);
			//�����
			pw=new PrintWriter(s.getOutputStream(),true);
			//������
			BufferedReader br=new BufferedReader(new InputStreamReader(s.getInputStream()));
			
			//ʵ��ѭ�����շ�������������Ϣ
			while(true)
			{
				//������������ӵ������ı���
				jta.append("������˵��"+br.readLine()+"\r\n");
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
			jta.append("�ͻ���˵��"+info+"\r\n");
			//�ɹ����ͺ���յ����ı���
			this.jtf.setText("");
		}
		
	}
}
