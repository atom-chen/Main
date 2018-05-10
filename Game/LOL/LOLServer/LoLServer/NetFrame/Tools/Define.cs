using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetFrame
{
  public delegate byte[] LengthEncode(byte[] value);//粘包编码
  public delegate byte[] LengthDecode(ref List<byte> value);//粘包解码

  public delegate byte[] encode(object value);//序列化
  public delegate object decode(byte[] value);//反序列化
}
