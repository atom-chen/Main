package com.QQ.Client.model;
import java.io.IOException;

import com.QQ.Client.Helper.QqClienConServer;
import com.QQ.Common.*;
//���ܣ��ͻ����û������߼���
public class QqClientLogin {
	QqClienConServer qccs=null;
	public QqClientLogin()
	{
		qccs=new QqClienConServer();
	}
	//���ܣ����û����������װ�����������ӷ���㣬�������Ƿ�ɵ�½���û�
	//���룺�û���������  ������ɷ��¼�Ĳ���ֵ
	public boolean LoginTrueOrFalse(String userid,String passed)
	{
		boolean b=false;
		//��װUser
		User u=new User();
		u.setUseid(userid);
		u.setPasswd(passed);
		//��u�������ӷ����,��֤��¼��ͬʱ����һ�����ӣ�
		b=qccs.ToServer(u);
		return b;
	}

	

}
