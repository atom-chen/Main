package com.hsp.service;

import java.sql.*;
import java.util.ArrayList;

import com.hsp.dao.SqlHelper;
import com.hsp.domain.User;

public class UsersService {

	//��ȡpagecount
	public int getPageCount(int pageSize){
		String sql="select count(*) from users";
		ResultSet rs=SqlHelper.executeQuery(sql, null);
		int rowCount=0;
		try {
			rs.next();
			rowCount=rs.getInt(1);
		} catch (SQLException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		}finally{
			SqlHelper.close(rs, SqlHelper.getPs(), SqlHelper.getct());
		}
		return (rowCount-1)/pageSize+1;
	}
	
	
	//���շ�ҳ����ȡ�û�
	//��˾ ��resultSet-��user����-��arrayList(����)
	public ArrayList getUsersByPage(int pageNow,int pageSize){
		ArrayList al=new ArrayList<User>();
		//��ѯsql
		String sql="select * from (select t.*,rownum rn from (select * from users order by id) t where rownum<="+pageSize*pageNow+") where rn>="+(pageSize*(pageNow-1)+1);
		ResultSet rs=SqlHelper.executeQuery(sql, null);
		try {
			//���η�װ ��resultSet-��user����-��arrayList(����)
			while (rs.next()) {
				User u = new User();
				u.setId(rs.getInt(1));
				u.setName(rs.getString(2));
				u.setEmail(rs.getString(3));
				u.setGrade(rs.getInt(4));
				//ǧ��Ҫ����u->arraylist
				al.add(u);
			}
		} catch (Exception e) {
			// TODO: handle exception
			e.printStackTrace();
		}finally{
			SqlHelper.close(rs, SqlHelper.getPs(), SqlHelper.getct());
		}
		return al;
	}
	
	//ͨ��id��ȡ�û�����
	public User getUserById(String id){
		User user=new User();
		String sql="select * from users where id=?";
		String[] parameters={id};
		ResultSet rs=SqlHelper.executeQuery(sql, parameters);
		try {
			while(rs.next()){
				//���η�װ
				user.setId(rs.getInt(1));
				user.setName(rs.getString(2));
				user.setEmail(rs.getString(3));
				user.setGrade(rs.getInt(4));
				user.setPwd(rs.getString(5));
			}
		} catch (SQLException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		}finally{
			SqlHelper.close(rs, SqlHelper.getPs(), SqlHelper.getct());
		}
		return user;
	}
	
	//�޸��û�
	public boolean updUser(User user){
		boolean b=true;
		String sql="update users set username=?,email=?,grade=?,passwd=? where id=?";
		String parameters[]={user.getName(),user.getEmail(),user.getGrade()+"",user.getPwd(),user.getId()+""};
		try{
			SqlHelper.executeQuery(sql, parameters);
		}catch(Exception e){
			b=false;
			e.printStackTrace();
		}
		return b;
	}
	
	//����û�
	public boolean addUser(User user){
		boolean b=true;
		String sql="insert into users values(users_seq.nextval,?,?,?,?)";
		String parameters[]={user.getName(),user.getEmail(),user.getGrade()+"",user.getPwd()};
		try{
			SqlHelper.executeUpdate(sql, parameters);
		}catch(Exception e){
			b=false;
			e.printStackTrace();
		}
		return b;
	}
	//ɾ���û�
	public boolean delUser(String id){
		boolean b=true;
		String sql="delete from users where id=?";
		String parameters[]={id};
		try {
			SqlHelper.executeUpdate(sql, parameters);
		} catch (Exception e) {
			b=false;
			// TODO Auto-generated catch block
			e.printStackTrace();
		}
		return b;
	}
	
	//дһ����֤�û��Ƿ�Ϸ��ĺ���
	public boolean checkUser(User user){
		boolean b=false;
		//ʹ��SqlHelper����ɲ�ѯ����
		String sql="select * from users where id=? and passwd=?";
		String[] parameters={user.getId()+"",user.getPwd()};
		ResultSet rs=SqlHelper.executeQuery(sql, parameters);
		try {
			//����rs���жϸ��û��Ƿ����
			if (rs.next()) {
				b=true;
			}
		} catch (Exception e) {
			// TODO: handle exception
			e.printStackTrace();
		}finally{
			SqlHelper.close(rs, SqlHelper.getPs(), SqlHelper.getct());
		}
		return b;
	}
}
