using Games.GlobeDefine;
using Games.LogicObj;
using UnityEngine;

//法宝角色

public class Obj_Talisman : Obj_Char
{
    private Obj_Char m_BindPlayer = null;
    public Obj_Char BindPlayer
    {
        get { return m_BindPlayer; }
    }

    private bool m_ModelOver = false;   // 模型是否加载完成
    private bool m_MoveFollowPos = true;   // 是否正在从其他位置移动到玩家身后状态中

    protected override OBJ_TYPE _getObjType()
    {
        return OBJ_TYPE.OBJ_TALISMAN;
    }

    void Update()
    {
        UpdateMoveFollowPos();
        UpdateFollowPos();
    }

    public void InitBind(Obj_Char bindPlayer)
    {
        m_BindPlayer = bindPlayer;

        if (m_BindPlayer == null)
        {
            return;
        }

        InitNavAgent();
    }

    public void HandleLoadModelOver()
    {
        m_ModelOver = true;
    }

    void UpdateMoveFollowPos()
    {
        if (false == m_ModelOver || false == m_MoveFollowPos || m_BindPlayer == null)
        {
            return;
        }

        Vector3 pos = ObjTransform.position;
        Vector3 targetPos = m_BindPlayer.ObjTransform.position + (m_BindPlayer.ObjTransform.right - m_BindPlayer.ObjTransform.forward).normalized;

        if (Vector3.Distance(pos, m_BindPlayer.ObjTransform.position) >= 1.0f)
        {
            m_MoveFollowPos = false;
            ObjTransform.rotation = m_BindPlayer.ObjTransform.rotation;
            return;
        }

        MoveTo(targetPos, null, 0.01f);
    }

    void UpdateFollowPos()
    {
        if (false == m_ModelOver || m_MoveFollowPos || m_BindPlayer == null)
        {
            return;
        }

        Vector3 playerPos = m_BindPlayer.ObjTransform.position;
        if (Vector3.Distance(ObjTransform.position, playerPos) < 1.5f)
        {
            return;
        }

        Vector3 pos = ObjTransform.position;
        Vector3 targetPos = playerPos + (m_BindPlayer.ObjTransform.right - m_BindPlayer.ObjTransform.forward).normalized;

        if (Vector3.Distance(pos, targetPos) <= 0.01f)
        {
            return;
        }

        MoveTo(targetPos, null, 0.01f);
    }
}