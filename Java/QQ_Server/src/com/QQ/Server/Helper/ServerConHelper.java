package com.QQ.Server.Helper;

//���ܣ���������֤�û���Ϣ
import com.QQ.Common.*;
import com.QQ.Server.model.ManageClient;

import java.io.*;
import java.net.*;

public class ServerConHelper {
	ServerSocket ss=null;
	Socket s=null;
	public ServerConHelper()
	{
		try {
			//ע�����
			System.out.println("�������9999�˿ڼ���");
			ss=new ServerSocket(9999);
			//������ѭ�����ն��� ��֤�û��˺�������ȷ��
			while(true)
			{
				//�ȴ�����
				s=ss.accept();
				//���մ�������User����
				ObjectInputStream ois=new ObjectInputStream(s.getInputStream());
				User u=(User)ois.readObject();
				//���Ƿ��Ѿ�����
				if(ManageClient.IsOnline(u))
				{
					//���Ѿ����ߣ��򷵻�ʧ����Ϣ
					Message m=new Message();
					m.setMessType(MessageType.Message_login_Fail);
					ObjectOutputStream oos=new ObjectOutputStream(s.getOutputStream());
					oos.writeObject(m);
					continue;
				}
				//��ȡu���û�����������Ϣ
				String userid=u.getUseid();
				String passwd=u.getPasswd();
				System.out.println("���յ�u���û���"+userid+"  ����"+passwd);
				//�����ݿ���֤
				if(passwd.equals("123"))
				{
					Message m=new Message();
					m.setMessType(MessageType.Message_login_Succeed);
					ObjectOutputStream oos=new ObjectOutputStream(s.getOutputStream());
					oos.writeObject(m);
					//ÿ��¼һ���û�����Ϊ��½�ߵ���һ���߳�,���߳���ѭ��������½�ߵ���Ϣ����
					QQUserThread qut=new QQUserThread(s);
					//���û���ӵ�����
					ManageClient.addToMap(u, qut);
					//�����߳�
					qut.start();
					//֪ͨ������ ��������
					qut.informAllUser(userid);
				}
				else
				{
					Message m=new Message();
					m.setMessType(MessageType.Message_login_Fail);
					//��m����ȥ
					ObjectOutputStream oos=new ObjectOutputStream(s.getOutputStream());
					oos.writeObject(m);
				}
				
			}
		} catch (Exception e) {
			// TODO: handle exception
			e.printStackTrace();
		}
	}
	public void close()
	{
		try {
			if(ss!=null)
				ss.close();
			if(s!=null)
				s.close();
		} catch (Exception e) {
			e.printStackTrace();
		}
	}

}
