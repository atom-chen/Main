package com.Test2_Server;
//����ˣ���ʵ���ڿ���̨�����Ի�
import java.net.*;
import java.io.*;
public class Server2 {

	public static void main(String[] args) {
		// TODO Auto-generated method stub
		new Server2();
	}
	public Server2()
	{	
		try {
			//����9999����
			ServerSocket ss=new ServerSocket(9988);
			//�ȴ�����
			Socket s=ss.accept();
			System.out.println("��������");
			//��Socket����������
			InputStreamReader isr=new InputStreamReader(s.getInputStream());
			BufferedReader br_s=new BufferedReader(isr);
			//��ȡSocket���������
			PrintWriter pw=new PrintWriter(s.getOutputStream(),true);
			
			//��ȡ����̨����
			BufferedReader br_con=new BufferedReader(new InputStreamReader(System.in));
			
			//ѭ������ȡ�ͻ���������ݣ�����ͻ����������
			while(true)
			{
				//��ȡs���
				String in=br_s.readLine();
				//��ʾ������������̨
				System.out.println("�ͻ���˵"+in);
				
				System.out.println("���������Կͻ��Ļظ�");
				//��ȡ������ڿ���̨������
				String out=br_con.readLine();
				//����˻ظ�
				pw.println("�����˵��"+out);
			}
			
			
			
			
			
		} catch (IOException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		}
		
	}

}
