package com.QQ.Common;

//该类规定了所传输信息的类型规则
//messType=1→登录成功
//messType=2→登录失败
//messType=3→普通的消息包
public class Message implements java.io.Serializable{
	//变量messType:代表所传输信息的类型
	private String messType;
	//Sender表示消息发送者
	private String Sender;
	//Getter表示消息接受者
	private String Getter;
	//con表示发送者发的内容
	private String con;
	//data表示发送时间
	private String time;

	public String getSender() {
		return Sender;
	}

	public void setSender(String sender) {
		Sender = sender;
	}

	public String getGetter() {
		return Getter;
	}

	public void setGetter(String getter) {
		Getter = getter;
	}

	public String getCon() {
		return con;
	}

	public void setCon(String con) {
		this.con = con;
	}

	public String getTime() {
		return time;
	}

	public void setTime(String time) {
		this.time = time;
	}

	public String getMessType() {
		return messType;
	}

	public void setMessType(String messType) {
		this.messType = messType;
	}
	
}
