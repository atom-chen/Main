package com.QQ.Client.view1;
//QQ��¼����

import javax.imageio.ImageIO;
import javax.swing.*;

import com.QQ.Client.Helper.QqUserThread;
import com.QQ.Client.model.QQMainViewHashMap;
import com.QQ.Client.model.QqClientLogin;
import com.QQ.Client.model.UserHashMap;
import com.QQ.Common.Message;
import com.QQ.Common.MessageType;
import com.QQ.Common.User;

import java.awt.*;
import java.awt.event.*;
import java.io.File;
import java.io.IOException;
import java.io.ObjectOutputStream;


public class Login extends JFrame implements ActionListener{
	QqClientLogin qcu=null;
	//�����ǩʱ�������ʽ
	Cursor c=null;
	//����
	JLabel Top_Jl=null;
	
	//�в�
	JTabbedPane Mid_jtp=null;//ѡ�����
	JPanel Mid_jp[]=new JPanel[3];
	//��һ��JPanel�����
	JLabel[] jp0_jl=new JLabel[4];
	JTextField jp0_jtf=null;
	JPasswordField jp0_jpass=null;
	JCheckBox jp0_jcb1,jp0_jcb2;
	JButton jp0_jb1=null;
	
	//�ϲ�
	JPanel jp4=null;
	JButton Bot_jb[]=new JButton[3];
	
	public Login()
	{
		qcu=new QqClientLogin();
		c=new Cursor(Cursor.HAND_CURSOR);
		//��ʼ������
		Top_Jl=new JLabel(new ImageIcon("image/tou.gif"));
		
		//��ʼ���в�
		Mid_jtp=new JTabbedPane();
		for(int i=0;i<Mid_jp.length;i++)
		{
			Mid_jp[i]=new JPanel();
		}
		//��ʼ��Mid_jp[0]�����
		Mid_jp[0].setLayout(new GridLayout(3,3,5,10));
		//QQ�����ǩ
		jp0_jl[0]=new JLabel("QQ����",JLabel.CENTER);
		Mid_jp[0].add(jp0_jl[0]);
		//���������
		jp0_jtf=new JTextField(10);
		jp0_jtf.setFocusable(true);//���ú����Ϊ����
		jp0_jtf.setBorder(BorderFactory.createLoweredBevelBorder());//�����°�Ч��
		Mid_jp[0].add(jp0_jtf);
		//������밴ť
		jp0_jb1=new JButton(new ImageIcon("image/clear.gif"));
		Mid_jp[0].add(jp0_jb1);
		//QQ�����ǩ
		jp0_jl[1]=new JLabel("QQ����",JLabel.CENTER);
		Mid_jp[0].add(jp0_jl[1]);
		//�����
		jp0_jpass=new JPasswordField(10);
		jp0_jpass.setBorder(BorderFactory.createLoweredBevelBorder());
		Mid_jp[0].add(jp0_jpass);
		//���������ǩ
		jp0_jl[2]=new JLabel("��������",JLabel.CENTER);
		jp0_jl[2].setCursor(c);//���������ʽ
		jp0_jl[2].setForeground(Color.blue);//����ǰ��ɫ
		Mid_jp[0].add(jp0_jl[2]);
		//�����¼ѡ��
		jp0_jcb1=new JCheckBox("�����¼");
		Mid_jp[0].add(jp0_jcb1);
		//��ס����ѡ��
		jp0_jcb2=new JCheckBox("��ס����");
		Mid_jp[0].add(jp0_jcb2);
		//�������뱣����ǩ
		jp0_jl[3]=new JLabel("�������뱣��",JLabel.CENTER);
		jp0_jl[3].setCursor(c);
		jp0_jl[3].setForeground(Color.blue);
		jp0_jl[3].setFont(new Font("΢���ź�",Font.BOLD,12));
		Mid_jp[0].add(jp0_jl[3]);
		
		//��ӵ�ѡ�����
		Mid_jtp.add(Mid_jp[0],"QQ����");
		Mid_jtp.add(Mid_jp[1],"�ֻ�����");
		Mid_jtp.add(Mid_jp[2],"�����ʼ�");

		
		//��ʼ���ϲ�
		jp4=new JPanel();
		Bot_jb[0]=new JButton(new ImageIcon("image/denglu.gif"));
		//����¼��ťע�����
		Bot_jb[0].addActionListener(this);
		Bot_jb[1]=new JButton(new ImageIcon("image/quxiao.gif"));
		Bot_jb[2]=new JButton(new ImageIcon("image/xiangdao.gif"));
		for(int i=0;i<Bot_jb.length;i++)
		{
			jp4.add(Bot_jb[i]);
		}
		
		
		this.add(Top_Jl,"North");
		this.add(Mid_jtp,"Center");
		this.add(jp4,"South");
		this.setSize(350,250);
		try {
			this.setIconImage(ImageIO.read(new File("image/qq.gif")));
		} catch (IOException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		}
		this.setLocation(Toolkit.getDefaultToolkit().getScreenSize().width/2-350,Toolkit.getDefaultToolkit().getScreenSize().height/2-300);
		this.setVisible(true);
		this.setResizable(false);
		this.setDefaultCloseOperation(JFrame.EXIT_ON_CLOSE);
	}

	public static void main(String[] args) {
		new Login();

	}

	@Override
	public void actionPerformed(ActionEvent e) {
		// TODO Auto-generated method stub
		//�������˵�¼��ť
		if(e.getSource()==Bot_jb[0])
		{
			//��ȡ�˺ź�����
			String useid=this.jp0_jtf.getText();
			String passwd=new String(this.jp0_jpass.getPassword());
			//���û��������봫��ҵ���߼���
			boolean b=qcu.LoginTrueOrFalse(useid, passwd);
			if(b)
			{
				//ת�������棬���û��˺Ŵ���������
				QQ_Main qm=new QQ_Main(this.jp0_jtf.getText());
				//��������浽����
				QQMainViewHashMap.addToQQMainMap(this.jp0_jtf.getText(), qm);
				System.out.println("�ɹ���ӽ��浽����   "+this.jp0_jtf.getText());
				
				//��������������������һ��Ҫ�󷵻����ߺ�����Ϣ�İ�
				//�õ����û����߳�
				QqUserThread qut=UserHashMap.getQqUserThread(this.jp0_jtf.getText());
				//�ø��߳����������Ҫ���
				Message m=new Message();
				m.setMessType(MessageType.Message_get_onLineFriendList);//���߷�������Ҫʲô���͵İ�
				m.setSender(useid);//���߷����� ��Ҫ˭�ĺ��Ѱ�
				qut.WriteToServer(m);
				this.dispose();
			}else {
				JOptionPane.showMessageDialog(this, "�û����������");
			}

			
		}
	}

}
