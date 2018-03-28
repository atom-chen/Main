package com.QQ.Client.model;
import java.util.*;
import com.QQ.Client.viewchildren.*;
//HashMap:管理所有聊天界面
public class ChatViewHashMap {
	//hm:key=Me+" "+you  Value是聊天界面
	//根据引用传递，这样就能获取这个聊天界面
	private static HashMap hm=new HashMap<String, Chat>();
	//功能：添加界面到集合
	public static void addToChatMap(String MeIdAndYouId,Chat c)
	{
		hm.put(MeIdAndYouId, c);
	}
	//功能：获取该界面
	public static Chat GetChat(String MeIdAndYouId)
	{
		return (Chat)hm.get(MeIdAndYouId);
	}
}
