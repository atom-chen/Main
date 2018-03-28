package com.Test2_Client;
import java.net.*;
import java.io.*;

//客户端 能实现和服务端在控制台对话
public class Client2 {

	public static void main(String[] args) {
		// TODO Auto-generated method stub
		new Client2();
	}
	public Client2()
	{

		try {
			//连接到本地的9988端口
			Socket s=new Socket("127.0.0.1", 9988);
			
			//读取用户在控制台的输入
			BufferedReader br_con=new BufferedReader(new InputStreamReader(System.in));
			//获取s的输出流
			PrintWriter pw=new PrintWriter(s.getOutputStream(),true);
			
			//获取s的输入流
			BufferedReader br_s=new BufferedReader(new InputStreamReader(s.getInputStream()));
			
			//实现向服务端发送信息
			while(true)
			{
				System.out.println("您正在向服务端发送信息");
				//读取用户在控制台的输入
				String out=br_con.readLine();
				//向服务端发送信息
				pw.println(out);
				
				//接收服务端的回信
				String in=br_s.readLine();
				//显示服务端回信到控制台
				System.out.println(in);
			}
		} catch (Exception e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		}
		
	}
	

}
