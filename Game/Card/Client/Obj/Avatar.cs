using UnityEngine;
using Games.Table;
using Games.GlobeDefine;
using System.Collections.Generic;
using Games.Animation_Modle;

namespace Games.LogicObj
{
    public class AvatarLoadInfo
    {
        public int m_BodyId = GlobeVar.INVALID_ID;      // 身体        
        public int m_WeaponId = GlobeVar.INVALID_ID;    // 武器 
        //public int m_FaceId = GlobeVar.INVALID_ID;      // 脸
        //public int m_HairId = GlobeVar.INVALID_ID;      // 头发
        public int m_SoulWareId = GlobeVar.INVALID_ID;      // 魂器模型id
        public int m_RoleMaskModelId = GlobeVar.INVALID_ID; //变身Id（用于特殊显示魂器和身体模型）

        public bool m_HasDyeColor = false;                 //是否有染色
        public Color m_DyeColor0 = Color.white;         //染色颜色
        public Color m_DyeColor1 = Color.white;
        public Color m_DyeColor2 = Color.white;
        public int m_OrnamentEffectTabID = GlobeVar.INVALID_ID;//装饰特效ID
        public void LoadDyeColor(int dyeColorId)
        {
            if (dyeColorId != GlobeVar.INVALID_ID)
            {
                m_HasDyeColor = true;
                Tab_DyeColor tabDyeColor = TableManager.GetDyeColorByID(dyeColorId, 0);
                if (tabDyeColor != null)
                {
                    m_DyeColor0 = Utils.ParseColor(tabDyeColor.GetRGBbyIndex(0));
                    m_DyeColor1 = Utils.ParseColor(tabDyeColor.GetRGBbyIndex(1));
                    m_DyeColor2 = Utils.ParseColor(tabDyeColor.GetRGBbyIndex(2));
                }
            }
            else
            {
                m_HasDyeColor = false;
            }
        }
    }

    public class Avatar
    {
        //public AvatarLoadInfo CloneLoadInfo()
        //{
        //    AvatarLoadInfo info = new AvatarLoadInfo();
        //    info.m_BodyId = m_BodyId;
        //    info.m_WeaponId = m_WeaponId;
        //    //info.m_FaceId = m_FaceId;
        //    //info.m_HairId = m_HairId;
        //    info.m_SoulWareId = m_SoulWareId;
        //    info.m_RoleMaskModelId = m_RoleMaskModelId;
        //    return info;
        //}

        //public AvatarLoadInfo CloneInteractLoadInfo(bool bLoadWeapon, bool bLoadTalisman)
        //{
        //    AvatarLoadInfo info = new AvatarLoadInfo();
        //    info.m_BodyId = m_BodyId;
        //    info.m_WeaponId = bLoadWeapon ? m_WeaponId : GlobeVar.INVALID_ID;
        //    //info.m_FaceId = m_FaceId;
        //    //info.m_HairId = m_HairId;
        //    info.m_RoleMaskModelId = m_RoleMaskModelId;
        //    return info;
        //}

        // Avatar总挂点
        private Transform m_AvatarTrans = null;
        public Transform AvatarTrans
        {
            get { return m_AvatarTrans; }
            set { m_AvatarTrans = value; }
        }

        // 身体
        private int m_BodyId = GlobeVar.INVALID_ID;
        public int BodyId
        {
            get { return m_BodyId; }
            set { m_BodyId = value; }
        }

        private int m_RoleMaskModelId = GlobeVar.INVALID_ID;
        public int RoleMaskModelId
        {
            get { return m_RoleMaskModelId; }
            set { m_RoleMaskModelId = value; }
        }

        // 武器
        private int m_WeaponId = GlobeVar.INVALID_ID;
        public int WeaponId
        {
            get { return m_WeaponId; }
            set { m_WeaponId = value; }
        }

        // 法宝
        private int m_TalismanId = GlobeVar.INVALID_ID;
        public int TalismanId
        {
            get { return m_TalismanId; }
            set { m_TalismanId = value; }
        }

        // 魂器模型id
        private int m_SoulWareId = GlobeVar.INVALID_ID;
        public int SoulWareId
        {
            get { return m_SoulWareId; }
            set { m_SoulWareId = value; }
        }

        //// 脸
        //private int m_FaceId = GlobeVar.INVALID_ID;
        //public int FaceId
        //{
        //    get { return m_FaceId; }
        //    set { m_FaceId = value; }
        //}

        //// 头发
        //private int m_HairId = GlobeVar.INVALID_ID;
        //public int HairId
        //{
        //    get { return m_HairId; }
        //    set { m_HairId = value; }
        //}

        //// 坐骑
        //private int m_MountId = GlobeVar.INVALID_ID;
        //public int MountId
        //{
        //    get { return m_MountId; }
        //    set { m_MountId = value; }
        //}

        private List<Renderer> m_Renderers;
        public List<Renderer> Renderers;

        private int m_FakeObjId = GlobeVar.INVALID_ID;
        public int FakeObjId
        {
            get { return m_FakeObjId; }
        }

        // 身体挂点
        private Transform m_BodyTrans = null;
        public Transform BodyTrans
        {
            get { return m_BodyTrans; }
            set { m_BodyTrans = value; }
        }

        public Transform HeightTrans { get; private set; }

        // 左手武器
        private Transform m_WeaponLTrans = null;
        public Transform WeaponLTrans
        {
            get { return m_WeaponLTrans; }
            set { m_WeaponLTrans = value; }
        }

        // 右手武器
        private Transform m_WeaponRTrans = null;
        public Transform WeaponRTrans
        {
            get { return m_WeaponRTrans; }
            set { m_WeaponRTrans = value; }
        }

        // 脸
        private Transform m_FaceTrans = null;
        public Transform FaceTrans
        {
            get { return m_FaceTrans; }
            set { m_FaceTrans = value; }
        }

        // 头发
        private Transform m_HairTrans = null;
        public Transform HairTrans
        {
            get { return m_HairTrans; }
            set { m_HairTrans = value; }
        }

        // 魂器
        private Transform m_SoulWareTrans = null;
        public Transform SoulWareTrans
        {
            get { return m_SoulWareTrans; }
            set { m_SoulWareTrans = value; }
        }

        private Dictionary<string, Transform> m_dicAvatarCache = new Dictionary<string, Transform>();         //存放一级节点
        private Dictionary<string, Transform> m_dicModelChildCache = new Dictionary<string, Transform>();     //Model下的二级节点

        public delegate void DelReloadAvatarFinish(List<string> reloadName = null);

        private AnimationLogic m_ModelAnimLogic = null;

        private MaterialPropertyBlock m_MatPropBlock;

        public MaterialPropertyBlock MatPropBlock
        {
            get
            {
                if (m_MatPropBlock == null)
                {
                    m_MatPropBlock = new MaterialPropertyBlock();
                }
                return m_MatPropBlock;
            }
        }

        private bool m_EnableDye = false;
        private bool m_HasEnabledDye = false;
        private bool m_IsMatDyeColored = false;

        private Color m_dyeColor0;
        private Color m_dyeColor1;
        private Color m_dyeColor2;

        private bool m_DyeTexLoaded = false;

        private bool m_bLoading = false;
        public bool InLoading
        {
            get { return m_bLoading; }
        }

        private bool m_MatPropertyDirty = false;

        private Color m_Color = Color.white;

        private Dictionary<string, Transform> m_dicBindPointCache = new Dictionary<string, Transform>();
        public Dictionary<string, Transform> BindPointCache
        {
            get { return m_dicBindPointCache; }
        }

        public void Init(int nCharModelId, Transform avatarTrans, AnimationLogic modelAnimLogic, bool initHeightPoint = true)
        {
            if (avatarTrans == null)
            {
                return;
            }

            m_AvatarTrans = avatarTrans.Find("Avatar");
            if (m_AvatarTrans == null)
            {
                m_AvatarTrans = avatarTrans;
            }

            m_BodyTrans = m_AvatarTrans;
            m_FaceTrans = m_AvatarTrans;
            m_HairTrans = m_AvatarTrans;
            m_BodyId = nCharModelId;
            m_FakeObjId = GlobeVar.INVALID_ID;
            m_ModelAnimLogic = modelAnimLogic;
            if (initHeightPoint)
            {
                InitHeightPoint();
            }
            UpdateAvatarPartsBind();
        }

        public void InitFakeAvatar(int nFakeObjId, Transform avatarTrans, AnimationLogic modelAnimLogic)
        {
            if (avatarTrans == null)
            {
                return;
            }

            m_AvatarTrans = avatarTrans.Find("Avatar");
            if (m_AvatarTrans == null)
            {
                m_AvatarTrans = avatarTrans;
            }

            m_BodyTrans = m_AvatarTrans;
            m_FaceTrans = null;
            m_HairTrans = null;
            m_BodyId = GlobeVar.INVALID_ID;
            m_FakeObjId = nFakeObjId;
            m_ModelAnimLogic = modelAnimLogic;
            m_RoleMaskModelId = GlobeVar.INVALID_ID;
        }

        private void InitHeightPoint()
        {
            if (HeightTrans != null)
            {
                GameObject.DestroyImmediate(HeightTrans.gameObject);
            }
            GameObject go = new GameObject("HeightPoint");
            HeightTrans = go.transform;
            HeightTrans.SetParent(m_AvatarTrans);
            HeightTrans.localPosition = Vector3.zero;
            HeightTrans.localRotation = Quaternion.identity;
            HeightTrans.localScale = Vector3.one;

            if (m_BodyId != GlobeVar.INVALID_ID)
            {
                Tab_CharModel tab = TableManager.GetCharModelByID(m_BodyId, 0);
                if (tab != null)
                {
                    HeightTrans.localPosition = new Vector3(0f, tab.Height, 0f);
                }
            }
        }

        public void ReloadAvatar(AvatarLoadInfo info, DelReloadAvatarFinish delFinish = null, AssetLoader.LoadQueueType queueType = AssetLoader.LoadQueueType.COMMON)
        {
            BundleTask task = new BundleTask(OnReloadAvatarFinish);
            task.AddParam(delFinish);

            Tab_CharModel tabAvatarBody = TableManager.GetCharModelByID(info.m_BodyId, 0);
            if (tabAvatarBody != null)
            {
                if (tabAvatarBody.ResPath != "Empty" && !string.IsNullOrEmpty(tabAvatarBody.ResPath))
                {
                    task.Add(BundleTask.BundleType.MODEL, tabAvatarBody.ResPath, "Model");
                }
                m_BodyId = info.m_BodyId;
            }

            Tab_AvatarModel tabAvatarWeapon = TableManager.GetAvatarModelByID(info.m_WeaponId, 0);
            if (tabAvatarWeapon != null)
            {
                if (!string.IsNullOrEmpty(tabAvatarWeapon.ResPath))
                {
                    task.Add(BundleTask.BundleType.MODEL, tabAvatarWeapon.ResPath, "WeaponR");
                }
                m_WeaponId = info.m_WeaponId;
            }

            //Tab_AvatarModel tabAvatarFace = TableManager.GetAvatarModelByID(info.m_FaceId, 0);
            //if (tabAvatarFace != null)
            //{
            //    task.Add(BundleTask.BundleType.MODEL, tabAvatarFace.ResPath, "Face");
            //    m_FaceId = info.m_FaceId;
            //}

            //Tab_AvatarModel tabAvatarHair = TableManager.GetAvatarModelByID(info.m_HairId, 0);
            //if (tabAvatarHair != null)
            //{
            //    task.Add(BundleTask.BundleType.MODEL, tabAvatarHair.ResPath, "Hair");
            //    m_HairId = info.m_HairId;
            //}

            if (info.m_SoulWareId == GlobeVar.INVALID_ID || GlobeVar.INVALID_ID != info.m_RoleMaskModelId)
            {
                Transform soulwareTrans = null;
                if (m_dicModelChildCache.TryGetValue("SoulWare", out soulwareTrans))
                {
                    GameObject.Destroy(soulwareTrans.gameObject);
                    m_dicModelChildCache.Remove("SoulWare");
                }
                //清除soulwareid
                m_SoulWareId = info.m_SoulWareId;
            }
            else
            {
                Tab_AvatarModel tabSoulWare = TableManager.GetAvatarModelByID(info.m_SoulWareId, 0);
                if (tabSoulWare != null)
                {
                    if (!string.IsNullOrEmpty(tabSoulWare.ResPath))
                    {
                        task.Add(BundleTask.BundleType.MODEL, tabSoulWare.ResPath, "SoulWare");
                    }
                    m_SoulWareId = info.m_SoulWareId;
                }
            }

            if (info.m_HasDyeColor)
            {
                SetDyeColor(info.m_DyeColor0,info.m_DyeColor1,info.m_DyeColor2);
            }
            else
            {
                //重新加载不同的模型，需要清掉旧的染色，染色数据和模型是绑定的
                ClearDyeColor();
            }
            m_bLoading = true;
            if (task.DoingCount() > 0)
            {
                AssetManager.LoadBundle(task, queueType);
            }
            else
            {
                OnReloadAvatarFinish(task);
            }
        }

        public void ReloadFakeAvatar(DelReloadAvatarFinish delFinish = null, AssetLoader.LoadQueueType queueType = AssetLoader.LoadQueueType.COMMON)
        {
            BundleTask task = new BundleTask(OnReloadAvatarFinish);
            task.AddParam(delFinish);

            Tab_FakeObject tFakeObj = TableManager.GetFakeObjectByID(m_FakeObjId, 0);
            if (tFakeObj != null)
            {
                task.Add(BundleTask.BundleType.MODEL, tFakeObj.ResPath, "Model");
            }
            m_bLoading = true;
            AssetManager.LoadBundle(task, queueType);
        }

        // 挂载关系 坐骑-身体-面部 发型 武器 法宝可能和身体平级 也可能在身体内部
        void OnReloadAvatarFinish(BundleTask task)
        {
            m_bLoading = false;

            if (this == null)
            {
                return;
            }

            if (null == task)
            {
                return;
            }

            if (null == m_AvatarTrans)
            {
                return;
            }

            List<string> reloadName = new List<string>();
            DelReloadAvatarFinish delFinish = task.GetParamByIndex(0) as DelReloadAvatarFinish;
            
            // 身体
            UnityEngine.Object BodyRes = task.GetFinishObjByTag("Model");
            if (BodyRes != null)
            {
                reloadName.Add("Model");

                GameObject objBody = AssetManager.InstantiateObjToParent(BodyRes, m_AvatarTrans, "Model");
                if (objBody != null)
                {
                    //if (m_dicAvatarCache.ContainsKey("Mount"))
                    //{
                    //    objBody.transform.parent = m_BodyTrans;
                    //    if (m_dicAvatarCache.ContainsKey("Model"))
                    //    {
                    //        objBody.transform.localRotation = m_dicAvatarCache["Model"].transform.localRotation;
                    //        objBody.transform.localPosition = m_dicAvatarCache["Model"].transform.localPosition;
                    //    }
                    //    else
                    //    {
                    //        objBody.transform.localPosition = Vector3.zero;
                    //    }
                    //}

                    if (m_dicAvatarCache.ContainsKey("Model") && null != m_dicAvatarCache["Model"])
                    {
                        m_dicAvatarCache["Model"].name = "ModelDel";
                    }

                    if (m_dicAvatarCache.ContainsKey("Model"))
                    {
                        GameObject.Destroy(m_dicAvatarCache["Model"].gameObject);
                        m_dicAvatarCache["Model"] = objBody.transform;
                    }
                    else
                    {
                        m_dicAvatarCache.Add("Model", objBody.transform);
                    }

                    // 更新各部位位置
                    UpdateAvatarPartsBind();
                }
            }

            // 武器均为右手
            UnityEngine.Object WeaponRRes = task.GetFinishObjByTag("WeaponR");
            if (WeaponRRes != null)
            {
                reloadName.Add("WeaponR");

                GameObject objWeaponR = AssetManager.InstantiateObjToParent(WeaponRRes, m_WeaponRTrans, "WeaponR");
                if (objWeaponR != null)
                {
                    Tab_AvatarModel tabModel = TableManager.GetAvatarModelByID(m_WeaponId, 0);
                    Transform mTransform = objWeaponR.transform;
                    if (tabModel != null)
                    {
                        mTransform.localPosition = new Vector3(tabModel.PosX, tabModel.PosY, tabModel.PosZ);
                        mTransform.localRotation = Quaternion.Euler(new Vector3(tabModel.RotX, tabModel.RotY, tabModel.RotZ));
                        mTransform.localScale = tabModel.Scale * Vector3.one;

                        //Tab_AvatarModel tWeaponModel = TableManager.GetAvatarModelByID(m_WeaponId, 0);
                        //UpdateModelEffectScale(mTransform, tWeaponModel);
                    }

                    if (m_dicModelChildCache.ContainsKey("WeaponR"))
                    {
                        GameObject.Destroy(m_dicModelChildCache["WeaponR"].gameObject);
                        m_dicModelChildCache["WeaponR"] = mTransform;
                    }
                    else
                    {
                        m_dicModelChildCache.Add("WeaponR", mTransform);
                    }
                }
            }

            // 脸
            //UnityEngine.Object FaceRes = task.GetFinishObjByTag("Face");
            //if (FaceRes != null)
            //{
            //    reloadName.Add("Face");

            //    GameObject objFace = AssetManager.InstantiateObjToParent(FaceRes, m_AvatarTrans, "Face");
            //    if (objFace != null)
            //    {
            //        if (m_dicAvatarCache.ContainsKey("Mount"))
            //        {
            //            objFace.transform.parent = m_FaceTrans;
            //            if (m_dicAvatarCache.ContainsKey("Face"))
            //            {
            //                if (null != m_dicAvatarCache["Face"])
            //                {
            //                    objFace.transform.localRotation = m_dicAvatarCache["Face"].transform.localRotation;
            //                    objFace.transform.localPosition = m_dicAvatarCache["Face"].transform.localPosition;
            //                }
            //            }
            //            else
            //            {
            //                objFace.transform.localPosition = Vector3.zero;
            //            }
            //        }

            //        if (m_dicAvatarCache.ContainsKey("Face"))
            //        {
            //            GameObject.Destroy(m_dicAvatarCache["Face"]);
            //            m_dicAvatarCache["Face"] = objFace;
            //        }
            //        else
            //        {
            //            m_dicAvatarCache.Add("Face", objFace);
            //        }
            //    }
            //}

            //// 头发
            //UnityEngine.Object HairRes = task.GetFinishObjByTag("Hair");
            //if (HairRes != null)
            //{
            //    reloadName.Add("Hair");

            //    GameObject objHair = AssetManager.InstantiateObjToParent(HairRes, m_AvatarTrans, "Hair");
            //    if (objHair != null)
            //    {
            //        if (m_dicAvatarCache.ContainsKey("Mount"))
            //        {
            //            objHair.transform.parent = m_HairTrans;
            //            if (m_dicAvatarCache.ContainsKey("Hair"))
            //            {
            //                if (null != m_dicAvatarCache["Hair"])
            //                {
            //                    objHair.transform.localRotation = m_dicAvatarCache["Hair"].transform.localRotation;
            //                    objHair.transform.localPosition = m_dicAvatarCache["Hair"].transform.localPosition;
            //                }
            //            }
            //            else
            //            {
            //                objHair.transform.localPosition = Vector3.zero;
            //            }
            //        }

            //        if (m_dicAvatarCache.ContainsKey("Hair"))
            //        {
            //            GameObject.Destroy(m_dicAvatarCache["Hair"]);
            //            m_dicAvatarCache["Hair"] = objHair;
            //        }
            //        else
            //        {
            //            m_dicAvatarCache.Add("Hair", objHair);
            //        }
            //    }
            //}

            // 魂器
            UnityEngine.Object SoulWareRes = task.GetFinishObjByTag("SoulWare");
            if (SoulWareRes != null)
            {
                reloadName.Add("SoulWare");

                GameObject objSoulWare = AssetManager.InstantiateObjToParent(SoulWareRes, m_SoulWareTrans, "SoulWare");
                if (objSoulWare != null)
                {
                    Tab_AvatarModel tabModel = TableManager.GetAvatarModelByID(m_SoulWareId, 0);
                    Transform mTransform = objSoulWare.transform;
                    if (tabModel != null)
                    {
                        mTransform.localPosition = new Vector3(tabModel.PosX, tabModel.PosY, tabModel.PosZ);
                        mTransform.localRotation = Quaternion.Euler(new Vector3(tabModel.RotX, tabModel.RotY, tabModel.RotZ));
                        mTransform.localScale = tabModel.Scale * Vector3.one;

                        // azuki temp: soulware point refix
                        // 根据每个皮肤调整位置
                        Vector3 offset = TalismanTool.GetSkinModelOffset(m_SoulWareId, m_BodyId);
                        mTransform.localPosition += offset;

                        //Tab_AvatarModel tModel = TableManager.GetAvatarModelByID(m_SoulWareId, 0);
                        //UpdateModelEffectScale(mTransform, tModel);
                    }

                    if (m_dicModelChildCache.ContainsKey("SoulWare"))
                    {
                        GameObject.Destroy(m_dicModelChildCache["SoulWare"].gameObject);
                        m_dicModelChildCache["SoulWare"] = mTransform;
                    }
                    else
                    {
                        m_dicModelChildCache.Add("SoulWare", mTransform);
                    }
                }
            }

            m_Renderers = new List<Renderer>();
            foreach (var pair in m_dicAvatarCache)
            {
                if (pair.Value != null)
                {
                    Transform trans = pair.Value;
                    var rS = trans.GetComponentsInChildren<Renderer>(true);
                    foreach (var renderer in rS)
                    {
                        if (renderer.GetComponent<ParticleSystem>() != null)
                        {
                            continue;
                        }
                        m_Renderers.Add(renderer);
                    }
                }
            }

            m_DyeTexLoaded = false;
            m_IsMatDyeColored = false;

            //是否本身就是激活染色
            foreach (var mRenderer in m_Renderers)
            {
                if (mRenderer == null) continue;
                Material[] mats = mRenderer.sharedMaterials;
                foreach (var mat in mats)
                {
                    if (mat == null) continue;
                    if (mat.HasProperty("_DyeColor") && mat.IsKeywordEnabled("DYE_ON"))
                    {
                        m_IsMatDyeColored = true;
                        break;
                    }
                }
            }
            ApplyMat();

            UpdateSubAnimation(m_ModelAnimLogic);
            InitCharModelPointList();

            if (delFinish != null)
            {
                delFinish(reloadName);
            }
        }

        //void UpdateModelEffectScale(Transform weaponTrans, Tab_AvatarModel tabAvatarModel)
        //{
        //    //Tab_AvatarModel tWeaponModel = TableManager.GetAvatarModelByID(m_WeaponId, 0);
        //    if (tabAvatarModel != null && weaponTrans != null)
        //    {
        //        ParticleSystem[] allChild = weaponTrans.GetComponentsInChildren<ParticleSystem>();
        //        for (int i = 0; i < allChild.Length; i++)
        //        {
        //            allChild[i].startSize = allChild[i].startSize * tabAvatarModel.Scale;
        //            allChild[i].startSpeed = allChild[i].startSpeed * tabAvatarModel.Scale;
        //            allChild[i].startRotation = allChild[i].startRotation * tabAvatarModel.Scale;

        //            allChild[i].gameObject.SetActive(false);
        //            allChild[i].gameObject.SetActive(true);
        //        }
        //    }
        //}
        
        //挂载其他部位的动画组建 到身体的动画逻辑里 这样该部位可以保持和身体做一样的动作
        public void UpdateSubAnimation(AnimationLogic _modeAnimationLogic)
        {
            //将头的动画组建 挂载到模型的动画逻辑上
            if (_modeAnimationLogic != null)
            {
                //if (m_dicAvatarCache.ContainsKey("Face") &&
                //    m_dicAvatarCache["Face"] != null)
                //{
                //    _modeAnimationLogic.AddSubAnimation("Face", m_dicAvatarCache["Face"].GetComponent<Animation>());
                //}
                //if (m_dicAvatarCache.ContainsKey("Hair") &&
                //    m_dicAvatarCache["Hair"] != null)
                //{
                //    _modeAnimationLogic.AddSubAnimation("Hair", m_dicAvatarCache["Hair"].GetComponent<Animation>());
                //}
                //if (m_dicAvatarCache.ContainsKey("Mount") &&
                //   m_dicAvatarCache["Mount"] != null)
                //{
                //    _modeAnimationLogic.AddSubAnimation("Mount", m_dicAvatarCache["Mount"].GetComponent<Animation>());
                //}
            }
            m_ModelAnimLogic = _modeAnimationLogic;
        }
        public void DestroyAll(bool immediately = true)
        {
            foreach (Transform trans in m_dicAvatarCache.Values)
            {
                if (immediately)
                {
                    GameObject.DestroyImmediate(trans.gameObject);
                }
                else
                {
                    GameObject.Destroy(trans.gameObject);
                }
            }
            m_dicAvatarCache.Clear();
            m_dicModelChildCache.Clear();

            m_BodyId = GlobeVar.INVALID_ID;
            m_WeaponId = GlobeVar.INVALID_ID;
            m_TalismanId = GlobeVar.INVALID_ID;
            //m_FaceId = GlobeVar.INVALID_ID;
            //m_MountId = GlobeVar.INVALID_ID;

            //if (m_ModelAnimLogic != null)
            //{
            //    m_ModelAnimLogic.RemoveSubAnimation("Face");
            //    m_ModelAnimLogic.RemoveSubAnimation("Hair");
            //}
        }

        //删除一个Avatar节点，删除坐骑重新绑定，删除Model会把Model子节点一起删除
        //其他节点只删除当前节点
        public void DestroyAvatarModel(string szAvatarName)
        {
            Dictionary<string, Transform> curDic = null;
            if (szAvatarName == "Mount")
            {
                //删除坐骑，重新绑定Model

                m_BodyTrans = m_AvatarTrans;
                if (m_dicAvatarCache.ContainsKey("Model"))
                {
                    Transform mTransform = m_dicAvatarCache["Model"].transform;
                    mTransform.parent = m_BodyTrans;
                    mTransform.localPosition = Vector3.zero;
                    mTransform.localRotation = Quaternion.identity;
                }

                //m_FaceTrans = m_AvatarTrans;
                //if (m_dicAvatarCache.ContainsKey("Face"))
                //{
                //    Transform mTransform = m_dicAvatarCache["Face"].transform;
                //    mTransform.parent = m_FaceTrans;
                //    mTransform.localPosition = Vector3.zero;
                //    mTransform.localRotation = Quaternion.identity;
                //}

                //m_HairTrans = m_AvatarTrans;
                //if (m_dicAvatarCache.ContainsKey("Hair"))
                //{
                //    Transform mTransform = m_dicAvatarCache["Hair"].transform;
                //    mTransform.parent = m_HairTrans;
                //    mTransform.localPosition = Vector3.zero;
                //    mTransform.localRotation = Quaternion.identity;
                //}
            }
            else if (szAvatarName == "Model")
            {
                m_dicModelChildCache.Clear();
            }

            if (szAvatarName == "Weapon")
            {
                if (m_dicModelChildCache.ContainsKey("WeaponR"))
                {
                    if (m_dicModelChildCache.ContainsKey("WeaponRefineEffect_RHand"))
                    {
                        GameObject.Destroy(m_dicModelChildCache["WeaponRefineEffect_RHand"].gameObject);
                        m_dicModelChildCache.Remove("WeaponRefineEffect_RHand");
                    }

                    GameObject.Destroy(m_dicModelChildCache["WeaponR"].gameObject);
                    m_dicModelChildCache.Remove("WeaponR");
                }
                if (m_dicModelChildCache.ContainsKey("WeaponL"))
                {
                    if (m_dicModelChildCache.ContainsKey("WeaponRefineEffect_LHand"))
                    {
                        GameObject.Destroy(m_dicModelChildCache["WeaponRefineEffect_LHand"].gameObject);
                        m_dicModelChildCache.Remove("WeaponRefineEffect_LHand");
                    }

                    GameObject.Destroy(m_dicModelChildCache["WeaponL"].gameObject);
                    m_dicModelChildCache.Remove("WeaponL");
                }
            }
            //else if (szAvatarName == "WeaponRefineVisual")
            //{
            //    if (m_dicModelChildCache.ContainsKey("WeaponRefineEffect_RHand"))
            //    {
            //        GameObject.Destroy(m_dicModelChildCache["WeaponRefineEffect_RHand"]);
            //        m_dicModelChildCache.Remove("WeaponRefineEffect_RHand");
            //    }
            //    if (m_dicModelChildCache.ContainsKey("WeaponRefineEffect_LHand"))
            //    {
            //        GameObject.Destroy(m_dicModelChildCache["WeaponRefineEffect_LHand"]);
            //        m_dicModelChildCache.Remove("WeaponRefineEffect_LHand");
            //    }
            //}
            //else if (szAvatarName == "WeaponPolishVisual")
            //{
            //    if (m_dicModelChildCache.ContainsKey("WeaponPolishEffect_RHand"))
            //    {
            //        GameObject.Destroy(m_dicModelChildCache["WeaponPolishEffect_RHand"]);
            //        m_dicModelChildCache.Remove("WeaponPolishEffect_RHand");
            //    }
            //    if (m_dicModelChildCache.ContainsKey("WeaponPolishEffect_LHand"))
            //    {
            //        GameObject.Destroy(m_dicModelChildCache["WeaponPolishEffect_LHand"]);
            //        m_dicModelChildCache.Remove("WeaponPolishEffect_LHand");
            //    }
            //}
            //else if (szAvatarName == "BodyColorEffect")
            //{
            //    if (m_dicModelChildCache.ContainsKey("BodyColorEffect"))
            //    {
            //        GameObject.Destroy(m_dicModelChildCache["BodyColorEffect"]);
            //        m_dicModelChildCache.Remove("BodyColorEffect");
            //    }
            //}
            else
            {
                if (m_dicAvatarCache.ContainsKey(szAvatarName))
                {
                    curDic = m_dicAvatarCache;
                }
                else if (m_dicModelChildCache.ContainsKey(szAvatarName))
                {
                    curDic = m_dicModelChildCache;
                }

                if (null != curDic && curDic.ContainsKey(szAvatarName))
                {
                    GameObject.Destroy(curDic[szAvatarName].gameObject);
                    curDic.Remove(szAvatarName);
                }

                if (m_ModelAnimLogic != null)
                {
                    //if (szAvatarName == "Face")
                    //{
                    //    m_ModelAnimLogic.RemoveSubAnimation("Face");
                    //}
                    //if (szAvatarName == "Hair")
                    //{
                    //    m_ModelAnimLogic.RemoveSubAnimation("Hair");
                    //}
                }
            }

            switch (szAvatarName)
            {
                case "Body":
                    m_BodyId = GlobeVar.INVALID_ID;
                    break;
                case "Weapon":
                    m_WeaponId = GlobeVar.INVALID_ID;
                    break;
                case "Talisman":
                    m_TalismanId = GlobeVar.INVALID_ID;
                    break;
                //case "Face":
                //    m_FaceId = GlobeVar.INVALID_ID;
                //    break;
                //case "Hair":
                //    m_HairId = GlobeVar.INVALID_ID;
                //    break;
                //case "Mount":
                //    m_MountId = GlobeVar.INVALID_ID;
                //    break;
                case "SoulWare":
                    m_SoulWareId = GlobeVar.INVALID_ID;
                    break;
                default:
                    break;
            }
        }

        public Transform GetAvatarModel(string szAvatarName)
        {
            if (m_dicAvatarCache.ContainsKey(szAvatarName))
            {
                return m_dicAvatarCache[szAvatarName];
            }
            return null;
        }

        void UpdateAvatarPartsBind()
        {
            if (m_BodyTrans == null)
            {
                return;
            }

            Tab_CharModel tabCharModel = TableManager.GetCharModelByID(m_BodyId, 0);
            if (tabCharModel == null)
            {
                return;
            }

            Tab_CharModelPoint tabPoint = TableManager.GetCharModelPointByID(tabCharModel.ModelType, 0);
            if (tabPoint == null)
            {
                return;
            }

            m_WeaponLTrans = m_BodyTrans.Find(tabPoint.WeaponL);
            m_WeaponRTrans = m_BodyTrans.Find(tabPoint.WeaponR);
            m_SoulWareTrans = m_BodyTrans.Find(tabPoint.SoulWarePoint);

            if (m_dicModelChildCache.ContainsKey("WeaponL"))
            {
                Transform mTransform = m_dicModelChildCache["WeaponL"].transform;
                mTransform.parent = m_WeaponLTrans;

                Tab_AvatarModel tabModel = TableManager.GetAvatarModelByID(m_WeaponId, 0);
                if (tabModel != null)
                {
                    mTransform.localPosition = new Vector3(tabModel.PosX, tabModel.PosY, tabModel.PosZ);
                    mTransform.localRotation = Quaternion.Euler(new Vector3(tabModel.RotX, tabModel.RotY, tabModel.RotZ));
                    mTransform.localScale = tabModel.Scale * Vector3.one;
                }
            }

            if (m_dicModelChildCache.ContainsKey("WeaponR"))
            {
                Transform mTransform = m_dicModelChildCache["WeaponR"].transform;
                mTransform.parent = m_WeaponRTrans;

                Tab_AvatarModel tabModel = TableManager.GetAvatarModelByID(m_WeaponId, 0);
                if (tabModel != null)
                {
                    mTransform.localPosition = new Vector3(tabModel.PosX, tabModel.PosY, tabModel.PosZ);
                    mTransform.localRotation = Quaternion.Euler(new Vector3(tabModel.RotX, tabModel.RotY, tabModel.RotZ));
                    mTransform.localScale = tabModel.Scale * Vector3.one;
                }
            }

            if (m_dicModelChildCache.ContainsKey("SoulWare"))
            {
                Transform mTransform = m_dicModelChildCache["SoulWare"].transform;
                mTransform.parent = m_SoulWareTrans;

                Tab_AvatarModel tabModel = TableManager.GetAvatarModelByID(m_SoulWareId, 0);
                if (tabModel != null)
                {
                    mTransform.localPosition = new Vector3(tabModel.PosX, tabModel.PosY, tabModel.PosZ);
                    mTransform.localRotation = Quaternion.Euler(new Vector3(tabModel.RotX, tabModel.RotY, tabModel.RotZ));
                    mTransform.localScale = tabModel.Scale * Vector3.one;

                    // azuki temp: soulware point refix
                    Vector3 offset = TalismanTool.GetSkinModelOffset(m_SoulWareId, m_BodyId);
                    mTransform.localPosition += offset;
                }
            }
        }

        public void InitCharModelPointList()
        {
            m_dicBindPointCache.Clear();
           
            Transform effectPointBone = null;
           
            int nModelType = GlobeVar.INVALID_ID;
            if (m_BodyId == GlobeVar.INVALID_ID)
            {
                Tab_FakeObject tFakeObj = TableManager.GetFakeObjectByID(m_FakeObjId, 0);
                if (tFakeObj != null)
                {
                    nModelType = tFakeObj.ModelType;
                }
            }
            else
            {
                Tab_CharModel tCharModel = TableManager.GetCharModelByID(m_BodyId, 0);
                if (tCharModel != null)
                {
                    nModelType = tCharModel.ModelType;
                }
            }

            Tab_CharModelPoint tCharModelPoint = TableManager.GetCharModelPointByID(nModelType, 0);
            if (tCharModelPoint == null)
            {
                return;
            }

            Transform FindRootTrans = null;
            Transform Model = GetAvatarModel("Model");
            if (Model != null)
            {
                FindRootTrans = Model.parent;
            }

            if (FindRootTrans == null)
            {
                return;
            }

            //根节点，Root
            m_dicBindPointCache.Add(CharBindPoint.ModelRoot, FindRootTrans);

            effectPointBone = FindRootTrans.Find(tCharModelPoint.CenterPointPath);
            if (effectPointBone != null)
            {
                m_dicBindPointCache.Add(CharBindPoint.CenterPoint, effectPointBone);
            }
            //头节点
            effectPointBone = FindRootTrans.Find(tCharModelPoint.HeadPointPath);
            if (effectPointBone != null)
            {
                m_dicBindPointCache.Add(CharBindPoint.HeadPoint, effectPointBone);
            }
            //左手（左前足）节点
            effectPointBone = FindRootTrans.Find(tCharModelPoint.LHandPointPath);
            if (effectPointBone != null)
            {
                m_dicBindPointCache.Add(CharBindPoint.LHandPoint, effectPointBone);
            }
            //右手（右前足）节点
            effectPointBone = FindRootTrans.Find(tCharModelPoint.RHandPointPath);
            if (effectPointBone != null)
            {
                m_dicBindPointCache.Add(CharBindPoint.RHandPoint, effectPointBone);
            }
            //左脚（左后足）节点
            effectPointBone = FindRootTrans.Find(tCharModelPoint.LFootPointPath);
            if (effectPointBone != null)
            {
                m_dicBindPointCache.Add(CharBindPoint.LFootPoint, effectPointBone);
            }
            //右脚（右后足）节点
            effectPointBone = FindRootTrans.Find(tCharModelPoint.RFootPointPath);
            if (effectPointBone != null)
            {
                m_dicBindPointCache.Add(CharBindPoint.RFootPoint, effectPointBone);
            }
            //左手武器
            effectPointBone = FindRootTrans.Find(tCharModelPoint.WeaponL);
            if (effectPointBone != null)
            {
                m_dicBindPointCache.Add(CharBindPoint.WeaponL, effectPointBone);
            }
            //右手武器
            effectPointBone = FindRootTrans.Find(tCharModelPoint.WeaponR);
            if (effectPointBone != null)
            {
                m_dicBindPointCache.Add(CharBindPoint.WeaponR, effectPointBone);
            }
            //颈部
            effectPointBone = FindRootTrans.Find(tCharModelPoint.NeckPoint);
            if (effectPointBone != null)
            {
                m_dicBindPointCache.Add(CharBindPoint.Neck, effectPointBone);
            }
            //左臂
            effectPointBone = FindRootTrans.Find(tCharModelPoint.LForeArmPoint);
            if (effectPointBone != null)
            {
                m_dicBindPointCache.Add(CharBindPoint.LForeArmPoint, effectPointBone);
            }
            //右臂
            effectPointBone = FindRootTrans.Find(tCharModelPoint.RForeArmPoint);
            if (effectPointBone != null)
            {
                m_dicBindPointCache.Add(CharBindPoint.RForeArmPoint, effectPointBone);
            }
            //左腿
            effectPointBone = FindRootTrans.Find(tCharModelPoint.LCalfPoint);
            if (effectPointBone != null)
            {
                m_dicBindPointCache.Add(CharBindPoint.LCalfPoint, effectPointBone);
            }
            //右腿
            effectPointBone = FindRootTrans.Find(tCharModelPoint.RCalfPoint);
            if (effectPointBone != null)
            {
                m_dicBindPointCache.Add(CharBindPoint.RCalfPoint, effectPointBone);
            }
            //魂器
            effectPointBone = FindRootTrans.Find(tCharModelPoint.SoulWarePoint);
            if (effectPointBone != null)
            {
                m_dicBindPointCache.Add(CharBindPoint.SoulWarePoint, effectPointBone);
            }
            //高度点
            m_dicBindPointCache.Add(CharBindPoint.HeightPoint,HeightTrans);
        }

        public Transform FindCharModelPoint(string pointName)
        {
            Transform effectBindPoint = null;
            if (m_dicBindPointCache.TryGetValue(pointName, out effectBindPoint))
            {
                return effectBindPoint;
            }
            else
            {
                //LogModule.WarningLog(string.Format("Model Point[{0}] Not Found,ModelId:{1}",pointName,m_BodyId));
            }
            return null;
        }

        public void ApplyMat()
        {
            if (m_Renderers == null)
            {
                return;
            }

            //关键字部分
            ApplyKeyword();

            //Property部分
            ApplyProperty();
        }

        public void ApplyKeyword()
        {
            //染色是否开启
            if (!m_IsMatDyeColored && ( m_HasEnabledDye || m_EnableDye) )
            {
                ApplyDye(m_EnableDye);
            }
        }

        public void ApplyProperty()
        {
            if (!m_MatPropertyDirty)
            {
                return;
            }
            if (MatPropBlock == null)
            {
                return;
            }

            //主色
            MatPropBlock.SetColor("_MainColor", m_Color);
            
            //染色
            if (m_EnableDye)
            {
                LoadDyeTexture();
                MatPropBlock.SetColor("_DyeColor", m_dyeColor0);
                MatPropBlock.SetColor("_DyeColor1", m_dyeColor1);
                MatPropBlock.SetColor("_DyeColor2", m_dyeColor2);
            }

            foreach (var r in m_Renderers)
            {
                if (r == null) continue;
                r.SetPropertyBlock(MatPropBlock);
            }
        }

        public void SetColor(Color color)
        {
            m_Color = color;
            m_MatPropertyDirty = true;

            if (!m_bLoading)
            {
                ApplyMat();
            }
        }

        public Color GetColor()
        {
            return m_Color;
        }

        public void SetDyeColor(Color color0, Color color1, Color color2)
        {
            m_dyeColor0 = color0;
            m_dyeColor1 = color1;
            m_dyeColor2 = color2;
            //手动开启染色，不使用属性
            m_EnableDye = true;
            m_MatPropertyDirty = true;

            if (!m_bLoading)
            {
                ApplyMat();
            }
        }

        public void SetMatDirty()
        {
            m_MatPropertyDirty = true;
        }

        public void SetVector(string name, Vector4 vector)
        {
            MatPropBlock.SetVector(name,vector);
            m_MatPropertyDirty = true;
            if (!m_bLoading)
            {
                ApplyMat();
            }
        }

        public bool HasDyeColor()
        {
            return m_EnableDye;
        }

        public void ClearDyeColor()
        {
            m_EnableDye = false;

            if (!m_bLoading)
            {
                ApplyMat();
            }
        }

        //不要在Update里调用
        public void ApplyDye(bool val)
        {
            if (m_Renderers == null)
            {
                return;
            }

            foreach (var mRenderer in m_Renderers)
            {
                if (mRenderer == null) continue;
                Material[] sharedMats = mRenderer.sharedMaterials;
                Material[] mats = null;
                for(int i=0; i<sharedMats.Length; i++)
                {
                    Material sharedMat = sharedMats[i];
                    if (sharedMat == null)
                    {
                        continue;
                    }
                    if (sharedMat.name.StartsWith("eye"))
                    {
                        continue;
                    }

                    //材质里本身开启了染色，配置的染色不生效
                    if (mats == null)
                    {
                        mats = mRenderer.materials;
                    }
                    if (mats != null &&  i < mats.Length)
                    {
                        Material mat = mats[i];
                        if (mat == null)
                        {
                            continue;
                        }
                        if (val)
                        {
                            mat.EnableKeyword("DYE_ON");
                            m_HasEnabledDye = true;
                        }
                        else
                        {
                            mat.DisableKeyword("DYE_ON");
                        }
                    }
                }
            }
        }

        private void LoadDyeTexture(bool isReload = false)
        {
            if (m_Renderers == null)
            {
                return;
            }
            if (isReload)
            {
                m_DyeTexLoaded = false;
            }
            if (m_DyeTexLoaded)
            {
                return;
            }
            m_DyeTexLoaded = true;
            int hash = Shader.PropertyToID("_DyeTex");
            foreach (var mRenderer in m_Renderers)
            {
                if (mRenderer == null) continue;
                Material[] mats = mRenderer.materials;
                int count = mats.Length;
                for (int i=0; i<count; i++)
                {
                    Material mat = mats[i];
                    if (mat == null)
                    {
                        continue;
                    }

                    if (mat.HasProperty(hash))
                    {
                        string realName = mat.name.Replace("(Instance)","").Trim();
                        string path = string.Format("Bundle/RoleDyeTex/{0}_C", realName);
                        Texture2D tex = AssetManager.LoadResource(path) as Texture2D;
                        if (tex != null)
                        {
                            mat.SetTexture(hash, tex);
                        }
                    }
                }
            }
        }
    }
}
