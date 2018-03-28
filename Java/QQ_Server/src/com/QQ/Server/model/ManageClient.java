package com.QQ.Server.model;
//�û��̹߳����ࣺÿ����һ���û��̣߳��Ͱ��û���ӵ���Map
import java.util.*;
import com.QQ.Common.*;
import com.QQ.Server.Helper.QQUserThread;
public class ManageClient {
	//���HaspMap�����û��̣߳�Key=�û�id,V=�û��߳�
	private static HashMap<String, QQUserThread> hm=new HashMap<String, QQUserThread>();
	public static HashMap getHm() {
		return hm;
	}
	public static void setHm(HashMap hm) {
		ManageClient.hm = hm;
	}
	//���ܣ����û���ӵ�����
	//���룺һ���û�����һ���߳�
	public static void addToMap(User u,QQUserThread qut)
	{
		hm.put(u.getUseid(), qut);
	}
	//���ܣ����ظ��̵߳�ֵ
	public static QQUserThread getUserThread(String id)
	{
		//��HashMap��ȡ����ֵ
		return (QQUserThread)hm.get(id);
	}
	//���ܣ���ȡ�������ߺ��ѵ�ID
	public static String ShowAllOnLineUser()
	{
		//��ʽ useid1 useid2 useid3����
		String res="";
		//�������ϣ��õ���Щ������
		Iterator<String> iter=hm.keySet().iterator();
		while(iter.hasNext())
		{
			//��ȡ�����ڼ���������û���id
			res+=iter.next().toString()+" ";
		}
		return res;
	}
	//���ܣ���ָ֤�������Ƿ�����ڼ���
	public static boolean IsOnline(User user)
	{
		return hm.containsKey(user.getUseid());
	}
	
	
	
	
	
	
	
}
