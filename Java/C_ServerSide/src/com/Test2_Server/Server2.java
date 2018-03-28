package com.Test2_Server;
//服务端，能实现在控制台持续对话
import java.net.*;
import java.io.*;
public class Server2 {

	public static void main(String[] args) {
		// TODO Auto-generated method stub
		new Server2();
	}
	public Server2()
	{	
		try {
			//监听9999窗口
			ServerSocket ss=new ServerSocket(9988);
			//等待连接
			Socket s=ss.accept();
			System.out.println("已连接上");
			//读Socket对象输入流
			InputStreamReader isr=new InputStreamReader(s.getInputStream());
			BufferedReader br_s=new BufferedReader(isr);
			//获取Socket对象输出流
			PrintWriter pw=new PrintWriter(s.getOutputStream(),true);
			
			//获取控制台输入
			BufferedReader br_con=new BufferedReader(new InputStreamReader(System.in));
			
			//循环：读取客户端输出内容，并向客户端输出内容
			while(true)
			{
				//读取s输出
				String in=br_s.readLine();
				//显示输出结果到控制台
				System.out.println("客户端说"+in);
				
				System.out.println("请输入您对客户的回复");
				//获取服务端在控制台的输入
				String out=br_con.readLine();
				//服务端回复
				pw.println("服务端说："+out);
			}
			
			
			
			
			
		} catch (IOException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		}
		
	}

}
