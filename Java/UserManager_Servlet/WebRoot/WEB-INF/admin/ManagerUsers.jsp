<%@ page language="java" import="java.util.*,com.hsp.domain.*" pageEncoding="utf-8"%>
<%
String path = request.getContextPath();
String basePath = request.getScheme()+"://"+request.getServerName()+":"+request.getServerPort()+path+"/";
%>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.01 Transitional//EN">
<html>
  <head>
    <base href="<%=basePath%>">
    
    <title>用户管理</title>
    
	<meta http-equiv="pragma" content="no-cache">
	<meta http-equiv="cache-control" content="no-cache">
	<meta http-equiv="expires" content="0">    
	<meta http-equiv="keywords" content="keyword1,keyword2,keyword3">
	<meta http-equiv="description" content="This is my page">
	<!--
	<link rel="stylesheet" type="text/css" href="styles.css">
	-->

  </head>
  <img src='images/1.jpg'/>欢迎  登陆   <a href='/UserManager/admin/ManagerFrame.jsp'>返回主界面</a>  <a href='/UserManager/index.jsp'>安全退出</a>
  <h1>管理用户</h1>
  <table border=1px bordercolor=green cellspacing=0 width=500px>
  <tr><th>id</th> <th>用户名</th> <th>email</th> <th>级别</th><th>删除用户</th><th>修改用户</th></tr>
  <!--循环显示所有用户信息-->
  <%
  ArrayList<User> al=(ArrayList<User>)request.getAttribute("al");
  for(User u:al){
  %>
   <tr><td><%=u.getId() %></td> <td><%=u.getName() %></td> <td><%=u.getEmail() %></td> <td><%=u.getGrade()%></td><td>删除用户</td><td>修改用户</td></tr>
  <%
  }
   %>
   </table>
   <%
   //取出pagecount、
   int pageCount=(Integer)request.getAttribute("pageConunt");
   for(int i=1;i<=pageCount;i++){
   %>
   <a href="/UserManager/GotoManage?pageNow=<%=i%>"><<%=i%>></a>
   <%
   }
    %>
  
  <body>
    
  </body>
</html>
