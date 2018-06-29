
using UnityEngine;
using System.Collections;
using Games.Animation_Modle;
using Games.GlobeDefine;
using Games.Table;
using System.Collections.Generic;


namespace Games.LogicObj
{

    public partial class Obj_Char : Obj
    {

        public Obj_Char()
        {
        }

        public Transform EffectPointTrans { get; protected set; }
        public Transform AvatarTrans { get; protected set; }
        public Transform ShadowTrans { get; protected set; }

        public Vector3 SpawnPos;
        public Quaternion SpawnRot;
        protected bool m_bIsNeedNavAgent = true;

        List<int> m_CachedEffectId = new List<int>();

        protected bool _showName;

        //根据数据初始化接口，派生类必须实现
        public virtual bool Init(Obj_Init_Data initData)
        {
            if (null == initData)
            {
                return false;
            }

            if (initData.m_CreatePos.y <= 0.0f)
            {
                bool bHit = false;
                Position = Scene.GetTerrainPosition(initData.m_CreatePos, out bHit);
            }
            else
            {
                //这种情况是定好了Y的高度，无需创建行走面
                Position = initData.m_CreatePos;
                m_bIsNeedNavAgent = false;
            }

			ObjTransform.rotation = Quaternion.Euler(initData.m_CreateRot);

		    SpawnPos = Position;
		    SpawnRot = ObjTransform.rotation;

		    EffectPointTrans = transform.Find("EffectPoint");
		    AvatarTrans = transform.Find("Avatar");
		    ShadowTrans = transform.Find("Shadow");
            if (initData.m_nModelID == -1&& null != ShadowTrans)
            {
                ShadowTrans.gameObject.SetActive(false);
            }
            ModelID = initData.m_nModelID;
		    IsBoss = initData.m_bIsBoss;

            //动画
            AnimLogic = Utils.TryAddComponent<AnimationLogic>(gameObject);
            //可见性检查
            Utils.TryAddComponent<MeshRenderer>(gameObject);
            CurAnimId = (int)CHAR_ANIM_ID.Stand;

            Tab_CharModel charModel = TableManager.GetCharModelByID(ModelID, 0);
            if (charModel != null)
            {
                NameDeltaHeight = charModel.HeadInfoHeight;     //设置名字版高度
                DamageBoardHeight = charModel.DamageBoardHeight;
                IsShowInActQ = charModel.IsShowInActQ;

                if (ShadowTrans != null)
                {
                    if (charModel.ShadowScale <= 0f)
                    {
                        ShadowTrans.gameObject.SetActive(false);
                    }
                    ShadowTrans.localScale = charModel.Scale * charModel.ShadowScale * Vector3.one;
                }


                if (IsNpc()&&charModel.CullFar==(int)NPC_CULL_TYPE.far)//改变NPC的层级，某些NPC不被相机裁剪掉
                {
                    Utils.SetAllChildLayer(ObjTransform, LayerMask.NameToLayer("NPC_Far"));
                }
                //交互
                InitCollder(charModel);
            }

            if (initData.m_bShowNameBoard)
            {
                InitNameBoard(initData.m_bShowNameBoard);
            }
            InitEffect();

		    if (initData.m_nMinionID != GlobeVar.INVALID_ID)
		    {
		        AddMinion(initData.m_nMinionID);
		    }

            IsCameraVisible = false;

            return true;
        }

        //是否有相机在拍摄
        public bool IsCameraVisible { get; private set; }

        protected virtual void OnBecameVisible()
        {
            IsCameraVisible = true;
        }

        protected virtual void OnBecameInvisible()
        {
            IsCameraVisible = false;
        }

        #region 交互
        protected CapsuleCollider m_Collider = null;
        public void InitCollder(Tab_CharModel tModel)
        {
            if (null == tModel)
            {
                return;
            }

            m_Collider = gameObject.GetComponent<CapsuleCollider>();
            if (m_Collider != null)
            {
                m_Collider.height = tModel.ColliderHeight;
                m_Collider.radius = tModel.ColliderRadius;
                if (tModel.ColliderHasOffset)
                {
                    m_Collider.center = new Vector3(tModel.ColliderOffsetX,tModel.ColliderOffsetY,tModel.ColliderOffsetZ);
                }
                else
                {
                    m_Collider.center = new Vector3(0f, tModel.ColliderHeight * 0.5f, 0f);
                }
            }
        }
        #endregion


        #region 动画
        public AnimationLogic AnimLogic { get; protected set; }
        public AvatarHeadMove HeadMove { get; protected set; }
        public SceneObjRandAnim RandomAnimLogic { get; protected set; }

        //TODO，需求复杂的时候，需要重写成状态机来维护
        //临时充当state的作用
        private int m_CurAnimId = -1;
        //处于loading阶段，赋值了动画
        private int m_InLoadingAnim = -1;

        public int CurAnimId
        {
            get
            {
                return m_CurAnimId;
            }
            set
            {
                m_CurAnimId = value;
            }
        }

        //当前处于的默认动画状态
        private int m_CurDefaultAnimId = (int)CHAR_ANIM_ID.Stand;

        public int CurDefaultAnimId
        {
            get
            {
                if (m_DefaultAnimInfoList == null || m_DefaultAnimInfoList.Count <= 0)
                {
                    return m_CurDefaultAnimId;
                }
                return m_DefaultAnimInfoList[m_DefaultAnimInfoList.Count - 1].AnimId;
            }
        }

        public class DefaultAnimChangeInfo
        {
            public int AnimId;
        }

        private List<DefaultAnimChangeInfo> m_DefaultAnimInfoList;
        public DefaultAnimChangeInfo ChangeDefAnim(int animId,bool direct = false)
        {
            if (direct)
            {
                m_CurDefaultAnimId = animId;
                return null;
            }

            if (m_DefaultAnimInfoList == null)
            {
                m_DefaultAnimInfoList = new List<DefaultAnimChangeInfo>();
            }

            DefaultAnimChangeInfo info = new DefaultAnimChangeInfo()
            {
                AnimId = animId,
            };
            m_DefaultAnimInfoList.Add(info);
            return info;
        }

        public void ReverDefAnim(DefaultAnimChangeInfo info)
        {
            if (m_DefaultAnimInfoList == null)
            {
                return;
            }
            m_DefaultAnimInfoList.Remove(info);
        }

        public void InitAnim()
        {
            if (AnimLogic == null)
            {
                return;
            }
            
            Transform modelTransform = ObjTransform.Find("Model");
            if (modelTransform == null)
            {
                modelTransform = CharAvatar.AvatarTrans.Find("Model");
            }
            if (modelTransform != null)
            {
                AnimLogic.InitAnimInfo(modelTransform.gameObject,CommonAnimPath,AnimPath);
                if (m_InLoadingAnim != -1)
                {
                    int animId = m_InLoadingAnim;
                    m_InLoadingAnim = -1;
                    if (!PlayAnim(animId))
                    {
                        PlayAnim(CurAnimId);
                    }
                }
                else
                {
                    PlayAnim(CurAnimId);
                }
            }
        }

        public bool PlayAnim(int nAnimId,bool nextPlayDefault = true)
        {
            if (nAnimId == -1)
            {
                return true;
            }

            if (!IsCurAnimCanHeadLookAt(nAnimId))
            {
                StopHeadLookAt();
            }

            //模型尚未加载，返回
            if (IsLoadingAvatar)
            {
                //CurAnimId = nAnimId; //保存应该播放的状态
                m_InLoadingAnim = nAnimId;
                return true;
            }

            if (AnimLogic != null)
            {
                if (nextPlayDefault)
                {
                    return PlayAnimWithNotify(nAnimId, OnPlayAnimFinish);
                }
                else
                {
                    return PlayAnimWithNotify(nAnimId, null);
                }
            }
            return false;
        }

        public void PlayDefaultAnim()
        {
            PlayAnim(CurDefaultAnimId);
        }

        private void OnPlayAnimFinish(int id, bool callByStop,bool hasNext)
        {
            if (callByStop)
            {
                return;
            }
            if (!hasNext)
            {
                CurAnimId = CurDefaultAnimId;
                if (AnimLogic != null)
                {
                    AnimLogic.ListenAnimFinish(null);
                    AnimLogic.Play(CurDefaultAnimId);
                }
            }
        }

        public bool PlayAnimWithNotify(int nAnimId,AnimationLogic.DelAnimFinish del)
        {
            if (nAnimId == -1)
            {
                return true;
            }
            int orgAnimId = CurAnimId;
            if (AnimLogic != null)
            {
                AnimationLogic.DelAnimFinish orgDel = AnimLogic.GetAnimFinish();
                AnimLogic.ListenAnimFinish(del);
                bool ret = AnimLogic.Play(nAnimId);
                if (!ret)
                {
                    AnimLogic.ListenAnimFinish(orgDel);
                    CurAnimId = orgAnimId; //播放失败，还是之前的状态
                }
                else
                {
                    CurAnimId = nAnimId;
                }
                return ret;
            }
            return false;
        }

        public void StopAnim()
        {
            if (AnimLogic != null)
            {
                CurAnimId = -1;
                AnimLogic.Stop();
            }
        }

        public void PlayRandomAnim(int id)
        {
            if (RandomAnimLogic == null)
            {
                RandomAnimLogic = Utils.TryAddComponent<SceneObjRandAnim>(gameObject);
            }
            if (RandomAnimLogic == null)
            {
                return;
            }
            RandomAnimLogic.enabled = true;
            RandomAnimLogic.PlayRandomAnim(id);
        }

        public void StopRandomAnim()
        {
            if (RandomAnimLogic != null)
            {
                RandomAnimLogic.enabled = false;
            }
        }

        public bool IsPlayRandomAnim()
        {
            return RandomAnimLogic != null && RandomAnimLogic.enabled;
        }

        /*
        protected void UpdateAnimation()
        {

        }
        */

        #endregion

        #region UI及名字版
        //名字板相关
        private GameObject m_HeadInfo;        // 头顶信息板总节点
        public GameObject HeadInfo
        {
            get { return m_HeadInfo; }
            protected set
            {
                m_HeadInfo = value;
            }
        }

        Transform m_HeadInfoTran;
        public UnityEngine.Transform HeadInfoTran
        {
            get
            {
                if (m_HeadInfoTran == null &&
                    m_HeadInfo != null)
                {
                    m_HeadInfoTran = m_HeadInfo.transform;
                }
                return m_HeadInfoTran;
            }

        }

        private NameBoard m_HeadNameborad;
        public NameBoard HeadNameborad
        {
            get
            {
                if (m_HeadNameborad == null &&
                   m_HeadInfo != null)
                {
                    m_HeadNameborad = m_HeadInfo.GetComponent<NameBoard>();
                }
                return m_HeadNameborad;
            }
        }

        public void Bubble(string msg,float time = 1f)
        {
            Scene scene = GameManager.CurScene;
            if (scene != null && scene.FlyTextMgr != null)
            {
                Vector3 offset = new Vector3(0f, this.NameDeltaHeight * 0.9f, 0f);
                scene.FlyTextMgr.Play(msg, ObjTransform, offset,FlyTextMgr.TextType.Bubble, false, time);
            }
        }

        public void Bubble(int dictId, float time = 1f)
        {
            Bubble(StrDictionary.GetDicByID(dictId),time);
        }

        public void RandomBubble(int randomId)
        {
            List<Tab_RandomBubble> tabs = TableManager.GetRandomBubbleByID(randomId);
            if (tabs == null)
            {
                return;
            }

            int count = 0;
            foreach (var tab in tabs)
            {
                count += tab.Chance;
            }

            Tab_RandomBubble last = null;
            int chance = 0;
            int random = Random.Range(0, count);
            foreach (var tab in tabs)
            {
                chance += tab.Chance;
                last = tab;
                if (random <= chance)
                {
                    break;
                }
            }

            if (last == null || string.IsNullOrEmpty(last.Text))
            {
                return;
            }

            //特殊处理，-1表示空，客户端表格id重复合并时，如果空，会自动取上一行
            if (last.Text == "-1")
            {
                return;
            }

            Bubble(last.Text,last.Time);
        }

        public virtual void InitNameBoard(bool showName = false)
        {
        }

        public virtual void ShowHeadInfo()
        {
        }

        public virtual void HideHeadInfo()
        {
        }

        public virtual void SetNameBoardColor()
        {
        }

        public virtual string GetNameBoardColor()
        {
            return "";//"[FFFFFF]";
        }

        //显示头顶上任务提示面板
        public virtual void ShowMissionBoard()
        {
        }

        //隐藏头顶上任务提示面板
        public virtual void HideMissionBoard()
        {
        }

        #endregion


        #region 特效相关
        public EffectLogic ObjCharEffectLogic { get; private set; }

        public void InitEffect()
        {
            //特效
            ObjCharEffectLogic = Utils.TryAddComponent<EffectLogic>(gameObject);
            ObjCharEffectLogic.InitEffect(this.gameObject,LayerMask.NameToLayer("ObjEffect"));
        }

        public void PlayEffect(int effectID, EffectLogic.EffectLoadOverDelegate delLoadOverEffect = null, EffectLogic.EffectStopDelegate delStopEffect = null, object param = null)
        {
            if (ObjCharEffectLogic != null)
            {
                if (IsBoss)
                {
                    Tab_Effect tabEffect = TableManager.GetEffectByID(effectID, 0);
                    if (tabEffect == null)
                    {
                        return;
                    }
                    if (!tabEffect.IsBossPlay)
                    {
                        return;
                    }
                }
                ObjCharEffectLogic.PlayEffect(effectID, delLoadOverEffect, delStopEffect, param);
            }
        }

        public GameObject PlayEffectAtScene(int effectID)
        {
            Scene scene = GameManager.CurScene;
            if (scene == null)
            {
                return null;
            }
            Tab_Effect tab = TableManager.GetEffectByID(effectID, 0);
            if (tab == null)
            {
                return null;
            }
            Transform effParent = FindCharModelPoint(tab.ParentName);
            if (effParent == null)
            {
                effParent = EffectPointTrans;
            }
            Vector3 pos = effParent.position;
            return scene.PlayEffect(effectID, pos);
        }

        public void StopEffect(int effectID, bool bStopAll = true)
        {
            if (ObjCharEffectLogic != null)
            {
                ObjCharEffectLogic.StopEffect(effectID, bStopAll);
            }
        }

        public void ClearEfffect()
        {
            if (ObjCharEffectLogic != null)
            {
                ObjCharEffectLogic.ClearEffect();
            }
        }
        #endregion

        #region 换装
        private Avatar m_CharAvatar = new Avatar();
        public Avatar CharAvatar
        {
            get { return m_CharAvatar; }
        }

        public string CommonAnimPath { get; private set; }
        public string AnimPath { get; private set; }

        public int ModelID { get; private set; }

        private List<AvatarLoadInfo> m_AvatarInfos;
        private AvatarLoadInfo m_OrgAvatarInfo;
        public Games.LogicObj.AvatarLoadInfo OrgAvatarInfo
        {
            get { return m_OrgAvatarInfo; }
            set { m_OrgAvatarInfo = value; }
        }

        public AvatarLoadInfo CurrAvatarInfo { get; set; }

        public bool IsLoadingAvatar { get; set; }
        public void LoadAvatar(Obj_Init_Data initData,AssetLoader.LoadQueueType queueType = AssetLoader.LoadQueueType.COMMON)
        {
            if (null == initData)
            {
                return;
            }

            if (initData.m_nModelID == -1 && initData.m_nRefixModelID == -1)
            {
                LogModule.WarningLog("ModelId -1");
                return;
            }

            AvatarLoadInfo info = new AvatarLoadInfo();
            //if (initData.m_nRoleMaskModelId != GlobeVar.INVALID_ID) //变身特殊处理
            //{
           //     info.m_BodyId = initData.m_nRoleMaskModelId;
           //     info.m_SoulWareId = GlobeVar.INVALID_ID;
           // }
          //  else
           // {
                if (initData.m_nRefixModelID == GlobeVar.INVALID_ID)
                {
                    info.m_BodyId = initData.m_nModelID;
                }
                else
                {
                    info.m_BodyId = initData.m_nRefixModelID;
                }
                info.m_SoulWareId = initData.m_nSoulWareModelID;
           // }
            info.m_RoleMaskModelId = initData.m_nRoleMaskModelId;
            info.LoadDyeColor(initData.m_nDyeColorId);
            info.m_OrnamentEffectTabID = initData.m_OrnamentEffectTabId;
            if (initData.m_EffectList != null)
            {
                m_CachedEffectId.Clear();
                m_CachedEffectId.AddRange(initData.m_EffectList);
            }

            OrgAvatarInfo = info;
            _LoadAvatar(info,queueType);
        }

        protected void _LoadAvatar(AvatarLoadInfo info, AssetLoader.LoadQueueType queueType = AssetLoader.LoadQueueType.COMMON)
        {
            if (info == null)
            {
                return;
            }

            if (GlobeVar.INVALID_ID == info.m_RoleMaskModelId)
            {
                if (info.m_BodyId != GlobeVar.INVALID_ID)
                {
                    ModelID = info.m_BodyId;
                }
            }
            else
            {
                ModelID = info.m_RoleMaskModelId;
                info.m_BodyId = ModelID;
            }

            Tab_CharModel charModel = TableManager.GetCharModelByID(info.m_BodyId, 0);
            if (charModel != null)
            {
                CommonAnimPath = charModel.CommonAnimPath;
                AnimPath = charModel.AnimPath;
                if (null != AvatarTrans)
                {
                    AvatarTrans.localScale = new Vector3(charModel.Scale, charModel.Scale, charModel.Scale);
                    Vector3 modelOffset = new Vector3(charModel.OffsetX, charModel.OffsetY, charModel.OffsetZ);
                    AvatarTrans.localPosition += modelOffset;
                }

                NameDeltaHeight = charModel.HeadInfoHeight;
                if (HeadNameborad != null)
                {
                    HeadNameborad.ResetNameBoardHeight();
            }
            }
            StopHeadLookAt(true);
            IsLoadingAvatar = true;
            CharAvatar.Init(info.m_BodyId, ObjTransform, AnimLogic);
            CurrAvatarInfo = info;
            CharAvatar.ReloadAvatar(info, OnAvatarLoaded, queueType);
        }

        public AvatarLoadInfo ChangeAvatar(int charModelId)
        {
            AvatarLoadInfo info = new AvatarLoadInfo();
            info.m_BodyId = charModelId;
            ChangeAvatar(info);
            return info;
        }

        public void ChangeAvatar(AvatarLoadInfo info)
        {
            if (m_AvatarInfos == null)
            {
                m_AvatarInfos = new List<AvatarLoadInfo>();
            }
            m_AvatarInfos.Add(info);
            _LoadAvatar(info,AssetLoader.LoadQueueType.SYNC);
        }

        public void RevertAvatar(AvatarLoadInfo info)
        {
            if (m_AvatarInfos == null)
            {
                _LoadAvatar(OrgAvatarInfo,AssetLoader.LoadQueueType.SYNC);
                return;
            }

            int stackTopIndex = m_AvatarInfos.Count - 1;
            //栈顶不相同，只是移除
            if (m_AvatarInfos[stackTopIndex] != info)
            {
                m_AvatarInfos.Remove(info);
            }
            else
            {
                m_AvatarInfos.RemoveAt(stackTopIndex);
                //变回之前的
                int newTopIndex = m_AvatarInfos.Count - 1;
                AvatarLoadInfo target = newTopIndex >= 0 ? m_AvatarInfos[newTopIndex] : OrgAvatarInfo;
                _LoadAvatar(target,AssetLoader.LoadQueueType.SYNC);
            }
        }

        public bool HasChangeAvatar()
        {
            if (m_AvatarInfos == null)
            {
                return false;
            }
            return m_AvatarInfos.Count > 0;
        }

        private Dictionary<int, int> visibleLayerDict = new Dictionary<int, int>();

        private int visibleMask = 0;
        public int VisibleMask
        {
            get
            {
                return visibleMask;
            }
            set
            {
                int last = visibleMask;
                visibleMask = value;

                if (AvatarTrans != null && Utils.IsVisible(last,ObjVisible.Avatar) != Utils.IsVisible(value, ObjVisible.Avatar))
                {
                    Utils.SetAvatarVisible(AvatarTrans, Utils.IsVisible(value, ObjVisible.Avatar),gameObject.layer);
                }
                if (EffectPointTrans != null && Utils.IsVisible(last,ObjVisible.Effect) != Utils.IsVisible(value, ObjVisible.Effect))
                {
                    OnEffectVisibleChange(EffectPointTrans,Utils.IsVisible(value,ObjVisible.Effect));
                }
                if (ShadowTrans != null && Utils.IsVisible(last,ObjVisible.Shadow) != Utils.IsVisible(value, ObjVisible.Shadow))
                {
                    OnTransVisibleChange(ShadowTrans, Utils.IsVisible(value, ObjVisible.Shadow));
                }
            }
        }

        public void SetVisible(ObjVisibleLayer visibleLayer, bool visible, params ObjVisible[] flag)
        {
            SetVisible((int) visibleLayer, visible, flag);
        }

        public void SetVisible(int visibleLayer, bool visible, params ObjVisible[] flag)
        {
            int mask = 0;
            visibleLayerDict.TryGetValue(visibleLayer, out mask);
            if (visible)
            {
                mask &= ~Utils.GetVisibleMask(flag);
            }
            else
            {
                mask |= Utils.GetVisibleMask(flag);
            }
            visibleLayerDict[visibleLayer] = mask;
            RecalcVisibleMask();
        }

        private void RecalcVisibleMask()
        {
            int mask = 0;
            foreach (var pair in visibleLayerDict)
            {
                mask |= pair.Value;
            }
            VisibleMask = mask;

            if (IsPlayer())
            {
                Obj_Player objPlayer = this as Obj_Player;
                if (objPlayer != null)
                {
                    if (objPlayer.CallCard != null)
                    {
                        objPlayer.CallCard.CopyVisibleMaskFrom(this);
                    }
                    if (objPlayer.Talisman != null)
                    {
                        objPlayer.Talisman.CopyVisibleMaskFrom(this);
                    }
                }
            }
        }

        public void CopyVisibleMaskFrom(Obj_Char obj)
        {
            visibleLayerDict.Clear();
            foreach (var pair in obj.visibleLayerDict)
            {
                visibleLayerDict.Add(pair.Key,pair.Value);
            }
            RecalcVisibleMask();
        }

        public void SetVisible(int visibleLayer, bool visible)
        {
            SetVisible(visibleLayer, visible, ObjVisible.Avatar, ObjVisible.Effect, ObjVisible.Shadow);
        }

        public void SetVisible(ObjVisibleLayer visibleLayer, bool visible)
        {
            SetVisible((int)visibleLayer, visible, ObjVisible.Avatar, ObjVisible.Effect, ObjVisible.Shadow);
        }

        public bool IsVisible(params ObjVisible[] flags)
        {
            return Utils.IsVisible(VisibleMask, flags);
        }

        private void OnTransVisibleChange(Transform trans,bool visible)
        {
            if (trans == null)
            {
                return;
            }
            Utils.SetAllChildLayer(trans, visible ? gameObject.layer : LayerMask.NameToLayer("Invisible"));
        }

        private void OnEffectVisibleChange(Transform trans, bool visible)
        {
            if (trans != null)
            {
                trans.gameObject.SetActive(visible);
            }
        }

        private static HashSet<string> ReloadAnimSet = new HashSet<string>()
        {
            "Model","Face","Hair"
        };
        void OnAvatarLoaded(List<string> reloadName)
        {
            if (this == ObjManager.MainPlayer)
            {
                if (GameManager.storyManager != null && GameManager.storyManager.CurStoryTable != null)
                {
                    bool bMainPlayerVisible = (GameManager.storyManager.CurStoryTable.StoryModel != GlobeVar.INVALID_ID);
                    SetVisible(ObjVisibleLayer.Story, bMainPlayerVisible);
                    ObjManager.RefixMainPlayerStoryPosition();
                }
            }

            IsLoadingAvatar = false;
            bool needReloadAnim = false;
            foreach (var mName in reloadName)
            {
                if (ReloadAnimSet.Contains(mName))
                {
                    needReloadAnim = true;
                    break;
                }
            }
            if (needReloadAnim)
            {
                InitAnim();
            }

            //重新初始化头部移动，必须在InitEffectPointList之后，需要骨骼信息
            InitHeadMove();

            //改变层级
            Utils.SetAllChildLayer(m_CharAvatar.AvatarTrans, gameObject.layer, LayerMask.NameToLayer("ObjEffect"));
            Utils.SetAvatarVisible(m_CharAvatar.AvatarTrans, IsVisible(ObjVisible.Avatar),gameObject.layer);
            if (IsMainPlayer())
            {
                if (RoleTouchController.Instance != null)
                {
                    RoleTouchController.Instance.HandlePlayerUpdateModel();
                }
            }
            else if (IsNpc())
            {
                Obj_NPC _npc = this as Obj_NPC;
                if (null != _npc && _npc.SceneNPCID != GlobeVar.INVALID_ID)
                {
                    Tab_SceneNpc _tabSceneNPC = TableManager.GetSceneNpcByID(_npc.SceneNPCID, 0);
                    if (null != _tabSceneNPC && _tabSceneNPC.Animtion != GlobeVar.INVALID_ID)
                    {
                        _npc.PlayAnim(_tabSceneNPC.Animtion);
                    }
                }
            }
            else if (IsPlayer())
            {
                Obj_Player objPlayer = this as Obj_Player;
                if (objPlayer != null)
                {
                    objPlayer.HandleLoadModelOver();
                }
            }

            // 挂载特效
            for (int i = 0; i < m_CachedEffectId.Count; ++i)
            {
                int effect = m_CachedEffectId[i];
                if (effect != GlobeVar.INVALID_ID)
                {
                    PlayEffect(effect);
                }
            }
            //挂载装饰特效
            if (OrgAvatarInfo != null && OrgAvatarInfo.m_OrnamentEffectTabID != GlobeVar.INVALID_ID)
            {
                 Tab_OrnamentEffect tbOrnamentEffect = TableManager.GetOrnamentEffectByID(OrgAvatarInfo.m_OrnamentEffectTabID, 0);
                if (tbOrnamentEffect != null)
                {
                    PlayEffect(tbOrnamentEffect.EffectID);
                }
            }

            Tab_CharModel charModel = TableManager.GetCharModelByID(ModelID, 0);
            if (null != charModel)
            {
                //绑定特效
                int count = charModel.getEffectCount();
                for (int i = 0; i < count; i++)
                {
                    int effID = charModel.GetEffectbyIndex(i);
                    if (effID <= -1)
                    {
                        continue;
                    }
                    PlayEffect(effID);
                }
            }
            else
            {
                LogModule.ErrorLog("Char Model is null, id:" + ModelID);
            }

            if (IsCard())
            {
                Obj_Card objCard = this as Obj_Card;
                if (objCard != null)
                {
                    objCard.HandleLoadModelOver();
                }
            }

            if (IsTalisman())
            {
                Obj_Talisman objTalisman = this as Obj_Talisman;
                if (objTalisman != null)
                {
                    objTalisman.HandleLoadModelOver();
                }
            }

            gameObject.BroadcastMessage("AvatarLoaded", SendMessageOptions.DontRequireReceiver);
        }

        #endregion

        #region 声音

        public void PlayDeadSound()
        {
            Tab_CharModel tab = GetBaseCharModelTab();
            if (tab == null) return;
            GameManager.SoundManager.PlayRandomSound(tab.DeadRandomSoundID,SOUND_TYPE.SFX);
        }

        public void PlayHurtSound()
        {
            Tab_CharModel tab = GetBaseCharModelTab();
            if (tab == null) return;
            GameManager.SoundManager.PlayRandomSound(tab.HurtRandomSoundID, SOUND_TYPE.SFX);
        }

        public void PlaySummon(bool playSound = true)
        {
            Tab_CharModel tab = GetBaseCharModelTab();
            if (tab == null) return;
            PlayAnim(tab.SummonAnimId);
            if (playSound && tab.SpawnRandomSoundID != -1)
            {
                GameManager.SoundManager.PlayRandomSound(tab.SpawnRandomSoundID,SOUND_TYPE.SFX);
            }
            if (ObjMinionLogic != null)
            {
                ObjMinionLogic.OwnerPlaySummon();
            }
            PlaySpawnEffect();
        }

        public void PlaySpawnEffect()
        {
            Tab_CharModel charModel = TableManager.GetCharModelByID(ModelID, 0);
            if (null != charModel)
            {
                //出生特效
                int count = charModel.getSpawnEffectCount();
                for (int i = 0; i < count; i++)
                {
                    int effID = charModel.GetSpawnEffectbyIndex(i);
                    if (effID <= -1)
                    {
                        continue;
                    }
                    PlayEffect(effID);
                }
            }
            else
            {
                if (ModelID==-1)
                {
                    return;
                }
                LogModule.ErrorLog("Char Model is null, id:" + ModelID);
            }
        }

        public void PlayDead()
        {
            m_IsAlive = false;
            ClearEfffect();
            if (IsVisible(ObjVisible.Avatar))
            {
                StartCoroutine(_PlayDead());
            }
        }

        protected virtual IEnumerator _PlayDead()
        {
            //死亡音效
            PlayDeadSound();
            //播放死亡特效
            //PlayEffectAt(GlobeVar.OBJ_DIE_EFFECT, ObjTransform.position, Quaternion.identity);
            Tab_CharModel tab = TableManager.GetCharModelByID(ModelID, 0);
            if (tab != null)
            {
                int count = tab.getDieEffectCount();
                for (int i = 0; i < count; i++)
                {
                    int effId = tab.GetDieEffectbyIndex(i);
                    if (effId != -1)
                    {
                        PlayEffectAtScene(effId);
                    }
                }
            }
            //StopAnim();
            if (tab != null)
            {
                PlayAnim(tab.DieAnimId, false);
            }
            float dieTime = tab != null ? tab.DieTime : 1f;
            //dieTime = Mathf.Min(dieTime, 3f);
            yield return new WaitForSeconds(dieTime);

            ObjManager.RemoveObj(this);
        }

        #endregion

        public Tab_CharModel GetBaseCharModelTab()
        {
            if (ModelID == -1) return null;

            return TableManager.GetCharModelByID(ModelID,0);
        }

        public virtual void FixedUpdate()
        {
            //UpdateAnimation();
            UpdateTargetMove();
            //UpdateStep();
        }

        public Transform FindCharModelPoint(string pointName)
        {
            if (pointName == "EffectPoint")
            {
                if (ObjCharEffectLogic != null)
                {
                    return ObjCharEffectLogic.EffectPointTrans;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                if (CharAvatar != null)
                {
                    return CharAvatar.FindCharModelPoint(pointName);
                }
                else
                {
                    LogModule.WarningLog("Avatar Not Loaded");
                    return null;
                }
            }
        }

#region 头部运动
        public bool IsHeadLooking { get; private set; }
        public void InitHeadMove()
        {
            if (HeadMove == null || CharAvatar == null)
            {
                return;
            }
            if (CharAvatar.InLoading)
            {
                return;
            }
            HeadMove.head = CharAvatar.FindCharModelPoint(CharBindPoint.HeadPoint);
            HeadMove.neck = CharAvatar.FindCharModelPoint(CharBindPoint.Neck);
        }

        public void HeadLookAt(Vector3 point)
        {
            if (HeadMove == null)
            {
                return;
            }
            HeadMove.LookAt(point);
        }

        public void HeadLookAt(Transform target)
        {
            if (HeadMove == null)
            {
                return;
            }
            HeadMove.lookAtTarget = target;
        }

        public void BeginHeadLookAt(bool pitch = true,bool ignoreAnimState = false)
        {
            if (HeadMove == null)
            {
                HeadMove = Utils.TryAddComponent<AvatarHeadMove>(gameObject);
                InitHeadMove();
                HeadMove.animLogc = AnimLogic;
            }

            if (ignoreAnimState || !IsCurAnimCanHeadLookAt(CurAnimId))
            {
                return;
            }
           
            HeadMove.isLookAtRotX = pitch;
            HeadMove.enabled = true;
            IsHeadLooking = true;
        }

        public void StopHeadLookAt(bool immediate = false)
        {
            if (HeadMove == null)
            {
                return;
            }
            HeadMove.StopLookAt();
            if (immediate)
            {
                HeadMove.enabled = false;
            }
            IsHeadLooking = false;
        }

        public static int[] CanLookAtAnimState =
        {
            (int)CHAR_ANIM_ID.Stand,
            (int)CHAR_ANIM_ID.Walk,
            (int)CHAR_ANIM_ID.Run,
        };

        public bool IsCurAnimCanHeadLookAt(int nAnimId)
        {
            
            //限定动画下，才能看过去，否则头部有动画时，会穿帮
            foreach (var animId in CanLookAtAnimState)
            {
                if (nAnimId == animId)
                {
                    return true;
                }
            }

            return false;
        }
        #endregion

        //绑定的法宝
        public Obj_Talisman Talisman { get; set; }

        public MinionLogic ObjMinionLogic { get; private set; }

        public Obj_Minion AddMinion(int id)
        {
            if (ObjMinionLogic == null)
            {
                ObjMinionLogic = Utils.TryAddComponent<MinionLogic>(gameObject);
            }

            if (ObjMinionLogic == null)
            {
                return null;
            }
            return ObjMinionLogic.AddMinion(id);
        }

        /// <summary>
        /// 更换装饰特效
        /// </summary>
        /// <param name="ornamentEffectID">装饰特效表中的ID 非特效ID</param>
        public void ChangeOrnamentEffect(int ornamentEffectTabID)
        {
            if (OrgAvatarInfo == null)
            {
                return;
            }
            if (ornamentEffectTabID == OrgAvatarInfo.m_OrnamentEffectTabID)
            {
                return;
            }

            if (OrgAvatarInfo.m_OrnamentEffectTabID != GlobeVar.INVALID_ID)
            {
                //终止旧特效的播放
                Tab_OrnamentEffect tbOldOrnamentEffect = TableManager.GetOrnamentEffectByID(OrgAvatarInfo.m_OrnamentEffectTabID, 0);
                if (tbOldOrnamentEffect != null)
                {
                    StopEffect(tbOldOrnamentEffect.EffectID);
                }
                OrgAvatarInfo.m_OrnamentEffectTabID = GlobeVar.INVALID_ID;
            }

            if (ornamentEffectTabID != GlobeVar.INVALID_ID)
            {
                //播放新添加的特效
                Tab_OrnamentEffect tbOrnamentEffect = TableManager.GetOrnamentEffectByID(ornamentEffectTabID, 0);
                if (tbOrnamentEffect == null)
                {
                    return;
                }
                PlayEffect(tbOrnamentEffect.EffectID);
                OrgAvatarInfo.m_OrnamentEffectTabID = ornamentEffectTabID;
            }
        }
    }
}