/*
 * 每登录一个客户端，就会启动一个该线程，一个线层代表一个账号
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
	//构造的时候，把它自己的Socket传进来
	public QqUserThread(Socket s)
	{
		this.s=s;
	}
	//在这里完成与客户端的消息收发
	//功能：发送消息到服务器
	public void WriteToServer(Message m)
	{
		try {
			//把o发送到服务器
			ObjectOutputStream oos=new ObjectOutputStream(s.getOutputStream());
			oos.writeObject(m);
		} catch (Exception e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		}
	}
	//功能:收服务器发来的信息     输出：一个Message对象
	public void run()
	{
		try {
			while(true)
			{
				//获取本次使用的输入流
				ObjectInputStream ois=new ObjectInputStream(s.getInputStream());
				//接收消息
				Message m=(Message)ois.readObject();
				
				//如果是普通消息包
				if(m.getMessType().equals(MessageType.Message_comm_mes))
				{
					//问题：如何把m给到对应的发送者窗口？,根据Sender，发送到指定的Chat?
					//答案：用一个集合统一管理所有聊天窗口！！！！！！！！
					
					//根据接收者拿到发送者+接收者的界面的引用
					Chat c=ChatViewHashMap.GetChat(m.getGetter()+" "+m.getSender());
					//把消息传给接受者，让接收者去添加
					c.appendTojta(m);
				}
				//如果是返回来的登录消息包
				else if(m.getMessType().equals(MessageType.Message_res_onLineFriendList))
				{
					//拿到接受者的主界面引用
					QQ_Main qm=QQMainViewHashMap.getQM(m.getGetter());
					//让主界面去更新在线好友
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
