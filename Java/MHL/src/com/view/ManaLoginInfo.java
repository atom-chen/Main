package com.view;
import java.awt.*;
import javax.swing.*;
import javax.swing.border.Border;

import com.Toolkit.MyFont;

/*
 * ���¹�����ҳ���view����
 */
public class ManaLoginInfo extends JPanel{
	//����
	JPanel jp1;
	JLabel jp1_jl;
	JTextField jp1_jtf;
	JButton jp1_jb;
	//�в�
	JTable jta;
	JScrollPane jsp;
	//�ϲ�
	JPanel jp2,jp3,jp4;
	JLabel jp3_jl;
	JButton jp4_jb1,jp4_jb2,jp4_jb3;
	//�������
	public ManaLoginInfo()
	{
		//��ʼ������
		jp1=new JPanel();
		jp1_jl=new JLabel("������Ա����");
		jp1.add(jp1_jl);
		jp1_jtf=new JTextField(10);
		jp1.add(jp1_jtf);
		jp1_jb=new JButton("����");
		jp1.add(jp1_jb);
		
		//��ʼ���в�
		jta=new JTable();
		jsp=new JScrollPane(jta);
		
		//��ʼ���ϲ�
		jp2=new JPanel(new BorderLayout());
		jp3=new JPanel();
		jp3_jl=new JLabel("����x����¼");
		jp3_jl.setFont(MyFont.f1);
		jp3.add(jp3_jl);
		jp2.add(jp3,BorderLayout.WEST);
		jp4=new JPanel(new FlowLayout(FlowLayout.TRAILING));
		jp4_jb1=new JButton("ˢ�¼�¼");
		jp4_jb1.setFont(MyFont.f3);
		jp4.add(jp4_jb1);
		jp4_jb2=new JButton("�޸�����");
		jp4_jb2.setFont(MyFont.f3);
		jp4.add(jp4_jb2);
		jp2.add(jp4,BorderLayout.EAST);
		
		this.setLayout(new BorderLayout());
		this.add(jp1,BorderLayout.NORTH);
		this.add(jsp,BorderLayout.CENTER);
		this.add(jp2,BorderLayout.SOUTH);
		this.setVisible(true);
		
	}

	
}







