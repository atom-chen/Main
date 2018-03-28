package com.QQ.Client.model;
/*
 * 功能：管理所有的QQ主界面
 */
import java.util.*;
import com.QQ.Client.view1.*;
public class QQMainViewHashMap {
	//Key:主界面所属用户的ID value:主界面本身
	private static HashMap hm=new HashMap<String, QQ_Main>();
	//添加
	public static void addToQQMainMap(String userid,QQ_Main qm)
	{
		hm.put(userid, qm);
	}
	//获取
	public static QQ_Main getQM(String userid)
	{
		return (QQ_Main)hm.get(userid);
	}
}
