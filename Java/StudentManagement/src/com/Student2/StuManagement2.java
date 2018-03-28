package com.Student2;
//���ܣ�ʵ�ֲ������ݿ�ķ�װ
import java.sql.*;
import java.util.*;
import javax.swing.*;
import java.awt.*;
import java.awt.event.*;


public class StuManagement2 extends JFrame implements ActionListener{
	Connection ct=null;
	PreparedStatement ps=null;
	ResultSet rs=null;
	public static void main(String[] args) {
		StuManagement2 sm2=new StuManagement2();
	}
	//��������ϣ�������������õ����ݿ�ģ�ͣ�������ͬһ�����󣬹���Ϊȫ�ֱ���
	DatabaseModel db=null;
	//������������border����
	//����
	JLabel jla1=null;
	JTextField jtf=null;
	JButton jb1=null;
	JPanel jp1=null;
	
	//�ϲ�
	JButton jb[]=new JButton[3];
	JPanel jp2=null;
	
	//�в�
	JTable jt=null;
	JScrollPane jsp=null;
	
	//��ʼ�������
	public StuManagement2()
	{
		jla1=new JLabel("����������");
		jtf=new JTextField(10);
		jb1=new JButton("��ѯ");//������
		jb1.addActionListener(this);
		
		jp1=new JPanel();
		jp1.add(jla1);
		jp1.add(jtf);
		jp1.add(jb1);
		
		jb[0]=new JButton("���");//������
		jb[0].addActionListener(this);
		jb[1]=new JButton("�޸�");//������
		jb[1].addActionListener(this);
		jb[2]=new JButton("ɾ��");//������
		jb[2].addActionListener(this);
		jp2=new JPanel();
		for(int i=0;i<jb.length;i++)
		{
			jp2.add(jb[i]);
		}
		
		db=new DatabaseModel("");
		jt=new JTable(db);
		jsp=new JScrollPane(jt);
		
		this.add(jp1,BorderLayout.NORTH);
		this.add(jsp);
		this.add(jp2,BorderLayout.SOUTH);
		
		this.setSize(500,500);
		
		this.setFocusable(false);
		this.setVisible(true);
		this.setDefaultCloseOperation(JFrame.EXIT_ON_CLOSE);
	}

	@Override
	public void actionPerformed(ActionEvent e) {
		//��ѯ
		if(e.getSource()==jb1)
		{
			System.out.println("�û���Ҫ��ѯ");
			String name=this.jtf.getText();
			String sql="select * from student where stuName='"+name+"'";
			
			db=new DatabaseModel(sql);
			jt.setModel(db);
		}
		//���
		else if(e.getSource()==jb[0])
		{
			System.out.println("�û���Ҫ���");
			DatabaseInsert du=new DatabaseInsert(this,"���ѧ��", true);
			//ˢ�µ�ǰ��ʾ
			db=new DatabaseModel();
			jt.setModel(db);
		}
		//�޸�
		else if(e.getSource()==jb[1])
		{
			System.out.println("�û���Ҫ�޸�");
			//���޸��ഫ����
			//��ȡ��ǰѡ����
			int RowData=this.jt.getSelectedRow();
			if(RowData==-1)
			{
				JOptionPane.showMessageDialog(this, "��ѡ��һ��");
				return;
			}
			DataUpdate du=new DataUpdate(this, "�޸�",true,db, RowData);
			//���µ�ǰ��ʾ
			db=new DatabaseModel();
			jt.setModel(db);
		}
		//ɾ��
		else if(e.getSource()==jb[2])
		{
			//���Ҫɾ���е�����
			int num=this.jt.getSelectedRow();
			if(num==-1)
			{
				//������ʾ��Ϣ
				JOptionPane.showMessageDialog(this, "��ѡ��һ��");
				return;
			}
			//��ȡ�������ĵ�һ������
			String stuID=db.getValueAt(num, 0).toString();
			//����������ɾ�����������������ֱ���ڴ˴�ɾ��
			try {
				Class.forName("com.microsoft.sqlserver.jdbc.SQLServerDriver");
				ct=DriverManager.getConnection("jdbc:sqlserver://127.0.0.1:1433;database=StudentManagement","sa","");
				ps=ct.prepareStatement("delete from student where stuID=?");
				ps.setString(1, stuID);
				int i=ps.executeUpdate();
				if(i==1)
				{
					System.out.println("�ɹ�");
				}else if(i==0)
				{
					System.out.println("ʧ��");
				}
			} catch (Exception e1) {
				// TODO Auto-generated catch block
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
			//ˢ�����ݿ�
			db=new DatabaseModel();
			this.jt.setModel(db);
		}

}
}
