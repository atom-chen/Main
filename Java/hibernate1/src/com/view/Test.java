package com.view;
import java.util.Date;

import org.hibernate.Session;
import org.hibernate.SessionFactory;
import org.hibernate.Transaction;
import org.hibernate.cfg.*;

import com.domain.Employee;
import com.sun.scenario.effect.impl.sw.sse.SSEBlend_ADDPeer;
import com.tools.MySessionFactory;

public class Test {
	

	public static void main(String[] args) {
		InsertEmp();
		
		
		
	}

	public static void DeleteEmp() {
		//删除
		Session session=MySessionFactory.getSessionFactory().openSession();
		Transaction transaction=session.beginTransaction();
		//获取该雇员，然后删除
		Employee emp=(Employee) session.load(Employee.class, 1);
		session.delete(emp);
		transaction.commit();
		session.close();
	}

	public static void UpdateEmp() {
		//修改用户
		//获取一个会话
		Session session=MySessionFactory.getSessionFactory().openSession();
		//修改用户1.获取用户 2.修改
		Transaction ts=session.beginTransaction();
		//load方法通过主键（id）属性，获取该对象实例<->和表的记录对应
		Employee employee=(Employee) session.load(Employee.class, 1);
		employee.setMyname("新的王必宇");//写下这句话，会导致一个update语句产生
		employee.setEmail("251237220@qq.com");//只会产生一个sql语句
		ts.commit();
		session.close();
	}

	public static void InsertEmp() {
		//这里我们只建对象
		//1.创建Configuration
		//该对象用于读取hibernate.cfg.xml,并完成初始化
		Configuration configuratio=new Configuration().configure();
		//2、创建SessoinFactory，这是一个会话工厂，是一个重量级对象
		SessionFactory sessionFactory=configuratio.buildSessionFactory();
		//3.创建一个会话 相当于jdbc中的Connection
		Session session=sessionFactory.openSession();
		//4.对hibernate而言，进行DDL语句要求程序员使用事务。
		Transaction transaction=session.beginTransaction();
		//添加一个雇员
		Employee newemp=new Employee();
		newemp.setMyname("biyu");
		newemp.setEmail("aaaaa@163.com");
		newemp.setHiredate(new Date());
		//保存
		session.save(newemp);//-->insert into语句 被hibernate封装
		//提交事务
		transaction.commit();
		//关闭资源
		session.close();
	}

}
