package com.jdbc;
import java.sql.*;


public class Test1 {

	public static void main(String[] args) {
		// TODO Auto-generated method stub
		try {
			Class.forName("oracle.jdbc.driver.OracleDriver");
			System.out.println(1);
			Connection ct=DriverManager.getConnection("jdbc:oracle:thin:@127.0.0.1:1521:MYORCL", "scott", "tiger");
			System.out.println(2);
			System.out.println(ct);
		} catch (Exception e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		}
	}

}
