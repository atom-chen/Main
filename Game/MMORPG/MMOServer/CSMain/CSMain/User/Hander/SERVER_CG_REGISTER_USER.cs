using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public partial class CG_REGISTER_PAK:CG_PAK_BASE
{

    public override void Handle(User user)
    {
        user.HandlePacket(this);
    }

    public override void SetDic(Dictionary<byte, object> para)
    {
        this.dic = para;
    }
}
