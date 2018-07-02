package com.shp.view;

import java.io.IOException;
import java.io.PrintWriter;
import javax.servlet.ServletException;
import javax.servlet.http.HttpServlet;
import javax.servlet.http.HttpServletRequest;
import javax.servlet.http.HttpServletResponse;

public class LoginServlet extends HttpServlet {
		public void doGet(HttpServletRequest request,HttpServletResponse response)throws ServletException,IOException {
			response.setContentType("text/html,charset=gb2312");
			response.setCharacterEncoding("gb2312");
			PrintWriter out=response.getWriter();
			//返回用户界面（html）
			out.println("<img src='images/1.jpg'/>");
			out.println("<hr/>");
			out.println("<h1>用户登陆</h1>");
			out.println("<form action='/UserManager/LoginClServlet' method='post'> ");
			out.println("用户名id:<input type='text' name='id'/> <br/>");
			out.println("密    码:<input type='password' name='password'/><br/>");
			out.println("<input type='submit' value='登陆'/><br/>");
			out.println("</form>");
			String errInfo=(String)request.getAttribute("err");
			if(errInfo!=null){
			out.println("<font color='red'>"+errInfo+"</font>");
			}
			out.println("<hr/><img src='images/2.jpg'>");
		}
		public void doPost(HttpServletRequest request,HttpServletResponse response)throws ServletException,IOException {
			this.doGet(request,response);
		}
	
	

}
