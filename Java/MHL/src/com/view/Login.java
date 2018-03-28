package com.view;
import java.awt.*;
import com.Toolkit.*;
import com.model.UserModel;

import javax.imageio.ImageIO;
import javax.swing.*;
import java.awt.event.*;
import java.io.*;
//功能：登录界面
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
		//设置布局
		this.setLayout(null);

		//设置标签
		jl1=new JLabel("请输入用户名：");
		jl1.setBounds(60, 190, 150, 30);
		jl1.setFont(MyFont.f1);
		this.add(jl1);
		//设置文本框
		ac=new JTextField();
		ac.setBounds(180, 190, 120, 30);
		//让用户名文本框做焦点
		ac.setFocusable(true);
		//文本框下凹效果
		ac.setBorder(BorderFactory.createLoweredBevelBorder());
		this.add(ac);
		
		jl3=new JLabel("（或员工号）");
		jl3.setBounds(80, 210, 100, 30);
		jl3.setFont(MyFont.f2);
		//设置前景色
		jl3.setForeground(Color.red);
		this.add(jl3);
		
		//设置密码框
		jl2=new JLabel("请输入密码：");
		jl2.setBounds(60, 240, 150, 30);
		jl2.setFont(MyFont.f1);
		this.add(jl2);
		passworld=new JPasswordField();
		passworld.setBounds(180, 240, 120, 30);
		//密码框下凹效果
		passworld.setBorder(BorderFactory.createLoweredSoftBevelBorder());
		this.add(passworld);
		
		//设置按钮
		jb1=new JButton("确定");
		jb1.addActionListener(this);
		//对确定注册监听
		jb1.setBounds(92, 303, 79, 30);
		jb1.setFont(MyFont.f1);
		this.add(jb1);
		jb2=new JButton("取消");
		//对取消注册监听
		jb2.addActionListener(this);
		jb2.setBounds(207, 303, 79, 30);
		jb2.setFont(MyFont.f1);
		this.add(jb2);
		
		Background b=new Background();
		b.setBounds(0, 0, 360, 360);
		this.add(b);
		
		this.setSize(360,360);
		//位置：正中间
		this.setLocation(659,322);
		
		//关闭JDialog的窗格
		this.setUndecorated(true);
		this.setVisible(true);
	}
	//作一个内部类，用作画板画出背景图
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
		//若点击了确定
		if(e.getSource()==jb1)
		{
			System.out.println("点击确定");
			//读取文本框和密码框
			String username= ac.getText().trim();
			String password=new String(passworld.getPassword());
			//调用逻辑层方法，获取职务
			UserModel um=new UserModel();
			String job=um.enter(username, password);
			System.out.println(job);
			//如果职务为经理 则允许登录
			if((job.trim()).equals("经理"))
			{
				System.out.println("进入到新界面");
				//跳转到window1
				new Window1();
				//弹出窗口
				JOptionPane.showMessageDialog(null, job+"欢迎您");
				//关闭该窗口
				this.dispose();
			}
			else
			{
				JOptionPane.showMessageDialog(null, "登陆失败！请确认您是否有足够的权限");
			}
		}
		else if(e.getSource()==jb2)
		{
			System.exit(0);
		}
	}

}
