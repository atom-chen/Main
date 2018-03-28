package com.Student.Model2;
import java.awt.*;
import javax.swing.*;
import java.awt.event.*;
//���ܣ��ṩһ�����봰�ڣ��ṩ�������ģ�ͶԽ�
public class DatabaseInsert extends javax.swing.JDialog implements ActionListener{
	JLabel jla[]=new JLabel[6];
	JTextField jtf[]=new JTextField[6];
	JButton jb[]=new JButton[2];
	DatabaseModel dbm=null;
	
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
			//���ղ��������
			String num=this.jtf[0].getText();//ѧ��
			String name=this.jtf[1].getText();//����
			String sex=this.jtf[2].getText();//�Ա�
			String age=this.jtf[3].getText();//����
			String Jg=this.jtf[4].getText();//����
			String dept=this.jtf[5].getText();//ϵ��
			String sql="insert into student values(?,?,?,?,?,?)";
			//��һ���ַ������飬������Щ����
			String[] wenhao={num,name,sex,age,Jg,dept};
			dbm=new DatabaseModel();
			boolean b=dbm.addToData(sql, wenhao);
			if(b==true)
			{
				JOptionPane.showMessageDialog(this, "����ɹ�");
				this.dispose();
				return;
			}
			else
			{
				JOptionPane.showMessageDialog(this, "����ʧ�ܣ�������������");
			}
		}else if(e.getSource()==jb[1])
		{
			this.dispose();
			return;
		}
		
	}
}
