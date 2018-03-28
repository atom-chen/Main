package com.Student.Model2_2;

import java.awt.*;
import javax.swing.*;
import java.awt.event.*;
import java.util.Vector;
//���ܣ�ʵ�ָ�������
public class DatabaseUpdatePane extends javax.swing.JDialog implements ActionListener{
	JLabel jla[]=new JLabel[6];
	JTextField jtf[]=new JTextField[6];
	JButton jb[]=new JButton[2];
	StuModel dbm=null;

	Vector<Vector> rowData=null;
	Vector<String> columnNames=null;
	
	//�������һ��������壺�����ʱ���ṩ�����ڡ����⡢�Ƿ�ģ̬��һ��ģ�͵����ã�ѡ�е�Ԫ��
	public DatabaseUpdatePane(Frame owner, String title, boolean Modal,StuModel dm,int RowData)
	{
		this.setModal(true);
		dbm=new StuModel();
		jla[0]=new JLabel("ѧ��");
		jtf[0]=new JTextField(10);
		//���ı���ֵ
		jtf[0].setText(dm.getValueAt(RowData, 0).toString());
		//ѧ�Ų����޸�
		jtf[0].setEditable(false);
		
		jla[1]=new JLabel("����");
		jtf[1]=new JTextField(10);
		jtf[1].setText(dm.getValueAt(RowData, 1).toString());
		
		jla[2]=new JLabel("�Ա�");
		jtf[2]=new JTextField(10);
		jtf[2].setText(dm.getValueAt(RowData, 2).toString());
		
		jla[3]=new JLabel("����");
		jtf[3]=new JTextField(10);
		jtf[3].setText(dm.getValueAt(RowData, 3).toString());
		
		jla[4]=new JLabel("����");
		jtf[4]=new JTextField(10);
		jtf[4].setText(dm.getValueAt(RowData, 4).toString());
		
		jla[5]=new JLabel("ϵ��");
		jtf[5]=new JTextField(10);
		jtf[5].setText(dm.getValueAt(RowData, 5).toString());
		
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
			String age=this.jtf[3].getText();//����
			String Jg=this.jtf[4].getText();//����
			String dept=this.jtf[5].getText();//ϵ��
			//����SQL���
			String sql="update student set stuName=?,stuSex=?,stuAge=?,stuJg=?,stuDept=? where stuId=?";

			//���գ�����
			String[] data={name,sex,age,Jg,dept,num};
			dbm=new StuModel();
			//ִ��sql���
			boolean b=dbm.addToData(sql, data);
			if(b==true)
			{
				JOptionPane.showMessageDialog(this, "�޸ĳɹ���");
				this.dispose();
				return;
			}
			
		}else if(e.getSource()==jb[1])
		{
			this.dispose();
		}
		
	}
}
