package com.hsp.controller;

import java.io.IOException;
import java.io.PrintWriter;
import java.sql.*;

import javax.servlet.ServletException;
import javax.servlet.http.*;

import com.hsp.domain.User;
import com.hsp.service.UsersService;

public class LoginClServlet extends HttpServlet {
	public void doGet(HttpServletRequest request,HttpServletResponse response)throws ServletException,IOException{
		request.setCharacterEncoding("gb2312");
		//�����û��ύ���û���������
		String id=request.getParameter("id");
		String password=request.getParameter("password");
		
		//����UserService������ɵ����ݿ����֤
		UsersService userService=new UsersService();
		User user=new User();
		user.setId(Integer.parseInt(id));
		user.setPwd(password);
		if(userService.checkUser(user)){
		
				request.getRequestDispatcher("/WEB-INF/admin/MainFrame.jsp").forward(request, response);
		}else{
				request.setAttribute("err", "�û�id�������д���");
				request.getRequestDispatcher("/WEB-INF/admin/login.jsp").forward(request, response);
		}
	}
		
	
	public void doPost(HttpServletRequest request,HttpServletResponse response)throws ServletException,IOException{
		this.doGet(request, response);
	}

}
