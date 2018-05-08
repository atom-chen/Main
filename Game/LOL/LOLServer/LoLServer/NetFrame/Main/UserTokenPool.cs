using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
/*
 * 连接池
 */
namespace NetFrame
{
  public class UserTokenPool
  {
    private Stack<UserToken> m_Pool;

    //初始化连接池
    public UserTokenPool(int max)
    {
      m_Pool = new Stack<UserToken>(max);
    }

    //创建一个用户连接
    public UserToken Pop()
    {
      if(m_Pool.Count>0)
      {
        UserToken user = m_Pool.Pop();
        return user;
      }
      else
      {
        return null;
      }
    }

    //释放连接手动调用
    public void Push(UserToken userToken)
    {
      if (userToken != null)
      {
        m_Pool.Push(userToken);
      }
    }

    //获取剩余大小
    public int Size
    {
      get { return m_Pool.Count; }
    }
  }
}
