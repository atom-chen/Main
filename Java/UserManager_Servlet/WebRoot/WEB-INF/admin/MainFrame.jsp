<%@ page language="java" import="java.util.*" pageEncoding="utf-8"%>
<%
String path = request.getContextPath();
String basePath = request.getScheme()+"://"+request.getServerName()+":"+request.getServerPort()+path+"/";
%>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.01 Transitional//EN">
<html>
  <head>
    <base href="<%=basePath%>">
    
    <title>管理页面</title>
    
	<meta http-equiv="pragma" content="no-cache">
	<meta http-equiv="cache-control" content="no-cache">
	<meta http-equiv="expires" content="0">    
	<meta http-equiv="keywords" content="keyword1,keyword2,keyword3">
	<meta http-equiv="description" content="This is my page">
	<!--
	<link rel="stylesheet" type="text/css" href="styles.css">
	-->

  </head>
  
  <body>
    <img src='images/1.jpg'/>欢迎  登陆   <a href='/UserManager/index.jsp'>返回登陆</a>
    <h3>请选择你要进行的操作</h3>
    <a href='/UserManager/GotoManage'>管理用户 </a><br/>
    <a href='/UserManager/UserClServlet?type=goAddUser'>添加用户 </a><br/>
    <a href=''>查找用户 </a><br/>
    <a href=''>退出系统 </a><br/>
    <hr/><img src='images/2.jpg'>
  </body>
</html>
