package com.view;
//���ܣ����µǼǱ����
import java.awt.*;
import javax.swing.*;

import com.Toolkit.MyFont;
import com.model.EmpModel;

import java.awt.event.*;

public class Empinfo extends JPanel implements ActionListener{
	//������1*JPanel 1*JLabel 1*JTextField 1*JButton
	//�в���1*JTable 1*JScrollPane
	//�ϲ���3*JPanel 1*JLabel 4*JButton
	JPanel jp1,jp2,jp3,jp4;
	JLabel jla1,jla2;
	JTextField jtf=null;
	JButton jb1,jb2,jb3,jb4,jb5;
	
	JScrollPane jsp=null;
	JTable jta=null;
	EmpModel em=null;
	//�������
	public Empinfo()
	{
		//����
		JPanel jp1=new JPanel();
		jla1=new JLabel("������������Ա���Ż�ְλ����");
		jla1.setFont(MyFont.f3);
		jtf=new JTextField(20);
		jb1=new JButton("ˢ��");
		jb1.addActionListener(this);//����ˢ��
		jb1.setFont(MyFont.f3);
		jp1.add(jla1);
		jp1.add(jtf);
		jp1.add(jb1);
		
		//�в�
		em=new EmpModel();
		em.shuaxin("");
		jta=new JTable(em);
		jsp=new JScrollPane(jta);
		
		
		//�ϲ�
		jp2=new JPanel(new BorderLayout());
		//��ߵļ�¼��
		jp3=new JPanel(new FlowLayout(FlowLayout.LEFT));
		jla2=new JLabel("����xx����¼");
		jla2.setFont(MyFont.f3);
		jp3.add(jla2);
		//�ұߵ�4����ť
		jp4=new JPanel(new FlowLayout(FlowLayout.RIGHT));
		jb2=new JButton("��ϸ��Ϣ");
		jb2.setFont(MyFont.f3);
		jp4.add(jb2);
		jb3=new JButton("���");
		jb3.addActionListener(this);//�������
		jb3.setFont(MyFont.f3);
		jp4.add(jb3);
		jb4=new JButton("�޸�");
		jb4.addActionListener(this);//�����޸�
		jb4.setFont(MyFont.f3);
		jp4.add(jb4);
		jb5=new JButton("ɾ��");
		jb5.addActionListener(this);//����ɾ��
		jb5.setFont(MyFont.f3);
		jp4.add(jb5);
		//��ӵ�jp2
		jp2.add(jp3,"West");
		jp2.add(jp4,"East");
		
		this.setLayout(new BorderLayout());
		this.add(jp1,"North");
		this.add(jsp);
		this.add(jp2,"South");
		this.setVisible(true);
	}
	@Override
	public void actionPerformed(ActionEvent e) {
		// TODO Auto-generated method stub
		//��������ˢ��
		if(e.getSource()==jb1)
		{
			//��ȡ�ı�������
			String name=this.jtf.getText();
			//ִ�в�ѯ
			em=new EmpModel();
			em.shuaxin(name);
			this.jta.setModel(em);
		}
		//�����������
		else if(e.getSource()==jb3)
		{
			System.out.println("���");
			//�������JDialog
			new Rsgl_InsertDialog(this, "����û�", true);
			//ˢ�±�
			em=new EmpModel();
			em.shuaxin("");
			jta.setModel(em);
			
		}
		//���������޸�
		else if(e.getSource()==jb4)
		{
			System.out.println("�޸�");
			//�������JDialog
		}
		//��������ɾ��
		else if(e.getSource()==jb5)
		{
			System.out.println("ɾ��");
			//��ȡ�û�ѡ�����
			int num=this.jta.getSelectedRow();
			//�ҵ���ѡ���˵�id
			String id=(String)this.em.getValueAt(num, 0);
			System.out.println("Ҫɾ����idΪ"+id);
			//ִ��ҵ���߼����ɾ������
			if(em.delete(id))
			{
				JOptionPane.showMessageDialog(null, "ɾ���ɹ���");
			}
			else
			{
				JOptionPane.showMessageDialog(null, "ɾ��ʧ�ܣ�");
			}
			//���±�
			em=new EmpModel();
			em.shuaxin("");
			jta.setModel(em);
			
		}
	}
}










