package com.shp.view;

import java.io.IOException;
import java.io.PrintWriter;

import javax.servlet.ServletException;
import javax.servlet.http.HttpServlet;
import javax.servlet.http.HttpServletRequest;
import javax.servlet.http.HttpServletResponse;

public class Ok extends HttpServlet {

	public void doGet(HttpServletRequest request, HttpServletResponse response)
			throws ServletException, IOException {

		response.setContentType("text/html,chaset=gb2312");
		response.setCharacterEncoding("gb2312");
		PrintWriter out = response.getWriter();
		out.println("恭喜你，操作成功!<br/>");
		out.println("<a href='/UserManager/MainFrame'>返回主界面 </a>");
	}

	
	public void doPost(HttpServletRequest request, HttpServletResponse response)
			throws ServletException, IOException {

		this.doGet(request, response);
	}

	
}
