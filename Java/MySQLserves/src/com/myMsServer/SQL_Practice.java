package com.myMsServer;
import java.sql.*;

   


public class SQL_Practice {

	public static void main(String[] args) {
		Connection ct=null;
		PreparedStatement ps=null;
		ResultSet rs=null;
		
		try {
			//1、加载驱动
			Class.forName("com.microsoft.sqlserver.jdbc.SQLServerDriver");
			//2、获得连接
			ct=DriverManager.getConnection("jdbc:sqlserver://127.0.0.1:1433;databaseName=SHUNPING","sa","");
			//3、创建火箭车（SQL语句）
			/*1、选择部门30中的所有员工
			 * select empno,ename from emp where deptno='30'
			 * 2、列出所有办事员(CLERK)的姓名，编号和部门编号
			 * select empno,ename,deptno from emp where job='CLERK'
			 * 3、找出佣金高于薪金的员工(佣金是奖金)
			 * select empno,ename from emp where isnull(comm,0)>sal
			 * 4、找出佣金高于薪金的60%的员工
			 * select empno,ename from emp where comm<(0.6*sal)
			 * 5、找出部门10中所有经理(MANAGER)和部门20中所有办事员(CLERK)的详细资料
			 * select empno,enamel from emp where (deptno='10' and job='MANAGER') or (deptno='20' and job='CLERK')
			   6、找出部门10中所有经理(MANAGER)，部门20中所有办事员(CLERK)，既不是经理又不是办事员但其薪金大于或等于2000的所有员工的详细资料。
			   select empno,ename from emp where (job='MANAGER' and deptno='10') or (job='CLERK' and deptno='20') or(job!='CLERK' and job!='MANAGER' and sal>2000)
			   7、找出收取佣金的员工的不同工作
			   select distinct job from emp where comm is not null
			   8、找出不收取佣金或收取的佣金低于100的员工
			   select empno,ename from emp where comm<100 or comm is null
			10、找出早于12年前受雇的员工
			 */
			ps=ct.prepareStatement("select  from emp where datediff(Current_Timestamp,giredate)<12");
			//4、接收查询结果
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
