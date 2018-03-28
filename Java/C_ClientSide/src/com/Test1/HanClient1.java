package com.Test1;

/**
* 这是一个客户端程序，可以连接服务器端口 9999
*/
import java.io.BufferedReader;
import java.io.IOException;
import java.io.InputStreamReader;
import java.io.PrintWriter;
import java.net.*;
public class HanClient1 {
public static void main(String[] args) {
new HanClient1();
}
public HanClient1(){
try {
	//Socket()就是去连接某个服务器端 127.0.0.1 表示服务器的 ip
	//9999 是服务器的端口号
	Socket s=new Socket("127.0.0.1",9999);
	//如果 s 连接成功，就可以发送数据到服务器端
	//我们通过 pw 向 s 写数据,true 表示即时刷新
	PrintWriter pw=new PrintWriter(s.getOutputStream(),true);
	pw.println("你好吗？我是客户端");

	//要读取 s 中传递的数据
	InputStreamReader isr=new InputStreamReader(s.getInputStream());
	BufferedReader br=new BufferedReader(isr);
	String info=br.readLine();
	System.out.println("接收到服务器：\t"+info);
	} catch (Exception e) {
		e.printStackTrace();
		}
}
}