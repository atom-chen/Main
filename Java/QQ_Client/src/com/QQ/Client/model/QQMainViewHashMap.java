package com.QQ.Client.model;
/*
 * ���ܣ��������е�QQ������
 */
import java.util.*;
import com.QQ.Client.view1.*;
public class QQMainViewHashMap {
	//Key:�����������û���ID value:�����汾��
	private static HashMap hm=new HashMap<String, QQ_Main>();
	//���
	public static void addToQQMainMap(String userid,QQ_Main qm)
	{
		hm.put(userid, qm);
	}
	//��ȡ
	public static QQ_Main getQM(String userid)
	{
		return (QQ_Main)hm.get(userid);
	}
}
