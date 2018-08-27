using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public abstract partial class GC_PAK_BASE
{
    public OperationCode  _OperationCode 
    {
        get { return (OperationCode)_Response.OperationCode; }
        protected set { _Response.OperationCode = (byte)value; }
    }

    public ReturnCode _ReturnCode
    {
        get { return (ReturnCode)_Response.ReturnCode; }
        set { _Response.ReturnCode = (short)value; }
    }

    public  string ErrorInfo
    {
        get { return ParaTools.GetErrInfo(_Response.Parameters); }
        set 
        { 
            _Response.Parameters.Remove((byte)ParameterCode.ErrorInfo);
            _Response.Parameters.Add((byte)ParameterCode.ErrorInfo, value); 
        }
    }

    public GC_PAK_BASE()
    {

    }

    public bool Success
    {
        get { return _ReturnCode == ReturnCode.Success; }
    }


}

