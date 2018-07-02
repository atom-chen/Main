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
		//接受用户提交的用户名和密码
		String id=request.getParameter("id");
		String password=request.getParameter("password");
		
		//创建UserService对象，完成到数据库的验证
		UsersService userService=new UsersService();
		User user=new User();
		user.setId(Integer.parseInt(id));
		user.setPwd(password);
		if(userService.checkUser(user)){
		
				request.getRequestDispatcher("/WEB-INF/admin/MainFrame.jsp").forward(request, response);
		}else{
				request.setAttribute("err", "用户id或密码有错误！");
				request.getRequestDispatcher("/WEB-INF/admin/login.jsp").forward(request, response);
		}
	}
		
	
	public void doPost(HttpServletRequest request,HttpServletResponse response)throws ServletException,IOException{
		this.doGet(request, response);
	}

}
