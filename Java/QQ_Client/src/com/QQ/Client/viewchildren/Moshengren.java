package com.QQ.Client.viewchildren;
import javax.swing.*;

import com.QQ.Client.view1.QQ_Main;

import java.awt.*;
import java.awt.event.*;

//İ���˿�Ƭ
public class Moshengren extends JPanel implements MouseListener{
	int count=20;
	JButton jb1,jb2,jb3;
	JScrollPane jsp=null;
	JLabel[] jla=new JLabel[count];
	JPanel jp1,jp2;
	//����İ�����б�
	public Moshengren(QQ_Main qm)
	{
		this.setLayout(new BorderLayout());
		//��ʼ������
		jp2=new JPanel(new GridLayout(2,1));
		jb1=new JButton("�ҵĺ���");
		jb1.addActionListener(qm);//ί�������洦��
		jb1.setActionCommand("hy");
		
		jb2=new JButton("İ����");
		jp2.add(jb1);
		jp2.add(jb2);
		this.add(jp2,"North");
		
		//��ʼ���в�
		jp1=new JPanel(new GridLayout(jla.length,1,8,8));
		for(int i=0;i<jla.length;i++)
		{
			//��ʼ��İ����
			jla[i]=new JLabel((i+1)+"",new ImageIcon("image/mm.jpg"), JLabel.LEFT);
			jla[i].addMouseListener(qm);
			//���İ����
			jp1.add(jla[i]);
		}
		jsp=new JScrollPane(jp1);
		this.add(jsp,"Center");
		
		//��ʼ���ϲ�
		jb3=new JButton("������");
		jb3.addActionListener(qm);
		jb3.setActionCommand("hmd");//ί�������洦���л���������
		this.add(jb3,"South");
		this.setSize(300,500);
		this.setVisible(true);
	}
	@Override
	public void mouseClicked(MouseEvent e) {
		// TODO Auto-generated method stub
		
	}
	@Override
	public void mousePressed(MouseEvent e) {
		// TODO Auto-generated method stub
		
	}
	@Override
	public void mouseReleased(MouseEvent e) {
		// TODO Auto-generated method stub
		
	}
	@Override
	//������ı���ɫ
	public void mouseEntered(MouseEvent e) {
		//��ȡ�������JLabel
		JLabel jl=(JLabel) e.getSource();
		//�ı���ɫ
		jl.setForeground(Color.red);
		
	}
	//����뿪�ı���ɫ
	public void mouseExited(MouseEvent e) {
		// TODO Auto-generated method stub
		//��ȡ�������JLabel
		JLabel jl=(JLabel) e.getSource();
		//�ı���ɫ
		jl.setForeground(Color.black);
	}

}
