package com.Test2_Client;
import java.net.*;
import java.io.*;

//�ͻ��� ��ʵ�ֺͷ�����ڿ���̨�Ի�
public class Client2 {

	public static void main(String[] args) {
		// TODO Auto-generated method stub
		new Client2();
	}
	public Client2()
	{

		try {
			//���ӵ����ص�9988�˿�
			Socket s=new Socket("127.0.0.1", 9988);
			
			//��ȡ�û��ڿ���̨������
			BufferedReader br_con=new BufferedReader(new InputStreamReader(System.in));
			//��ȡs�������
			PrintWriter pw=new PrintWriter(s.getOutputStream(),true);
			
			//��ȡs��������
			BufferedReader br_s=new BufferedReader(new InputStreamReader(s.getInputStream()));
			
			//ʵ�������˷�����Ϣ
			while(true)
			{
				System.out.println("�����������˷�����Ϣ");
				//��ȡ�û��ڿ���̨������
				String out=br_con.readLine();
				//�����˷�����Ϣ
				pw.println(out);
				
				//���շ���˵Ļ���
				String in=br_s.readLine();
				//��ʾ����˻��ŵ�����̨
				System.out.println(in);
			}
		} catch (Exception e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		}
		
	}
	

}
