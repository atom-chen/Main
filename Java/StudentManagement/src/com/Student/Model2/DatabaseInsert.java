package com.Student.Model2;
import java.awt.*;
import javax.swing.*;
import java.awt.event.*;
//功能：提供一个插入窗口，提供数据与表模型对接
public class DatabaseInsert extends javax.swing.JDialog implements ActionListener{
	JLabel jla[]=new JLabel[6];
	JTextField jtf[]=new JTextField[6];
	JButton jb[]=new JButton[2];
	DatabaseModel dbm=null;
	
	public DatabaseInsert(Frame owner, String title, boolean modal)
	{
		//要显示查询到的数据
		jla[0]=new JLabel("学号");
		jtf[0]=new JTextField(10);
		
		jla[1]=new JLabel("姓名");
		jtf[1]=new JTextField(10);
		
		jla[2]=new JLabel("性别");
		jtf[2]=new JTextField(10);
		
		jla[3]=new JLabel("年龄");
		jtf[3]=new JTextField(10);
		
		jla[4]=new JLabel("籍贯");
		jtf[4]=new JTextField(10);
		
		jla[5]=new JLabel("系名");
		jtf[5]=new JTextField(10);
		
		jb[0]=new JButton("确定");//监听
		jb[0].addActionListener(this);
		jb[1]=new JButton("取消");//监听
		jb[1].addActionListener(this);
		
		this.setLayout(new GridLayout(7,2));
		for(int i=0;i<jtf.length;i++)
		{
			this.add(jla[i]);
			this.add(jtf[i]);
		}
		this.add(jb[0]);
		this.add(jb[1]);
		
		this.setSize(300,300);
		this.setLocation(300, 100);
		this.setVisible(true);
		this.setFocusable(false);
		this.setDefaultCloseOperation(JDialog.DO_NOTHING_ON_CLOSE);
	}

	@Override
	public void actionPerformed(ActionEvent e) {
		//如果点击了确定
		if(e.getSource()==jb[0])
		{
			//接收插入的内容
			String num=this.jtf[0].getText();//学号
			String name=this.jtf[1].getText();//姓名
			String sex=this.jtf[2].getText();//性别
			String age=this.jtf[3].getText();//年龄
			String Jg=this.jtf[4].getText();//籍贯
			String dept=this.jtf[5].getText();//系名
			String sql="insert into student values(?,?,?,?,?,?)";
			//做一个字符串数组，接收这些数据
			String[] wenhao={num,name,sex,age,Jg,dept};
			dbm=new DatabaseModel();
			boolean b=dbm.addToData(sql, wenhao);
			if(b==true)
			{
				JOptionPane.showMessageDialog(this, "插入成功");
				this.dispose();
				return;
			}
			else
			{
				JOptionPane.showMessageDialog(this, "插入失败！请检查您的输入");
			}
		}else if(e.getSource()==jb[1])
		{
			this.dispose();
			return;
		}
		
	}
}
