package com.shp.view;

import java.io.IOException;
import java.io.PrintWriter;
import java.sql.*;
import java.util.ArrayList;

import javax.servlet.ServletException;
import javax.servlet.http.HttpServlet;
import javax.servlet.http.HttpServletRequest;
import javax.servlet.http.HttpServletResponse;

import com.hsp.domain.User;
import com.hsp.service.UsersService;

public class ManagerUsers extends HttpServlet {
	public void doGet(HttpServletRequest request, HttpServletResponse response)
			throws ServletException, IOException {

		response.setContentType("text/html;charset=utf-8");
		PrintWriter out = response.getWriter();
		out.println("<script type='text/javascript' language='javascript'>");
		out.println("function gotoPageNow(){" +
				"var pageNow=document.getElementById('pageNow').value; " +
				"window.open('/UserManager/ManagerUsers?pageNow='+pageNow,'_self');}");
		out.println("function confirmOper(){ return window.confirm('���Ҫɾ�����û���');}");
		out.println("</script>");
		out.println("<img src='images/1.jpg'/>��ӭ  ��½   <a href='/UserManager/MainFrame'>����������</a>  <a href='/UserManager/LoginServlet'>��ȫ�˳�</a>");
		out.println("<h1>�����û�</h1>");
		
		
		//�����ҳ�ı���
		int pageNow=1;   //��ǰҳ
		//�����û���pageNow
		
		String spageNow=request.getParameter("pageNow");
		if(spageNow!=null)
		pageNow=Integer.parseInt(spageNow);
		
		int pageSize=3;  //ָ��ÿҳ��ʾ3����¼
		int pageCount=0;
		
		
		try{
			UsersService usersService=new UsersService();
			pageCount=usersService.getPageCount(pageSize);
			ArrayList<User> al=usersService.getUsersByPage(pageNow, pageSize);
			out.println("<table border=1px bordercolor=green cellspacing=0 width=500px>");
			out.println("<tr><th>id</th> <th>�û���</th> <th>email</th> <th>����</th><th>ɾ���û�</th><th>�޸��û�</th></tr>");
			//ѭ����ʾ�����û���Ϣ
			for(User u:al){
				out.println("<tr><td>"+u.getId()+"</td>" +
						"<td>"+u.getName()+"</td>" +
								"<td>"+u.getEmail()+"</td>" +
										"<td>"+u.getGrade()+"</td>" +
										"<td><a onClick='return confirmOper();' href='/UserManager/UserClServlet?type=del&id="+u.getId()+"'>ɾ���û�</a> </td>"+
										"<td><a href='/UserManager/UserClServlet?type=gotoUpdView&id="+u.getId()+"'>�޸��û� </a></td>"+
												"</tr>");
			}
			
			out.println("</table><br/>");
			
			//��ʾ��һҳ
			if(pageNow!=1){
			out.println("<a href='/UserManager/ManagerUsers?pageNow="+(pageNow-1)+"'>��һҳ</a>");
			}
			//��ʾ��ҳ
			for(int i=1;i<=pageCount;i++){
				out.println("<a href='/UserManager/ManagerUsers?pageNow="+i+"'><"+i+"></a>");
			}
			//��ʾ��һҳ
			if(pageNow!=pageCount){
			out.println("<a href='/UserManager/ManagerUsers?pageNow="+(pageNow+1)+"'>��һҳ</a>");
			}
			//��ʾ��ҳ����Ϣ
			out.println("&nbsp;&nbsp;&nbsp;��ǰҳ"+pageNow+"/��ҳ��"+pageCount+"<br/>");
			out.println("��ת��:<input type='text' id='pageNow' name='pageNow'/> <input type='button'  onClick='gotoPageNow()' value='��'/>");
			
		}catch(Exception e){
			e.printStackTrace();
		}
		out.println("<hr/><img src='images/2.jpg'>");
	}

	public void doPost(HttpServletRequest request, HttpServletResponse response)
			throws ServletException, IOException {

		
	}

}
