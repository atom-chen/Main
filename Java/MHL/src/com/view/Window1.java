package com.view;
import javax.imageio.ImageIO;
import javax.swing.*;
import javax.swing.Timer;

import java.awt.*;
import java.awt.event.*;
import com.Toolkit.*;

import java.io.*;
import java.util.*;


//操作页面：有权限才能进入
public class Window1 extends JFrame implements ActionListener,MouseListener{

	public static void main(String[] args) {
		// TODO Auto-generated method stub
		Window1 w=new Window1();
	}
	JMenuBar jmb=null;
	JMenu []jm1=new JMenu[5];
	JMenuItem []jm1_1=new JMenuItem[5];//系统管理的树叶
	JToolBar jtb;
	JButton jb_jtb[]=new JButton[10];//工具条的按钮
	//中间部分：认为是4个JPanel 底部时间窗口是1个JPanel
	JPanel jp1,jp2,jp3,jp4,jp5;
	//jp1:8个图标的部分
	JLabel jp1_jl[]=new JLabel[8];
	//点击jp1图标时的鼠标样式
	Cursor c_jp1=null;
	//jp2:装着jp3和jp4(边界布局)
	//jp3:左边部分(卡片布局）
	CardLayout card_jp3=null;
	JLabel jp3_jl1,jp3_jl2; 

	//jp4:中间报表部分(卡片布局)
	CardLayout card_jp4=null;
	//jp4的背景图片
	Background jp4_b=null;
	//8个子界面
	Empinfo jp4_1=null;
	ManaLoginInfo jp4_2=null;
	
	
	//jp1和jp2是互为拆分窗格的关系
	JSplitPane jsp=null;
	
	JLabel timenow;//显示当前时间
	//javax.swing包中的Timer可以定时地触发Action事件,我们可以利用它完成一些事情
	javax.swing.Timer timer;
	
	//功能：初始化菜单栏
	public void initMenu()
	{
        //树干
		jmb=new JMenuBar();
		//树枝1-系统管理
		jm1[0]=new JMenu("系统管理");
		jm1[0].setFont(MyFont.f1);
		//树枝1-系统管理-树叶
		ImageIcon ii1_1=new ImageIcon("image/login_b.jpg");
		jm1_1[0]=new JMenuItem("切换用户",ii1_1);
		jm1_1[0].setFont(MyFont.f2);
		ImageIcon ii1_2=new ImageIcon("image/hr_b.jpg");
		jm1_1[1]=new JMenuItem("切换到收款页面",ii1_2);
		jm1_1[1].setFont(MyFont.f2);
		ImageIcon ii1_3=new ImageIcon("image/pc_b.jpg");
		jm1_1[2]=new JMenuItem("登录管理",ii1_3);
		jm1_1[2].setFont(MyFont.f2);
		ImageIcon ii1_4=new ImageIcon("image/wnl.jpg");
		jm1_1[3]=new JMenuItem("万年历",ii1_4);
		jm1_1[3].setFont(MyFont.f2);
		ImageIcon ii1_5=new ImageIcon("image/info_b.jpg");
		jm1_1[4]=new JMenuItem("退出",ii1_5);
		jm1_1[4].setFont(MyFont.f2);
		//添加到系统管理
		for(int i=0;i<jm1_1.length;i++)
		{
			jm1[0].add(jm1_1[i]);
		}
		jm1[1]=new JMenu("人事管理");
		jm1[1].setFont(MyFont.f1);
		jm1[2]=new JMenu("菜单服务");
		jm1[2].setFont(MyFont.f1);
		jm1[3]=new JMenu("报表统计");
		jm1[3].setFont(MyFont.f1);
		jm1[4]=new JMenu("成本及库存");
		jm1[4].setFont(MyFont.f1);
		//添加树枝到树干
		for(int i=0;i<jm1.length;i++)
		{
			jmb.add(jm1[i]);
		}
	}

	//功能：初始化工具栏
	public void initToolBar()
	{
		//处理工具栏的组件
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
		//设置工具栏不可以取出来
		jtb.setFloatable(false);
	}
	
	//功能：初始化中间部分
	public void initMid()
	{
		//处理中部
		//首先是jp1
		jp1=new JPanel(new BorderLayout());
		jp1_jl[0]=new JLabel(new ImageIcon("image/jp1/blue.png"));
		jp1_jl[1]=new JLabel("人事登记",new ImageIcon("image/jp1/p1_rsgl.jpg"),0);
		jp1_jl[2]=new JLabel("登录管理",new ImageIcon("image/jp1/p1_dlgl.jpg"),0);
		jp1_jl[3]=new JLabel("菜谱价格",new ImageIcon("image/jp1/p1_cpjg.jpg"),0);
		jp1_jl[4]=new JLabel("报表统计",new ImageIcon("image/jp1/p1_bbtj.jpg"),0);
		jp1_jl[5]=new JLabel("成本及库房",new ImageIcon("image/jp1/p1_cb.jpg"),0);
		jp1_jl[6]=new JLabel("系统设置",new ImageIcon("image/jp1/p1_xtsz.jpg"),0);
		jp1_jl[7]=new JLabel("动画帮助",new ImageIcon("image/jp1/p1_dhbz.jpg"),0);
		
		//构造一个背景图片
		Background b=null;
		try {
			b=new Background(ImageIO.read(new File("image/jp1/jp1_bg.jpg")));
		} catch (IOException e2) {
			// TODO Auto-generated catch block
			e2.printStackTrace();
		}
		//设置背景图片为网格布局
		b.setLayout(new GridLayout(8,1));
		//注册监听，设置不可用,设置字体，设置鼠标样式。并把图标添加到背景图片
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
		//把背景图片添加到jp1
		jp1.add(b);
		
		//再处理jp2,jp3,jp4
		jp2=new JPanel(new BorderLayout());
		card_jp3=new CardLayout();
		card_jp4=new CardLayout();
		//设置jp3,jp4为卡片布局
		jp3=new JPanel(card_jp3);
		jp4=new JPanel(card_jp4);
		//jp3:应该有两个卡片
		jp3_jl1=new JLabel(new ImageIcon("image/p2_left.jpg"));
		jp3_jl2=new JLabel(new ImageIcon("image/p2_right.jpg"));
		jp3.add(jp3_jl1,"0");
		jp3.add(jp3_jl2,"1");
		
		//jp4:应该有8个面板
		//主面板
		try {
			jp4_b=new Background(ImageIO.read(new File("image/jp1/jp1_bg.jpg")));
		} catch (IOException e2) {
			// TODO Auto-generated catch block
			e2.printStackTrace();
		}
		//添加背景图片到jp4
		jp4.add(jp4_b,"0");
		//人事管理界面
		jp4_1=new Empinfo();
		jp4.add(jp4_1,"1");
		//登陆管理界面
		jp4_2=new ManaLoginInfo();
		jp4.add(jp4_2,"2");
	
		
		//添加到大的面板jp2
		jp2.add(jp3,"West");
		jp2.add(jp4,"Center");
		//处理jp1和jp2的拆分窗格关系(按y轴划分)
		jsp=new JSplitPane(JSplitPane.HORIZONTAL_SPLIT,jp1,jp2);
		//设置jp1占据的宽度
		jsp.setDividerLocation(250);
		//取消y轴下划线
		jsp.setDividerSize(0);
	}
	
	//功能：初始化底部状态栏
	public void initTime()
	{
		//处理底部状态栏 jp5面板(Border布局)
		jp5=new JPanel(new BorderLayout());
		//拿到当前时间
		timenow=new JLabel(Calendar.getInstance().getTime().toLocaleString()+"   ");
		timenow.setFont(MyFont.f1);
		//timer对象：每1000毫秒调用一次Listener的actionPerformed方法
		timer=new Timer(1000, this);
		timer.start();//开启计时器
		//做一个背景图片的Panel
		Image im_timer = null;
		try {
			im_timer = ImageIO.read(new File("image/time_bg.jpg"));
		} catch (IOException e1) {
			// TODO Auto-generated catch block
			e1.printStackTrace();
		}
		Background timerbg=new Background(im_timer);//创建时间页面（JP5）的背景
		timerbg.setLayout(new BorderLayout());
		timerbg.add(timenow,BorderLayout.EAST);//添加时间到背景图片的东面
		jp5.add(timerbg);//添加背景图片到jp5
	}
	
	//构造方法：构造主窗体属性
	public Window1()
	{
		//初始化菜单
		this.initMenu();
		//初始化工具栏
		this.initToolBar();
		//初始化中间
		this.initMid();
		//初始化底部
		this.initTime();
		//添加主窗口元素
		this.setJMenuBar(this.jmb);
		//从JFrame拿到Container（更好用)
		Container ct=this.getContentPane();
		ct.add(jtb,"North");
		ct.add(jp5,"South");
		ct.add(jsp,"Center");

		
		///设置大小以及位置
		this.setSize(Toolkit.getDefaultToolkit().getScreenSize().width, Toolkit.getDefaultToolkit().getScreenSize().height-50);
		this.setLocation(0, 0);
		//设置标题图片以及文本
		Image im=null;
		try {
			im = ImageIO.read(new File("image/cup.gif"));
		} catch (IOException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		}
		this.setIconImage(im);
		this.setTitle("满汉楼餐饮管理系统");
		this.setVisible(true);
		this.setDefaultCloseOperation(JFrame.EXIT_ON_CLOSE);
	}

	@Override
	public void actionPerformed(ActionEvent e) {
		//刷新timenow标签的时间
		this.timenow.setText(java.util.Calendar.getInstance().getTime().toLocaleString()+"  ");
	}

	@Override
	//处理鼠标点击:切换jp3的卡片布局
	public void mouseClicked(MouseEvent e) {
		// TODO Auto-generated method stub
		//如果点击了满汉楼图标
		if(e.getSource()==jp1_jl[0])
		{
			System.out.println("回到主界面");
			card_jp4.show(jp4, "0");
		}
		//如果点击了人事管理
		else if(e.getSource()==jp1_jl[1])
		{
			System.out.println("点击人事登记");
			//切换到人事登记卡片
			card_jp4.show(jp4, "1");
		}
		//如果点击了登陆管理
		else if(e.getSource()==jp1_jl[2])
		{
			//切换到登陆管理卡片
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
	//处理鼠标悬停 
	public void mouseEntered(MouseEvent e) {
		// TODO Auto-generated method stub
		for(int i=1;i<jp1_jl.length;i++)
		{
			//当鼠标悬停时，激活jp1_jl[i]
			if(e.getSource()==jp1_jl[i])
			{
				jp1_jl[i].setEnabled(true);
			}
		}
	}

	@Override
	//处理鼠标离开
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
