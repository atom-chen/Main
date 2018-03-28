package com.QQ.Client.viewchildren;
import javax.imageio.ImageIO;
//���ѿ�Ƭ
import javax.swing.*;

import com.QQ.Client.view1.QQ_Main;
import com.QQ.Common.Message;

import java.awt.*;
import java.awt.event.*;

public class Haoyou extends JPanel{
	int count=50;
	JButton jb1,jb2,jb3;
	JScrollPane jsp=null;
	JLabel[] jla=new JLabel[count];
	JPanel jp1,jp2;
	//��������б�
	public Haoyou(QQ_Main qm)
	{
		this.setLayout(new BorderLayout());
		//��ʼ������
		jb1=new JButton("�ҵĺ���");
		this.add(jb1,"North");
		
		//��ʼ���в�
		jp1=new JPanel(new GridLayout(jla.length,1,8,8));
		for(int i=0;i<jla.length;i++)
		{
			//��ʼ������
			jla[i]=new JLabel((i+1)+"",new ImageIcon("image/mm.jpg"), JLabel.LEFT);
			//���ò����ߺ����ǻ�ɫ��
			jla[i].setEnabled(false);
			//������Լ�������ӵ������б�
			if(jla[i].getText().equals(qm.Onwer))
			{
				continue;
			}
			//��ÿ������ע�����
			jla[i].addMouseListener(qm);
			//��Ӻ��ѵ�jp1
			jp1.add(jla[i]);
		}
		jsp=new JScrollPane(jp1);
		this.add(jsp,"Center");
		
		//��ʼ���ϲ�
		jp2=new JPanel(new GridLayout(2,1));
		jb2=new JButton("İ����");
		jb2.addActionListener(qm);
		jb2.setActionCommand("msr");//ί���������л���İ����
		jb3=new JButton("������");
		jb3.addActionListener(qm);
		jb3.setActionCommand("hmd");//ί���������л���������
		jp2.add(jb2);
		jp2.add(jb3);
		this.add(jp2,"South");
	}
	
	//���ܣ��������ߺ�����Ϣ
	public void UpdateFriendsList(Message m)
	{
		//�Կո�  �и���Ϣ����
		String OnLineFriend[]=m.getCon().split(" ");
		//����Ϣ���ݼ������ߺ���
		for(int i=0;i<OnLineFriend.length;i++)
		{
			//����ǵ�1������Ӧ�õ�����0����ǩ���Դ�����
			this.jla[Integer.parseInt(OnLineFriend[i])-1].setEnabled(true);
		}
	}
	

	

}
