package com.model;
import java.sql.ResultSet;
import java.sql.SQLException;

//业务逻辑：登录界面的验证 
import javax.swing.*;

import com.databasehelper.SQLHelper;

public class UserModel{
	SQLHelper helper=null;
	//功能：实现权限验证
	//输入：用户名和密码     输出：登陆者的职位
	public String enter(String username,String password)
	{
		//编辑SQL语句
		String sql="Select r.zhiwei from login l,rszl r where l.empid=r.empid and l.empid=? and l.passwd=?";
		//编辑传入SQLHelper的参数组
		String parameter[]={username,password};
		//调用SQLHelper的查询方法并接收结果集
		helper=new SQLHelper();
		ResultSet rs=helper.Select(sql, parameter);
		String job="";
		//接受职务
		try {
			//若存在则给职务赋值，若不存在则为空字符串
			if(rs.next())
			{
				job=rs.getString(1);
			}
		} catch (SQLException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		}
		//返回职务
		return job;
	}

}
