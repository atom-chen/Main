<%@ page language="java" import="java.util.*" pageEncoding="utf-8"%>
<%
String path = request.getContextPath();
String basePath = request.getScheme()+"://"+request.getServerName()+":"+request.getServerPort()+path+"/";
%>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.01 Transitional//EN">
<html>
  <head>
    <base href="<%=basePath%>">
    
    <title>登陆页面</title>
    
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
   <img src='images/1.jpg'/>
   <hr/>
   <h1>用户登陆</h1>
   <form action='/UserManager/LoginClServlet' method='post'>
       用户名id:<input type='text' name='id'/> <br/>
       密    码:<input type='password' name='password'/><br/>
   <input type='submit' value='登陆'/><br/>
   </form>
   <hr/><img src='images/2.jpg'>
  </body>
</html>
