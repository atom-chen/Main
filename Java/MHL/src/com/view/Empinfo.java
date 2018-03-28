package com.view;
//功能：人事登记表界面
import java.awt.*;
import javax.swing.*;

import com.Toolkit.MyFont;
import com.model.EmpModel;

import java.awt.event.*;

public class Empinfo extends JPanel implements ActionListener{
	//北部：1*JPanel 1*JLabel 1*JTextField 1*JButton
	//中部：1*JTable 1*JScrollPane
	//南部：3*JPanel 1*JLabel 4*JButton
	JPanel jp1,jp2,jp3,jp4;
	JLabel jla1,jla2;
	JTextField jtf=null;
	JButton jb1,jb2,jb3,jb4,jb5;
	
	JScrollPane jsp=null;
	JTable jta=null;
	EmpModel em=null;
	//构造界面
	public Empinfo()
	{
		//北部
		JPanel jp1=new JPanel();
		jla1=new JLabel("请输入姓名（员工号或职位）：");
		jla1.setFont(MyFont.f3);
		jtf=new JTextField(20);
		jb1=new JButton("刷新");
		jb1.addActionListener(this);//监听刷新
		jb1.setFont(MyFont.f3);
		jp1.add(jla1);
		jp1.add(jtf);
		jp1.add(jb1);
		
		//中部
		em=new EmpModel();
		em.shuaxin("");
		jta=new JTable(em);
		jsp=new JScrollPane(jta);
		
		
		//南部
		jp2=new JPanel(new BorderLayout());
		//左边的记录数
		jp3=new JPanel(new FlowLayout(FlowLayout.LEFT));
		jla2=new JLabel("共有xx条记录");
		jla2.setFont(MyFont.f3);
		jp3.add(jla2);
		//右边的4个按钮
		jp4=new JPanel(new FlowLayout(FlowLayout.RIGHT));
		jb2=new JButton("详细信息");
		jb2.setFont(MyFont.f3);
		jp4.add(jb2);
		jb3=new JButton("添加");
		jb3.addActionListener(this);//监听添加
		jb3.setFont(MyFont.f3);
		jp4.add(jb3);
		jb4=new JButton("修改");
		jb4.addActionListener(this);//监听修改
		jb4.setFont(MyFont.f3);
		jp4.add(jb4);
		jb5=new JButton("删除");
		jb5.addActionListener(this);//监听删除
		jb5.setFont(MyFont.f3);
		jp4.add(jb5);
		//添加到jp2
		jp2.add(jp3,"West");
		jp2.add(jp4,"East");
		
		this.setLayout(new BorderLayout());
		this.add(jp1,"North");
		this.add(jsp);
		this.add(jp2,"South");
		this.setVisible(true);
	}
	@Override
	public void actionPerformed(ActionEvent e) {
		// TODO Auto-generated method stub
		//如果点击了刷新
		if(e.getSource()==jb1)
		{
			//获取文本框内容
			String name=this.jtf.getText();
			//执行查询
			em=new EmpModel();
			em.shuaxin(name);
			this.jta.setModel(em);
		}
		//如果点击了添加
		else if(e.getSource()==jb3)
		{
			System.out.println("添加");
			//弹出添加JDialog
			new Rsgl_InsertDialog(this, "添加用户", true);
			//刷新表
			em=new EmpModel();
			em.shuaxin("");
			jta.setModel(em);
			
		}
		//如果点击了修改
		else if(e.getSource()==jb4)
		{
			System.out.println("修改");
			//弹出添加JDialog
		}
		//如果点击了删除
		else if(e.getSource()==jb5)
		{
			System.out.println("删除");
			//获取用户选择的人
			int num=this.jta.getSelectedRow();
			//找到所选的人的id
			String id=(String)this.em.getValueAt(num, 0);
			System.out.println("要删的人id为"+id);
			//执行业务逻辑层的删除方法
			if(em.delete(id))
			{
				JOptionPane.showMessageDialog(null, "删除成功！");
			}
			else
			{
				JOptionPane.showMessageDialog(null, "删除失败！");
			}
			//更新表
			em=new EmpModel();
			em.shuaxin("");
			jta.setModel(em);
			
		}
	}
}










