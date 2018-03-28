package com.QQ.Server.model;
//用户线程管理类：每创建一个用户线程，就把用户添加到该Map
import java.util.*;
import com.QQ.Common.*;
import com.QQ.Server.Helper.QQUserThread;
public class ManageClient {
	//这个HaspMap管理用户线程，Key=用户id,V=用户线程
	private static HashMap<String, QQUserThread> hm=new HashMap<String, QQUserThread>();
	public static HashMap getHm() {
		return hm;
	}
	public static void setHm(HashMap hm) {
		ManageClient.hm = hm;
	}
	//功能：把用户添加到集合
	//输入：一个用户，和一个线程
	public static void addToMap(User u,QQUserThread qut)
	{
		hm.put(u.getUseid(), qut);
	}
	//功能：返回该线程的值
	public static QQUserThread getUserThread(String id)
	{
		//从HashMap中取出该值
		return (QQUserThread)hm.get(id);
	}
	//功能：获取所有在线好友的ID
	public static String ShowAllOnLineUser()
	{
		//格式 useid1 useid2 useid3……
		String res="";
		//遍历集合，拿到哪些人在线
		Iterator<String> iter=hm.keySet().iterator();
		while(iter.hasNext())
		{
			//获取所有在集合里面的用户的id
			res+=iter.next().toString()+" ";
		}
		return res;
	}
	//功能：验证指定号码是否存在于集合
	public static boolean IsOnline(User user)
	{
		return hm.containsKey(user.getUseid());
	}
	
	
	
	
	
	
	
}
