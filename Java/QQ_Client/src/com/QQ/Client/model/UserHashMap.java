package com.QQ.Client.model;
import java.util.*;
import com.QQ.Client.Helper.*;
//��������QQ�߳�
public class UserHashMap {
	private static HashMap<String, QqUserThread> hm=new HashMap<String, QqUserThread>();
	//����Ϊkey,�û��߳�Ϊvalue����ӵ�����ͳһ����
	public static void addToMap(String useid,QqUserThread qut)
	{
		hm.put(useid, qut);
	}
	//ͨ��id���õ�һ���û����߳�
	public static QqUserThread getQqUserThread(String useid)
	{
		return hm.get(useid);
	}
}
