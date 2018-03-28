package com.view;
//实现人事管理添加的JDialog
import java.awt.*;
import javax.swing.*;

import com.Toolkit.MyFont;
import com.model.EmpModel;

import java.awt.event.*;
public class Rsgl_InsertDialog extends JDialog implements ActionListener{
	//我认为界面有2*JPanel 15*JLabel 15*JTextField 2*JButton
	JPanel jp1,jp2;
	JLabel jl1,jl2,jl4,jl5,jl6,jl7,jl8,jl9,jl10,jl11,jl12,jl13,jl14,jl15;
	JTextField jtf1,jtf2,jtf4,jtf5,jtf6,jtf7,jtf8,jtf9,jtf10,jtf11,jtf12,jtf13,jtf14,jtf15;
	JButton jb1,jb2;
	EmpModel em=null;
	//构造方法，提供父窗口、标题和是否模态
	public Rsgl_InsertDialog(Empinfo empinfo, String title, boolean modal)
	{
		em=new EmpModel();
		//设置为模态
		this.setModal(true);
		//整体应为边界布局
		this.setLayout(new BorderLayout());

		//初始化中部
		jp1=new JPanel(new GridLayout(15,2,20,20));
		jl1=new JLabel("工号",JLabel.CENTER);
		jtf1=new JTextField(10);
		jp1.add(jl1);
		jp1.add(jtf1);
		
		jl2=new JLabel("姓名",JLabel.CENTER);
		jtf2=new JTextField(10);
		jp1.add(jl2);
		jp1.add(jtf2);
		
		
		jl4=new JLabel("性别",JLabel.CENTER);
		jtf4=new JTextField(10);
		jp1.add(jl4);
		jp1.add(jtf4);
		
		jl5=new JLabel("家庭住址",JLabel.CENTER);
		jtf5=new JTextField(10);
		jp1.add(jl5);
		jp1.add(jtf5);
		
		jl6=new JLabel("生日",JLabel.CENTER);
		jtf6=new JTextField(10);
		jp1.add(jl6);
		jp1.add(jtf6);
		
		jl7=new JLabel("身份证号码",JLabel.CENTER);
		jtf7=new JTextField(10);
		jp1.add(jl7);
		jp1.add(jtf7);
		
		jl8=new JLabel("学历",JLabel.CENTER);
		jtf8=new JTextField(10);
		jp1.add(jl8);
		jp1.add(jtf8);
		
		jl9=new JLabel("职位",JLabel.CENTER);
		jtf9=new JTextField(10);
		jp1.add(jl9);
		jp1.add(jtf9);
		
		jl10=new JLabel("婚姻状态",JLabel.CENTER);
		jtf10=new JTextField(10);
		jp1.add(jl10);
		jp1.add(jtf10);
		
		jl11=new JLabel("家庭电话",JLabel.CENTER);
		jtf11=new JTextField(10);
		jp1.add(jl11);
		jp1.add(jtf11);
		
		jl12=new JLabel("手机号",JLabel.CENTER);
		jtf12=new JTextField(10);
		jp1.add(jl12);
		jp1.add(jtf12);
		
		jl13=new JLabel("e-mail",JLabel.CENTER);
		jtf13=new JTextField(10);
		jp1.add(jl13);
		jp1.add(jtf13);
		
		jl14=new JLabel("入职时间",JLabel.CENTER);
		jtf14=new JTextField(10);
		jp1.add(jl14);
		jp1.add(jtf14);
		
		jl15=new JLabel("备注",JLabel.CENTER);
		jtf15=new JTextField(10);
		jp1.add(jl15);
		jp1.add(jtf15);
		this.add(jp1);
		
		//初始化南部
		jp2=new JPanel();
		jb1=new JButton("添加");
		jb1.addActionListener(this);//给确定注册监听
		jb2=new JButton("取消");
		jp2.add(jb1);
		jp2.add(jb2);
		this.add(jp2,"South");
		
		this.setSize(300,900);
		this.setLocation(500, 100);
		this.setTitle("添加用户");
		this.setVisible(true);
		this.setDefaultCloseOperation(JDialog.DISPOSE_ON_CLOSE);
		this.setResizable(false);
	}
	@Override
	public void actionPerformed(ActionEvent e) {
		if(e.getSource()==jb1)
		{
			//如果点击了确定
			//获取每个文本框的内容
			String id=this.jtf1.getText();
			String name=this.jtf2.getText();
			String sex=this.jtf4.getText();
			String address=this.jtf5.getText();
			
			String birthday=this.jtf6.getText();
			String peopleId=this.jtf7.getText();
			String xl=this.jtf8.getText();
			String Job=this.jtf9.getText();
			String hf=this.jtf10.getText();
			
			String hometel=this.jtf11.getText();
			String modeltel=this.jtf12.getText();
			String email=this.jtf13.getText();
			String zctime=this.jtf14.getText();
			String bz=this.jtf15.getText();
			
			
			//传进逻辑层
			boolean b=em.insert(id,name,sex,address,birthday,peopleId,xl,Job,hf,hometel,modeltel,email,zctime,bz);;
			if(b)
			{
				JOptionPane.showMessageDialog(this, "添加成功!");
				this.dispose();
				return;
			}else
			{
				JOptionPane.showMessageDialog(this, "添加失败！请检查输入");
			}
		}
		
	}
}
