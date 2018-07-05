/********************************************************************************
 *	文件名：	Obj_Fake.cs
 *	全路径：	\Script\Obj\Obj_Fake.cs
 *	创建人：	李嘉
 *	创建时间：2017-02-14
 *
 *	功能说明：游戏中的预览模型
 *	修改记录：
*********************************************************************************/
using System;
using Games.Animation_Modle;
using Games.GlobeDefine;
using UnityEngine;
using Games.Table;
using System.Collections.Generic;
using UnityChan;

namespace Games.LogicObj
{
    public class Obj_Fake : Obj
    {
        public delegate void OnLoadFakeObjModelOver();

        private int m_FakeObjId = GlobeVar.INVALID_ID;
        public int FakeObjId
        {
            get { return m_FakeObjId; }
            set { m_FakeObjId = value; }
        }

        private Avatar m_FakeAvatar = new Avatar();
        public Avatar FakeAvatar
        {
            get { return m_FakeAvatar; }
            set { m_FakeAvatar = value; }
        }

        private AvatarHeadMove m_AvatarHeadMove = null;
        private bool m_IsHeadLooking = false;
        private Transform m_LookAtTargetTrans = null;

        private int m_nLayer = GlobeVar.INVALID_ID;

        protected AnimationLogic m_AnimLogic = null;
        public AnimationLogic AnimLogic
        {
            get { return m_AnimLogic; }
            set { m_AnimLogic = value; }
        }
        protected Animation m_Objanimation; //!!!使用前记得判空

        private EffectLogic m_EffectLogic = null;
        public EffectLogic EffLogic
        {
            get { return m_EffectLogic; }
        }

        private List<OnLoadFakeObjModelOver> m_delOnLoadFakeObjModelOver = new List<OnLoadFakeObjModelOver>();

        private bool m_bModelOver = false;
        public bool ModelOver
        {
            get { return m_bModelOver; }
        }

        private Camera m_FakeCamera = null;
        public Camera FakeCamera
        {
            get { return m_FakeCamera; }
            set { m_FakeCamera = value; }
        }

        private bool m_IsRightFake;
        
        private List<SpringCollider> m_SpringCollierList = new List<SpringCollider>();

        private AvatarLoadInfo m_OrgAvatarInfo;

        public Obj_Fake()
        {
        }

        protected override OBJ_TYPE _getObjType()
        {
            return OBJ_TYPE.OBJ_FAKE;
        }

        void Update()
        {
            UpdateLookAtCameara();
        }

        //根据FakeObj配表加载
        public bool Init(int nFakeObjId, int nLayer = GlobeVar.LAYER_FAKEOBJ, OnLoadFakeObjModelOver delLoadOver = null, bool rightFake = false)
        {
            Tab_FakeObject tabFake = TableManager.GetFakeObjectByID(nFakeObjId, 0);
            if (tabFake == null)
            {
                return false;
            }

            m_IsRightFake = rightFake;

            m_AnimLogic = Utils.TryAddComponent<AnimationLogic>(gameObject);
            m_EffectLogic = Utils.TryAddComponent<EffectLogic>(gameObject);

            m_FakeObjId = nFakeObjId;
            m_nLayer = nLayer;

            m_delOnLoadFakeObjModelOver.Add(delLoadOver);
            m_bModelOver = false;

            m_FakeAvatar.InitFakeAvatar(nFakeObjId, ObjTransform, m_AnimLogic);
            m_FakeAvatar.ReloadFakeAvatar(OnLoadAvatarVisualFinish, AssetLoader.LoadQueueType.COMMON);

            return true;
        }

        //根据玩家加载
        public bool Init(int nFakeObjId, AvatarLoadInfo info, int nLayer = GlobeVar.LAYER_FAKEOBJ, OnLoadFakeObjModelOver delLoadOver = null, bool rightFake = false)
        {
            if (info == null)
            {
                return false;
            }
            m_OrgAvatarInfo = info;

            m_IsRightFake = rightFake;

            m_AnimLogic = Utils.TryAddComponent<AnimationLogic>(gameObject);
            m_EffectLogic = Utils.TryAddComponent<EffectLogic>(gameObject);
            
            m_FakeObjId = nFakeObjId;
            m_nLayer = nLayer;

            m_delOnLoadFakeObjModelOver.Add(delLoadOver);
            m_bModelOver = false;

            m_FakeAvatar.Init(info.m_BodyId, ObjTransform, m_AnimLogic);
            m_FakeAvatar.ReloadAvatar(info, OnLoadAvatarVisualFinish, AssetLoader.LoadQueueType.COMMON);

            return true;
        }

        public bool Init(int nFakeObjId, Obj_Char objChar, int nLayer = GlobeVar.LAYER_FAKEOBJ, OnLoadFakeObjModelOver delLoadOver = null, bool rightFake = false)
        {
            if (objChar == null)
            {
                return false;
            }

            if (objChar.CharAvatar == null)
            {
                return false;
            }

            m_IsRightFake = rightFake;

            AvatarLoadInfo info = new AvatarLoadInfo();
            info.m_BodyId = objChar.ModelID;

            return Init(nFakeObjId, info, nLayer, delLoadOver);
        }
        
        void OnLoadAvatarVisualFinish(List<string> reloadName)
        {
            if (this == null)
            {
                return;
            }

            //设置缩放和旋转
            ResetFakeObjTransform();
            LoadFakeObjFinish();

            m_FakeAvatar.SetVector("_LightDirection", new Vector4(10f, -0.8f, 25.1f, 0f));
        }

        void LoadFakeObjFinish()
        {
            if (this == null)
            {
                return;
            }
            if (transform == null)
            {
                return;
            }

			Utils.SetAllChildLayer(transform, m_nLayer);

            InitAnimation();
            m_EffectLogic.InitEffect(gameObject,gameObject.layer);
            
            m_AvatarHeadMove = Utils.TryAddComponent<AvatarHeadMove>(gameObject);
            InitHeadMove();

            if (m_delOnLoadFakeObjModelOver.Count > 0)
            {
                if (m_delOnLoadFakeObjModelOver[0] != null)
                {
                    m_delOnLoadFakeObjModelOver[0]();
                }
                m_delOnLoadFakeObjModelOver.RemoveAt(0);
            }
            //加载装饰特效
            if (m_OrgAvatarInfo != null && m_OrgAvatarInfo.m_OrnamentEffectTabID != GlobeVar.INVALID_ID)
            {
                Tab_OrnamentEffect tbOrnamentEffect = TableManager.GetOrnamentEffectByID(m_OrgAvatarInfo.m_OrnamentEffectTabID, 0);
                if (tbOrnamentEffect == null)
                {
                    return;
                }
                PlayEffect(tbOrnamentEffect.EffectID);
            }
            m_bModelOver = true;
        }

        public bool UseRightCoord()
        {
            Tab_FakeObject tabFake = TableManager.GetFakeObjectByID(m_FakeObjId, 0);
            if (tabFake == null)
            {
                return false;
            }
            return (m_IsRightFake && tabFake.MirrorValid == 1);
        }

        public void ResetFakeObjTransform()
        {
            Tab_FakeObject tabFake = TableManager.GetFakeObjectByID(m_FakeObjId, 0);
            if (tabFake == null)
            {
                return;
            }

            Transform _trans = transform;
            if (null == _trans)
            {
                return;
            }

            //这里先特殊处理，根据Parent的名字来进行不同的旋转和偏移
            //if (_trans.parent != null && GameManager.CurScene != null && _trans.parent == GameManager.CurScene.m_FakeObjModelRightRoot)
            if (m_IsRightFake && tabFake.MirrorValid == 1)
            {
                //LogModule.DebugLog("right fake " + m_FakeObjId);
                _trans.localPosition = new Vector3(tabFake.PosXR, tabFake.PosYR, tabFake.PosZR);
                _trans.localRotation = Quaternion.Euler(tabFake.RotXR, tabFake.RotYR, tabFake.RotZR);
            }
            else
            {
                //LogModule.DebugLog("left fake" + m_FakeObjId);
                _trans.localPosition = new Vector3(tabFake.PosXL, tabFake.PosYL, tabFake.PosZL);
                _trans.localRotation = Quaternion.Euler(tabFake.RotXL, tabFake.RotYL, tabFake.RotZL);
            }

            //设置缩放
            _trans.localScale = new Vector3(tabFake.Scale, tabFake.Scale, tabFake.Scale);

            m_SpringCollierList.Clear();
            SpringBone[] allBone = GetComponentsInChildren<SpringBone>();
            foreach (SpringBone bone in allBone)
            {
                bone.radius *= _trans.lossyScale.x;

                foreach (SpringCollider collider in bone.colliders)
                {
                    if (false == m_SpringCollierList.Contains(collider))
                    {
                        collider.radius *= _trans.lossyScale.x;
                        m_SpringCollierList.Add(collider);
                    }                 
                }
            }
        }

        public void ResetRotation()
        {
            Tab_FakeObject tabFake = TableManager.GetFakeObjectByID(m_FakeObjId, 0);
            if (tabFake == null)
            {
                return;
            }

            Transform _trans = transform;
            if (null == _trans)
            {
                return;
            }

            if (_trans.parent != null && _trans.parent.gameObject.name == "ModelRoot_R")
            {
                _trans.localRotation = Quaternion.Euler(tabFake.RotXR, tabFake.RotYR, tabFake.RotZR);
            }
            else
            {
                _trans.localRotation = Quaternion.Euler(tabFake.RotXL, tabFake.RotYL, tabFake.RotZL);
            }
        }

        public void UseNewFakeObjId_For_Position( int newFakeObjId )
        {
            Tab_FakeObject tabFake = TableManager.GetFakeObjectByID(newFakeObjId, 0);
            if (tabFake == null)
                return;

            m_FakeObjId = newFakeObjId;

            //transform.localPosition = new Vector3(tabFake.PosX, tabFake.PosY, tabFake.PosZ);
            //transform.localRotation = Quaternion.Euler(tabFake.RotX, tabFake.RotY, tabFake.RotZ);
            //transform.localScale = Vector3.one;
        }

        #region obj_fake的动画相关
        public void InitAnimation()
        {
            if ( null == m_AnimLogic )
            { // 尝试后还没有,则返回
                return;
            }

            Transform modelTransform = gameObject.transform.Find("Model");
            if ( modelTransform == null && FakeAvatar != null && FakeAvatar.BodyTrans != null )
            {
                modelTransform = FakeAvatar.BodyTrans.Find("Model");
            }
            if ( modelTransform )
            {
                m_Objanimation = modelTransform.gameObject.GetComponent<Animation>();
            }

            if ( m_Objanimation )
            {
                if (FakeAvatar.BodyId == GlobeVar.INVALID_ID)
                {
                    Tab_FakeObject tFakeObj = TableManager.GetFakeObjectByID(m_FakeObjId, 0);
                    if (tFakeObj != null)
                    {
                        m_AnimLogic.InitAnimInfo(m_Objanimation.gameObject, tFakeObj.CommonAnimPath, tFakeObj.AnimPath);
                    }
                }
                else
                {
                    Tab_CharModel tabCharModel = TableManager.GetCharModelByID(FakeAvatar.BodyId, 0);
                    if (null != tabCharModel)
                    {
                        m_AnimLogic.InitAnimInfo(m_Objanimation.gameObject, tabCharModel.CommonAnimPath, tabCharModel.AnimPath);
                    }
                }
            }
            else
            {
                //LogModule.DebugLog(
                //   "!!! The character you would like to control doesn't have animations. Moving her might look weird.");
            }

            if ( m_AnimLogic != null)
            {
                PlayAnimation((int)CHAR_ANIM_ID.Stand);
            }
        }

        public bool PlayAnimation(int animId, bool nextPlayDefault = true)
        {
            if (AnimLogic != null)
            {
                m_CurAnimId = animId;
                if (nextPlayDefault)
                {
                    return PlayAnimWithNotify(animId, OnPlayAnimFinish);
                }
                return AnimLogic.Play(animId);
            }
            return false;
        }

        private void OnPlayAnimFinish(int id, bool callByStop, bool hasNext)
        {
            if (callByStop)
            {
                return;
            }
            if (!hasNext)
            {
                m_CurAnimId = (int)CHAR_ANIM_ID.Stand;
                if (AnimLogic != null)
                {
                    AnimLogic.ListenAnimFinish(null);
                    AnimLogic.Play((int)CHAR_ANIM_ID.Stand);
                }
            }
        }

        public bool PlayAnimWithNotify(int nAnimId, AnimationLogic.DelAnimFinish del)
        {
            if (AnimLogic != null)
            {
                m_CurAnimId = nAnimId;
                AnimLogic.ListenAnimFinish(del);
                return AnimLogic.Play(nAnimId);
            }
            return false;
        }
        #endregion

        public void InitHeadMove()
        {
            if (m_AvatarHeadMove == null || m_FakeAvatar == null)
            {
                return;
            }
            m_AvatarHeadMove.head = m_FakeAvatar.FindCharModelPoint(CharBindPoint.HeadPoint);
            m_AvatarHeadMove.neck = m_FakeAvatar.FindCharModelPoint(CharBindPoint.Neck);
        }

        public void HeadLookAt(Transform target)
        {
            if (m_AvatarHeadMove == null)
            {
                return;
            }
            m_AvatarHeadMove.lookAtTarget = target;
            m_LookAtTargetTrans = target;
        }

        public void BeginHeadLookAt(bool pitch = true, bool ignoreAnimState = false)
        {
            if (m_AvatarHeadMove == null)
            {
                m_AvatarHeadMove = Utils.TryAddComponent<AvatarHeadMove>(gameObject);
                InitHeadMove();
            }

            if (ignoreAnimState || !IsCurAnimCanHeadLookAt())
            {
                return;
            }

            m_AvatarHeadMove.isLookAtRotX = pitch;
            m_AvatarHeadMove.enabled = true;
            m_AvatarHeadMove.animLogc = m_AnimLogic;
            m_IsHeadLooking = true;
        }

        private int m_CurAnimId = -1;

        public bool IsCurAnimCanHeadLookAt()
        {
            //限定动画下，才能看过去，否则头部有动画时，会穿帮
            foreach (var animId in Obj_Char.CanLookAtAnimState)
            {
                if (m_CurAnimId == animId)
                {
                    return true;
                }
            }

            return false;
        }

        void UpdateLookAtCameara()
        {
            bool isStanding = m_CurAnimId == (int)CHAR_ANIM_ID.Stand;
            bool isInStoryMode = GameManager.storyManager.StoryMode;
            bool shouldPlay = isStanding && !isInStoryMode;

            if (!m_IsHeadLooking)
            {
                //保证只触发一次,不会每次都取transform
                if (shouldPlay && m_LookAtTargetTrans != null)
                {
                    BeginHeadLookAt();
                    HeadLookAt(m_LookAtTargetTrans);
                }
            }
            else
            {
                if (!shouldPlay)
                {
                    StopHeadLookAt();
                }
            }
        }

        void StopHeadLookAt(bool immediate = false)
        {
            if (m_AvatarHeadMove == null)
            {
                return;
            }
            m_AvatarHeadMove.StopLookAt();
            if (immediate)
            {
                m_AvatarHeadMove.enabled = false;
            }
            m_IsHeadLooking = false;
        }

        public Obj_Talisman Talisman { get; set; }
        public void AddCurHeroEquipedTalisman()
        {
            Hero hero = GameManager.PlayerDataPool.PlayerHeroData.GetCurHero();
            if (hero.EquipedTalisman != null && hero.EquipedTalisman.IsValid())
            {
                AddTalisman(hero.EquipedTalisman.GetVisualId());
            }
        }

        public void AddTalisman(int visualId)
        {
            Tab_TalismanVisual tab = TableManager.GetTalismanVisualByID(visualId, 0);
            if (tab == null)
            {
                return;
            }

            Obj_Init_Data initData = new Obj_Init_Data();
            initData.m_nModelID = tab.ModelId;
            Talisman = ObjManager.CreateTalisman(initData, ObjTransform);
            AssetManager.SetObjParent(Talisman.gameObject, ObjTransform);
            Utils.SetAllChildLayer(Talisman.ObjTransform,gameObject.layer);
            Talisman.PlayAnim(43);
        }

        public void RefreshCurHeroEquipedTalisman()
        {
            Hero hero = GameManager.PlayerDataPool.PlayerHeroData.GetCurHero();

            if (Talisman != null)
            {
                ObjManager.RemoveObj(Talisman);
                Talisman = null;
            }

            if (hero.EquipedTalisman != null && hero.EquipedTalisman.IsValid())
            {
                AddTalisman(hero.EquipedTalisman.GetVisualId());
            }
        }

        public void PlayEffect(int nEffectId, EffectLogic.EffectLoadOverDelegate delEffectLoadOver = null, EffectLogic.EffectStopDelegate delEffectStop = null, object param = null, bool bFromUseSkill = false)
        {
            if (m_EffectLogic == null)
            {
                return;
            }

            m_EffectLogic.PlayEffect(nEffectId, delEffectLoadOver, delEffectStop, param, bFromUseSkill);
        }

        public void StopEffect(int effectID, bool bStopAll = true)
        {
            if (m_EffectLogic != null)
            {
                m_EffectLogic.StopEffect(effectID, bStopAll);
            }
        }

        public Transform FindCharModelPoint(string pointName)
        {
            if (pointName == "EffectPoint")
            {
                if (m_EffectLogic != null)
                {
                    return m_EffectLogic.EffectPointTrans;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                if (FakeAvatar != null)
                {
                    return FakeAvatar.FindCharModelPoint(pointName);
                }
                else
                {
                    LogModule.WarningLog("Avatar Not Loaded");
                    return null;
                }
            }
        }

        /// <summary>
        /// 更换装饰特效
        /// </summary>
        /// <param name="ornamentEffectID"></param>
        public void ChangeOrnamentEffect(int ornamentEffectTabID)
        {
            if (m_OrgAvatarInfo == null)
            {
                return;
            }
            if (ornamentEffectTabID == m_OrgAvatarInfo.m_OrnamentEffectTabID)
            {
                return;
            }

            if (m_OrgAvatarInfo.m_OrnamentEffectTabID != GlobeVar.INVALID_ID)
            {
                //终止旧特效的播放
                Tab_OrnamentEffect tbOldOrnamentEffect = TableManager.GetOrnamentEffectByID(m_OrgAvatarInfo.m_OrnamentEffectTabID, 0);
                if (tbOldOrnamentEffect != null)
                {
                    StopEffect(tbOldOrnamentEffect.EffectID);
                }
                m_OrgAvatarInfo.m_OrnamentEffectTabID = GlobeVar.INVALID_ID;
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
                m_OrgAvatarInfo.m_OrnamentEffectTabID = ornamentEffectTabID;
            }
        }
    }
}
