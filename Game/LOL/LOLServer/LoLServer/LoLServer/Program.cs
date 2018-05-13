using NetFrame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoLServer
{
  class Program
  {
    static void Main(string[] args)
    {
      LOLCenter center = new LOLCenter();
      //服务器初始化...
      ServerStart server = new ServerStart(11111, center);
      while(true)
      {

      }
    }
  }
}
