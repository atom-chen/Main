package com.QQ.Common;
/*
 * 规定了消息的类型
 */
public interface MessageType {
	String Message_login_Succeed="1";//表明登录成功
	String Message_login_Fail="2";//表明登录失败
	String Message_comm_mes="3";//表明是普通消息报
	String Message_get_onLineFriendList="4";//表明是要求返回在线好友列表的包
	String Message_res_onLineFriendList="5";//表明是返回在线好友列表的包
}
