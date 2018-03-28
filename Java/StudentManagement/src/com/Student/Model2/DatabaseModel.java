package com.Student.Model2;

import javax.swing.table.AbstractTableModel;
import java.sql.*;
import java.util.Vector;
//������ִ�ж����ݿ�����в���
public class DatabaseModel extends AbstractTableModel {
	    //rowData����������
		//columnNames����Ԫ��
		Vector<Vector> rowData=null;
		Vector<String> columnNames=null;
		
		Connection ct=null;
		PreparedStatement ps=null;
		ResultSet rs=null;
		
		//��ɾ�ķ����������Ľӿ�Ϊsql����һ���ַ������飨������
		public boolean addToData(String sql,String []data)
		{
			boolean b=true;
			//�������ݿ�
			try {
				Class.forName("com.microsoft.sqlserver.jdbc.SQLServerDriver");
				ct=DriverManager.getConnection("jdbc:sqlserver://127.0.0.1:1433;database=StudentManagement","sa","");	
				ps=ct.prepareStatement(sql);
				//������ֵ
				for(int i=0;i<data.length;i++)
				{
					//��ֵ
					ps.setString(i+1, data[i]);
				}		
				//ִ�л����
				int i=ps.executeUpdate();
				if(i==1)
				{
					System.out.println("�ɹ�");
				}else if(i==0)
				{
					b=false;
					System.out.println("ʧ��");
				}
			} catch (Exception e1) {
				// TODO Auto-generated catch block
				b=false;
				e1.printStackTrace();
			}finally{
				try {
					ps.close();
				} catch (SQLException e1) {
					// TODO Auto-generated catch block
					e1.printStackTrace();
				}
				try {
					ct.close();
				} catch (SQLException e1) {
					// TODO Auto-generated catch block
					e1.printStackTrace();
				}
			}
			return b;
		}
		//��ѯ����
		public void init(String sql)
		{
			//���Ϊ�գ���Ĭ��Ϊ��ʾ����
			if(sql.equals(""))
			{
				sql="select * from student";
			}
			//�����Ϊ�� ˵��Ҫִ�в�ѯ
	    	columnNames=new Vector<String>();
			//�������
			columnNames.add("ѧ��");
			columnNames.add("����");
			columnNames.add("�Ա�");
			columnNames.add("����");
			columnNames.add("����");
			columnNames.add("ϵ��");
			//������Ե�����
			rowData=new Vector<Vector>();
			//���Ԫ��
			try {
				Class.forName("com.microsoft.sqlserver.jdbc.SQLServerDriver");
				ct=DriverManager.getConnection("jdbc:sqlserver://127.0.0.1:1433;database=StudentManagement","sa","zhanmengao");
				
				ps=ct.prepareStatement(sql);
				
				rs=ps.executeQuery();
				while(rs.next())
				{
					Vector<Comparable> person=new Vector<Comparable>();
					person.add(rs.getString(1));
					person.add(rs.getString(2));
					person.add(rs.getString(3));
					person.add(rs.getInt(4));
					person.add(rs.getString(5));
					person.add(rs.getString(6));
					//��һ���˵���Ϣ��ӵ�Ԫ��
					rowData.add(person);
				}				
			} catch (Exception e) {
				e.printStackTrace();
			}finally{
				try {
					rs.close();
				} catch (SQLException e) {
					e.printStackTrace();
				}
				try {
					ps.close();
				} catch (SQLException e) {
					e.printStackTrace();
				}
				try {
					ct.close();
				} catch (SQLException e) {
					e.printStackTrace();
				}
			}
		}
	    DatabaseModel(String sql)
		{
	    	this.init(sql);
		}
	    DatabaseModel()
		{
	    	this.init("");
		}

	//��ȡ����
	public int getRowCount() {
		// TODO Auto-generated method stub
		return this.rowData.size();
	}

	//��ȡ����
	public int getColumnCount() {
		// TODO Auto-generated method stub
		return this.columnNames.size();
	}

	//��ȡ���꣨�У��У���Ӧ����ֵ��
	public Object getValueAt(int rowIndex, int columnIndex) {
		// TODO Auto-generated method stub
		return this.rowData.get(rowIndex).get(columnIndex);
	}

	@Override
	//����������
	public String getColumnName(int column) {
		return this.columnNames.get(column);
	}
	
	

}
