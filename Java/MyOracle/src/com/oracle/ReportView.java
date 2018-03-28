package com.oracle;
import java.io.*;
import java.sql.ResultSet;

public class ReportView {
	public static void main(String args[])
	{
		ReportView rv=new ReportView();
		rv.Menu();
	}
	BufferedReader br=new BufferedReader(new InputStreamReader(System.in));
	Sqlhelper sqlhelper=new Sqlhelper();
	public void Menu()
	{
		//菜单界面
		while(true)
		{
			String username;
			String password;
			ResultSet  rs=null;
			try {	
				System.out.println("*********欢迎来到新闻管理系统***************");
				System.out.print("请输入用户名：");
				username=br.readLine();
				System.out.print("\t\r请输入密码：");
				password=br.readLine();
				//到数据库验证
				String sql="select * from report_login where username=? and password=?";
				String[] parameter={username,password};
				rs=sqlhelper.executeQuery(sql, parameter);
				if(rs.next())
				{
					select_view();
					//跳转服务界面
					sqlhelper.close(rs, sqlhelper.getPs(), sqlhelper.getCt());
				}else
				{
					System.out.println("您输入的账号名密码有误，请重新输入！");
					sqlhelper.close(rs, sqlhelper.getPs(), sqlhelper.getCt());
				}
			} catch (Exception e) {
				// TODO Auto-generated catch block
				e.printStackTrace();
			}finally{
			}	
		}
	}
	//功能界面
	public void select_view()
	{
		
		while(true)
		{
			System.out.println("**********新闻管理系统***********");
			System.out.println("请选择您要进行的操作");
			System.out.println("search：查询新闻");
			System.out.println("add：添加新闻");
			System.out.println("exit：退出系统");
			try {
				String MyChoose=br.readLine();
				if("search".equals(MyChoose))
				{
					Search();
					//选择了查询新闻
				}else if(("add").equals(MyChoose))
				{
					add();
					//选择了添加新闻
				}else if(("exit").equals(MyChoose))
				{
					//选择了退出系统
					System.exit(0);
				}else{
					System.out.println("-------输入有误，请重新输入---------");
				}
			} catch (IOException e) {
				// TODO Auto-generated catch block
				e.printStackTrace();
			}
		}

	}
	//查询新闻模块
	public void Search()
	{	
		ResultSet rs=null;
		System.out.print("请输入您关心的新闻关键字：");
		try {
			String MyConsole[]=br.readLine().split(" ");
			//到数据库查询
			String sql="select no,name,time from repost where";
			for(int i=0;i<MyConsole.length;i++)
			{
				if(i==0)
				{
					sql+=" (matter like '%"+MyConsole[i]+"%' or name like '%"+MyConsole[i]+"%')";
				}
				else
				{
					sql+=" (and matter like '%"+MyConsole[i]+"%' or name like '%"+MyConsole[i]+"%')";
				}	
			}
			System.out.println(sql);
			//查询
			rs=sqlhelper.executeQuery(sql, null);
			//取出结果
			if(rs.next())
			{
				System.out.print(rs.getString(1)+"   "+rs.getString(2)+"    "+rs.getString(3));
				while(rs.next())
				{
					System.out.print(rs.getString(1)+"   "+rs.getString(2)+"    "+rs.getString(3));
				}
				//请输入编号以查看详细信息
				String ReportNo=br.readLine();
				sql="select name,time,matter from repost where no="+ReportNo;
				rs=sqlhelper.executeQuery(sql, null);
				if(rs.next())
				{
					//取出数据
					System.out.println("*************标题："+rs.getString("name")+"   日期："+rs.getString("time")+"***************");
					System.out.println(rs.getString(3));
				}
			}else{
				System.out.println("没有查到结果.....");
			}
				
			
		} catch (Exception e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		}finally{
			sqlhelper.close(rs, sqlhelper.getPs(), sqlhelper.getCt());
		}
	}
	//查询新闻模块
	public void add()
	{
		System.out.println("**************请输入新闻标题************");
		try {
			String title=br.readLine();
			System.out.println("请输入新闻内容：");
			String neirong=br.readLine();
			//插入到sql
			String sql="insert into repost values(?,?,?,?)";
			String parameter[]={"5",title,"sysdate",neirong};
			sqlhelper.executeUpdate(sql, parameter);
			System.out.println("插入成功！");
		} catch (Exception e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
			System.out.println("插入失败！");
		}finally{
			sqlhelper.close(null, sqlhelper.getPs(), sqlhelper.getCt());
		}
	}
}











