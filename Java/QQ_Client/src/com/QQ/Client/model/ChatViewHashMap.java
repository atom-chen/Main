package com.QQ.Client.model;
import java.util.*;
import com.QQ.Client.viewchildren.*;
//HashMap:���������������
public class ChatViewHashMap {
	//hm:key=Me+" "+you  Value���������
	//�������ô��ݣ��������ܻ�ȡ����������
	private static HashMap hm=new HashMap<String, Chat>();
	//���ܣ���ӽ��浽����
	public static void addToChatMap(String MeIdAndYouId,Chat c)
	{
		hm.put(MeIdAndYouId, c);
	}
	//���ܣ���ȡ�ý���
	public static Chat GetChat(String MeIdAndYouId)
	{
		return (Chat)hm.get(MeIdAndYouId);
	}
}
