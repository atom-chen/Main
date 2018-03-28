package com.Toolkit;
//工具类：画出背景图片
import javax.swing.*;
import java.awt.*;
public class Background extends JPanel{
	Image im=null;
	//构造背景框
	public Background(Image im)
	{
		//传进来一张图片，让它作为背景
		this.im=im;
		//获取屏幕宽和高（自适应背景）
		int w=Toolkit.getDefaultToolkit().getScreenSize().width;
		int h=Toolkit.getDefaultToolkit().getScreenSize().height;
		
		//设置为窗体的宽和高
		this.setSize(w,h);
	}
	public void paintComponent(Graphics g)
	{
		//清屏
		super.paintComponent(g);
		//画出背景图
		g.drawImage(im, 0, 0, this.getWidth(),this.getHeight(),this);
	}
}
