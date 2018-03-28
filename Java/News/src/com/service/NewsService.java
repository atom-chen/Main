package com.service;

import java.sql.ResultSet;
import java.util.ArrayList;

import com.domain.News;
import com.dao.*;

/*
 * ��ɶ�News�ࣨNews���ĸ��ֲ���
 */

public class NewsService {
	//��дһ���������ŵĺ���
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
			//�����ݿ��ѯ
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
			//��ѯ
			rs=sqlhelper.executeQuery(sql, null);
			arrayList=new ArrayList<News>(); 
			//��rs->ArrayList(News)
			//ҵ���߼��Ķ��η�װ
			while(rs.next())
			{
				News news=new News();
				news.setNo(rs.getInt(1));
				news.setName(rs.getString(2));
				news.setTime(rs.getDate(3));
				arrayList.add(news);
				//��ʱ��Դ�Ѿ������ڵ�arraylist��
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









