package com.view;
import javax.imageio.ImageIO;
import javax.swing.*;
import javax.swing.Timer;

import java.awt.*;
import java.awt.event.*;
import com.Toolkit.*;

import java.io.*;
import java.util.*;


//����ҳ�棺��Ȩ�޲��ܽ���
public class Window1 extends JFrame implements ActionListener,MouseListener{

	public static void main(String[] args) {
		// TODO Auto-generated method stub
		Window1 w=new Window1();
	}
	JMenuBar jmb=null;
	JMenu []jm1=new JMenu[5];
	JMenuItem []jm1_1=new JMenuItem[5];//ϵͳ�������Ҷ
	JToolBar jtb;
	JButton jb_jtb[]=new JButton[10];//�������İ�ť
	//�м䲿�֣���Ϊ��4��JPanel �ײ�ʱ�䴰����1��JPanel
	JPanel jp1,jp2,jp3,jp4,jp5;
	//jp1:8��ͼ��Ĳ���
	JLabel jp1_jl[]=new JLabel[8];
	//���jp1ͼ��ʱ�������ʽ
	Cursor c_jp1=null;
	//jp2:װ��jp3��jp4(�߽粼��)
	//jp3:��߲���(��Ƭ���֣�
	CardLayout card_jp3=null;
	JLabel jp3_jl1,jp3_jl2; 

	//jp4:�м䱨����(��Ƭ����)
	CardLayout card_jp4=null;
	//jp4�ı���ͼƬ
	Background jp4_b=null;
	//8���ӽ���
	Empinfo jp4_1=null;
	ManaLoginInfo jp4_2=null;
	
	
	//jp1��jp2�ǻ�Ϊ��ִ���Ĺ�ϵ
	JSplitPane jsp=null;
	
	JLabel timenow;//��ʾ��ǰʱ��
	//javax.swing���е�Timer���Զ�ʱ�ش���Action�¼�,���ǿ������������һЩ����
	javax.swing.Timer timer;
	
	//���ܣ���ʼ���˵���
	public void initMenu()
	{
        //����
		jmb=new JMenuBar();
		//��֦1-ϵͳ����
		jm1[0]=new JMenu("ϵͳ����");
		jm1[0].setFont(MyFont.f1);
		//��֦1-ϵͳ����-��Ҷ
		ImageIcon ii1_1=new ImageIcon("image/login_b.jpg");
		jm1_1[0]=new JMenuItem("�л��û�",ii1_1);
		jm1_1[0].setFont(MyFont.f2);
		ImageIcon ii1_2=new ImageIcon("image/hr_b.jpg");
		jm1_1[1]=new JMenuItem("�л����տ�ҳ��",ii1_2);
		jm1_1[1].setFont(MyFont.f2);
		ImageIcon ii1_3=new ImageIcon("image/pc_b.jpg");
		jm1_1[2]=new JMenuItem("��¼����",ii1_3);
		jm1_1[2].setFont(MyFont.f2);
		ImageIcon ii1_4=new ImageIcon("image/wnl.jpg");
		jm1_1[3]=new JMenuItem("������",ii1_4);
		jm1_1[3].setFont(MyFont.f2);
		ImageIcon ii1_5=new ImageIcon("image/info_b.jpg");
		jm1_1[4]=new JMenuItem("�˳�",ii1_5);
		jm1_1[4].setFont(MyFont.f2);
		//��ӵ�ϵͳ����
		for(int i=0;i<jm1_1.length;i++)
		{
			jm1[0].add(jm1_1[i]);
		}
		jm1[1]=new JMenu("���¹���");
		jm1[1].setFont(MyFont.f1);
		jm1[2]=new JMenu("�˵�����");
		jm1[2].setFont(MyFont.f1);
		jm1[3]=new JMenu("����ͳ��");
		jm1[3].setFont(MyFont.f1);
		jm1[4]=new JMenu("�ɱ������");
		jm1[4].setFont(MyFont.f1);
		//�����֦������
		for(int i=0;i<jm1.length;i++)
		{
			jmb.add(jm1[i]);
		}
	}

	//���ܣ���ʼ��������
	public void initToolBar()
	{
		//�������������
		jtb=new JToolBar();
		jb_jtb[0]=new JButton(new ImageIcon("image/ToolBar/hr.png"));
		jb_jtb[1]=new JButton(new ImageIcon("image/ToolBar/login.jpg"));
		jb_jtb[2]=new JButton(new ImageIcon("image/ToolBar/pc.jpg"));
		jb_jtb[3]=new JButton(new ImageIcon("image/ToolBar/earth.jpg"));
		jb_jtb[4]=new JButton(new ImageIcon("image/ToolBar/uDisk.jpg"));
		jb_jtb[5]=new JButton(new ImageIcon("image/ToolBar/fish.jpg"));
		jb_jtb[6]=new JButton(new ImageIcon("image/ToolBar/cuke.jpg"));
		jb_jtb[7]=new JButton(new ImageIcon("image/ToolBar/butterfly.jpg"));
		jb_jtb[8]=new JButton(new ImageIcon("image/ToolBar/robot.jpg"));
		jb_jtb[9]=new JButton(new ImageIcon("image/ToolBar/info.jpg"));
		for(int i=0;i<jb_jtb.length;i++)
		{
			jtb.add(jb_jtb[i]);
		}
		//���ù�����������ȡ����
		jtb.setFloatable(false);
	}
	
	//���ܣ���ʼ���м䲿��
	public void initMid()
	{
		//�����в�
		//������jp1
		jp1=new JPanel(new BorderLayout());
		jp1_jl[0]=new JLabel(new ImageIcon("image/jp1/blue.png"));
		jp1_jl[1]=new JLabel("���µǼ�",new ImageIcon("image/jp1/p1_rsgl.jpg"),0);
		jp1_jl[2]=new JLabel("��¼����",new ImageIcon("image/jp1/p1_dlgl.jpg"),0);
		jp1_jl[3]=new JLabel("���׼۸�",new ImageIcon("image/jp1/p1_cpjg.jpg"),0);
		jp1_jl[4]=new JLabel("����ͳ��",new ImageIcon("image/jp1/p1_bbtj.jpg"),0);
		jp1_jl[5]=new JLabel("�ɱ����ⷿ",new ImageIcon("image/jp1/p1_cb.jpg"),0);
		jp1_jl[6]=new JLabel("ϵͳ����",new ImageIcon("image/jp1/p1_xtsz.jpg"),0);
		jp1_jl[7]=new JLabel("��������",new ImageIcon("image/jp1/p1_dhbz.jpg"),0);
		
		//����һ������ͼƬ
		Background b=null;
		try {
			b=new Background(ImageIO.read(new File("image/jp1/jp1_bg.jpg")));
		} catch (IOException e2) {
			// TODO Auto-generated catch block
			e2.printStackTrace();
		}
		//���ñ���ͼƬΪ���񲼾�
		b.setLayout(new GridLayout(8,1));
		//ע����������ò�����,�������壬���������ʽ������ͼ����ӵ�����ͼƬ
		c_jp1=new Cursor(Cursor.HAND_CURSOR);
		for(int i=0;i<jp1_jl.length;i++)
		{
			jp1_jl[i].addMouseListener(this);
			jp1_jl[i].setCursor(c_jp1);
			if(i!=0)
			{
				jp1_jl[i].setEnabled(false);
				jp1_jl[i].setFont(MyFont.f3);
			}	
			b.add(jp1_jl[i]);
		}
		//�ѱ���ͼƬ��ӵ�jp1
		jp1.add(b);
		
		//�ٴ���jp2,jp3,jp4
		jp2=new JPanel(new BorderLayout());
		card_jp3=new CardLayout();
		card_jp4=new CardLayout();
		//����jp3,jp4Ϊ��Ƭ����
		jp3=new JPanel(card_jp3);
		jp4=new JPanel(card_jp4);
		//jp3:Ӧ����������Ƭ
		jp3_jl1=new JLabel(new ImageIcon("image/p2_left.jpg"));
		jp3_jl2=new JLabel(new ImageIcon("image/p2_right.jpg"));
		jp3.add(jp3_jl1,"0");
		jp3.add(jp3_jl2,"1");
		
		//jp4:Ӧ����8�����
		//�����
		try {
			jp4_b=new Background(ImageIO.read(new File("image/jp1/jp1_bg.jpg")));
		} catch (IOException e2) {
			// TODO Auto-generated catch block
			e2.printStackTrace();
		}
		//��ӱ���ͼƬ��jp4
		jp4.add(jp4_b,"0");
		//���¹������
		jp4_1=new Empinfo();
		jp4.add(jp4_1,"1");
		//��½�������
		jp4_2=new ManaLoginInfo();
		jp4.add(jp4_2,"2");
	
		
		//��ӵ�������jp2
		jp2.add(jp3,"West");
		jp2.add(jp4,"Center");
		//����jp1��jp2�Ĳ�ִ����ϵ(��y�Ữ��)
		jsp=new JSplitPane(JSplitPane.HORIZONTAL_SPLIT,jp1,jp2);
		//����jp1ռ�ݵĿ��
		jsp.setDividerLocation(250);
		//ȡ��y���»���
		jsp.setDividerSize(0);
	}
	
	//���ܣ���ʼ���ײ�״̬��
	public void initTime()
	{
		//����ײ�״̬�� jp5���(Border����)
		jp5=new JPanel(new BorderLayout());
		//�õ���ǰʱ��
		timenow=new JLabel(Calendar.getInstance().getTime().toLocaleString()+"   ");
		timenow.setFont(MyFont.f1);
		//timer����ÿ1000�������һ��Listener��actionPerformed����
		timer=new Timer(1000, this);
		timer.start();//������ʱ��
		//��һ������ͼƬ��Panel
		Image im_timer = null;
		try {
			im_timer = ImageIO.read(new File("image/time_bg.jpg"));
		} catch (IOException e1) {
			// TODO Auto-generated catch block
			e1.printStackTrace();
		}
		Background timerbg=new Background(im_timer);//����ʱ��ҳ�棨JP5���ı���
		timerbg.setLayout(new BorderLayout());
		timerbg.add(timenow,BorderLayout.EAST);//���ʱ�䵽����ͼƬ�Ķ���
		jp5.add(timerbg);//��ӱ���ͼƬ��jp5
	}
	
	//���췽������������������
	public Window1()
	{
		//��ʼ���˵�
		this.initMenu();
		//��ʼ��������
		this.initToolBar();
		//��ʼ���м�
		this.initMid();
		//��ʼ���ײ�
		this.initTime();
		//���������Ԫ��
		this.setJMenuBar(this.jmb);
		//��JFrame�õ�Container��������)
		Container ct=this.getContentPane();
		ct.add(jtb,"North");
		ct.add(jp5,"South");
		ct.add(jsp,"Center");

		
		///���ô�С�Լ�λ��
		this.setSize(Toolkit.getDefaultToolkit().getScreenSize().width, Toolkit.getDefaultToolkit().getScreenSize().height-50);
		this.setLocation(0, 0);
		//���ñ���ͼƬ�Լ��ı�
		Image im=null;
		try {
			im = ImageIO.read(new File("image/cup.gif"));
		} catch (IOException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		}
		this.setIconImage(im);
		this.setTitle("����¥��������ϵͳ");
		this.setVisible(true);
		this.setDefaultCloseOperation(JFrame.EXIT_ON_CLOSE);
	}

	@Override
	public void actionPerformed(ActionEvent e) {
		//ˢ��timenow��ǩ��ʱ��
		this.timenow.setText(java.util.Calendar.getInstance().getTime().toLocaleString()+"  ");
	}

	@Override
	//���������:�л�jp3�Ŀ�Ƭ����
	public void mouseClicked(MouseEvent e) {
		// TODO Auto-generated method stub
		//������������¥ͼ��
		if(e.getSource()==jp1_jl[0])
		{
			System.out.println("�ص�������");
			card_jp4.show(jp4, "0");
		}
		//�����������¹���
		else if(e.getSource()==jp1_jl[1])
		{
			System.out.println("������µǼ�");
			//�л������µǼǿ�Ƭ
			card_jp4.show(jp4, "1");
		}
		//�������˵�½����
		else if(e.getSource()==jp1_jl[2])
		{
			//�л�����½����Ƭ
			card_jp4.show(jp4, "2");
		}
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
	//���������ͣ 
	public void mouseEntered(MouseEvent e) {
		// TODO Auto-generated method stub
		for(int i=1;i<jp1_jl.length;i++)
		{
			//�������ͣʱ������jp1_jl[i]
			if(e.getSource()==jp1_jl[i])
			{
				jp1_jl[i].setEnabled(true);
			}
		}
	}

	@Override
	//��������뿪
	public void mouseExited(MouseEvent e) {
		// TODO Auto-generated method stub
		for(int i=1;i<jp1_jl.length;i++)
		{
			if(e.getSource()==jp1_jl[i])
			{
				jp1_jl[i].setEnabled(false);
			}
		}
	}
}
