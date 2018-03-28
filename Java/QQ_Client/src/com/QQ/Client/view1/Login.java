package com.QQ.Client.view1;
//QQ登录界面

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
	//点击标签时的鼠标样式
	Cursor c=null;
	//北部
	JLabel Top_Jl=null;
	
	//中部
	JTabbedPane Mid_jtp=null;//选项卡窗格
	JPanel Mid_jp[]=new JPanel[3];
	//第一个JPanel的组件
	JLabel[] jp0_jl=new JLabel[4];
	JTextField jp0_jtf=null;
	JPasswordField jp0_jpass=null;
	JCheckBox jp0_jcb1,jp0_jcb2;
	JButton jp0_jb1=null;
	
	//南部
	JPanel jp4=null;
	JButton Bot_jb[]=new JButton[3];
	
	public Login()
	{
		qcu=new QqClientLogin();
		c=new Cursor(Cursor.HAND_CURSOR);
		//初始化北部
		Top_Jl=new JLabel(new ImageIcon("image/tou.gif"));
		
		//初始化中部
		Mid_jtp=new JTabbedPane();
		for(int i=0;i<Mid_jp.length;i++)
		{
			Mid_jp[i]=new JPanel();
		}
		//初始化Mid_jp[0]的组件
		Mid_jp[0].setLayout(new GridLayout(3,3,5,10));
		//QQ号码标签
		jp0_jl[0]=new JLabel("QQ号码",JLabel.CENTER);
		Mid_jp[0].add(jp0_jl[0]);
		//号码输入框
		jp0_jtf=new JTextField(10);
		jp0_jtf.setFocusable(true);//设置号码框为焦点
		jp0_jtf.setBorder(BorderFactory.createLoweredBevelBorder());//设置下凹效果
		Mid_jp[0].add(jp0_jtf);
		//清除号码按钮
		jp0_jb1=new JButton(new ImageIcon("image/clear.gif"));
		Mid_jp[0].add(jp0_jb1);
		//QQ密码标签
		jp0_jl[1]=new JLabel("QQ密码",JLabel.CENTER);
		Mid_jp[0].add(jp0_jl[1]);
		//密码框
		jp0_jpass=new JPasswordField(10);
		jp0_jpass.setBorder(BorderFactory.createLoweredBevelBorder());
		Mid_jp[0].add(jp0_jpass);
		//忘记密码标签
		jp0_jl[2]=new JLabel("忘记密码",JLabel.CENTER);
		jp0_jl[2].setCursor(c);//设置鼠标样式
		jp0_jl[2].setForeground(Color.blue);//设置前景色
		Mid_jp[0].add(jp0_jl[2]);
		//隐身登录选项
		jp0_jcb1=new JCheckBox("隐身登录");
		Mid_jp[0].add(jp0_jcb1);
		//记住密码选项
		jp0_jcb2=new JCheckBox("记住密码");
		Mid_jp[0].add(jp0_jcb2);
		//申请密码保护标签
		jp0_jl[3]=new JLabel("申请密码保护",JLabel.CENTER);
		jp0_jl[3].setCursor(c);
		jp0_jl[3].setForeground(Color.blue);
		jp0_jl[3].setFont(new Font("微软雅黑",Font.BOLD,12));
		Mid_jp[0].add(jp0_jl[3]);
		
		//添加到选项卡窗格
		Mid_jtp.add(Mid_jp[0],"QQ号码");
		Mid_jtp.add(Mid_jp[1],"手机号码");
		Mid_jtp.add(Mid_jp[2],"电子邮件");

		
		//初始化南部
		jp4=new JPanel();
		Bot_jb[0]=new JButton(new ImageIcon("image/denglu.gif"));
		//给登录按钮注册监听
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
		//如果点击了登录按钮
		if(e.getSource()==Bot_jb[0])
		{
			//获取账号和密码
			String useid=this.jp0_jtf.getText();
			String passwd=new String(this.jp0_jpass.getPassword());
			//把用户名和密码传给业务逻辑层
			boolean b=qcu.LoginTrueOrFalse(useid, passwd);
			if(b)
			{
				//转到主界面，把用户账号传给主窗口
				QQ_Main qm=new QQ_Main(this.jp0_jtf.getText());
				//添加主界面到集合
				QQMainViewHashMap.addToQQMainMap(this.jp0_jtf.getText(), qm);
				System.out.println("成功添加界面到集合   "+this.jp0_jtf.getText());
				
				//构造主界面后，向服务器发一个要求返回在线好友信息的包
				//拿到该用户的线程
				QqUserThread qut=UserHashMap.getQqUserThread(this.jp0_jtf.getText());
				//让该线程向服务器发要求包
				Message m=new Message();
				m.setMessType(MessageType.Message_get_onLineFriendList);//告诉服务器我要什么类型的包
				m.setSender(useid);//告诉服务器 ，要谁的好友包
				qut.WriteToServer(m);
				this.dispose();
			}else {
				JOptionPane.showMessageDialog(this, "用户或密码错误");
			}

			
		}
	}

}
