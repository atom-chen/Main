package com.Test1;
//�ͻ���
import java.net.*;
import java.io.*;

public class MyClient1 {
	public MyClient1()
	{
		try {
			//�����˽�������
			Socket s=new Socket("127.0.0.1",8888);
			
			//�����˷�����Ϣ
			PrintWriter pw=new PrintWriter(s.getOutputStream(),true);
			pw.println("��ã����ǿͻ���");
			
			//Ҫ��ȡs�д��ݵ�����
			InputStreamReader isr=new InputStreamReader(s.getInputStream());
			BufferedReader br=new BufferedReader(isr);
			String info=br.readLine();
			System.out.println("���յ���������Ϣ��"+info);
			
		} catch (Exception e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
			System.out.println("�������쳣������");
		}
	}
	public static void main(String []args)
	{
		new MyClient1();
	}
}
