package com.Test1;

/**
* ����һ���ͻ��˳��򣬿������ӷ������˿� 9999
*/
import java.io.BufferedReader;
import java.io.IOException;
import java.io.InputStreamReader;
import java.io.PrintWriter;
import java.net.*;
public class HanClient1 {
public static void main(String[] args) {
new HanClient1();
}
public HanClient1(){
try {
	//Socket()����ȥ����ĳ���������� 127.0.0.1 ��ʾ�������� ip
	//9999 �Ƿ������Ķ˿ں�
	Socket s=new Socket("127.0.0.1",9999);
	//��� s ���ӳɹ����Ϳ��Է������ݵ���������
	//����ͨ�� pw �� s д����,true ��ʾ��ʱˢ��
	PrintWriter pw=new PrintWriter(s.getOutputStream(),true);
	pw.println("��������ǿͻ���");

	//Ҫ��ȡ s �д��ݵ�����
	InputStreamReader isr=new InputStreamReader(s.getInputStream());
	BufferedReader br=new BufferedReader(isr);
	String info=br.readLine();
	System.out.println("���յ���������\t"+info);
	} catch (Exception e) {
		e.printStackTrace();
		}
}
}