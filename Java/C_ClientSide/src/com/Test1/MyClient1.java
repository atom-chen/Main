package com.Test1;
//客户端
import java.net.*;
import java.io.*;

public class MyClient1 {
	public MyClient1()
	{
		try {
			//与服务端建立连接
			Socket s=new Socket("127.0.0.1",8888);
			
			//向服务端发送信息
			PrintWriter pw=new PrintWriter(s.getOutputStream(),true);
			pw.println("你好，我是客户端");
			
			//要读取s中传递的数据
			InputStreamReader isr=new InputStreamReader(s.getInputStream());
			BufferedReader br=new BufferedReader(isr);
			String info=br.readLine();
			System.out.println("接收到服务器消息："+info);
			
		} catch (Exception e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
			System.out.println("遇到了异常！！！");
		}
	}
	public static void main(String []args)
	{
		new MyClient1();
	}
}
