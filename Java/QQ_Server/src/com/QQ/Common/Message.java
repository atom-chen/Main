package com.QQ.Common;

//����涨����������Ϣ�����͹���
//messType=1����¼�ɹ�
//messType=2����¼ʧ��
//messType=3����ͨ����Ϣ��
public class Message implements java.io.Serializable{
	//����messType:������������Ϣ������
	private String messType;
	//Sender��ʾ��Ϣ������
	private String Sender;
	//Getter��ʾ��Ϣ������
	private String Getter;
	//con��ʾ�����߷�������
	private String con;
	//data��ʾ����ʱ��
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
