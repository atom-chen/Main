package com.QQ.Client.model;
import java.io.IOException;

import com.QQ.Client.Helper.QqClienConServer;
import com.QQ.Common.*;
//功能：客户端用户操作逻辑类
public class QqClientLogin {
	QqClienConServer qccs=null;
	public QqClientLogin()
	{
		qccs=new QqClienConServer();
	}
	//功能：把用户名和密码包装起来发给连接服务层，并返回是否可登陆给用户
	//输入：用户名和密码  输出：可否登录的布尔值
	public boolean LoginTrueOrFalse(String userid,String passed)
	{
		boolean b=false;
		//包装User
		User u=new User();
		u.setUseid(userid);
		u.setPasswd(passed);
		//把u传给连接服务层,验证登录（同时创建一个连接）
		b=qccs.ToServer(u);
		return b;
	}

	

}
