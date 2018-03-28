package com.Student2;
import java.awt.*;
import javax.swing.*;
import java.awt.event.*;
import java.sql.*;
import java.util.*;
//���ܣ�ʵ�ֲ���ʫ��
public class DatabaseInsert extends javax.swing.JDialog implements ActionListener{
	JLabel jla[]=new JLabel[6];
	JTextField jtf[]=new JTextField[6];
	JButton jb[]=new JButton[2];
	
	Connection ct=null;
	PreparedStatement ps=null;

	
	public DatabaseInsert(Frame owner, String title, boolean modal)
	{
		//Ҫ��ʾ��ѯ��������
		jla[0]=new JLabel("ѧ��");
		jtf[0]=new JTextField(10);
		
		jla[1]=new JLabel("����");
		jtf[1]=new JTextField(10);
		
		jla[2]=new JLabel("�Ա�");
		jtf[2]=new JTextField(10);
		
		jla[3]=new JLabel("����");
		jtf[3]=new JTextField(10);
		
		jla[4]=new JLabel("����");
		jtf[4]=new JTextField(10);
		
		jla[5]=new JLabel("ϵ��");
		jtf[5]=new JTextField(10);
		
		jb[0]=new JButton("ȷ��");//����
		jb[0].addActionListener(this);
		jb[1]=new JButton("ȡ��");//����
		jb[1].addActionListener(this);
		
		this.setLayout(new GridLayout(7,2));
		for(int i=0;i<jtf.length;i++)
		{
			this.add(jla[i]);
			this.add(jtf[i]);
		}
		this.add(jb[0]);
		this.add(jb[1]);
		
		this.setSize(300,300);
		this.setLocation(300, 100);
		this.setVisible(true);
		this.setFocusable(false);
		this.setDefaultCloseOperation(JDialog.DO_NOTHING_ON_CLOSE);
	}

	@Override
	public void actionPerformed(ActionEvent e) {
		//��������ȷ��
		if(e.getSource()==jb[0])
		{
			String num=this.jtf[0].getText();//ѧ��
			String name=this.jtf[1].getText();//����
			String sex=this.jtf[2].getText();//�Ա�
			int age=Integer.parseInt(this.jtf[3].getText());//����
			String Jg=this.jtf[4].getText();//����
			String dept=this.jtf[5].getText();//ϵ��
			String sql="insert into student values(?,?,?,?,?,?)";
			System.out.println("�û���Ҫ���");
			//�������ݿ�
			try {
				Class.forName("com.microsoft.sqlserver.jdbc.SQLServerDriver");
				ct=DriverManager.getConnection("jdbc:sqlserver://127.0.0.1:1433;database=StudentManagement","sa","");	
				ps=ct.prepareStatement(sql);
				ps.setString(1, this.jtf[0].getText());
				ps.setString(2, this.jtf[1].getText());
				ps.setString(3, this.jtf[2].getText());
				ps.setInt(4,Integer.parseInt(this.jtf[3].getText()));
				ps.setString(5, this.jtf[4].getText());
				ps.setString(6, this.jtf[5].getText());
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
			this.dispose();
		}else if(e.getSource()==jb[1])
		{
			this.dispose();
		}
		
	}
}
