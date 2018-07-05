
using UnityEngine;
using System.Collections;
using Games.GlobeDefine;
using System;

namespace Games.LogicObj
{

    abstract public class Obj : MonoBehaviour
    {
        public Obj()
        {
           
        }


        private Int32 _objId;
        public Int32 ObjID
        {
            get { return _objId; }
            set { _objId = value; }
        }

        #region Obj及其派生类共有基础属性
        protected abstract OBJ_TYPE _getObjType();

        public OBJ_TYPE ObjType
        {
            get { return _getObjType(); }
        }


        //基础信息数据，包括暂存的ObjTransform，以及位置、旋转、缩放等基础信息
        public Vector3 Position
        {
            get { return ObjTransform.localPosition; }
            set { ObjTransform.localPosition = value; }
        }


        private Transform m_ObjTransform = null;
        public Transform ObjTransform
        {
            get
            {
                if (null == m_ObjTransform)
                {
                    m_ObjTransform = transform;
                }
                return m_ObjTransform;
            }
        }

        #endregion

        #region 类型判断
        public bool IsPlayer()
        {
            return (ObjType == OBJ_TYPE.OBJ_PLAYER);
        }

        public bool IsMainPlayer()
        {
            if (null == ObjManager.MainPlayer)
            {
                return false;
            }

            return (this == ObjManager.MainPlayer);
        }


        public bool IsNpc()
        {
            return (ObjType == OBJ_TYPE.OBJ_NPC);
        }

        public bool IsCard()
        {
            return ObjType == OBJ_TYPE.OBJ_CARD;
        }

        public bool IsMyCallCard()
        {
            if (ObjManager.MainPlayer == null)
            {
                return false;
            }

            return ObjType == OBJ_TYPE.OBJ_CARD && this == ObjManager.MainPlayer.CallCard;
        }

        public bool IsTalisman()
        {
            return ObjType == OBJ_TYPE.OBJ_TALISMAN;
        }

        #endregion


    }


}
