using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public enum MD_MsgCode
{
    LOGIN,
    REGISTER,
    ROLE_ADD,
    MAX
}
public enum DM_MsgCode
{
    USER_CONNECT,
    USER,
    ROLE,
}

public enum RETURN_CODE
{
    SUCCESS,
    FAILD,
}

public enum ParaCode
{
    IPADDREA,

    USER ,
    USER_ID,
    USER_NAME,
    USER_PWD,

    ROLE ,
    ROLELIST ,
    ROLE_NAME,
    ROLE_SEX,
    ROLE_OWNER,

    ITEM ,
    ITEMLIST ,
    EQUIPLIST,

    ERRORINFO,
    ERRORID,      
}