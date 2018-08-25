using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public partial class CG_ADD_ROLE_PAK:CG_PAK_BASE
{
    private Dictionary<byte, object> dic = new Dictionary<byte, object>();
    public Role _Role
    {
        set
        {
            dic.Remove((byte)ParameterCode.Role);
            dic.Add((byte)ParameterCode.Role, ParaTools.GetJson<Role>(value));
        }
        get { return ParaTools.GetParameter<Role>(dic, ParameterCode.Role); }
    }
    public override void Handle(User user)
    {
        user.HandlePacket(this);
    }

    public override void SetDic(Dictionary<byte, object> para)
    {
        this.dic = para;
    }
}
