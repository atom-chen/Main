package com.QQ.Client.Helper;
//����:�ͻ������ӷ���˵ĺ�̨�������������ݽ���
import java.net.*;
import java.io.*;

import com.QQ.Client.model.*;

import com.QQ.Common.*;
public class QqClienConServer {
	public Socket s=null;
	//���ܣ�����������֤��¼��Ϣ
	//���룺����User ���:����Message
	public boolean ToServer(User o)
	{
		//ÿ��������һ�Σ���Ӧ�û��һ������
		try {
			s=new Socket("192.168.2.102",9999);
		} catch (IOException e) {
			e.printStackTrace();
		}
		boolean b=false;
		try {
			//��o���͵�������
			ObjectOutputStream oos=new ObjectOutputStream(s.getOutputStream());
			oos.writeObject(o);
			//���շ�������m
			ObjectInputStream ois=new ObjectInputStream(s.getInputStream());
			Message m=(Message)ois.readObject();
			//�жϷ��ص�¼��Ϣ������
			if(m.getMessType().equals(MessageType.Message_login_Succeed))
			{
				//��¼�ɹ�
				b=true;
				//��¼�ɹ��󴴽�User���߳�
				QqUserThread qut=new QqUserThread(s);
				//����̵߳�����
				UserHashMap.addToMap(o.getUseid(), qut);
				//�����߳�
				qut.start();
				
			}else
			{
				//����ʧ�ܾ͹ر�����
				try {
					s.close();
				} catch (IOException e) {
					// TODO Auto-generated catch block
					e.printStackTrace();
				}
			}
		} catch (Exception e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		}finally{
			return b;
		}
	}

}
