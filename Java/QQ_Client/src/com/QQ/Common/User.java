package com.QQ.Common;

//�����ࣺ�û��࣬���ڿͻ���������������û�����������Ϣ
//������Ҫ�õ����������ʰ������л�
public class User implements java.io.Serializable{
	//�û������ԣ��û���
	private String Useid;
	//�û������ԣ�����
	private String Passwd;
	public String getUseid() {
		return Useid;
	}
	public void setUseid(String useid) {
		Useid = useid;
	}
	public String getPasswd() {
		return Passwd;
	}
	public void setPasswd(String passwd) {
		Passwd = passwd;
	}
	
}
