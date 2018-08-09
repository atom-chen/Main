/********************************************************************************
 *	文件名：Obj_Char_Move.cs
 *	全路径：	\Script\Obj\Obj_Char_Move.cs
 *	创建人：	李嘉
 *	创建时间：2015-11-04
 *
 *	功能说明：Obj_Char涉及到移动功能部分
 *	修改记录：
*********************************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Games.GlobeDefine;
using Games.Events;
using Games.Table;
using UnityEngine.AI;

namespace Games.LogicObj
{
    public partial class Obj_Char : Obj
    {
        #region Obj移动相关
        
        //是否在移动中
        private bool m_bIsMoving;
        public bool IsMoving
        {
            get { return m_bIsMoving; }
        }

	    private float m_fMoveSpeed = 3.0f;
        public float MoveSpeed
        {
            get { return m_fMoveSpeed; }
            // 剧情中直接设置了navAgent的速度, 导致两者不一致
            // 在外界配置速度<1.5左右的时候, 会误认移动目标被卡住, 在这里加接口同步一下数值试试
            set { m_fMoveSpeed = value; } 
        }
        
        //是否在追踪
        private bool m_bIsTracing = false;
        public bool IsTracing
        {
            get { return m_bIsTracing; }
            set { m_bIsTracing = value; }
        }

        //是否在播放剧情移动
        private bool m_bIsStoryMove = false;
        public bool IsStoryMove
        {
            get { return m_bIsStoryMove; }
            set { m_bIsStoryMove = value; }
        }

        //目的地坐标
        private Vector3 m_vecTargetPos;
        public UnityEngine.Vector3 VecTargetPos
        {
            get { return m_vecTargetPos; }
            set { m_vecTargetPos = value; }
        }
        //移动导航控件
        private UnityEngine.AI.NavMeshAgent m_NavAgent = null;
        public UnityEngine.AI.NavMeshAgent NavAgent
        {
            get { return m_NavAgent; }
            set { m_NavAgent = value; }
        }

        //private float m_fRedTime = 0.0f;

        //寻路结束时间监听函数,每次通知完会清空
        public delegate void DelegateMoveOver();
        private DelegateMoveOver m_delMoveOver;
        public void AddMoveOverNotify(DelegateMoveOver delFun){m_delMoveOver += delFun;}
        public void ClearMoveOverNotify()
        {
            m_delMoveOver = null;
        }

        //初始化寻路代理
        public void InitNavAgent()
        {
            //为了确保玩家身上一定挂有NavMeshAgent，先删除对起引用
            NavAgent = null;

            if (NavAgent == null)
            {
                NavAgent = gameObject.GetComponent<UnityEngine.AI.NavMeshAgent>();

                //如果玩家没有NavMeshAgent，主动给其绑定
                if (NavAgent == null)
                {
                    NavAgent = gameObject.AddComponent<UnityEngine.AI.NavMeshAgent>();
                }
            }

            //初始化自动寻路
            if (null != NavAgent && 0 != gameObject.transform.localScale.x)
            {
                NavAgent.enabled = true;
                NavAgent.speed = MoveSpeed;
                //设置成0，否则gameobject之间会互相碰撞
                NavAgent.radius = 0.1f;
                NavAgent.obstacleAvoidanceType = ObstacleAvoidanceType.NoObstacleAvoidance;
                NavAgent.height = 2.0f / gameObject.transform.localScale.x;
                NavAgent.acceleration = 10000.0f;
                NavAgent.angularSpeed = 30000.0f;
                NavAgent.autoRepath = false;
                NavAgent.autoBraking = true;

                NavAgent.enabled = false;
                NavAgent.enabled = true;
            }
        }

        private NavMeshPath pathTemp;

        public bool IsValidMovePos(Vector3 targetVector3)
        {
            if (!IsNavAgentValid())
            {
                return false;
            }

            try
            {
                if (pathTemp == null) pathTemp = new NavMeshPath();
                return NavAgent.CalculatePath(targetVector3, pathTemp);
            }
            catch (System.Exception e)
            {
                LogModule.ErrorLog("::IsMoveNavAgent:" + e.ToString());
                return false;
            }
        }

        private bool m_bIsMoveToNoFaceTo = false; //调用moveto时 是否禁用了朝向旋转
        public bool IsMoveToNoFaceTo
        {
            get { return m_bIsMoveToNoFaceTo; }
            set { m_bIsMoveToNoFaceTo = value; }
        }

        private bool m_EnableMovingRotation = true;
        public void EnableMovingRotation(bool bEnable)
        {
            if (null != NavAgent)
            {
                if (bEnable)
                {
                    NavAgent.angularSpeed = 30000.0f;
                }
                else
                {
                    NavAgent.angularSpeed = 0.0f;
                }
            }

            m_EnableMovingRotation = bEnable;
        }

        //移动目标
        private GameObject m_MoveTarget = null;
        private int m_MoveAnimId = (int) CHAR_ANIM_ID.Walk;
        public GameObject MoveTarget
        {
            get { return m_MoveTarget; }
            set
            {
                if (value != m_MoveTarget)
                {
                    m_MoveTarget = value;
                    if (null != m_MoveTarget)
                    {
                        m_moveTargetTrans = m_MoveTarget.transform;
                        m_moveTargetLastPos = m_moveTargetTrans.localPosition;
                    }
                    else
                    {
                        m_moveTargetTrans = null;
                    }
                }
            }
        }

        //移动结束点
        private float m_fStopRange;
        public float StopRange
        {
            get { return m_fStopRange; }
            set { m_fStopRange = value; }
        }

        private Vector3 m_LastPosition = new Vector3(0, 0, 0);
        private float m_LastPositionTime = 0.0f;
        public bool m_LastInObstacles = false;

        protected GameEvent m_MoveOverEvent = null;
        public GameEvent MoveOverEvent
        {
            get { return m_MoveOverEvent; }
            set { m_MoveOverEvent = value; }
        }
        void ResetMoveOverEvent()
        {
            if (null != m_MoveOverEvent)
                m_MoveOverEvent.Reset();
        }

        public void BeforeMoveTo(bool bIsAutoSearch)
        {

            //if (false == bIsAutoSearch &&
            //    null != GameManager.AutoSearch &&
            //    true == GameManager.AutoSearch.IsAutoSearching)
            //{
            //    GameManager.AutoSearch.Stop();
            //}

            //if (NavAgent != null)
            //{
            //    NavAgent.enabled = false;
            //    NavAgent.enabled = true;
            //}

        }

		public virtual bool IsCanMove()
        {
			//剧情的移动事件中，不能移动
            if (m_bIsStoryMove)
            {
                return false;
            }

		    if (IsPlayer())
		    {
		        Obj_Player objPlayer = this as Obj_Player;
		        if (objPlayer != null && false == objPlayer.IsRelaxCanMove())
		        {
		            return false;
		        }
		    }

            return true;
        }

        public bool IsNavAgentValid()
        {
            if (NavAgent == null)
            {
                return false;
            }

            if (!NavAgent.enabled)
            {
                return false;
            }

            if (!NavAgent.isOnNavMesh)
            {
                return false;
            }

            return true;
        }

        //移动到npc处,stopRange -1表示走CharModel表，其他使用设置值
        public virtual void MoveTo(Obj_Char target, float fStopRange = -1f, int nMoveAnimID = (int)CHAR_ANIM_ID.Walk)
        {
            if (target == null)
            {
                return;
            }

            Vector3 pos = target.Position;

            if (fStopRange <= -1f)
            {
                Tab_CharModel tab = target.GetBaseCharModelTab();
                if (tab != null)
                {
                    fStopRange = tab.StopRange;
                }
                if (pathTemp == null) pathTemp = new NavMeshPath();
                if (IsNavAgentValid() && NavAgent.CalculatePath(pos, pathTemp))
                {
                    if (pathTemp.status == NavMeshPathStatus.PathPartial)
                    {
                        pos = pathTemp.corners[pathTemp.corners.Length - 1];
                    }
                }
            }

            MoveTo(pos, target.gameObject, fStopRange, nMoveAnimID);
            FaceTo(pos);
        }

        public void MoveTo(Vector3 targetPos, GameObject targetObj = null, float fStopRange = 2.0f, int nMoveAnimID = (int)CHAR_ANIM_ID.Walk)
        {
            //首先判断是否哦可以移动
            if (false == IsCanMove())
            {
                return;
            }

            m_fStopRange = fStopRange;
            if (null != targetObj)
            {
                MoveTarget = targetObj;
            }
            else
            {
                MoveTarget = null;
            }

            m_vecTargetPos = targetPos;

            float fDis = Vector3.Distance(this.Position, m_vecTargetPos);
            float fDiffDis = fDis - fStopRange;

            if (fDiffDis <= 0)
            {
                StopMove();
                ResetMoveOverEvent();
                return;
            }

            //BeforeMoveTo(bIsAutoSearch);
            if (IsNavAgentValid())
            {
                //if (!NavMesh.CalculatePath(Position, m_vecTargetPos, NavMesh.AllAreas, NavAgent.path))
                //{
                //    return;
                //}
                NavAgent.stoppingDistance = fStopRange;
                NavAgent.speed = MoveSpeed; // 已经初始化navagent的情况下更新速度
                if (!NavAgent.SetDestination(m_vecTargetPos))
                {
                    return;
                }
                NavAgent.isStopped = false;
                m_justMove = true;

                if (IsMoveToNoFaceTo)
                {
                    EnableMovingRotation(false);
                }
            }

            // 如果是默认行走动作 且是玩家 需要做一次判断
            if (( nMoveAnimID == (int)CHAR_ANIM_ID.Run 
                    || nMoveAnimID == (int)CHAR_ANIM_ID.Walk)
                && IsPlayer())
            {
                Obj_Player objPlayer = this as Obj_Player;
                if (objPlayer != null)
                {
                    int nRelaxMoveAnim = objPlayer.GetRelaxAnimMoveAnim();
                    if (nRelaxMoveAnim != GlobeVar.INVALID_ID)
                    {
                        nMoveAnimID = nRelaxMoveAnim;
                    }

                    if (objPlayer.IsRelaxWithCard && objPlayer.CallCard != null)
                    {
                        int nCardMoveAnim = objPlayer.CallCard.GetRelaxAnimMoveAnim();
                        if (nCardMoveAnim != GlobeVar.INVALID_ID)
                        {
                            objPlayer.CallCard.PlayAnim(nCardMoveAnim);
                        }
                        else
                        {
                            objPlayer.CallCard.PlayAnim((int)CHAR_ANIM_ID.Walk);
                        }
                    }

                    if (objPlayer.IsRelaxWithHero && objPlayer.RelaxPlayer != null)
                    {
                        int nReceiverMoveAnim = objPlayer.RelaxPlayer.GetRelaxAnimMoveAnim();
                        if (nReceiverMoveAnim != GlobeVar.INVALID_ID)
                        {
                            objPlayer.RelaxPlayer.PlayAnim(nReceiverMoveAnim);
                        }
                        else
                        {
                            objPlayer.RelaxPlayer.PlayAnim((int)CHAR_ANIM_ID.Run);
                        }
                    }
                }
            }

            //PlayAnim(nMoveAnimID);
            m_MoveAnimId = nMoveAnimID;
            m_bIsMoving = true;

            // 如果是玩家在移动教学
            if (IsMainPlayer() && TutorialRoot.IsGroupStep(TutorialGroup.MoveRot, 2))
            {
                TutorialRoot.TutorialOver();
            }
        }

        public void StopMove(bool bAddNotify = true)
        {
            StoryCopyAssistantManager.IsMoving2Target = false;
            if (IsNavAgentValid())
            {
                NavAgent.ResetPath();
                NavAgent.isStopped = true;
                if (IsMoveToNoFaceTo)
                {
                    EnableMovingRotation(true);
                    IsMoveToNoFaceTo = false;
                }
            }


            m_bIsMoving = false;
            m_bIsTracing = false;
           
            if (IsPlayer())
            {
                Obj_Player objPlayer = this as Obj_Player;
                if (objPlayer != null)
                {
                    int nStandAnim = objPlayer.GetRelaxAnimStandAnim();
                    if (nStandAnim != GlobeVar.INVALID_ID)
                    {
                        PlayAnim(nStandAnim);
                    }
                    else
                    {
                        PlayAnim((int)CHAR_ANIM_ID.Stand);
                    }

                    if (objPlayer != null)
                    {
                        if (objPlayer.IsRelaxWithCard && objPlayer.CallCard != null)
                        {
                            int nCardStandAnim = objPlayer.CallCard.GetRelaxAnimStandAnim(objPlayer);
                            if (nCardStandAnim != GlobeVar.INVALID_ID)
                            {
                                objPlayer.CallCard.PlayAnim(nCardStandAnim);
                            }
                            else
                            {
                                objPlayer.CallCard.PlayAnim((int)CHAR_ANIM_ID.Stand);
                            }
                        }

                        if (objPlayer.IsRelaxWithHero && objPlayer.RelaxPlayer != null)
                        {
                            int nReceiverStandAnim = objPlayer.RelaxPlayer.GetRelaxAnimStandAnim();
                            if (nReceiverStandAnim != GlobeVar.INVALID_ID)
                            {
                                objPlayer.RelaxPlayer.PlayAnim(nReceiverStandAnim);
                            }
                            else
                            {
                                objPlayer.RelaxPlayer.PlayAnim((int)CHAR_ANIM_ID.Stand);
                            }
                        }
                    }
                }
            }
            else
            {
                PlayAnim((int)CHAR_ANIM_ID.Stand);
            }

            float fDis = Vector3.Distance(Position, m_vecTargetPos);
            if (m_fStopRange >= fDis)
            {
                OnMoveOver(bAddNotify);
            }

            ResetMoveOverEvent();

            if (null != m_delMoveOver)
            {
                var tmp = m_delMoveOver;
                m_delMoveOver = null;
                tmp();
            }

            m_LastPosition = Vector3.zero;
            m_LastPositionTime = 0;
            m_LastInObstacles = false;


        }

        public void FaceToScreen()
        {
			if (null != Camera.main)
				ObjTransform.LookAt (Camera.main.gameObject.transform.position);
        }

        public void FaceTo(Vector3 facePos)
        {
            if (!m_EnableMovingRotation)
            {
                return;
            }

            Vector3 lookRot = facePos - ObjTransform.position;
            lookRot.y = 0;
            if (lookRot == Vector3.zero)
            {
                return;
            }

            ObjTransform.rotation = Quaternion.LookRotation(lookRot);
        }

        private Coroutine lastFaceTo;
        public Coroutine FaceToWithTween(Vector3 facePos,float tweenTime = 0.2f)
        {
            if (m_lastRotateTo != null)
            {
                StopCoroutine(m_lastRotateTo);
                m_lastRotateTo = null;
            }

            if (lastFaceTo != null)
            {
                StopCoroutine(lastFaceTo);
            }
            lastFaceTo = StartCoroutine(_faceTo(facePos,tweenTime));
            return lastFaceTo;
        }

        public void StopFaceToWithTween(Vector3 facePos)
        {
            if (null != lastFaceTo)
            {
                StopCoroutine(lastFaceTo);
                lastFaceTo = null;

                FaceTo(facePos);
            }
        }

        private IEnumerator _faceTo(Vector3 facePos,float rotTime = 0.2f)
        {
            if (!m_EnableMovingRotation)
            {
                yield break;
            }

            Vector3 lookRot = facePos - ObjTransform.position;
            lookRot.y = 0;
            if (lookRot == Vector3.zero)
            {
                yield break;
            }

            Quaternion target = Quaternion.LookRotation(lookRot);
            Quaternion begin = ObjTransform.rotation;
            float timer = 0f;
            if (rotTime <= 0.00001f)
            {
                ObjTransform.rotation = target;
                yield break;
            }
            while (timer <= rotTime)
            {
                timer += Time.deltaTime;
                ObjTransform.rotation = Quaternion.Lerp(begin, target, timer/rotTime);
                yield return null;
            }
        }

        //rotation
        private Coroutine m_lastRotateTo;
        private Quaternion m_targetRotateTo;
        public Coroutine RotateToWithTween(Vector3 rot, float tweenTime)
        {
            if (null != lastFaceTo)
            {
                StopCoroutine(lastFaceTo);
                lastFaceTo = null;
            }

            if (m_lastRotateTo != null)
            {
                StopCoroutine(m_lastRotateTo);
            }
            m_lastRotateTo = StartCoroutine(_rotateTo(rot, tweenTime));
            return lastFaceTo;
        }
        public void StopRotateToWithTween()
        {
            if (null != m_lastRotateTo)
            {
                StopCoroutine(m_lastRotateTo);
                m_lastRotateTo = null;

                if (null != ObjTransform)
                {
                    ObjTransform.rotation = m_targetRotateTo;
                }
            }
        }

        private IEnumerator _rotateTo(Vector3 rot, float rotTime = 0.2f)
        {
            Quaternion begin = ObjTransform.rotation;
            m_targetRotateTo = Quaternion.Euler(rot) * begin;

            if (rot == Vector3.zero)
            {
                yield break;
            }

            if (rotTime <= 0.00001f)
            {
                ObjTransform.rotation = m_targetRotateTo;
                yield break;
            }

            float timer = 0.0f;
            while (timer < rotTime)
            {
                timer += Time.deltaTime;
                ObjTransform.rotation = Quaternion.Lerp(begin, m_targetRotateTo, timer / rotTime);
                yield return null;
            }
        }

        Transform m_moveTargetTrans = null;
        private Vector3 m_moveTargetLastPos;
        private bool m_justMove = false;

        protected void UpdateTargetMove()
        {
            if (m_bIsMoving)
            {
                if (null != MoveTarget && null != m_moveTargetTrans)
                {
                    //追踪过程 目标发生变动了
                    if (m_moveTargetTrans.localPosition != m_moveTargetLastPos)
                    {
                        m_vecTargetPos = m_moveTargetTrans.localPosition;
                        if (IsNavAgentValid())
                        {
                            NavAgent.stoppingDistance = m_fStopRange;
                            if (!NavAgent.SetDestination(m_vecTargetPos))
                            {
                                StopMove();
                                return;
                            }
                            NavAgent.isStopped = false;

                            if (IsMoveToNoFaceTo)
                            {
                                EnableMovingRotation(false);
                            }
                        }
                        m_moveTargetLastPos = m_moveTargetTrans.localPosition;
                    }
                }

                if (m_justMove && IsNavAgentValid() && !NavAgent.isStopped)
                {
                    if (!NavAgent.hasPath)
                    {
                        return;
                    }
                    else
                    {
                        m_justMove = false;
                    }
                }

                MoveToPosition(m_vecTargetPos, m_fStopRange);
            }
        }

        public virtual void OnMoveOver(bool bAddNotify = true)
        {
        }

        // 是否卡死在阻挡里
        public bool IsInObstacles() 
        {
            // 获得当前坐标
            if (ObjTransform == null)
            {
                return false;
            }

            Vector3 vecPos = ObjTransform.position;
            if (MoveSpeed > 1.5f &&
                m_LastPositionTime > 0 &&
                Vector3.Distance(m_LastPosition, vecPos) <= 0.2f && //移动距离过短
                Vector3.Distance(m_LastPosition, vecPos) <= MoveSpeed * (Time.time - m_LastPositionTime) - 0.2f //照当前的移动速度去移动未达到应该的距离
                )
            {
                return true;
            }

            Tab_SceneClass tsc = TableManager.GetSceneClassByID(GameManager.RunningScene, 0);
            if (tsc == null)
                return false;

            return false;
        }

        //防卡死校验间隔
        //static float s_MovingCheckInterval = 0.2f;
        protected void MoveToPosition(Vector3 targetPos, float fStopRange)
        {
            //获得当前坐标
            Vector3 vecPos = ObjTransform.position;

            ////由于移动其实是在2D平面进行距离判定，所以直接将y置0即可
            //float fDistance = Vector2.Distance(new Vector2(vecPos.x, vecPos.z), new Vector2(targetPos.x, targetPos.z));

            ////阻挡卡怪问题 上次移动校验和本次移动坐标相同 停止移动
            //if (Time.time - m_LastPositionTime > s_MovingCheckInterval)
            //{
            //    //发现两次校验间隔移动距离过小，则停止移动
            //    if (IsInObstacles())
            //    {
            //        //如果是其他玩家 NPC 则将挪到目标点
            //        if (ObjType == OBJ_TYPE.OBJ_NPC)
            //        {
            //            ObjTransform.position = targetPos;
            //            if (NavAgent != null && NavAgent.enabled == true)
            //            {
            //                ObjTransform.position += new Vector3(0, NavAgent.baseOffset, 0);
            //            }
            //        }
            //        StopMove();
            //        m_LastInObstacles = true; // 这里检测被卡住了
            //        return;
            //    }
            //    m_LastPositionTime = Time.time;
            //    m_LastPosition = vecPos;
            //    m_LastInObstacles = false;
            //}

            if ( null != NavAgent && NavAgent.enabled && NavAgent.desiredVelocity.sqrMagnitude <= 0.1f)
            {
                StopMove();
            }
            else if (fStopRange >= Vector3.Distance(vecPos,targetPos))
            {
                StopMove();
            }
            else
            {
                if (m_CurAnimId != m_MoveAnimId)
                {
                    if (!PlayAnim(m_MoveAnimId) &&
                        m_MoveAnimId == (int)CHAR_ANIM_ID.Run)
                    {
                        if (PlayAnim((int)CHAR_ANIM_ID.Walk))
                        {
                            m_MoveAnimId = (int)CHAR_ANIM_ID.Run;
                        }
                    }
                }
            }


            if (null == NavAgent || NavAgent.enabled == false)
            {
                Vector3 vecMovDirction = targetPos - vecPos;
                vecMovDirction = vecMovDirction.normalized;
                vecMovDirction *= MoveSpeed;
                vecMovDirction *= Time.deltaTime;

                Vector3 pos = vecPos + vecMovDirction;
                ObjTransform.position = pos;

                if (IsPlayer())
                {
                    Obj_Player objPlayer = this as Obj_Player;
                    if (objPlayer != null)
                    {
                        if (objPlayer.IsRelaxWithCard && objPlayer.CallCard != null)
                        {
                            int nCardMoveAnim = objPlayer.CallCard.GetRelaxAnimMoveAnim();
                            if (nCardMoveAnim != GlobeVar.INVALID_ID)
                            {
                                objPlayer.CallCard.PlayAnim(nCardMoveAnim);
                            }
                            else
                            {
                                objPlayer.CallCard.PlayAnim((int)CHAR_ANIM_ID.Walk);
                            }
                        }

                        if (objPlayer.IsRelaxWithHero && objPlayer.RelaxPlayer != null)
                        {
                            int nReceiverMoveAnim = objPlayer.RelaxPlayer.GetRelaxAnimMoveAnim();
                            if (nReceiverMoveAnim != GlobeVar.INVALID_ID)
                            {
                                objPlayer.RelaxPlayer.PlayAnim(nReceiverMoveAnim);
                            }
                            else
                            {
                                objPlayer.RelaxPlayer.PlayAnim((int)CHAR_ANIM_ID.Run);
                            }
                        }
                    }
                }

                FaceTo(pos);
            }
        }

        #endregion

        protected override OBJ_TYPE _getObjType()
        {
            return OBJ_TYPE.OBJ_CHAR;
        }

        private float m_fPlayEffectInterval = 0.15f;      //每次播放脚步特效间隔
        private float m_fLastEffectTime = 0.0f;          //上次播放脚步特效时间

        //脚步主要包括两方面，一个是踩在不同地面的特效，一个是声音
       // public void UpdateStep()
       // {
        //    //非移动状态不更新脚步
        //    if (!IsMoving)
        //        return;

        //    //由于当前速度有可能加快或者减慢，所以效果播放间隔也需要变化
        //    //间隔的频率根据当前移动速度除以标准移动速度（3.0）得到周期
        //    if (MoveSpeed <= 0.0f)
        //        return;

        //    float fSpeedRate = 3.0f / MoveSpeed;
        //    float fRealInterval = m_fPlayEffectInterval * fSpeedRate;

        //    //更新时间未到，不进行更新
        //    if (Time.time - m_fLastEffectTime < fRealInterval)
        //    {
        //        return;
        //    }
        //    else
        //    {
        //        //设置这次播放特效的时间
        //        m_fLastEffectTime = Time.time;
        //    }

        //    //目前的实现方式是：
        //    //脚步声，如果在坐骑上优先播放坐骑的声音，否则根据不同地形播放不同脚步声
        //    //特效，根据不同地形播放特效
        //    //bool bPlayTerrainEffect = true;
        //    //int nEffectID = GlobeVar.INVALID_ID;
        //    int nStepSoundID = GlobeVar.STEPSOUND_XIAOQI_NORMAL;

        //    /*
        //    if (IsOnMount())
        //    {
        //        Tab_MountBase tabMountBase = TableManager.GetMountBaseByID(MountID, 0);
        //        if (null != tabMountBase)
        //        {
        //            Tab_CharMount tabCharMount = TableManager.GetCharMountByID(GetPlayerCurAvatarModelId("Mount"), 0);
        //            if (null != tabCharMount)
        //            {
        //                bPlayTerrainEffect = tabCharMount.AffectByTerrain == 1 ? true : false;
        //                nStepSoundID = tabCharMount.SoundID;
        //            }
        //        }
        //    }
        //    */

        //    if (IsMainPlayer())
        //    {
        //        Obj_Player objPlayer = this as Obj_Player;
        //        if (objPlayer != null)
        //        {
        //            nStepSoundID = objPlayer.GetStepSound();
        //        }
        //    }

        //    //根据前面确认的特效和脚步声ID，来确认是否播放
        //    //if (nEffectID > 0)
        //        //PlayEffect(nEffectID);
        //    if (nStepSoundID > 0)
        //    {
        //        GameManager.SoundManager.PlaySoundEffect(nStepSoundID);
        //    }  
       // }
    }
}
