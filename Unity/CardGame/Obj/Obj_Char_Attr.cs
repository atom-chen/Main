
using UnityEngine;
using System.Collections;


namespace Games.LogicObj
{

    public partial class Obj_Char : Obj
    {
        // 服务器ID
        private int m_ServerID;
        public int ServerID
        {
            get { return m_ServerID; }
            set { m_ServerID = value; }
        }

        // 头顶信息板高度调整
        protected float m_NameDeltaHeight;
        public float NameDeltaHeight
        {
            get { return m_NameDeltaHeight; }
            set { m_NameDeltaHeight = value; }
        }

        public float DamageBoardHeight { private set; get; }


        //名字
        protected string m_RoleName = "";
        public string RoleName
        {
            get { return m_RoleName; }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    return;
                }

                if (value.Length > 0 && value[0] == '#')
                {
                    m_RoleName = StrDictionary.GetServerDictionaryFormatString(value);
                }
                else
                {
                    m_RoleName = value;
                }
            }
        }

        public bool IsBoss = false;

    }
}
