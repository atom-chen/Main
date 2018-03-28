package com.QQ.Common;

//共有类：用户类，用于客户端与服务器传递用户名和密码信息
//由于需要用到对象流，故把它序列化
public class User implements java.io.Serializable{
	//用户的属性：用户名
	private String Useid;
	//用户的属性：密码
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
