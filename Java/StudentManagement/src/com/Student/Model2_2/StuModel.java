package com.Student.Model2_2;

import javax.swing.table.AbstractTableModel;
import java.sql.*;
import java.util.Vector;
//���ܣ�չ��ѧ����
public class StuModel extends AbstractTableModel {
	    DatabaseHelper helper=null;
	    ResultSet rs=null;
	    //rowData����������
		//columnNames����Ԫ��
		Vector<Vector> rowData=null;
		Vector<String> columnNames=null;
		
		//���ܣ���ɾ��Stu���ݿ⣨�߼��㣩
		//���룺 StuManager(sql��������)���ú���(sql��������)��DatabaseHelper
		//���������DatabaseHelper���صĲ���ɹ���񣬷��ص�StuManager
		public boolean addToData(String sql,String []data)
		{
			helper=new DatabaseHelper();
			//�Ѵ�DatabaseInsertPane��DaatabaseInsertPane�յ������ݴ���DatabaseHelper
			boolean b=true;
			//ִ�в�����ȡ�ý��
			b=helper.update(sql, data);
			return b;
		}
		//��ѯ���ݿ�
		public void select(String sql,String data[])
		{
			
			helper=new DatabaseHelper();
			//��SQL��䴫���ײ㣬�����ܲ�ѯ���RS
			rs=helper.select(sql, data);

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
			} catch (SQLException e) {
				// TODO Auto-generated catch block
				e.printStackTrace();
			}
			helper.close();
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
		return this.columnNames.get(column).toString();
	}
	
	
		
}

