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
		DeleteEmp();
		
		
		
	}

	public static void DeleteEmp() {
		//ɾ��
		Session session=MySessionFactory.getSessionFactory().openSession();
		Transaction transaction=session.beginTransaction();
		//��ȡ�ù�Ա��Ȼ��ɾ��
		Employee emp=(Employee) session.load(Employee.class, 1);
		session.delete(emp);
		transaction.commit();
		session.close();
	}

	public static void UpdateEmp() {
		//�޸��û�
		//��ȡһ���Ự
		Session session=MySessionFactory.getSessionFactory().openSession();
		//�޸��û�1.��ȡ�û� 2.�޸�
		Transaction ts=session.beginTransaction();
		//load����ͨ��������id�����ԣ���ȡ�ö���ʵ��<->�ͱ�ļ�¼��Ӧ
		Employee employee=(Employee) session.load(Employee.class, 1);
		employee.setMyname("�µ�������");//д����仰���ᵼ��һ��update������
		employee.setEmail("251237220@qq.com");//ֻ�����һ��sql���
		ts.commit();
		session.close();
	}

	public static void InsertEmp() {
		//��������ֻ������
		//1.����Configuration
		//�ö������ڶ�ȡhibernate.cfg.xml,����ɳ�ʼ��
		Configuration configuratio=new Configuration().configure();
		//2������SessoinFactory������һ���Ự��������һ������������
		SessionFactory sessionFactory=configuratio.buildSessionFactory();
		//3.����һ���Ự �൱��jdbc�е�Connection
		Session session=sessionFactory.openSession();
		//4.��hibernate���ԣ�����DDL���Ҫ�����Աʹ������
		Transaction transaction=session.beginTransaction();
		//���һ����Ա
		Employee newemp=new Employee();
		newemp.setMyname("biyu");
		newemp.setEmail("aaaaa@163.com");
		newemp.setHiredate(new Date());
		//����
		session.save(newemp);//-->insert into��� ��hibernate��װ
		//�ύ����
		transaction.commit();
		//�ر���Դ
		session.close();
	}

}
