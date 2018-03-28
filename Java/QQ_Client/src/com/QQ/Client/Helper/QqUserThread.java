/*
 * ÿ��¼һ���ͻ��ˣ��ͻ�����һ�����̣߳�һ���߲����һ���˺�
 */
package com.QQ.Client.Helper;
import java.net.*;
import java.io.*;

import com.QQ.Client.model.ChatViewHashMap;
import com.QQ.Client.model.QQMainViewHashMap;
import com.QQ.Client.view1.QQ_Main;
import com.QQ.Client.viewchildren.Chat;
import com.QQ.Common.*;

public class QqUserThread extends Thread{
	private Socket s=null;
	//�����ʱ�򣬰����Լ���Socket������
	public QqUserThread(Socket s)
	{
		this.s=s;
	}
	//�����������ͻ��˵���Ϣ�շ�
	//���ܣ�������Ϣ��������
	public void WriteToServer(Message m)
	{
		try {
			//��o���͵�������
			ObjectOutputStream oos=new ObjectOutputStream(s.getOutputStream());
			oos.writeObject(m);
		} catch (Exception e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		}
	}
	//����:�շ�������������Ϣ     �����һ��Message����
	public void run()
	{
		try {
			while(true)
			{
				//��ȡ����ʹ�õ�������
				ObjectInputStream ois=new ObjectInputStream(s.getInputStream());
				//������Ϣ
				Message m=(Message)ois.readObject();
				
				//�������ͨ��Ϣ��
				if(m.getMessType().equals(MessageType.Message_comm_mes))
				{
					//���⣺��ΰ�m������Ӧ�ķ����ߴ��ڣ�,����Sender�����͵�ָ����Chat?
					//�𰸣���һ������ͳһ�����������촰�ڣ���������������
					
					//���ݽ������õ�������+�����ߵĽ��������
					Chat c=ChatViewHashMap.GetChat(m.getGetter()+" "+m.getSender());
					//����Ϣ���������ߣ��ý�����ȥ���
					c.appendTojta(m);
				}
				//����Ƿ������ĵ�¼��Ϣ��
				else if(m.getMessType().equals(MessageType.Message_res_onLineFriendList))
				{
					//�õ������ߵ�����������
					QQ_Main qm=QQMainViewHashMap.getQM(m.getGetter());
					//��������ȥ�������ߺ���
					if(m!=null)
					{
						qm.Tohaoyou(m);
					}

				}

				
			}
		} catch (Exception e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		}
	}


}
