package com.QQ.Server.Helper;
//功能：登录验证，并在验证成功后为每个用户单开一个线程
import java.net.*;
import java.util.HashMap;
import java.util.Iterator;
import java.io.*;
import com.QQ.Common.*;
import com.QQ.Server.model.ManageClient;

public class QQUserThread extends Thread{
	public Socket s;
	//构造的时候要获得该用户的连接
	public QQUserThread(Socket s)
	{
		this.s=s;
	}
	//功能：通知所有人，我来了
	public void informAllUser(String MyId)
	{
		Message m=new Message();
		m.setSender(MyId);
		m.setMessType(MessageType.Message_res_onLineFriendList);
		m.setCon(MyId);
			
		//拿到用户管理类的引用
		HashMap hm=ManageClient.getHm();
		//遍历当前存在于集合的所有用户
		Iterator iter=hm.keySet().iterator();
		while(iter.hasNext())
		{
			//拿到该好友id
			String onLineFriendId=iter.next().toString();
			if(onLineFriendId.equals(MyId))
			{
				continue;
			}
			System.out.println("在线好友id为"+onLineFriendId);
			//拿到该好友线程
			QQUserThread qsh=ManageClient.getUserThread(onLineFriendId);
			//拿到该好友输出流
			try {
				ObjectOutputStream oos=new ObjectOutputStream(qsh.s.getOutputStream());
				m.setGetter(onLineFriendId);
				//发送消息
				oos.writeObject(m);
			} catch (IOException e) {
				// TODO Auto-generated catch block
				e.printStackTrace();
			}
		}
	}
	//为每个用户单开的线程，完成与服务器的收发消息
	public void run()
	{
		while(true)
		{
			try {
				//接收客户端发送
				ObjectInputStream ois=new ObjectInputStream(s.getInputStream());
				Message m=(Message)ois.readObject();
				
				//如果是普通消息包
				if(m.getMessType().equals(MessageType.Message_comm_mes))
				{
					//获取目标的id号
					String Getter=m.getGetter();
					//取出接收方的Socket
					Socket g=ManageClient.getUserThread(Getter).s;
					//获取接受方的输出流
					ObjectOutputStream oos=new ObjectOutputStream(g.getOutputStream());
					//发送出去
					oos.writeObject(m);
				}
				//如果是要求在线好友信息的包
				else if(m.getMessType().equals(MessageType.Message_get_onLineFriendList))
				{
					//拿到目标字符串
					String res=ManageClient.ShowAllOnLineUser();
					//构造新的消息
					Message m2=new Message();
					m2.setCon(res);
					m2.setMessType(MessageType.Message_res_onLineFriendList);
					m2.setGetter(m.getSender());
					//原路返回
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
