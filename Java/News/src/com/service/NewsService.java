package com.service;

import java.sql.ResultSet;
import java.util.ArrayList;

import com.domain.News;
import com.dao.*;

/*
 * 完成对News类（News表）的各种操作
 */

public class NewsService {
	//编写一个搜索新闻的函数
	/*
	 * @au
	 */
	public ArrayList<News> SearchNews(String keys)
	{
		Sqlhelper sqlhelper=new Sqlhelper();
		ResultSet rs=null;
		ArrayList<News> arrayList=null;
		try {
			String MyConsole[]=keys.split(" ");
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
			arrayList=new ArrayList<News>(); 
			//将rs->ArrayList(News)
			//业务逻辑的二次封装
			while(rs.next())
			{
				News news=new News();
				news.setNo(rs.getInt(1));
				news.setName(rs.getString(2));
				news.setTime(rs.getDate(3));
				arrayList.add(news);
				//此时资源已经被捣腾到arraylist了
			}

		} catch (Exception e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		}finally{
			sqlhelper.close(rs, sqlhelper.getPs(), sqlhelper.getCt());
		}
		return arrayList;
	}
			
}









