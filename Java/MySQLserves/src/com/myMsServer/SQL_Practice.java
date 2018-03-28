package com.myMsServer;
import java.sql.*;

   


public class SQL_Practice {

	public static void main(String[] args) {
		Connection ct=null;
		PreparedStatement ps=null;
		ResultSet rs=null;
		
		try {
			//1����������
			Class.forName("com.microsoft.sqlserver.jdbc.SQLServerDriver");
			//2���������
			ct=DriverManager.getConnection("jdbc:sqlserver://127.0.0.1:1433;databaseName=SHUNPING","sa","");
			//3�������������SQL��䣩
			/*1��ѡ����30�е�����Ա��
			 * select empno,ename from emp where deptno='30'
			 * 2���г����а���Ա(CLERK)����������źͲ��ű��
			 * select empno,ename,deptno from emp where job='CLERK'
			 * 3���ҳ�Ӷ�����н���Ա��(Ӷ���ǽ���)
			 * select empno,ename from emp where isnull(comm,0)>sal
			 * 4���ҳ�Ӷ�����н���60%��Ա��
			 * select empno,ename from emp where comm<(0.6*sal)
			 * 5���ҳ�����10�����о���(MANAGER)�Ͳ���20�����а���Ա(CLERK)����ϸ����
			 * select empno,enamel from emp where (deptno='10' and job='MANAGER') or (deptno='20' and job='CLERK')
			   6���ҳ�����10�����о���(MANAGER)������20�����а���Ա(CLERK)���Ȳ��Ǿ����ֲ��ǰ���Ա����н����ڻ����2000������Ա������ϸ���ϡ�
			   select empno,ename from emp where (job='MANAGER' and deptno='10') or (job='CLERK' and deptno='20') or(job!='CLERK' and job!='MANAGER' and sal>2000)
			   7���ҳ���ȡӶ���Ա���Ĳ�ͬ����
			   select distinct job from emp where comm is not null
			   8���ҳ�����ȡӶ�����ȡ��Ӷ�����100��Ա��
			   select empno,ename from emp where comm<100 or comm is null
			10���ҳ�����12��ǰ�ܹ͵�Ա��
			 */
			ps=ct.prepareStatement("select  from emp where datediff(Current_Timestamp,giredate)<12");
			//4�����ղ�ѯ���
			rs=ps.executeQuery();
			while(rs.next())
			{
//				float num=rs.getFloat(1);
				int num=rs.getInt(1);
				String name=rs.getString(2);
				System.out.println(num+","+name);
			}
		} catch (Exception e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		}finally{
			try {
				ps.close();
			} catch (SQLException e) {
				// TODO Auto-generated catch block
				e.printStackTrace();
			}
			try {
				ct.close();
			} catch (SQLException e) {
				// TODO Auto-generated catch block
				e.printStackTrace();
			}
		}

	}

}
