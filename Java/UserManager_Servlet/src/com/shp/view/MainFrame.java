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
		
		out.println("<img src='images/1.jpg'/>��ӭ  ��½   <a href='/UserManager/LoginServlet'>���ص�½</a>");
		out.println("<h3>��ѡ����Ҫ���еĲ���</h3>");
		out.println("<a href='/UserManager/ManagerUsers'>�����û� </a><br/>");
		out.println("<a href='/UserManager/UserClServlet?type=goAddUser'>����û� </a><br/>");
		out.println("<a href=''>�����û� </a><br/>");
		out.println("<a href=''>�˳�ϵͳ </a><br/>");
		out.println("<hr/><img src='images/2.jpg'>");
	}
	public void doGet(HttpServletRequest request,HttpServletResponse response)throws ServletException,IOException{
		this.doPost(request, response);
	}

}
