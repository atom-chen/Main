package com.Test1;

/**
* ���ǵ�һ���������˳��������� 9999 �˿ڼ���
* ���Խ��տͻ��˷���������
*/
import java.io.BufferedReader;
import java.io.IOException;
import java.io.InputStream;
import java.io.InputStreamReader;
import java.io.PrintWriter;
import java.net.*;
public class HanServer1 {
public static void main(String[] args) {
new HanServer1();
}
//���캯��
public HanServer1(){
try {
	//�� 9999 �Ŷ˿��ϼ���
	ServerSocket ss=new ServerSocket(9999);
	System.out.println("9999 �˿ڼ���...");
	//�ȴ�ĳ���ͻ��������ӣ��ú����᷵��һ�� Socket ����
	Socket s=ss.accept();
	//Ҫ��ȡ s �д��ݵ�����
	InputStreamReader isr=new InputStreamReader(s.getInputStream());
	BufferedReader br=new BufferedReader(isr);
	String info=br.readLine();
	System.out.println("���������յ�:\t"+info);
	
	PrintWriter pw=new PrintWriter(s.getOutputStream(),true);
	pw.println("���Ƿ����������յ��㷢�͵���Ϣ!");
	} catch (IOException e) {
		e.printStackTrace();
		}
}
}