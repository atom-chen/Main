package com.shp.view;

import java.io.IOException;
import java.io.PrintWriter;

import javax.servlet.ServletException;
import javax.servlet.http.HttpServlet;
import javax.servlet.http.HttpServletRequest;
import javax.servlet.http.HttpServletResponse;

public class AddUserView extends HttpServlet {

	
	public void doGet(HttpServletRequest request, HttpServletResponse response)
			throws ServletException, IOException {

		response.setContentType("text/html,charset=gb2132");
		response.setCharacterEncoding("gb2312");
		PrintWriter out = response.getWriter();
		
		out.println("<img src='images/1.jpg'/> <a href='/UserManager/MainFrame'>����������</a>  <a href='/UserManager/LoginServlet'>��ȫ�˳�</a>");
		out.println("<h1>����û�</h1>");
		out.println("<form action='/UserManager/UserClServlet?type=add' method='post'>");
		out.println("<table border=1px bordercolor=green cellspacing=0 width=500px>");
		out.println("<tr><td>�û���</td><td><input type='text' name='username' /></td></tr>");
		out.println("<tr><td>email</td><td><input type='text' name='email' /></td></tr>");
		out.println("<tr><td>����</td><td><input type='text' name='grade' /></td></tr>");
		out.println("<tr><td>����</td><td><input type='text' name='passwd' /></td></tr>");
		out.println("<tr><td><input type='submit' value='����û�'/></td><td><input type='reset' value='������д'/></td></tr>");
		out.println("</table>");
		out.println("</form>");
		out.println("<hr/><img src='images/2.jpg'>");
		
	}

	public void doPost(HttpServletRequest request, HttpServletResponse response)
			throws ServletException, IOException {

		this.doGet(request, response);
		
	}

	

}
