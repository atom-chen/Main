package com.Toolkit;
//�����ࣺ��������ͼƬ
import javax.swing.*;
import java.awt.*;
public class Background extends JPanel{
	Image im=null;
	//���챳����
	public Background(Image im)
	{
		//������һ��ͼƬ��������Ϊ����
		this.im=im;
		//��ȡ��Ļ��͸ߣ�����Ӧ������
		int w=Toolkit.getDefaultToolkit().getScreenSize().width;
		int h=Toolkit.getDefaultToolkit().getScreenSize().height;
		
		//����Ϊ����Ŀ�͸�
		this.setSize(w,h);
	}
	public void paintComponent(Graphics g)
	{
		//����
		super.paintComponent(g);
		//��������ͼ
		g.drawImage(im, 0, 0, this.getWidth(),this.getHeight(),this);
	}
}
