/********************************************************************************
 *	文件名：	Obj_Player.cs
 *	全路径：	\Script\Obj\Obj_Player.cs
 *	创建人：	李嘉
 *	创建时间：2017-02-14
 *
 *	功能说明：游戏中的玩家角色
 *	修改记录：
*********************************************************************************/
using UnityEngine;
using System.Collections;
using System;
using System.Net.Sockets;
using Games.GlobeDefine;
using Games.Table;
using ProtobufPacket;
using Random = UnityEngine.Random;


namespace Games.LogicObj
{
    public partial class Obj_Player : Obj_Char
    {
        private Collider m_ObjCollider = null;
        public Collider ObjCollider
        {
            get { return m_ObjCollider; }
        }

        void Update()
        {
            if (this == ObjManager.MainPlayer)
            {
                Update_MainPlayer();
            }
            UpdateStep();
        }

        void LateUpdate()
        {
            UpdateRelaxAnimBindPos();
        }

        void Update_MainPlayer()
        {
            UpdateCameraEnterRoleTouch();
            UpdateTalismanInScene_MainPlayer();
            UpdateRTPosToServer();
        }

        bool m_RoleTouch_UpdateCamera = false;
        float m_RoleTouch_UpdateAngle = 0;
        float m_RoleTouch_CameraSpeed;
        private float m_CameraSmoothTime;

        public void CameraFaceTo(float smoothTime = 0.3f)
        {
            // 摄像机平滑旋转到玩家面前
            if (Camera.main == null)
            {
                return;
            }

            Vector3 from = Camera.main.transform.position - ObjTransform.position;
            from.y = 0;
            Vector3 to = ObjTransform.forward;
            int direction = Vector3.Cross(from, to).y > 0 ? 1 : -1;
            m_RoleTouch_UpdateAngle = direction * Vector3.Angle(from, to);
            m_RoleTouch_UpdateCamera = true;
            m_CameraSmoothTime = smoothTime;
        }

        public void CameraBackTo(float smoothTime = 0.3f)
        {
            // 摄像机平滑旋转到玩家面前
            if (Camera.main == null)
            {
                return;
            }

            Vector3 from = Camera.main.transform.position - ObjTransform.position;
            from.y = 0;
            Vector3 to = -ObjTransform.forward;
            int direction = Vector3.Cross(from, to).y > 0 ? 1 : -1;
            m_RoleTouch_UpdateAngle = direction * Vector3.Angle(from, to);
            m_RoleTouch_UpdateCamera = true;
            m_CameraSmoothTime = smoothTime;
        }

        void UpdateCameraEnterRoleTouch()
        {
            // 摄像机平滑旋转到玩家面前
            if (Camera.main == null)
            {
                return;
            }

            if (m_RoleTouch_UpdateCamera == false)
            {
                return;
            }

            float last = m_RoleTouch_UpdateAngle;
            m_RoleTouch_UpdateAngle = Mathf.SmoothDampAngle(m_RoleTouch_UpdateAngle, 0f, ref m_RoleTouch_CameraSpeed, m_CameraSmoothTime);
            float delta = last - m_RoleTouch_UpdateAngle;
            Camera.main.transform.RotateAround(ObjTransform.position, Vector3.up, delta);

            if (Mathf.Abs(m_RoleTouch_UpdateAngle) <= 0.01f)
            {
                m_RoleTouch_UpdateCamera = false;
            }
        }

        protected override OBJ_TYPE _getObjType()
        {
            return OBJ_TYPE.OBJ_PLAYER; ;
        }

        public override bool Init(Obj_Init_Data initData)
        {
            if (base.Init(initData) == false)
            {
                return false;
            }

            InitNavAgent();

            m_ObjCollider = GetComponent<Collider>();

            RoleMaskModelId = initData.m_nRoleMaskModelId;

            m_nWaitRelaxAnimIdWithCard = initData.m_nRelaxAnimIdWithCard;
            m_nWaitRelaxAnimIdWithHero = initData.m_nRelaxAnimIdWithHero;
            m_WaitRelaxAnimWithSender = initData.m_RelaxAnimWithSender;
            m_WaitRelaxAnimWithReceiver = initData.m_RelaxAnimWithReceiver;

            return true;
        }

#region 玩家名字版
        public override void InitNameBoard(bool showName = false)
        {
            _showName = showName;
            AssetManager.LoadHeadInfoPrefab(UIInfo.PlayerHeadInfo, gameObject, "PlayerHeadInfo", OnLoadHeadInfo);
        }

        void OnLoadHeadInfo(GameObject resHeadInfo)
        {
            if (this == null)
            {
                return;
            }

            if (resHeadInfo == null)
            {
                return;
            }

            HeadInfo = resHeadInfo;
            m_PlayerHeadInfoLogic = HeadInfo.GetComponent<PlayerHeadInfoLogic>();

            if (m_PlayerHeadInfoLogic == null)
            {
                return;
            }

            if (RoleName.Equals(""))
            {
                RoleName = "小祈";
            }
            if(_showName)
            {
            m_PlayerHeadInfoLogic.Init(RoleName, 0, "", 1, GlobeVar.INVALID_ID,
                0, false, -1, false);
            }

            m_PlayerHeadInfoLogic.UpdateRoleNameColor(GetNameBoardColor());
            HideShowHeadInfo();
        }
        public void HideShowHeadInfo()
        {
            if (GameManager.PlayerDataPool.IsShieldPlayer&&IsPlayer())
            {
                HideHeadInfo();
            }
            else
            {
                ShowHeadInfo();
            }
        }
        public override void HideHeadInfo()
        {
            if (m_PlayerHeadInfoLogic == null)
            {
                return;
            }
            m_PlayerHeadInfoLogic.ShowOriginalHeadInfo(false);
        }
        public override void ShowHeadInfo()
        {
            if (m_PlayerHeadInfoLogic == null)
            {
                return;
            }
            m_PlayerHeadInfoLogic.ShowOriginalHeadInfo(true);
        }
        public void UpdateHeadInfoName()
        {
            if (m_PlayerHeadInfoLogic == null)
            {
                return;
            }

            m_PlayerHeadInfoLogic.ShowRoleName(RoleName);
        }
#endregion

#region 缓存组件
        public UnityEngine.AI.NavMeshAgent m_NavAgent = null;                  //移动组件
#endregion

#region 玩家外观
        public void UpdatePlayerModel(int nCharModel, int nSoulWare,int dyeColorId = -1)
        {
            AvatarLoadInfo info = new AvatarLoadInfo();

            info.m_BodyId = nCharModel;
            //if (IsMainPlayer())
            //{
            //    info.m_SoulWareId = PlayerPreferenceData.MagicWeaponShow ? nSoulWare : GlobeVar.INVALID_ID;
            //}
            //else
            {
                info.m_SoulWareId = nSoulWare;
            }
            info.m_RoleMaskModelId = RoleMaskModelId;
            info.LoadDyeColor(dyeColorId);
            //判断是否需要更新（变身状态特殊处理）
            if (RoleMaskModelId != GlobeVar.INVALID_ID
                && RoleMaskModelId == ModelID               //身体模型不变
                && null != CharAvatar
                && !CharAvatar.BindPointCache.ContainsKey("SoulWare"))        //魂器没有佩戴（防止变身id和原模型id一致，导致魂器仍然显示）
            {
                CharAvatar.SoulWareId = nSoulWare;  //进行记录,防止其他系统调用该数据
                return;
            }

            _LoadAvatar(info);
        }
#endregion

#region 摇杆操作
        private ThirdPersonController m_thirdController = null;
        public ThirdPersonController ThirdController
        {
            get { return m_thirdController; }
            set { m_thirdController = value; }
        }
#endregion

#region 法宝相关
        private SCENE_ENVIRONMENT_TYPE lastframeEnvType = SCENE_ENVIRONMENT_TYPE.INVALID;
        private ulong m_LastFrameEquipTalisman = GlobeVar.INVALID_GUID;
        public ulong LastFrameEquipTalisman
        {
            get { return m_LastFrameEquipTalisman; }
            set { m_LastFrameEquipTalisman = value; }
        }

        private int mCurSoulWareEffect = GlobeVar.INVALID_ID;
        private int mLastFrameHeroModel = GlobeVar.INVALID_ID;
        private int m_TalismanVisualId = GlobeVar.INVALID_ID;

        void UpdateTalismanInScene_MainPlayer()
        {
            if (false == IsVisible())
            {
                return;
            }

            if (IsLoadingAvatar)
            {
                //LogModule.DebugLog("waiting for loading avatar finished @ UpdateTalismanInScene");
                return;
            }

            if (GameManager.EnvManager == null || GameManager.EnvManager.CurEvnTable == null)
            {
                return;
            }

            if (GameManager.PlayerDataPool == null || GameManager.PlayerDataPool.PlayerHeroData == null)
            {
                return;
            }

            bool refreshSoulwareEffect = false;
            SCENE_ENVIRONMENT_TYPE cur = SCENE_ENVIRONMENT_TYPE.NIGHT;//(SCENE_ENVIRONMENT_TYPE)GameManager.EnvManager.CurEvnTable.EnvirType;
            /*if (lastframeEnvType != cur)
            {
                refreshSoulwareEffect = true;
                //切换了
                if (cur == SCENE_ENVIRONMENT_TYPE.NIGHT)
                {
                    RefreshPlayerTalisman();
                    if (Talisman != null)
                    {
                        PlayEffectAtScene(63);
                    }
                }
                else
                {
                    if (Talisman != null)
                    {
                        PlayEffectAtScene(64);
                        ObjManager.RemoveObj(Talisman);
                        Talisman = null;
                    }
                }
            }
            else*/
            if (ModelID != mLastFrameHeroModel && mLastFrameHeroModel != GlobeVar.INVALID_ID)
            {
                refreshSoulwareEffect = true;
            }
            else
            {
                // 如果庭院里可以有其他玩家, 这里会有问题
                Hero hero = GameManager.PlayerDataPool.PlayerHeroData.GetCurHero();
                if (hero != null && hero.EquipedTalisman != null && hero.EquipedTalisman.IsValid() && PlayerPreferenceData.MagicWeaponShow)
                {

                    if (hero.EquipedTalisman.Guid != m_LastFrameEquipTalisman)
                    {
                        refreshSoulwareEffect = true;
                        if (cur == SCENE_ENVIRONMENT_TYPE.NIGHT)
                        {
                            RefreshMainPlayerTalisman();
                        }
                        m_LastFrameEquipTalisman = hero.EquipedTalisman.Guid;
                    }
                }
                else
                {
                    if (m_LastFrameEquipTalisman != GlobeVar.INVALID_GUID)
                    {
                        refreshSoulwareEffect = true;
                    }
                    if (cur == SCENE_ENVIRONMENT_TYPE.NIGHT && Talisman != null)
                    {
                        ObjManager.RemoveObj(Talisman);
                    }
                    m_LastFrameEquipTalisman = GlobeVar.INVALID_GUID;
                }
            }

            if (refreshSoulwareEffect)
            {
                RefreshMainPlayerSoulWareEffect(cur);
            }
            //lastframeEnvType = cur;
        }

        private void RefreshMainPlayerTalisman()
        {
            if (false == IsVisible())
            {
                return;
            }

            if (Talisman != null)
            {
                ObjManager.RemoveObj(Talisman);
                Talisman = null;
            }

            if (GameManager.storyManager.StoryMode)
            {
                return;
            }

            if (GameManager.PlayerDataPool == null || GameManager.PlayerDataPool.PlayerHeroData == null)
            {
                return;
            }

            Hero hero = GameManager.PlayerDataPool.PlayerHeroData.GetCurHero();
            if (hero == null)
            {
                return;
            }
            if (hero.EquipedTalisman != null && hero.EquipedTalisman.IsValid()
                && !GameManager.PlayerDataPool.IsRoleMasking )
            {
                int visualId = hero.EquipedTalisman.GetVisualId();
                Tab_TalismanVisual tabTalisman = TableManager.GetTalismanVisualByID(visualId, 0);
                if (tabTalisman != null)
                {
                    Obj_Init_Data initData = new Obj_Init_Data();
                    initData.m_nModelID = tabTalisman.ModelId;
                    Talisman = ObjManager.CreateTalisman(initData, ObjTransform);
                    if (Talisman != null)
                    {
                        Talisman.InitBind(this);

                        if (IsRelaxWithHero)
                        {
                            Talisman.SetVisible(ObjVisibleLayer.Player, false);
                        }
                    }
                }
            }
        }

        public void RefreshOtherPlayerTalisman(int nTalismanVisualId)
        {
            Tab_TalismanVisual tVisual = TableManager.GetTalismanVisualByID(nTalismanVisualId, 0);
            if (tVisual == null)
            {
                // nTalismanVisualId只有是-1时是卸下法宝
                if (nTalismanVisualId == GlobeVar.INVALID_ID)
                {
                    if (Talisman != null)
                    {
                        ObjManager.RemoveObj(Talisman);
                        Talisman = null;
                    }

                    if (mCurSoulWareEffect != GlobeVar.INVALID_ID)
                    {
                        StopEffect(mCurSoulWareEffect);
                    }
                }

                return;
            }

            if (Talisman != null)
            {
                ObjManager.RemoveObj(Talisman);
                Talisman = null;
            }

            // 更新法宝外观
            m_TalismanVisualId = nTalismanVisualId;

            //如果是变身中，不处理
            if (IsRoleMasking)
            {
                return;
            }

            Obj_Init_Data initData = new Obj_Init_Data();
            initData.m_nModelID = tVisual.ModelId;
            Talisman = ObjManager.CreateTalisman(initData, ObjTransform);

            if (Talisman != null)
            {
                Talisman.InitBind(this);

                Talisman.CopyVisibleMaskFrom(this);
                if (IsRelaxWithHero)
                {
                    Talisman.SetVisible(ObjVisibleLayer.Player, false);
                }
            }

            // 魂器特效
            if (mCurSoulWareEffect != GlobeVar.INVALID_ID)
            {
                StopEffect(mCurSoulWareEffect);
            }

            if (GameManager.EnvManager.CurEvnTable != null)
            {
                mCurSoulWareEffect = (GameManager.EnvManager.CurEvnTable.EnvirType == (int)SCENE_ENVIRONMENT_TYPE.DAY ? tVisual.DayEffect : tVisual.NightEffect);
            }
            
            PlayEffect(mCurSoulWareEffect);
        }

        public void HideTalisman()
        {
            if (Talisman != null)
            {
                ObjManager.RemoveObj(Talisman);
                Talisman = null;
            }

            // 魂器特效
            if (mCurSoulWareEffect != GlobeVar.INVALID_ID)
            {
                StopEffect(mCurSoulWareEffect);
            }
        }
        
        void RefreshMainPlayerSoulWareEffect(SCENE_ENVIRONMENT_TYPE env)
        {
            if (false == IsVisible())
            {
                return;
            }

            int newEffect = GlobeVar.INVALID_ID;
            int curHeroModel = ModelID;
            Hero hero = GameManager.PlayerDataPool.PlayerHeroData.GetCurHero();
            if (hero != null && hero.EquipedTalisman != null && hero.EquipedTalisman.IsValid())
            {
                int visualId = hero.EquipedTalisman.GetVisualId();
                Tab_TalismanVisual tabTalisman = TableManager.GetTalismanVisualByID(visualId, 0);
                if (tabTalisman != null)
                {
                    newEffect = (env == SCENE_ENVIRONMENT_TYPE.DAY ? tabTalisman.DayEffect : tabTalisman.NightEffect);
                }
            }

            bool changeModel = (mLastFrameHeroModel != curHeroModel && mLastFrameHeroModel != GlobeVar.INVALID_ID);
            if (newEffect != mCurSoulWareEffect || changeModel)
            {
                if (mCurSoulWareEffect != GlobeVar.INVALID_ID || changeModel)
                {
                    StopEffect(mCurSoulWareEffect);
                    //LogModule.DebugLog("stop soulware effect " + mCurSoulWareEffect);
                    mCurSoulWareEffect = GlobeVar.INVALID_ID;
                }
                if (newEffect != GlobeVar.INVALID_ID)
                {
                    mCurSoulWareEffect = newEffect;
                    //LogModule.DebugLog("load soulware effect " + mCurSoulWareEffect);
                    PlayEffect(mCurSoulWareEffect);
                }
            }
            mLastFrameHeroModel = curHeroModel;
        }
        #endregion

        private STEP_TYPE m_CurStepType;
        public STEP_TYPE CurStepType
        {
            get { return m_CurStepType; }
            set { m_CurStepType = value; }
        }

        public int GetStepSound()
        {
            if (GameManager.PlayerDataPool.PlayerHeroData.CurHeroId == (int)HeroId.XiaoQi)
            {
                if (m_CurStepType == STEP_TYPE.NORMAL)
                {
                    return GlobeVar.STEPSOUND_XIAOQI_RANDOM[Random.Range(0, GlobeVar.STEPSOUND_XIAOQI_RANDOM.Length)];
                }
                else if (m_CurStepType == STEP_TYPE.WATER)
                {
                    return GlobeVar.STEPSOUND_XIAOQI_WATER;
                }
            }
            else if (GameManager.PlayerDataPool.PlayerHeroData.CurHeroId == (int)HeroId.YunMengZhang)
            {
                if (m_CurStepType == STEP_TYPE.NORMAL)
                {
                    return GlobeVar.STEPSOUND_YUNMENGZHENG_NORMAL;
                }
                else if (m_CurStepType == STEP_TYPE.WATER)
                {
                    return GlobeVar.STEPSOUND_YUNMENGZHENG_WATER;
                }
            }

            return GlobeVar.INVALID_ID;
        }

        private ulong m_HeadIcon = 1;
        public ulong HeadIcon
        {
            get { return m_HeadIcon; }
            set { m_HeadIcon = value; }
        }

        private int m_nHeadFrame = GlobeVar.INVALID_ID;     //头像框，主要看其他玩家使用，主角的使用PlayerDataPool中的数据
        public int HeadFrame
        {
            get { return m_nHeadFrame; }
            set { m_nHeadFrame = value; }
        }

        public void UpdateHeadIcon(ulong headIcon)
        {
            m_HeadIcon = headIcon;

            if (ObjManager.MainPlayer != null)
            {
                Obj_Player objPlayer = ObjManager.MainPlayer.m_selectTarget as Obj_Player;
                if (objPlayer != null && objPlayer == this && TargetFrameController.Instance != null)
                {
                    TargetFrameController.Instance.RefreshTarget();
                }
            }
        }

        private int m_Level = 1;
        public int Level
        {
            get
            {
                // 主要是给多人场景的其他玩家用 此处加玩家自己的处理是防止其他人取错
                if (IsMainPlayer())
                {
                    return GameManager.PlayerDataPool.PlayerHeroData.Level;
                }
                else
                {
                    return m_Level;
                }
            }
            set { m_Level = value; }
        }

        private int m_CurHeroId = GlobeVar.INVALID_ID;
        public int CurHeroId
        {
            get { return m_CurHeroId; }
            set { m_CurHeroId = value; }
        }

        public int GetCurHeroId()
        {
            if (IsMainPlayer())
            {
                return GameManager.PlayerDataPool.PlayerHeroData.CurHeroId;
            }
            else
            {
                return m_CurHeroId;
            }
        }

        public void HandleLoadModelOver()
        {
            if (m_nWaitRelaxAnimIdWithCard != GlobeVar.INVALID_ID)
            {
                PlayRelaxAnimWithCardOnCreate(m_nWaitRelaxAnimIdWithCard);
            }

            if (m_nWaitRelaxAnimIdWithHero != GlobeVar.INVALID_ID)
            {
                PlayRelaxAnimWithHeroOnCreate(m_nWaitRelaxAnimIdWithHero, m_WaitRelaxAnimWithSender, m_WaitRelaxAnimWithReceiver);
            }

            m_nWaitRelaxAnimIdWithCard = GlobeVar.INVALID_ID;
            m_nWaitRelaxAnimIdWithHero = GlobeVar.INVALID_ID;
            m_WaitRelaxAnimWithSender = GlobeVar.INVALID_GUID;
            m_WaitRelaxAnimWithReceiver = GlobeVar.INVALID_GUID;
        }

        public void OnDisplayHide()
        {
            if (ObjManager.MainPlayer != null && ObjManager.MainPlayer.m_selectTarget == this)
            {
                ObjManager.MainPlayer.CancelSelectTarget();
            }
        }

        public void OnDisplayShow()
        {
            
        }
    }
}