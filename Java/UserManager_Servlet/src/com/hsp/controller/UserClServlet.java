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
			//����id
			String id=request.getParameter("id");
		//����UserService���ɾ������
		if(usersService.delUser(id)){
			//����ת��
			request.getRequestDispatcher("/Ok").forward(request, response);
		}else{
			request.getRequestDispatcher("/Err").forward(request, response);
		}
		}else if("gotoUpdView".equals(type)){
			//����id
			String id=request.getParameter("id");
			//ͨ��id���һ��userbean
			User user=usersService.getUserById(id);
			//Ϊ������һ��servlet�����棩ʹ�����ǵ�user�������ǿ��԰Ѹ�user������뵽request������
			request.setAttribute("userinfo", user);
			//����ת��
			request.getRequestDispatcher("/UpdateUserView").forward(request, response);
			
		}else if("update".equals(type)){
			//�����û��µ���Ϣ[����û��ύ�ĸ�ʽ���ԣ���Ҫ��֤]
			String id=request.getParameter("id");
			String username=request.getParameter("username");
			String email=request.getParameter("email");
			String grade=request.getParameter("grade");
			String passwd=request.getParameter("passwd");
			//�ѽ��ܵ�����Ϣ��װ��user
			User user=new User();
			user.setId(Integer.parseInt(id));
			user.setName(username);
			user.setEmail(email);
			user.setGrade(Integer.parseInt(grade));
			user.setPwd(passwd);
			//�޸��û�
			if(usersService.updUser(user)){
				request.getRequestDispatcher("/Ok").forward(request, response);
			}else{
				request.getRequestDispatcher("/Err").forward(request, response);
			}
		}else if("goAddUser".equals(type)){
			//����û��ʲôҪ�����
			request.getRequestDispatcher("/AddUserView").forward(request, response);
		}else if("add".equals(type)){
			//�����û���Ϣ
			String username=request.getParameter("username");
			String email=request.getParameter("email");
			String grade=request.getParameter("grade");
			String passwd=request.getParameter("passwd");
			//����һ��user����
			//�ѽ��ܵ�����Ϣ��װ��user
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
