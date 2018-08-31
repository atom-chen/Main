using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


class ItemMsg:MD_MsgBase
{
    public override MD_MsgCode code
    {
        get { return MD_MsgCode.MAX; }
    }

}

