package com.QQ.Server.Helper;

//功能：服务器验证用户信息
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
			//注册监听
			System.out.println("服务端在9999端口监听");
			ss=new ServerSocket(9999);
			//服务器循环接收对象 验证用户账号密码正确性
			while(true)
			{
				//等待连接
				s=ss.accept();
				//接收传过来的User对象
				ObjectInputStream ois=new ObjectInputStream(s.getInputStream());
				User u=(User)ois.readObject();
				//看是否已经在线
				if(ManageClient.IsOnline(u))
				{
					//若已经在线，则返回失败信息
					Message m=new Message();
					m.setMessType(MessageType.Message_login_Fail);
					ObjectOutputStream oos=new ObjectOutputStream(s.getOutputStream());
					oos.writeObject(m);
					continue;
				}
				//读取u的用户名和密码信息
				String userid=u.getUseid();
				String passwd=u.getPasswd();
				System.out.println("已收到u，用户名"+userid+"  密码"+passwd);
				//到数据库验证
				if(passwd.equals("123"))
				{
					Message m=new Message();
					m.setMessType(MessageType.Message_login_Succeed);
					ObjectOutputStream oos=new ObjectOutputStream(s.getOutputStream());
					oos.writeObject(m);
					//每登录一个用户，就为登陆者单开一个线程,在线程中循环完成与登陆者的消息交互
					QQUserThread qut=new QQUserThread(s);
					//把用户添加到集合
					ManageClient.addToMap(u, qut);
					//启动线程
					qut.start();
					//通知所有人 我在线了
					qut.informAllUser(userid);
				}
				else
				{
					Message m=new Message();
					m.setMessType(MessageType.Message_login_Fail);
					//把m发回去
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
