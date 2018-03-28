package com.QQ.Client.view1;
import com.QQ.Client.model.ChatViewHashMap;
import com.QQ.Client.model.QqClientLogin;
//QQ������
import com.QQ.Client.viewchildren.*;
import com.QQ.Common.Message;

import javax.swing.*;
import java.awt.*;
import java.awt.event.*;

public class QQ_Main extends JFrame implements ActionListener,MouseListener{
	//�������ǿ�Ƭ����
	CardLayout card=null;
	JPanel jp=null;
	//��������3��JPanel
	Haoyou hy=null;
	Moshengren msr=null;
	Heimingdan hmd=null;
	//����Ϊʹ�����Լ�Ҳ�Ǵ��������
	public String Onwer;
	public QQ_Main(String Onwer)
	{
		this.Onwer=Onwer;
		this.card=new CardLayout();
		jp=new JPanel(card);
		//��ʼ��3����Ƭ
		hy=new Haoyou(this);
		msr=new Moshengren(this);
		hmd=new Heimingdan(this);
			
		//��ӵ���Ƭ
		jp.add(hy,"1");
		jp.add(msr,"2");
		jp.add(hmd,"3");
		this.add(jp);
		
		this.setSize(200,500);
		this.setLocation(Toolkit.getDefaultToolkit().getScreenSize().width-400, Toolkit.getDefaultToolkit().getScreenSize().height-1000);
		this.setTitle(Onwer);
		this.setVisible(true);
		this.setDefaultCloseOperation(JFrame.EXIT_ON_CLOSE);
	}
	//���ܣ��ѷ��������ߺ�����Ϣ���������б�
	public void Tohaoyou(Message m)
	{
		hy.UpdateFriendsList(m);
	}
	@Override
	public void actionPerformed(ActionEvent e) {
		//�������˺���
		if(e.getActionCommand().equals("hy"))
		{
			this.card.show(jp, "1");
		}
		//��������İ����
	    else if(e.getActionCommand().equals("msr"))
		{
			//�л���İ����ѡ�
			this.card.show(this.jp, "2");
		}
	    else if(e.getActionCommand().equals("hmd"))
	    {
	    	this.card.show(jp, "3");
	    }
		
	}
	@Override
	public void mouseClicked(MouseEvent e) {
		// TODO Auto-generated method stub
		
	}
	@Override
	public void mousePressed(MouseEvent e) {
		//���˫��ĳ����
		if(e.getClickCount()==2)
		{
			//��ȡ������˵�����
			JLabel jl=(JLabel)e.getSource();
			//������û�������
			if(!(jl.isEnabled()))
			{
				JOptionPane.showMessageDialog(null, "�ú��Ѳ����ߣ����ܷ���");
				
			}
			else
			{
				System.out.println("����Ҫ��"+jl.getText()+"������");
				//�������촰��   ��ʹ�����Լ����Լ������ӣ�����Ҫ������˴���ȥ
				Chat c=new Chat(this.Onwer,jl.getText());
				//�����������ӵ�����
				ChatViewHashMap.addToChatMap(this.Onwer+" "+jl.getText(), c);
			}
		}
	}
	@Override
	public void mouseReleased(MouseEvent e) {
		// TODO Auto-generated method stub
		
	}
	@Override
	//������ı���ɫ
	public void mouseEntered(MouseEvent e) {
		//��ȡ�������JLabel
		JLabel jl=(JLabel) e.getSource();
		//�ı���ɫ
		jl.setForeground(Color.red);
		
	}
	//����뿪�ı���ɫ
	public void mouseExited(MouseEvent e) {
		// TODO Auto-generated method stub
		//��ȡ�������JLabel
		JLabel jl=(JLabel) e.getSource();
		//�ı���ɫ
		jl.setForeground(Color.black);
	}
	
	

}











