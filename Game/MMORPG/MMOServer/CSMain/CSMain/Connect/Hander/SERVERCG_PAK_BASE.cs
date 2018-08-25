using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public abstract class CG_PAK_BASE
{
    public abstract void Handle(User user);
    public abstract void SetDic(Dictionary<byte, object> para);
}
