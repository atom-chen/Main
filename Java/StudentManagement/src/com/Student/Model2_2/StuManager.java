package com.Student.Model2_2;
//���ܣ�ʵ�������
import java.sql.*;
import java.util.*;
import javax.swing.*;
import java.awt.*;
import java.awt.event.*;


public class StuManager extends JFrame implements ActionListener{
	Connection ct=null;
	PreparedStatement ps=null;
	ResultSet rs=null;
	public static void main(String[] args) {
		StuManager sm2=new StuManager();
	}
	//��������ϣ�������������õ����ݿ�ģ�ͣ�������ͬһ�����󣬹���Ϊȫ�ֱ���
	StuModel db=null;
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
	public StuManager()
	{
		db=new StuModel();
		String []data={"1"};
		db.select("select * from student where 1=?", data);
		
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
		
//		db=new StuModel("");
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
	//�������ݶ����ص������
	@Override
	public void actionPerformed(ActionEvent e) {
		//��ѯ
		if(e.getSource()==jb1)
		{
			System.out.println("�û���Ҫ��ѯ");
			String name=this.jtf.getText();
			//����SQL���
			String sql="select * from student where stuName=?";	
			//����Ҫ��ѯ��ѧ������
			String data[]={name};
			db=new StuModel();
			//�Ѳ�ѯҪ�󴫵ݸ�ѧ����ģ�ͣ��������µ�ѧ����ģ��
			db.select(sql,data);
			this.jt.setModel(db);
		}
		//���
		else if(e.getSource()==jb[0])
		{
			System.out.println("�û���Ҫ���");
			//�������JDialog
			DatabaseInsertPane du=new DatabaseInsertPane(this,"���ѧ��", true);
			
			//��������ģ��
			System.out.println("����");
			db=new StuModel();
			String[] d1={"1"};
			db.select("select * from student where 1=?", d1);
			this.jt.setModel(db);
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
			//����������ͨ��������޸Ľ���
			DatabaseUpdatePane du=new DatabaseUpdatePane(this, "�޸�",true,db, RowData);
			System.out.println("�޸�");
			//���µ�ǰ��ʾ
			db=new StuModel();
			String[] d1={"1"};
			db.select("select * from student where 1=?", d1);
			this.jt.setModel(db);
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
			//��ȡ��Ԫ��ĵ�һ������
			String stuID=db.getValueAt(num, 0).toString();
			String sql="delete from student where stuID=?";
			String data[]={stuID};

			boolean b=db.addToData(sql, data);
			if(b==false)
			{
				JOptionPane.showMessageDialog(this, "ɾ��ʧ�ܣ���ˢ�£�");
				return;
			}
			//ˢ�����ݿ�
			System.out.println("ɾ���ɹ�");
			db=new StuModel();
			String[] d1={"1"};
			db.select("select * from student where 1=?", d1);
			this.jt.setModel(db);
		}
}
}









