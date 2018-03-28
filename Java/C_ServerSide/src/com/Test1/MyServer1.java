package com.Test1;
//服务端
import java.net.*;
import java.io.*;

public class MyServer1 {
	//构造方法
	public MyServer1()
	{
		try {
			//构造一个ServerSocket类，在8888号端口上监听
			ServerSocket ss=new ServerSocket(8888);
			//阻塞，等待用户连接
			System.out.println("我是服务端，正在等待连接……");
			//等待某个客户端来连接，该函数会返回一个 Socket 连接
			Socket s=ss.accept();
			
			//接受客户端发来的数据
			InputStreamReader isr=new InputStreamReader(s.getInputStream());
			BufferedReader br=new BufferedReader(isr);
			String info=br.readLine();
			System.out.println("服务器接收到:"+info);
			
			//向服务端回信息
			PrintWriter pw=new PrintWriter(s.getOutputStream(),true);
			pw.println("我是服务端，消息已收到！");
			
		} catch (IOException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		}
		
	}
	public static void main(String []args)
	{
		new MyServer1();
	}
}
