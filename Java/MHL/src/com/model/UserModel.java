package com.model;
import java.sql.ResultSet;
import java.sql.SQLException;

//ҵ���߼�����¼�������֤ 
import javax.swing.*;

import com.databasehelper.SQLHelper;

public class UserModel{
	SQLHelper helper=null;
	//���ܣ�ʵ��Ȩ����֤
	//���룺�û���������     �������½�ߵ�ְλ
	public String enter(String username,String password)
	{
		//�༭SQL���
		String sql="Select r.zhiwei from login l,rszl r where l.empid=r.empid and l.empid=? and l.passwd=?";
		//�༭����SQLHelper�Ĳ�����
		String parameter[]={username,password};
		//����SQLHelper�Ĳ�ѯ���������ս����
		helper=new SQLHelper();
		ResultSet rs=helper.Select(sql, parameter);
		String job="";
		//����ְ��
		try {
			//���������ְ��ֵ������������Ϊ���ַ���
			if(rs.next())
			{
				job=rs.getString(1);
			}
		} catch (SQLException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		}
		//����ְ��
		return job;
	}

}
