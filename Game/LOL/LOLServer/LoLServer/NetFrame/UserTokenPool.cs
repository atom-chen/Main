using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetFrame
{
    public class UserTokenPool
    {
        private Stack<UserToken> pool;
        
        public UserTokenPool(int max)
        {
            pool = new Stack<UserToken>(max);

        }
        //新建一个连接，就要从集合中取出一个给用户使用———创建连接
        public UserToken pop()
        {
            return pool.Pop();
        }
        //断开连接，就塞回去一个到集合———释放连接
        public void push(UserToken userToken)
        {
            if(userToken!=null)
            {
                //插入
                pool.Push(userToken);
            }
        }
        //获取剩余大小
        public int size { 
            get { return pool.Count; } 
        }
    }
}
