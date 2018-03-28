package com.Student.Model2_2;

import java.awt.*;
import javax.swing.*;
import java.awt.event.*;
import java.util.Vector;
//功能：实现更新数据
public class DatabaseUpdatePane extends javax.swing.JDialog implements ActionListener{
	JLabel jla[]=new JLabel[6];
	JTextField jtf[]=new JTextField[6];
	JButton jb[]=new JButton[2];
	StuModel dbm=null;

	Vector<Vector> rowData=null;
	Vector<String> columnNames=null;
	
	//建立表的一个更新面板：构造的时候提供父窗口、标题、是否模态、一个模型的引用，选中的元组
	public DatabaseUpdatePane(Frame owner, String title, boolean Modal,StuModel dm,int RowData)
	{
		this.setModal(true);
		dbm=new StuModel();
		jla[0]=new JLabel("学号");
		jtf[0]=new JTextField(10);
		//给文本框传值
		jtf[0].setText(dm.getValueAt(RowData, 0).toString());
		//学号不能修改
		jtf[0].setEditable(false);
		
		jla[1]=new JLabel("姓名");
		jtf[1]=new JTextField(10);
		jtf[1].setText(dm.getValueAt(RowData, 1).toString());
		
		jla[2]=new JLabel("性别");
		jtf[2]=new JTextField(10);
		jtf[2].setText(dm.getValueAt(RowData, 2).toString());
		
		jla[3]=new JLabel("年龄");
		jtf[3]=new JTextField(10);
		jtf[3].setText(dm.getValueAt(RowData, 3).toString());
		
		jla[4]=new JLabel("籍贯");
		jtf[4]=new JTextField(10);
		jtf[4].setText(dm.getValueAt(RowData, 4).toString());
		
		jla[5]=new JLabel("系名");
		jtf[5]=new JTextField(10);
		jtf[5].setText(dm.getValueAt(RowData, 5).toString());
		
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
			String num=this.jtf[0].getText();//学号
			String name=this.jtf[1].getText();//姓名
			String sex=this.jtf[2].getText();//性别
			String age=this.jtf[3].getText();//年龄
			String Jg=this.jtf[4].getText();//籍贯
			String dept=this.jtf[5].getText();//系名
			//创建SQL语句
			String sql="update student set stuName=?,stuSex=?,stuAge=?,stuJg=?,stuDept=? where stuId=?";

			//接收？内容
			String[] data={name,sex,age,Jg,dept,num};
			dbm=new StuModel();
			//执行sql语句
			boolean b=dbm.addToData(sql, data);
			if(b==true)
			{
				JOptionPane.showMessageDialog(this, "修改成功！");
				this.dispose();
				return;
			}
			
		}else if(e.getSource()==jb[1])
		{
			this.dispose();
		}
		
	}
}
