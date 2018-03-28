package com.view;
//功能：闪烁界面
import javax.swing.*;
import java.awt.*;
import java.awt.event.*;

//JWindow：没有关闭按钮的窗格
public class Index extends JWindow implements Runnable{

	public static void main(String[] args) {
		// TODO Auto-generated method stub
		Index i=new Index();
		Thread t=new Thread(i);
		t.start();

	}
	public Index()
	{
		//获取闪屏
		paint p=new paint();
		this.add(p);
		//设置闪烁界面各个参数
		this.setSize(400,250);
		//获取屏幕宽和高
		int width=Toolkit.getDefaultToolkit().getScreenSize().width;
		int height=Toolkit.getDefaultToolkit().getScreenSize().height;
		this.setLocation(width/2-200, height/2-150);
		
		this.setVisible(true);
	}
	//实现等待文字读完再过图
	@Override
	public void run() {
		// TODO Auto-generated method stub
		while(true)
		{
			try {
				Thread.sleep(30*380);
			} catch (InterruptedException e) {
				// TODO Auto-generated catch block
				e.printStackTrace();
			}
			//字全部出现后，然后跳转到下一界面
			new Login();
			//关闭界面
			this.dispose();
			break;
		}
		
	}
	
	// 开发一个闪屏类

	class paint extends JPanel implements Runnable{// 可以自动刷新 Panel
	Thread t;
	int x = 10;
	int i = 0, j = 40, u = 10;
	String gg[] = { "系", "统", "正", "在", "加", "载", "请", "稍", "侯" };
	int k = 0, tt = 0;
	String shi[] = { "满", "汉", "楼", "融", "满", "汉", "精", "华", "做", "天",
	"下",
	"美", "味", "招", "八", "方", "食", "客", "结", "四", "海", "良", "朋", "王", "必", "宇", "程", "序", "演", "示" };
	Font f = new Font("隶书", Font.PLAIN, 18);
	boolean ifok = true;
	int width = 180;
	int height = 0;
	int dian = 0;
	paint() {
	t = new Thread(this);
	t.start();
	}
	public void run() {
	while (true) {
	if (x <= 380);
	repaint();
	try {
	Thread.sleep(70);
	i++;// i=0
	j = j - 6;// j=40
	u = u + 10;// u=10
	if (tt == 3)
	width = width - 20; // tt=0,width=180
	if (i == 4) {
	tt++;
	if (ifok && x > 120 + k * 20)
	k++;// k=0
	if (k >= gg.length - 1)
	ifok = false;
	x = x + 10;
	i = 0;
	j = 40;
	u = 10;
	dian++;
	if (dian > 3)
	dian = 0;
	}
	} catch (InterruptedException e) {
	e.printStackTrace();
	}
	}
	}
	public void paintComponent(Graphics g) {
	// super.paintComponent(g);
	Image image;
	image = Toolkit.getDefaultToolkit().getImage("image/index.jpg");//
	g.drawImage(image, 0, 0, this.getWidth(), 200, this);
	int r = (int) (Math.random() * 255);
	int b = (int) (Math.random() * 255);
	int y = (int) (Math.random() * 255);
	g.setColor(new Color(253, 250, 250));// White
	g.fillRect(x, 210, 380 - x, 30);
	g.setColor(new Color(253, 250, 250));// white
	if (i > 1)
	g.fillRect(x, 235 - (j + 20) / 2, 10, j + 10);// 领头的长方块儿
	if (j > 25)
	g.setColor(new Color(r, b, y));
	else
	g.setColor(new Color(123, 194, 252));// light blue
	g.fillRect(x, 235 - (j + 20) / 2, 10, j);// 后面跟着的短方块儿,10 代表闪
	g.setColor(new Color(123, 194, 252));// light blue
	g.drawRect(10, 210, 380, 30);
	if (x < 120) {
	for (int l = 0; l < gg.length; l++) {
	g.setColor(new Color(0, 0, 0));
	g.drawString(gg[l], 120 + l * 20, 230);
	}
	for (int l = 0; l < dian; l++) {
	g.setColor(new Color(0, 0, 0));
	g.drawString("*", 300 + l * 10, 235);
	}
	g.drawString("*", 300 + dian * 10, 235);
	} else {
	// 设置写字的颜色和位置
	g.setColor(new Color(23, 23, 230));
	g.drawString(gg[k], 120 + k * 20, 230);
	for (int l = k + 1; l < gg.length; l++) {
	g.setColor(new Color(0, 0, 0));
	g.drawString(gg[l], 120 + l * 20, 230);
	}
	if (x > 300 + dian * 10)
	g.setColor(new Color(23, 23, 230));
	for (int l = 0; l < dian; l++) {
	g.drawString("*", 300 + l * 10, 235);
	}
	g.drawString("*", 300 + dian * 10, 235);
	}
	// ------------逐字写诗
	if (tt < 3) {
	for (int rr = 0; rr <= tt; rr++) {
	g.setColor(new Color(250, 0, 0));
	g.drawString(shi[rr], 180, 60 + rr * 20);
	}
	g.drawString(shi[tt], 180, 60 + tt * 20);
	}
	if (tt >= 3 && tt < 8) {
	g.setColor(new Color(230, 0, 0));
	for (int rr = 0; rr < 3; rr++)
	g.drawString(shi[rr], 180, 60 + rr * 20);
	g.setColor(new Color(r, b, y));
	if (tt < 8)
	for (int rr = 3; rr <= tt; rr++)
	g.drawString(shi[rr], 150, rr * 20);
	if (tt >= 7)
	for (int rr = 3; rr < 8; rr++)
	g.drawString(shi[rr], 150, rr * 20);
	}
	if (tt >= 8 && tt < 13) {
	g.setColor(new Color(230, 0, 0));
	for (int rr = 0; rr < 3; rr++)
	g.drawString(shi[rr], 180, 60 + rr * 20);
	for (int rr = 3; rr <= 7; rr++)
	g.drawString(shi[rr], 150, rr * 20);
	g.setColor(new Color(r, b, y));
	if (tt < 13)
	for (int rr = 8; rr <= tt; rr++)
	g.drawString(shi[rr], 120, rr * 20 - 100);
	if (tt >= 13)
	for (int rr = 8; rr < 13; rr++)
	g.drawString(shi[rr], 120, rr * 20 - 100);
	}
	if (tt >= 13 && tt < 18) {
	g.setColor(new Color(230, 0, 0));
	for (int rr = 0; rr < 3; rr++)
	g.drawString(shi[rr], 180, 60 + rr * 20);
	for (int rr = 3; rr <= 7; rr++)
	g.drawString(shi[rr], 150, rr * 20);
	for (int rr = 8; rr < 13; rr++)
	g.drawString(shi[rr], 120, rr * 20 - 100);
	g.setColor(new Color(r, b, y));
	if (tt < 18)
	for (int rr = 13; rr <= tt; rr++)
	g.drawString(shi[rr], 90, rr * 20 - 200);
	if (tt >= 18)
	for (int rr = 13; rr < 13; rr++)
	g.drawString(shi[rr], 90, rr * 20 - 200);
	}
	if (tt >= 18 && tt < 23) {
	g.setColor(new Color(230, 0, 0));
	for (int rr = 0; rr < 3; rr++)
	g.drawString(shi[rr], 180, 60 + rr * 20);
	for (int rr = 3; rr <= 7; rr++)
	g.drawString(shi[rr], 150, rr * 20);
	for (int rr = 8; rr < 13; rr++)
	g.drawString(shi[rr], 120, rr * 20 - 100);
	for (int rr = 13; rr < 18; rr++)
	g.drawString(shi[rr], 90, rr * 20 - 200);
	g.setColor(new Color(r, b, y));
	if (tt < 23)
	for (int rr = 18; rr <= tt; rr++)
	g.drawString(shi[rr], 60, rr * 20 - 300);
	if (tt >= 23)
	for (int rr = 18; rr < 23; rr++)
	g.drawString(shi[rr], 60, rr * 20 - 300);
	}
	if (tt >= 23 && tt < 30) {
	g.setColor(new Color(230, 0, 0));
	for (int rr = 0; rr < 3; rr++)
	g.drawString(shi[rr], 180, 60 + rr * 20);
	for (int rr = 3; rr <= 7; rr++)
	g.drawString(shi[rr], 150, rr * 20);
	for (int rr = 8; rr < 13; rr++)
	g.drawString(shi[rr], 120, rr * 20 - 100);
	for (int rr = 13; rr < 18; rr++)
	g.drawString(shi[rr], 90, rr * 20 - 200);
	for (int rr = 18; rr < 23; rr++)
	g.drawString(shi[rr], 60, rr * 20 - 300);
	g.setColor(new Color(r, b, y));
	if (tt < 30)
	for (int rr = 23; rr <= tt; rr++)
	g.drawString(shi[rr], 30, rr * 20 - 400);
	if (tt >= 30) {
	tt = 0;
	for (int rr = 18; rr < 23; rr++)
	g.drawString(shi[rr], 30, rr * 20 - 300);
	}
	}
	if (tt > 30) {
	g.setColor(new Color(230, 0, 0));
	for (int rr = 0; rr < 3; rr++)
	g.drawString(shi[rr], 180, 60 + rr * 20);
	for (int rr = 3; rr <= 7; rr++)
	g.drawString(shi[rr], 150, rr * 20);
	for (int rr = 8; rr < 13; rr++)
	g.drawString(shi[rr], 120, rr * 20 - 100);
	for (int rr = 13; rr < 18; rr++)
	g.drawString(shi[rr], 90, rr * 20 - 200);
	for (int rr = 18; rr < 23; rr++)
	g.drawString(shi[rr], 60, rr * 20 - 300);
	for (int rr = 23; rr < 30; rr++)
	g.drawString(shi[rr], 30, rr * 20 - 400);
	}
	}
	}

}
