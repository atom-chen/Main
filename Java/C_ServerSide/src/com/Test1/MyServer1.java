package com.Test1;
//�����
import java.net.*;
import java.io.*;

public class MyServer1 {
	//���췽��
	public MyServer1()
	{
		try {
			//����һ��ServerSocket�࣬��8888�Ŷ˿��ϼ���
			ServerSocket ss=new ServerSocket(8888);
			//�������ȴ��û�����
			System.out.println("���Ƿ���ˣ����ڵȴ����ӡ���");
			//�ȴ�ĳ���ͻ��������ӣ��ú����᷵��һ�� Socket ����
			Socket s=ss.accept();
			
			//���ܿͻ��˷���������
			InputStreamReader isr=new InputStreamReader(s.getInputStream());
			BufferedReader br=new BufferedReader(isr);
			String info=br.readLine();
			System.out.println("���������յ�:"+info);
			
			//�����˻���Ϣ
			PrintWriter pw=new PrintWriter(s.getOutputStream(),true);
			pw.println("���Ƿ���ˣ���Ϣ���յ���");
			
		} catch (IOException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		}
		
	}
	public static void main(String []args)
	{
		new MyServer1();
	}
}
