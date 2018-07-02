package com.hsp.controller;

import java.io.IOException;
import java.io.PrintWriter;
import java.util.ArrayList;

import javax.servlet.ServletException;
import javax.servlet.http.HttpServlet;
import javax.servlet.http.HttpServletRequest;
import javax.servlet.http.HttpServletResponse;

import com.hsp.service.UsersService;

public class GotoManage extends HttpServlet {

	
	public void doGet(HttpServletRequest request, HttpServletResponse response)
			throws ServletException, IOException {

		//�����û������pageNow
		int pageNow=1;
		String s_pageNow=request.getParameter("pageNow");
		//�ж�s_pageNow�Ƿ�Ϊ��
		if(s_pageNow!=null){
			pageNow=Integer.parseInt(s_pageNow);
		}
		
		//����service��׼������
		UsersService usersService=new UsersService();
		ArrayList al=usersService.getUsersByPage(pageNow, 3);
		int pageCount=usersService.getPageCount(3);
		
		//��Ϊ����Ҫ�������jspʹ��
		//pageContext session request application
		request.setAttribute("al", al);
		request.setAttribute("pageConunt", pageCount);
		
		//����
		request.getRequestDispatcher("/WEB-INF/admin/ManagerUsers.jsp").forward(request, response);
	}

	public void doPost(HttpServletRequest request, HttpServletResponse response)
			throws ServletException, IOException {
		this.doGet(request, response);
		
	}

}
