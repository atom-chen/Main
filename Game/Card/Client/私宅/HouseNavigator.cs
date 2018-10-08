using UnityEngine;
using Games.LogicObj;
using Games.Table;
using Games.GlobeDefine;
using System.Collections.Generic;
using System.Linq;

public class HouseNavigator : MonoBehaviour
{
    // config 
    public float range;
    public float stepLenMax;
    public float waitTimeMin;
    public float waitTimeMax;
    public float stopTimeMin;
    public float stopTimeMax;

    // logic
    private Transform mCachedTrans;
    private Vector3 mNextStop;

    private Obj_Char mAgent;
    private int[] mAnimList;

    private bool mStart = false;
    
    private enum State
    {
        NONE,
        ENTER_WAITING,
        WAITING,
        ENTER_IDLE,
        IDLE,
        ENTER_WALKING,
        WALKING, 
    }
    private State mState = State.NONE;
    
    void Awake()
    {
        mCachedTrans = transform;
        mNextStop = mCachedTrans.position;
    }

    private float mStateTimer = 0;

    void Update()
    {
        if (!mStart)
            return;
        if (mAgent == null || mAnimList == null)
            return;

        switch (mState)
        {
            case State.NONE:
                EnterState(State.ENTER_WALKING);
                break;
            case State.ENTER_WALKING:
                float r = range;
                if (r < 0)
                    r = -r;
                Vector3 nextPoint = mCachedTrans.position;
                nextPoint.x += Random.Range(-r, r);
                nextPoint.z += Random.Range(-r, r);
                float dist = Vector3.Distance(nextPoint, mAgent.ObjTransform.position);
                if (dist > r)
                {
                    if (dist < 0.0001 && dist > -0.0001)
                    {
                        // 避免死循环, 不回到none
                        EnterState(State.ENTER_IDLE);
                        break;
                    }
                    mNextStop = mAgent.ObjTransform.position + (nextPoint - mAgent.ObjTransform.position) * r / dist;
                }
                else
                {
                    mNextStop = nextPoint;
                }

                // refix后可能会稍微出圈, 可容忍
                RefixGroudPos(ref mNextStop);
                
                mAgent.ClearMoveOverNotify();
                mAgent.AddMoveOverNotify(DelegateMoveOver);
                mAgent.FaceTo(mNextStop);
                mAgent.MoveTo(mNextStop, null, 0.1f);
                EnterState(State.WALKING);
                break;
            case State.WALKING:
                // just waiting...
                break;
            case State.ENTER_WAITING:
                mStateTimer = Random.Range(waitTimeMin, waitTimeMax);
                EnterState(State.WAITING);
                break;
            case State.WAITING:
                mStateTimer -= Time.deltaTime;
                if (mStateTimer <= 0)
                {
                    EnterState(State.ENTER_IDLE);
                }
                break;
            case State.ENTER_IDLE:
                if (mAnimList.Length > 0)
                {
                    int animindex = Random.Range(0, mAnimList.Length);
                    mAgent.PlayAnim(mAnimList[animindex]);
                }
                mStateTimer = Random.Range(stopTimeMin, stopTimeMax);
                EnterState(State.IDLE);
                break;
            case State.IDLE:
                //idle状态中被叫停交互时, 可能保留idle状态和时长, 所以严格采用上一帧时长
                mStateTimer -= Time.deltaTime;
                if (mStateTimer <= 0)
                {
                    EnterState(State.ENTER_WALKING);
                }
                break;
        }
    }

    private void EnterState(State s)
    {
        mState = s;
    }

    private void DelegateMoveOver()
    {
        if (!mStart || mState != State.WALKING)
            return;

        EnterState(State.ENTER_WAITING);
    }
    
    public void SetAgent(Obj_Char a, int[] al)
    {
        if (a == null || al == null)
            return;

        mAgent = a;
        mAnimList = al;
    }

    public bool HasAgent()
    {
        return mAgent != null;
    }

    public bool HasAgent(Obj_Char agent)
    {
        return agent != null && mAgent == agent;
    }

    public void ClearAgent()
    {
        mAgent = null;
        mAnimList = null;
    }

    public void Run(float speed)
    {
        mStart = true;
        mAgent.MoveSpeed = speed;
        EnterState(State.NONE);
    }

    public void Stop()
    {
        mStart = false;
        if (mAgent != null)
        {
            mAgent.StopMove(false);
        }
    }

    void OnDrawGizmos()
    {
        if (mCachedTrans == null)
            return;

        Gizmos.color = Color.red;
        Gizmos.DrawSphere(mCachedTrans.position, 0.15f);
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(mNextStop, 0.15f);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(mCachedTrans.position, range);
    }

    bool RefixGroudPos(ref Vector3 pos)
    {
        UnityEngine.AI.NavMeshHit hit;
        bool ret = UnityEngine.AI.NavMesh.SamplePosition(pos, out hit, 100f, UnityEngine.AI.NavMesh.AllAreas);
        if (!ret)
        {
            return false;
        }
        pos = hit.position;
        return true;
    }

    [ContextMenu("ForceGround")]
    void ForceGround()
    {
        Vector3 groudPos = mCachedTrans.position;
        if (RefixGroudPos(ref groudPos))
        {
            mCachedTrans.position = groudPos;
        }
        mNextStop = mCachedTrans.position;
    }

    [Header("Agent Attributes")]
    public int testAgentCard;

    [ContextMenu("LoadAgentAndStart")]
    void LoadAgent()
    {
        if (!Application.isPlaying)
        {
            Debug.LogError("请启动游戏后使用");
            return;
        }

        if (mAgent != null)
        {
            Debug.LogError("已绑定符灵");
            return;
        }

        var tab = TableManager.GetCardByID(testAgentCard, 0);
        if (tab == null)
        {
            Debug.LogError("符灵不存在");
            return;
        }

        var tabRole = TableManager.GetRoleBaseAttrByID(tab.GetRoleBaseIDStepbyIndex(0))[0];
        if (tabRole == null)
        {
            Debug.LogError("Rolebase不存在");
            return;
        }
        var tabExtend = TableManager.GetCardExtendByID(CardTool.GetCardDefaultExtendId(testAgentCard), 0);
        if (tabExtend==null)
        {

            Debug.LogError("符灵不存在");
            return;
        }
        var anims = CardTool.GetAnimList(tabExtend);

        Obj_Init_Data initData = new Obj_Init_Data();
        initData.m_nModelID = tabRole.CharModelID;
        initData.m_CreatePos = mCachedTrans.position;
        Obj_Card obj = ObjManager.CreateCard(initData, "NavTest");
        SetAgent(obj, anims);

        Run(obj.MoveSpeed);
    }
}
