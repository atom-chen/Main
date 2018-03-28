package com.view;
import java.awt.*;
import java.awt.event.*;

import javax.swing.*;

public class TestCard extends JFrame implements MouseListener{

	//定义需要的组件
	JSplitPane jsp=null;
	JPanel jPanel_left,jPanel_right,jPanel_right_1,jPanel_right_2,jPanel_right_3;
	JLabel jbl,jb2,jb3;
	CardLayout cl=new CardLayout();
	
	public static void main(String[] args) {
		// TODO Auto-generated method stub
		new TestCard();
	}
	
	public TestCard(){
		//创建组件
		jPanel_left=new JPanel(new GridLayout(5,1));
		jPanel_left.setBorder(BorderFactory.createEtchedBorder());
		jbl=new JLabel("学生选课系统",JLabel.CENTER);
		jbl.addMouseListener(this);
		jb2=new JLabel("老师管理",JLabel.CENTER);
		jb2.addMouseListener(this);
		jb3=new JLabel("学生管理",JLabel.CENTER);
		jb3.addMouseListener(this);
		jPanel_left.add(jbl);
		jPanel_left.add(jb2);
		jPanel_left.add(jb3);
		
		//右边的panel
		jPanel_right=new JPanel(cl);
		
		jPanel_right_1=new JPanel();
		//jPanel_right_1.setBackground(Color.RED);
		jPanel_right_1.add(new JLabel(new ImageIcon("sc.jpg")));
		jPanel_right_2=new JPanel();
		jPanel_right_2.setBackground(Color.BLACK);
		jPanel_right_3=new JPanel();
		jPanel_right_3.setBackground(Color.BLUE);
		
		jPanel_right.add("1",jPanel_right_1 );
		jPanel_right.add("2",jPanel_right_2 );
		jPanel_right.add("3",jPanel_right_3 );
		//设置默认显示的卡片, 第一个卡片.
		cl.show(jPanel_right, "2");
		
		jsp=new JSplitPane(JSplitPane.HORIZONTAL_SPLIT,jPanel_left,jPanel_right);
		jsp.setDividerLocation(140);
		jsp.setDividerSize(0);
		this.add(jsp);
		
		this.setSize(800, 600);
		this.setVisible(true);
		this.setDefaultCloseOperation(JFrame.EXIT_ON_CLOSE);
		
		
		
	}

	public void mouseClicked(MouseEvent e) {
		// TODO Auto-generated method stub
		if(e.getClickCount()==1){
			//判断用户点击哪个JLabel
			if(e.getSource()==jbl){
				cl.show(jPanel_right,"1");
			}else if(e.getSource()==jb2){
				cl.show(jPanel_right,"2");
			}else if(e.getSource()==jb3){
				cl.show(jPanel_right,"3");
			}
		}
		
	}

	public void mouseEntered(MouseEvent e) {
		// TODO Auto-generated method stub
		
		((JLabel)e.getSource()).setForeground(Color.RED);
		((JLabel)e.getSource()).setCursor(new Cursor(Cursor.HAND_CURSOR));
		
	}

	public void mouseExited(MouseEvent e) {
		// TODO Auto-generated method stub
		((JLabel)e.getSource()).setForeground(Color.BLACK);
		((JLabel)e.getSource()).setCursor(new Cursor(Cursor.DEFAULT_CURSOR));
	}

	public void mousePressed(MouseEvent e) {
		// TODO Auto-generated method stub
		
	}

	public void mouseReleased(MouseEvent e) {
		// TODO Auto-generated method stub
		
	}

}
