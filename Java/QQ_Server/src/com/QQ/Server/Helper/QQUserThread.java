package com.QQ.Server.Helper;
//���ܣ���¼��֤��������֤�ɹ���Ϊÿ���û�����һ���߳�
import java.net.*;
import java.util.HashMap;
import java.util.Iterator;
import java.io.*;
import com.QQ.Common.*;
import com.QQ.Server.model.ManageClient;

public class QQUserThread extends Thread{
	public Socket s;
	//�����ʱ��Ҫ��ø��û�������
	public QQUserThread(Socket s)
	{
		this.s=s;
	}
	//���ܣ�֪ͨ�����ˣ�������
	public void informAllUser(String MyId)
	{
		Message m=new Message();
		m.setSender(MyId);
		m.setMessType(MessageType.Message_res_onLineFriendList);
		m.setCon(MyId);
			
		//�õ��û������������
		HashMap hm=ManageClient.getHm();
		//������ǰ�����ڼ��ϵ������û�
		Iterator iter=hm.keySet().iterator();
		while(iter.hasNext())
		{
			//�õ��ú���id
			String onLineFriendId=iter.next().toString();
			if(onLineFriendId.equals(MyId))
			{
				continue;
			}
			System.out.println("���ߺ���idΪ"+onLineFriendId);
			//�õ��ú����߳�
			QQUserThread qsh=ManageClient.getUserThread(onLineFriendId);
			//�õ��ú��������
			try {
				ObjectOutputStream oos=new ObjectOutputStream(qsh.s.getOutputStream());
				m.setGetter(onLineFriendId);
				//������Ϣ
				oos.writeObject(m);
			} catch (IOException e) {
				// TODO Auto-generated catch block
				e.printStackTrace();
			}
		}
	}
	//Ϊÿ���û��������̣߳��������������շ���Ϣ
	public void run()
	{
		while(true)
		{
			try {
				//���տͻ��˷���
				ObjectInputStream ois=new ObjectInputStream(s.getInputStream());
				Message m=(Message)ois.readObject();
				
				//�������ͨ��Ϣ��
				if(m.getMessType().equals(MessageType.Message_comm_mes))
				{
					//��ȡĿ���id��
					String Getter=m.getGetter();
					//ȡ�����շ���Socket
					Socket g=ManageClient.getUserThread(Getter).s;
					//��ȡ���ܷ��������
					ObjectOutputStream oos=new ObjectOutputStream(g.getOutputStream());
					//���ͳ�ȥ
					oos.writeObject(m);
				}
				//�����Ҫ�����ߺ�����Ϣ�İ�
				else if(m.getMessType().equals(MessageType.Message_get_onLineFriendList))
				{
					//�õ�Ŀ���ַ���
					String res=ManageClient.ShowAllOnLineUser();
					//�����µ���Ϣ
					Message m2=new Message();
					m2.setCon(res);
					m2.setMessType(MessageType.Message_res_onLineFriendList);
					m2.setGetter(m.getSender());
					//ԭ·����
					ObjectOutputStream oos=new ObjectOutputStream(s.getOutputStream());
					oos.writeObject(m2);
					
				}

			} catch (Exception e) {
				// TODO Auto-generated catch block
				e.printStackTrace();
			}
			
		}
	}

}
