using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

class LinkList
{

  public void Add(float value)
  {
    if(m_Top==null)
    {
      m_Top = new Node(value);
      m_Last = m_Top;
      m_CurSize++;
    }
    else
    {
      //将新的float作为头结点
      Node newTop=new Node(value);
      m_Top.pre = newTop;
      newTop.next = m_Top;
      m_Top = newTop;
      m_CurSize++;
    }
    if(m_CurSize>=m_MaxSize)
    {
      Node temp = m_Last.pre;
      temp.next = null;
      m_Last = temp;
      m_CurSize--;
    }
   
  }

  public float GetSpeed()
  {
    float sum = 0;
    Node temp=m_Top;
    while(temp!=null)
    {
      sum += temp.data;
      temp = temp.next;
    }
    return m_CurSize==0?0:sum / m_CurSize;
  }

  private Node m_Top;
  private Node m_Last;
  private int m_CurSize=0;
  private int m_MaxSize = 60;
}

class Node
{
  public float data;
  public Node next;
  public Node pre;
  public Node(float value)
  {
    data = value;
  }
}
