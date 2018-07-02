package com.shp.view;

import java.io.IOException;
import java.io.PrintWriter;
import javax.servlet.ServletException;
import javax.servlet.http.HttpServlet;
import javax.servlet.http.HttpServletRequest;
import javax.servlet.http.HttpServletResponse;

public class MainFrame extends HttpServlet {
	public void doPost(HttpServletRequest request,HttpServletResponse response)throws ServletException,IOException{
		response.setContentType("text/html,chaset=gb2312");
		response.setCharacterEncoding("gb2312");
		PrintWriter out=response.getWriter();
		
		out.println("<img src='images/1.jpg'/>欢迎  登陆   <a href='/UserManager/LoginServlet'>返回登陆</a>");
		out.println("<h3>请选择你要进行的操作</h3>");
		out.println("<a href='/UserManager/ManagerUsers'>管理用户 </a><br/>");
		out.println("<a href='/UserManager/UserClServlet?type=goAddUser'>添加用户 </a><br/>");
		out.println("<a href=''>查找用户 </a><br/>");
		out.println("<a href=''>退出系统 </a><br/>");
		out.println("<hr/><img src='images/2.jpg'>");
	}
	public void doGet(HttpServletRequest request,HttpServletResponse response)throws ServletException,IOException{
		this.doPost(request, response);
	}

}
