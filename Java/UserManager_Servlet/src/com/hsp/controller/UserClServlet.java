package com.hsp.controller;

import java.io.IOException;
import java.io.PrintWriter;

import javax.servlet.ServletException;
import javax.servlet.http.HttpServlet;
import javax.servlet.http.HttpServletRequest;
import javax.servlet.http.HttpServletResponse;

import com.hsp.domain.User;
import com.hsp.service.UsersService;

public class UserClServlet extends HttpServlet {
	
	public void doGet(HttpServletRequest request, HttpServletResponse response)
			throws ServletException, IOException {

		response.setContentType("text/html;charset=gb2312");
		PrintWriter out = response.getWriter();
		UsersService usersService=new UsersService();
		
		String type=request.getParameter("type");
		if("del".equals(type)){
			//接受id
			String id=request.getParameter("id");
		//调用UserService完成删除任务
		if(usersService.delUser(id)){
			//请求转向
			request.getRequestDispatcher("/Ok").forward(request, response);
		}else{
			request.getRequestDispatcher("/Err").forward(request, response);
		}
		}else if("gotoUpdView".equals(type)){
			//接受id
			String id=request.getParameter("id");
			//通过id获得一个userbean
			User user=usersService.getUserById(id);
			//为了让下一个servlet（界面）使用我们的user对象，我们可以把该user对象放入到request对象中
			request.setAttribute("userinfo", user);
			//请求转发
			request.getRequestDispatcher("/UpdateUserView").forward(request, response);
			
		}else if("update".equals(type)){
			//接受用户新的信息[如果用户提交的格式不对，需要验证]
			String id=request.getParameter("id");
			String username=request.getParameter("username");
			String email=request.getParameter("email");
			String grade=request.getParameter("grade");
			String passwd=request.getParameter("passwd");
			//把接受到的信息封装成user
			User user=new User();
			user.setId(Integer.parseInt(id));
			user.setName(username);
			user.setEmail(email);
			user.setGrade(Integer.parseInt(grade));
			user.setPwd(passwd);
			//修改用户
			if(usersService.updUser(user)){
				request.getRequestDispatcher("/Ok").forward(request, response);
			}else{
				request.getRequestDispatcher("/Err").forward(request, response);
			}
		}else if("goAddUser".equals(type)){
			//这里没有什么要处理的
			request.getRequestDispatcher("/AddUserView").forward(request, response);
		}else if("add".equals(type)){
			//接受用户信息
			String username=request.getParameter("username");
			String email=request.getParameter("email");
			String grade=request.getParameter("grade");
			String passwd=request.getParameter("passwd");
			//构建一个user对象
			//把接受到的信息封装成user
			User user=new User();
			user.setName(username);
			user.setEmail(email);
			user.setGrade(Integer.parseInt(grade));
			user.setPwd(passwd);
			
			if(usersService.addUser(user)){
				request.getRequestDispatcher("/Ok").forward(request, response);
			}else{
				request.getRequestDispatcher("/Err").forward(request, response);
			}
		}
	}

	
	public void doPost(HttpServletRequest request, HttpServletResponse response)
			throws ServletException, IOException {

		this.doGet(request, response);
	}

}
