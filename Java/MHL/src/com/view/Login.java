package com.view;
import java.awt.*;
import com.Toolkit.*;
import com.model.UserModel;

import javax.imageio.ImageIO;
import javax.swing.*;
import java.awt.event.*;
import java.io.*;
//���ܣ���¼����
public class Login extends JDialog implements ActionListener{
	JLabel jl1,jl2,jl3;
	JTextField ac=null;
	JPasswordField passworld=null;
	JButton jb1,jb2;
	public static void main(String []args)
	{
		Login l=new Login();
	}
	
	public Login()
	{
		//���ò���
		this.setLayout(null);

		//���ñ�ǩ
		jl1=new JLabel("�������û�����");
		jl1.setBounds(60, 190, 150, 30);
		jl1.setFont(MyFont.f1);
		this.add(jl1);
		//�����ı���
		ac=new JTextField();
		ac.setBounds(180, 190, 120, 30);
		//���û����ı���������
		ac.setFocusable(true);
		//�ı����°�Ч��
		ac.setBorder(BorderFactory.createLoweredBevelBorder());
		this.add(ac);
		
		jl3=new JLabel("����Ա���ţ�");
		jl3.setBounds(80, 210, 100, 30);
		jl3.setFont(MyFont.f2);
		//����ǰ��ɫ
		jl3.setForeground(Color.red);
		this.add(jl3);
		
		//���������
		jl2=new JLabel("���������룺");
		jl2.setBounds(60, 240, 150, 30);
		jl2.setFont(MyFont.f1);
		this.add(jl2);
		passworld=new JPasswordField();
		passworld.setBounds(180, 240, 120, 30);
		//������°�Ч��
		passworld.setBorder(BorderFactory.createLoweredSoftBevelBorder());
		this.add(passworld);
		
		//���ð�ť
		jb1=new JButton("ȷ��");
		jb1.addActionListener(this);
		//��ȷ��ע�����
		jb1.setBounds(92, 303, 79, 30);
		jb1.setFont(MyFont.f1);
		this.add(jb1);
		jb2=new JButton("ȡ��");
		//��ȡ��ע�����
		jb2.addActionListener(this);
		jb2.setBounds(207, 303, 79, 30);
		jb2.setFont(MyFont.f1);
		this.add(jb2);
		
		Background b=new Background();
		b.setBounds(0, 0, 360, 360);
		this.add(b);
		
		this.setSize(360,360);
		//λ�ã����м�
		this.setLocation(659,322);
		
		//�ر�JDialog�Ĵ���
		this.setUndecorated(true);
		this.setVisible(true);
	}
	//��һ���ڲ��࣬�������廭������ͼ
	class Background extends JPanel{
		Image im=null;
		public Background()
		{
			try {
				im=ImageIO.read(new File("image/login.gif"));
			} catch (IOException e) {
				// TODO Auto-generated catch block
				e.printStackTrace();
			}
		}
		public void paint(Graphics g)
		{
			g.drawImage(im, 0,0,360,360,this);
		}
	}
	@Override
	public void actionPerformed(ActionEvent e) {
		//�������ȷ��
		if(e.getSource()==jb1)
		{
			System.out.println("���ȷ��");
			//��ȡ�ı���������
			String username= ac.getText().trim();
			String password=new String(passworld.getPassword());
			//�����߼��㷽������ȡְ��
			UserModel um=new UserModel();
			String job=um.enter(username, password);
			System.out.println(job);
			//���ְ��Ϊ���� �������¼
			if((job.trim()).equals("����"))
			{
				System.out.println("���뵽�½���");
				//��ת��window1
				new Window1();
				//��������
				JOptionPane.showMessageDialog(null, job+"��ӭ��");
				//�رոô���
				this.dispose();
			}
			else
			{
				JOptionPane.showMessageDialog(null, "��½ʧ�ܣ���ȷ�����Ƿ����㹻��Ȩ��");
			}
		}
		else if(e.getSource()==jb2)
		{
			System.exit(0);
		}
	}

}
