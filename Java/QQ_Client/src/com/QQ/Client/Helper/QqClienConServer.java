package com.QQ.Client.Helper;
//功能:客户端连接服务端的后台用于与服务端数据交互
import java.net.*;
import java.io.*;

import com.QQ.Client.model.*;

import com.QQ.Common.*;
public class QqClienConServer {
	public Socket s=null;
	//功能：到服务器验证登录信息
	//输入：对象User 输出:对象Message
	public boolean ToServer(User o)
	{
		//每尝试连接一次，就应该获得一个连接
		try {
			s=new Socket("127.0.0.1",9999);
		} catch (IOException e) {
			e.printStackTrace();
		}
		boolean b=false;
		try {
			//把o发送到服务器
			ObjectOutputStream oos=new ObjectOutputStream(s.getOutputStream());
			oos.writeObject(o);
			//接收返回来的m
			ObjectInputStream ois=new ObjectInputStream(s.getInputStream());
			Message m=(Message)ois.readObject();
			//判断返回登录消息的类型
			if(m.getMessType().equals(MessageType.Message_login_Succeed))
			{
				//登录成功
				b=true;
				//登录成功后创建User的线程
				QqUserThread qut=new QqUserThread(s);
				//添加线程到集合
				UserHashMap.addToMap(o.getUseid(), qut);
				//启动线程
				qut.start();
				
			}else
			{
				//连接失败就关闭连接
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
