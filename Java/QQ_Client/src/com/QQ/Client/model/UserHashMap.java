package com.QQ.Client.model;
import java.util.*;
import com.QQ.Client.Helper.*;
//管理所有QQ线程
public class UserHashMap {
	private static HashMap<String, QqUserThread> hm=new HashMap<String, QqUserThread>();
	//号码为key,用户线程为value，添加到集合统一管理
	public static void addToMap(String useid,QqUserThread qut)
	{
		hm.put(useid, qut);
	}
	//通过id号拿到一个用户的线程
	public static QqUserThread getQqUserThread(String useid)
	{
		return hm.get(useid);
	}
}
