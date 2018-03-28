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
		//�˵�����
		while(true)
		{
			String username;
			String password;
			ResultSet  rs=null;
			try {	
				System.out.println("*********��ӭ�������Ź���ϵͳ***************");
				System.out.print("�������û�����");
				username=br.readLine();
				System.out.print("\t\r���������룺");
				password=br.readLine();
				//�����ݿ���֤
				String sql="select * from report_login where username=? and password=?";
				String[] parameter={username,password};
				rs=sqlhelper.executeQuery(sql, parameter);
				if(rs.next())
				{
					select_view();
					//��ת�������
					sqlhelper.close(rs, sqlhelper.getPs(), sqlhelper.getCt());
				}else
				{
					System.out.println("��������˺��������������������룡");
					sqlhelper.close(rs, sqlhelper.getPs(), sqlhelper.getCt());
				}
			} catch (Exception e) {
				// TODO Auto-generated catch block
				e.printStackTrace();
			}finally{
			}	
		}
	}
	//���ܽ���
	public void select_view()
	{
		
		while(true)
		{
			System.out.println("**********���Ź���ϵͳ***********");
			System.out.println("��ѡ����Ҫ���еĲ���");
			System.out.println("search����ѯ����");
			System.out.println("add���������");
			System.out.println("exit���˳�ϵͳ");
			try {
				String MyChoose=br.readLine();
				if("search".equals(MyChoose))
				{
					Search();
					//ѡ���˲�ѯ����
				}else if(("add").equals(MyChoose))
				{
					add();
					//ѡ�����������
				}else if(("exit").equals(MyChoose))
				{
					//ѡ�����˳�ϵͳ
					System.exit(0);
				}else{
					System.out.println("-------������������������---------");
				}
			} catch (IOException e) {
				// TODO Auto-generated catch block
				e.printStackTrace();
			}
		}

	}
	//��ѯ����ģ��
	public void Search()
	{	
		ResultSet rs=null;
		System.out.print("�����������ĵ����Źؼ��֣�");
		try {
			String MyConsole[]=br.readLine().split(" ");
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
			//ȡ�����
			if(rs.next())
			{
				System.out.print(rs.getString(1)+"   "+rs.getString(2)+"    "+rs.getString(3));
				while(rs.next())
				{
					System.out.print(rs.getString(1)+"   "+rs.getString(2)+"    "+rs.getString(3));
				}
				//���������Բ鿴��ϸ��Ϣ
				String ReportNo=br.readLine();
				sql="select name,time,matter from repost where no="+ReportNo;
				rs=sqlhelper.executeQuery(sql, null);
				if(rs.next())
				{
					//ȡ������
					System.out.println("*************���⣺"+rs.getString("name")+"   ���ڣ�"+rs.getString("time")+"***************");
					System.out.println(rs.getString(3));
				}
			}else{
				System.out.println("û�в鵽���.....");
			}
				
			
		} catch (Exception e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		}finally{
			sqlhelper.close(rs, sqlhelper.getPs(), sqlhelper.getCt());
		}
	}
	//��ѯ����ģ��
	public void add()
	{
		System.out.println("**************���������ű���************");
		try {
			String title=br.readLine();
			System.out.println("�������������ݣ�");
			String neirong=br.readLine();
			//���뵽sql
			String sql="insert into repost values(?,?,?,?)";
			String parameter[]={"5",title,"sysdate",neirong};
			sqlhelper.executeUpdate(sql, parameter);
			System.out.println("����ɹ���");
		} catch (Exception e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
			System.out.println("����ʧ�ܣ�");
		}finally{
			sqlhelper.close(null, sqlhelper.getPs(), sqlhelper.getCt());
		}
	}
}











