package com.QQ.Client.viewchildren;
import javax.swing.*;

import com.QQ.Client.view1.QQ_Main;

import java.awt.*;
import java.awt.event.*;
//��������Ƭ

public class Heimingdan extends JPanel{
	int count=10;
	JButton jb1,jb2,jb3;
	JScrollPane jsp=null;
	JLabel[] jla=new JLabel[count];
	JPanel jp1,jp2;
	//��������б�
	public Heimingdan(QQ_Main qm)
	{
		this.setLayout(new BorderLayout());
		//��ʼ������
		jp2=new JPanel(new GridLayout(3,1));
		jb1=new JButton("�ҵĺ���");
		jb1.addActionListener(qm);
		jb1.setActionCommand("hy");//ί���������л�������
		
		jb2=new JButton("İ����");
		jb2.addActionListener(qm);
		jb2.setActionCommand("msr");//ί���������л���İ����
		
		jb3=new JButton("������");
		jp2.add(jb1);
		jp2.add(jb2);
		jp2.add(jb3);
		this.add(jp2,"North");
		
		//��ʼ���в�
		jp1=new JPanel(new GridLayout(jla.length,1,8,8));
		for(int i=0;i<jla.length;i++)
		{
			//��ʼ������
			jla[i]=new JLabel((i+1)+"",new ImageIcon("image/mm.jpg"), JLabel.LEFT);
			jla[i].addMouseListener(qm);
			//��Ӻ��ѵ�jp1
			jp1.add(jla[i]);
		}
		jsp=new JScrollPane(jp1);
		this.add(jsp,"Center");
	}

}
