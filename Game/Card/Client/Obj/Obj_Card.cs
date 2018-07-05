using System.Collections;
using System.Collections.Generic;
using Games.GlobeDefine;
using Games.LogicObj;
using Games.Table;
using UnityEngine;

namespace Games.LogicObj
{
    public class Obj_Card : Obj_Char
    {
        private Obj_Player m_BindPlayer = null;
        public Obj_Player BindPlayer
        {
            get { return m_BindPlayer; }
        }

        private int m_CardId = GlobeVar.INVALID_ID;
        public int CardId
        {
            get { return m_CardId; }
        }

        private List<int> m_RelaxAnimId = new List<int>();
        private bool m_WaitRelax = false;   // 等待播放休闲动作
        private bool m_ModelOver = false;   // 模型是否加载完成
        private bool m_MoveBeginPos = true;   // 是否正在从其他位置移动到玩家身后状态中
        private bool m_IsPlayerRelaxWith = false;   // 是否正在和玩家互动
        private bool m_IsBindMove = false;  // 是否被绑定移动

        private bool m_IsMoving = false;
        public bool IsMoving
        {
            get { return m_IsMoving; }
            set { m_IsMoving = value; }
        }

        private int m_PlayRealSound = GlobeVar.INVALID_ID;

        private const int RandomRelaxTimeMin = 30;
        private const int RandomRelaxTimeMax = 60;

        protected override OBJ_TYPE _getObjType()
        {
            return OBJ_TYPE.OBJ_CARD;
        }

        void Update()
        {
            UpdateMoveBeginPos();
            UpdateFollowPos();
        }

        public void InitBind(Obj_Player bindPlayer, int nCardId)
        {
            m_BindPlayer = bindPlayer;
            m_CardId = nCardId;

            if (m_BindPlayer == null)
            {
                return;
            }

            Tab_Card tCard = TableManager.GetCardByID(nCardId, 0);
            if (tCard == null)
            {
                return;
            }

            Tab_RoleBaseAttr tRoleBase = TableManager.GetRoleBaseAttrByID(tCard.GetRoleBaseIDStepbyIndex(0), 0);
            if (tRoleBase == null)
            {
                return;
            }

            InitNavAgent();

            RoleName = tRoleBase.Name;

            if (m_BindPlayer.IsMainPlayer())
            {
                m_RelaxAnimId.Clear();
                foreach (var pair in TableManager.GetRelaxAnim())
                {
                    if (pair.Value == null || pair.Value.Count < 1)
                    {
                        continue;
                    }

                    Tab_RelaxAnim tRelax = pair.Value[0];
                    if (tRelax == null)
                    {
                        continue;
                    }

                    if (tRelax.ClassId != (int)RelaxAnimClassId.Card)
                    {
                        continue;
                    }

                    if (tRelax.CardId == GlobeVar.INVALID_ID || tRelax.CardId == nCardId)
                    {
                        m_RelaxAnimId.Add(tRelax.Id);
                    }
                }
            }

            m_WaitRelax = false;
        }

        public void HandleLoadModelOver()
        {
            m_ModelOver = true;
        }

        public void SetPlayerRelaxWith(bool bRelax, bool bBindMove = false)
        {
            bool bOldBindMove = m_IsBindMove;

            m_IsPlayerRelaxWith = bRelax;
            m_IsBindMove = bBindMove;

            if (bOldBindMove && m_IsBindMove == false)
            {
                // 结束绑定交互 需要重新回到随从位
                m_MoveBeginPos = true;
            }
        }

        void UpdateMoveBeginPos()
        {
            if (m_IsBindMove || false == m_ModelOver || false == m_MoveBeginPos || m_BindPlayer == null)
            {
                return;
            }

            Vector3 pos = ObjTransform.position;
            Vector3 targetPos = m_BindPlayer.ObjTransform.position + (- m_BindPlayer.ObjTransform.right - m_BindPlayer.ObjTransform.forward).normalized;

            if (Vector3.Distance(pos, m_BindPlayer.ObjTransform.position) >= 1.0f || (m_IsMoving && NavAgent != null && false == NavAgent.hasPath))
            {
                m_MoveBeginPos = false;
                ObjTransform.rotation = m_BindPlayer.ObjTransform.rotation;

                if (m_IsMoving)
                {
                    if (m_BindPlayer.IsMainPlayer())
                    {
                        OnMyCallCardMoveStop();
                    }

                    m_IsMoving = false;
                }

                return;
            }

            MoveTo(targetPos, null, 0.01f);
            m_IsMoving = true;
        }

        void UpdateFollowPos()
        {
            if (m_IsBindMove || false == m_ModelOver || m_MoveBeginPos || m_BindPlayer == null)
            {
                return;
            }

            Vector3 playerPos = m_BindPlayer.ObjTransform.position;
            if (Vector3.Distance(ObjTransform.position, playerPos) < 1.5f)
            {
                if (m_IsMoving)
                {
                    if (m_BindPlayer.IsMainPlayer())
                    {
                        OnMyCallCardMoveStop();
                    }

                    m_IsMoving = false;
                }
                
                return;
            }

            Vector3 pos = ObjTransform.position;
            Vector3 targetPos = playerPos + (- m_BindPlayer.ObjTransform.right - m_BindPlayer.ObjTransform.forward).normalized;

            if (Vector3.Distance(pos, targetPos) <= 0.01f || (m_IsMoving && NavAgent != null && false == NavAgent.hasPath))
            {
                if (m_IsMoving)
                {
                    if (m_BindPlayer.IsMainPlayer())
                    {
                        OnMyCallCardMoveStop();
                    }

                    m_IsMoving = false;
                }

                return;
            }

            MoveTo(targetPos, null, 0.01f);
            m_IsMoving = true;

            if (m_BindPlayer.IsMainPlayer())
            {
                m_WaitRelax = false;
                CancelInvoke("TimePlayRelaxAnim");
            }
        }

        void OnMyCallCardMoveStop()
        {
            if (m_BindPlayer == null || m_BindPlayer.IsInRelaxAnim())
            {
                return;
            }

            Invoke("TimePlayRelaxAnim", Random.Range(RandomRelaxTimeMin, RandomRelaxTimeMax + 1));
        }

        void TimePlayRelaxAnim()
        {
            if (m_BindPlayer == null || m_BindPlayer.IsInRelaxAnim())
            {
                return;
            }

            if (GameManager.CurScene == null || m_RelaxAnimId.Count <= 0)
            {
                return;
            }

            int nRelaxAnimId = m_RelaxAnimId[Random.Range(0, m_RelaxAnimId.Count)];

            if (GameManager.CurScene.IsRealTimeScene())
            {
                // 多人场景发包
                CG_CARD_RELAXANIM_PAK pak = new CG_CARD_RELAXANIM_PAK();
                pak.data.CardGuid = GameManager.PlayerDataPool.PlayerCardBag.CallCardGuid;
                pak.data.RelaxAnimId = nRelaxAnimId;
                pak.SendPacket();
            }
            else
            {
                // 单人场景直接动
                PlayRelaxAnim(nRelaxAnimId);
            }
        }

        public void PlayRelaxAnim(int nRelaxAnimId)
        {
            Tab_RelaxAnim tRelax = TableManager.GetRelaxAnimByID(nRelaxAnimId, 0);
            if (tRelax == null)
            {
                return;
            }

            if (tRelax.ClassId != (int)RelaxAnimClassId.Card)
            {
                return;
            }

            if (tRelax.CardId != GlobeVar.INVALID_ID && tRelax.CardId != m_CardId)
            {
                return;
            }

            if (tRelax.AnimId != GlobeVar.INVALID_ID)
            {
                PlayAnim(tRelax.AnimId);
            }

            if (false == string.IsNullOrEmpty(tRelax.Bubble))
            {
                Bubble(tRelax.Bubble, tRelax.BubbleTime);
            }

            if (IsMyCallCard())
            {
                if (GameManager.SoundManager.GetCurRealSoundId() == m_PlayRealSound)
                {
                    GameManager.SoundManager.StopRealSound();
                }

                m_PlayRealSound = GlobeVar.INVALID_ID;
                if (tRelax.CardVoice != GlobeVar.INVALID_ID)
                {
                    m_PlayRealSound = tRelax.CardVoice;
                    GameManager.SoundManager.PlayRealSound(tRelax.CardVoice);
                }
            }

            if (m_BindPlayer != null && false == m_BindPlayer.IsInRelaxAnim())
            {
                // 等待下一次休闲动作
                Invoke("TimePlayRelaxAnim", Random.Range(RandomRelaxTimeMin, RandomRelaxTimeMax + 1));
            }
        }

        public int GetRelaxAnimStandAnim()
        {
            if (m_BindPlayer == null)
            {
                return GlobeVar.INVALID_ID;
            }

            if (m_IsPlayerRelaxWith)
            {
                Tab_RelaxAnimWithCard tRelax = TableManager.GetRelaxAnimWithCardByID(m_BindPlayer.RelaxWithId, 0);
                if (tRelax == null)
                {
                    return GlobeVar.INVALID_ID;
                }

                return tRelax.ReceiverAnimId;
            }

            return GlobeVar.INVALID_ID;
        }

        public int GetRelaxAnimMoveAnim()
        {
            if (m_BindPlayer == null)
            {
                return GlobeVar.INVALID_ID;
            }

            if (m_IsPlayerRelaxWith)
            {
                Tab_RelaxAnimWithCard tRelax = TableManager.GetRelaxAnimWithCardByID(m_BindPlayer.RelaxWithId, 0);
                if (tRelax == null)
                {
                    return GlobeVar.INVALID_ID;
                }

                return tRelax.ReceiverMoveAnimId;
            }

            return GlobeVar.INVALID_ID;
        }

        public int GetRelaxAnimEndAnim()
        {
            if (m_BindPlayer == null)
            {
                return GlobeVar.INVALID_ID;
            }

            if (m_IsPlayerRelaxWith)
            {
                Tab_RelaxAnimWithCard tRelax = TableManager.GetRelaxAnimWithCardByID(m_BindPlayer.RelaxWithId, 0);
                if (tRelax == null)
                {
                    return GlobeVar.INVALID_ID;
                }

                return tRelax.ReceiverEndAnimId;
            }

            return GlobeVar.INVALID_ID;
        }
    }
}