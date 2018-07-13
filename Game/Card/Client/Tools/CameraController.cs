/********************************************************************************
 *	文件名：	CameraController.cs
 *	全路径：	\Script\Player\Controller\CameraController.cs
 *	创建人：	李嘉
 *	创建时间：2017-01-12
 *
 *	功能说明： 摄像机控制类
 *	          所有的摄像机逻辑都在其中
 *	修改记录：
*********************************************************************************/

using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Games.GlobeDefine;
using Games.LogicObj;
using Games.SceneLogic;
using Games.Table;
using Games;

public class CameraController : MonoBehaviour
{
    #region 摄像机默认参数
    public float m_CameraXOffset3D = 8.2f;                //3D摄像机相对主角的X偏移
    public float m_CameraYOffset3D = 4.3f;                //3D摄像机相对主角的Y偏移
    public float m_CameraZOffset3D = 4.0f;                //3D摄像机相对主角的Z偏移

    public float m_Fov = 60.0f;

    public float m_CenterOffest = 1.0f;                   //摄像机相对抬高一个距离
    public float m_CameraRotAngle = 20.0f;                //摄像机每秒旋转角度

    public float m_HorizenRotMin = -30f;
    public float m_HorizenRotMax = 30f;

    public float m_VerticalRotMin = 40.0f;            //上下旋转镜头最小值
    public float m_VerticalRotMax = 60.0f;            //上下旋转镜头最大值

    public float LookAtOffestMin = 0f;
    public float LookAtOffestMax = 1f;
    private float m_LookAtOffset = 0f;

    //public float m_CameraScaleRefix = 1.4f;
    #endregion

    public float m_HorizenRotateSpeed = 300;            //水平方向移动速度
    public float m_VerticalRotateSpeed = 60;           //垂直方向移动速度

    static private Vector3 m_vecLastPos = Vector3.zero; //记录玩家上一帧坐标，和这一帧比较获得相对位移进行摄像机移动使用
    static public bool bCameraTrace = false;            // 是否处于摄像机追踪的情况(播放动画时,摄像机俯仰或平移跟随对象的某个节点)

    //Figher Gesture
    public float m_Scale = 1.0f;			            //Initallize scale of the Camera,scale range is [m_ScaleMin, m_ScaleMax]
    public float m_ScaleMin = 1.5f;                   //Min scale of the Camera
    public float m_ScaleMax = 2.8f;                    //Max scale of the Camera
    private float m_OldScale;
    private float m_CurScale;

    public float m_pinchSpeed = 80.0f;
    public float m_PinchMin = -10.0f;
    public float m_PinchMax = 10.0f;

    private float m_fHorizenRotCur = 0.0f;
    private float m_fVerticalRotCur = 0.0f;            //上下旋转镜头当前值
    //private float m_CameraVerticalMinHeight = 1.0f;     //摄像机上下移动最小高度（相对于角色的差值）

    public float m_CameraTrackDelay = 2.0f;             //摄像机跟踪延迟
    private bool m_bTrackAutoSearching = false;
    //private bool m_bBeginDelayResetCamera = false;

    #region 摄像机重置参数
    private bool m_bIsCameraResting = false;            //摄像机是否正在重置过程中
    public float resetHorizenRotSpeed = 120f;
    public float resetVerticalRotSpeed = 120f;
    
    //public float m_CameraResetRotAngle = 100.0f;        //摄像机重置时候的每秒旋转角度
    //public float m_CameraResetScale = 0.03f;            //摄像机重置时候的每帧缩放数值
    //private float m_CameraResetScaleDst = GlobeVar.CAMERA_SCALE;         //重置摄像机的最终缩放
    //private float m_CameraResetVericalDelta = 2.0f;     //重置摄像机的时候每次高度变化
    //const float m_CameraResetVerticalDst = 40.0f;     //重置摄像机高度最终值
    #endregion

    #region 摄像机最低后进入缩放模式
    private bool m_bVerticalMinScaleMode = false;       //进入缩放模式
    private float m_fVerticalMinSceleMode = -1.0f;      //进入缩放模式时的缩放数值
    public float m_VerticalMinScaleDelta = 0.05f;       //进入摄像机最低高度缩放模式的速率

    #endregion

    private float m_ScaleTarget = 0f;
    private float m_HorizenRotDelta = 0f;
    private float m_VerticalRotDelta = 0f;

    public float ScaleSmooth = 4f;
    public float HorizenRotSmooth = 15f;
    public float VerticalRotSmooth = 15f;

    public bool isFlip = false;

    #region 震屏
    /// <summary>
    /// 震屏START
    /// </summary>
    public class CameraRockInfo
    {
        public void CleanUp()
        {
            m_nCameraRockId = -1;
            m_fRockTime = 0.0f; //震屏已经震了多久
            m_fNeedRockTime = 0.0f; //震屏持续时间
            m_fDelayTime = 0.0f; //震屏延迟时间
            m_bContinueRockDie = false; //主角死亡是否继续震屏
            m_XRockOff = new AnimationCurve(); //震屏 摄像机X位置偏移
            m_YRockOff = new AnimationCurve(); //震屏 摄像机Y位置偏移
            m_ZRockOff = new AnimationCurve();//震屏 摄像机Z位置偏移
            m_RXRockOff = new AnimationCurve();//震屏 摄像机X旋转偏移
            m_RYRockOff = new AnimationCurve();//震屏 摄像机Y旋转偏移
            m_RZRockOff = new AnimationCurve();//震屏 摄像机Z旋转偏移
            m_fFieldDepthTime1 = 0;
            m_nFieldDepth1 = 0;
            m_fFieldDepthTime2 = 0;
            m_nFieldDepth2 = 0;
            m_fFieldDepthTime3 = 0;
            m_nFieldDepth3 = 0;
            m_nFieldStat = 0;
        }

        public bool IsValid()
        {
            return (m_nCameraRockId != -1);
        }
        public int m_nCameraRockId;
        public float m_fRockTime; //震屏已经震了多久
        public float m_fNeedRockTime; //震屏持续时间
        public float m_fDelayTime; //震屏延迟时间
        public bool m_bContinueRockDie; //主角死亡是否继续震屏
        public AnimationCurve m_XRockOff; //震屏 摄像机X位置偏移
        public AnimationCurve m_YRockOff; //震屏 摄像机Y位置偏移
        public AnimationCurve m_ZRockOff;//震屏 摄像机Z位置偏移
        public AnimationCurve m_RXRockOff;//震屏 摄像机X旋转偏移
        public AnimationCurve m_RYRockOff;//震屏 摄像机Y旋转偏移
        public AnimationCurve m_RZRockOff;//震屏 摄像机Z旋转偏移
        public float m_fFieldDepthTime1; // 景深变化时刻1
        public float m_nFieldDepth1; // 景深1
        public float m_fFieldDepthTime2; // 景深变化时刻2
        public float m_nFieldDepth2; // 景深2
        public float m_fFieldDepthTime3; // 景深变化时刻3
        public float m_nFieldDepth3; // 景深3
        public int m_nFieldStat;
    }
    private List<CameraRockInfo> m_CameraRockInfoList = new List<CameraRockInfo>();
    /// <summary>
    /// 震屏 END
    /// </summary>
    #endregion

    private Camera m_MainCamera = null; // 主摄像机
    public Camera MainCamera { get { return m_MainCamera; } }

    //缓存Transform
    private Transform m_CameraTransform = null;
    public UnityEngine.Transform CameraTransform
    {
        get { return m_CameraTransform; }
        set { m_CameraTransform = value; }
    }
    private Transform m_FollowTarget = null;

    private bool m_bPinching = false;
    public bool IsPinching() { return m_bPinching; }

    private bool m_bDraging = false;
    public bool Draging
    {
        set { m_bDraging = value; }
    }
    public bool IsDraging() { return m_bDraging; }

    public bool IsReseting()
    {
        return m_bIsCameraResting;
    }

    public TransparentController TransparentCtrl { get; private set; }

    //由于目前游戏需要既支持点击操作又支持拖拽，所以之前曾经进行过一次拖拽的话，就记录一下，保证不会结束拖拽之后又出发一次点击移动
    static public bool IsEverDraged = false;
    static public bool IsEverPinched = false;

    public Transform FollowTarget
    {
        get { return m_FollowTarget; }
        set
        {
            Clear();
            InitCameraController(value);
            m_FollowTarget = value;
        }
    }

    //恢复到摄像机跟随主角状态
    public void ResetCameraToTarget()
    {
        UpdateCamera();
    }

    public void InitParam(int cfgId)
    {
        if (cfgId == -1)
        {
            return;
        }
        Tab_CameraCfg cameraCfg = TableManager.GetCameraCfgByID(cfgId, 0);
        if (cameraCfg != null)
        {
            InitParam(cameraCfg);
        }
    }

    public void InitParam(Tab_CameraCfg cfg)
    {
        m_IsChanging = false;
        m_OnCameraChangeFinished = null;

        m_CameraXOffset3D = cfg.CameraXOffset3D;
        m_CameraYOffset3D = cfg.CameraYOffset3D;
        m_CameraZOffset3D = cfg.CameraZOffset3D;
        m_CenterOffest = cfg.CenterOffest;
        m_Fov = cfg.Fov;
        m_HorizenRotMin = cfg.HorizenRotMin;
        m_HorizenRotMax = cfg.HorizenRotMax;
        m_VerticalRotMin = cfg.VerticalRotMin;
        m_VerticalRotMax = cfg.VerticalRotMax;
        m_PinchMin = cfg.PinchMin;
        m_PinchMax = cfg.PinchMax;
        m_ScaleMax = cfg.ScaleMax;
        m_ScaleMin = cfg.ScaleMin;
        m_Scale = cfg.DefaultScale;
        LookAtOffestMin = cfg.LookAtOffsetMin;
        LookAtOffestMax = cfg.LookAtOffsetMax;
    }

    private bool isEnterScene = false;

    public void OnEnterScene()
    {
        if (!enabled)
        {
            return;
        }
        isEnterScene = true;
        if (null == m_MainCamera)
        {
            m_MainCamera = Camera.main;
            if (null == m_MainCamera)
            {
                return;
            }
        }

        //再次判空
        if (null == m_MainCamera)
        {
            return;
        }
		else
		{
		    if (!(GameManager.CurScene is BattleScene))
		    {
                TransparentCtrl = Utils.TryAddComponent<TransparentController>(m_MainCamera.gameObject);
            }
        }

        if (null == m_CameraTransform)
        {
            m_CameraTransform = m_MainCamera.transform;
            if (null == m_CameraTransform)
            {
                return;
            }
        }

        InitCameraController(FollowTarget);

#region 镜头轨迹

        if (CameraTraceMgr.Ins == null)
        {
            CameraTraceMgr.Create();
        }

#endregion
    }

    public void OnLeaveScene()
    {
        isEnterScene = false;
        m_FollowTarget = null;
        m_CameraTransform = null;
        Clear();
        if (CameraTraceMgr.Ins != null)
        {
            DestroyImmediate(CameraTraceMgr.Ins.gameObject);
        }
    }

    public void Clear()
    {
        IsEverDraged = false;
        IsEverPinched = false;
        m_fHorizenRotCur = 0f;
        m_fVerticalRotCur = 0f;
        m_bDraging = false;
        m_bPinching = false;
        m_OldScale = m_Scale;
        m_CurScale = m_Scale;
    }

    void OnEnable()
    {
        UICamera.onDragStart += OnDragStart;
        UICamera.onDrag += OnDragUpdate;
        UICamera.onDragEnd += OnDragEnd;
    }

    void OnDisable()
    {
        UICamera.onDragStart -= OnDragStart;
        UICamera.onDrag -= OnDragUpdate;
        UICamera.onDragEnd -= OnDragEnd;
    }

    void Update()
    {
        if (TransparentCtrl != null)
        {
            TransparentCtrl.enabled = Display.IsTransparentRaycast();
        }
        UpdateCamera();
    }

    public bool IsDragLock { get; set; }
    public bool IsPinchLock { get; set; }

    void OnDragStart(GameObject obj)
    {
        if (!isEnterScene)
        {
            return;
        }

        if (IsDragLock)
        {
            return;
        }

        /*
        if (null == ObjManager.MainPlayer)
        {
            return;
        }
        */

        if (UIManager.IsSubUIShow() &&  RoleTouchController.Instance == null && HouseEditorUI.Instance() == null)
        {
            return;
        }

        if (m_bPinching)
        {
            return;
        }

        if (m_bIsCameraResting)
        {
            return;
        }
        
        if (m_bTrackAutoSearching)
        {
            m_bTrackAutoSearching = false;
        }

        if (CameraTraceMgr.Ins != null && CameraTraceMgr.Ins.IsPlaying())
        {
            return;
        }

        if (UICamera.Raycast(UICamera.lastEventPosition))
        {
            return;
        }

        // 特殊逻辑 剧情副本引导妖气灯笼时不可转视角
        if (TutorialRoot.IsGroupStep(TutorialGroup.StoryCopyScene, 6) ||
            TutorialRoot.IsGroupStep(TutorialGroup.StoryCopyScene, 8))
        {
            return;
        }

        m_bDraging = true;
    }

    void OnDragUpdate(GameObject obj, Vector2 delta)
    {
        // 拖拽中突然锁定, 立即释放
        if (IsDragLock)
        {
            OnDragEnd(obj);
            return;
        }

        if (!m_bDraging)
        {
            return;
        }

        //如果在缩放阶段，忽略拖拽
        if (m_bPinching)
        {
            m_bDraging = false;
            return;
        }

        //如果在重置阶段忽略拖拽
        if (m_bIsCameraResting)
        {
            m_bDraging = false;
            return;
        }

        if (CameraTraceMgr.Ins != null && CameraTraceMgr.Ins.IsPlaying())
        {
            return;
        }

        //水平位移
        float fHorizenDelta = delta.x;
        //垂直位移
        float fVerticalDelta = delta.y;

        if (null == m_CameraTransform || null == m_FollowTarget)
            return;

        if (Screen.width <= 0 || Screen.height <= 0)
            return;

        //水平位移
        //本次位移相对于屏幕宽度的比例，比例越大滑动距离越大
        float fHorizenRot = (fHorizenDelta / (float)Screen.width) * m_HorizenRotateSpeed;
        //处理水平位移
        if (Math.Abs(fHorizenDelta) > 0.001f)
        {
            m_HorizenRotDelta += fHorizenRot;
            //UpdateHorizenPosition(fHorizenRot);
            CameraController.IsEverDraged = true;
        }

        //垂直位移
        //本次位移相对于屏幕宽度的比例，比例越大滑动距离越大
        float fVertivalRot = -(fVerticalDelta / (float)Screen.height) * m_VerticalRotateSpeed;
        //处理垂直位移
        if (Math.Abs(fVerticalDelta) > 0.001f)
        {
            //根据两次滑动的VerticalDelta判断是向上还是向下滑动
            //VerticalDelta>0,摄像机向下
            if (fVerticalDelta > 0)
            {
                 if (m_fVerticalRotCur - m_VerticalRotMin < Mathf.Abs(fVertivalRot))
					fVertivalRot = m_VerticalRotMin - m_fVerticalRotCur;

                 //由于VertivalRot进行了一次修正，所以需要再度判断正负
                 if (fVertivalRot < 0)
                 {
                     m_VerticalRotDelta += fVertivalRot;
                     //UpdateVerticalPosition(fVertivalRot);
                     CameraController.IsEverDraged = true;
                 }
            }
            //VerticalDelta<0,摄像机向上
            else
            {
                if (m_fVerticalRotCur < m_VerticalRotMax)
                {
                    //如果之前进行过镜头最低时候的缩放，则此时首先恢复缩放
                    if (m_bVerticalMinScaleMode)
                    {
                        if (m_ScaleTarget < m_fVerticalMinSceleMode)
                        {
                            m_ScaleTarget += m_VerticalMinScaleDelta;
                            if (m_ScaleTarget >= m_fVerticalMinSceleMode)
                            {
                                m_ScaleTarget = m_fVerticalMinSceleMode;
                                m_bVerticalMinScaleMode = false;
                            }
                        }
                        else
                        {
                            m_bVerticalMinScaleMode = false;
                        }
                    }
                    else
                    {
                        if (m_VerticalRotMax - m_fVerticalRotCur < fVertivalRot)
                            fVertivalRot = m_VerticalRotMax - m_fVerticalRotCur;

                        m_VerticalRotDelta += fVertivalRot;
                        //UpdateVerticalPosition(fVertivalRot);
                        CameraController.IsEverDraged = true;
                    }
                }

            }
        }
    }

    //调整水平位置变化
    void UpdateHorizenPosition(float fHorizenRot)
    {
        if (null == m_FollowTarget || null == m_CameraTransform)
            return;
        float fVal = m_fHorizenRotCur + fHorizenRot;
        if ((fVal < m_HorizenRotMin && m_HorizenRotMin > -180.0f) || (fVal > m_HorizenRotMax && m_HorizenRotMax < 180.0f))
        {
            return;
        }
        m_CameraTransform.RotateAround(m_FollowTarget.position, new Vector3(0, 1, 0), fHorizenRot);
        m_fHorizenRotCur += fHorizenRot;
    }

    //调整高度位置变化
    void UpdateVerticalPosition(float fVertivalRot)
    {
        if (null == m_FollowTarget || null == m_CameraTransform)
        {
            return;
        }

        
        float fVal = m_fVerticalRotCur + fVertivalRot;
        if ((fVal < m_VerticalRotMin && m_VerticalRotMin > -180.0f) || (fVal > m_VerticalRotMax && m_VerticalRotMax < 180.0f))
        {
            return;
        }
        Vector3 pos = new Vector3(m_FollowTarget.position.x, m_FollowTarget.position.y + m_CenterOffest, m_FollowTarget.position.z);
        m_CameraTransform.RotateAround(pos, m_CameraTransform.right, fVertivalRot);
        m_fVerticalRotCur += fVertivalRot;
    }

    void OnDragEnd(GameObject obj)
    {
        m_bDraging = false;
		CameraController.IsEverDraged = false;
    }

    //更新场景摄像机
    public void UpdateCamera()
    {
        if (null == m_CameraTransform || null == m_FollowTarget)
        {
            return;
        }

        //如果在特写，不缩放，跟随，旋转
        if (CameraTraceMgr.Ins == null || !CameraTraceMgr.Ins.IsPlaying())
        {

            //更新摄像机位置

            //根据玩家上一次位置和本次位置进行相对位移
            Vector3 moveVec = m_FollowTarget.position - m_vecLastPos;
            m_vecLastPos = m_FollowTarget.position;
            m_CameraTransform.position += moveVec;

            //更新可能的cfg改变
            UpdateCameraChange(Time.deltaTime);

            UpdateReset();
            UpdateResetScale();
            UpdateCameraScale();

            if (Mathf.Abs(m_HorizenRotDelta) > 0.001f)
            {
                float next = Mathf.Lerp(m_HorizenRotDelta, 0f, Time.deltaTime * HorizenRotSmooth);
                UpdateHorizenPosition(m_HorizenRotDelta - next);
                m_HorizenRotDelta = next;
            }
            if (Mathf.Abs(m_VerticalRotDelta) > 0.001f)
            {
                float next = Mathf.Lerp(m_VerticalRotDelta, 0f, Time.deltaTime * VerticalRotSmooth);
                UpdateVerticalPosition(m_VerticalRotDelta - next);
                m_VerticalRotDelta = next;

                //UpdateVerticalPosition(m_VerticalRotDelta);
                //m_VerticalRotDelta = 0f;
            }

            if (false == m_bIsCameraResting)
            {
                AdjustCameraHeightByTerrainHeight();
            }

            UpdateLookAt();
        }

        UpdateCameraRock();//震屏
        UpdateShake();//新版本震屏
        m_bPinching = false;
    }

    //更新摄像机缩放
    void UpdateCameraScale()
    {
        if (null == m_CameraTransform)
            return;

        //如果发现OldScale和m_Scale不一样，则进入两指划屏摄像机缩放阶段
        if (Mathf.Abs(m_OldScale - m_ScaleTarget) > 0.0001f)
        {
            m_CurScale = Mathf.Lerp(m_CurScale, m_ScaleTarget, Time.deltaTime*ScaleSmooth);
            //锁定摄像机Y轴高度
            Vector3 dir = (GetCenterPos() - m_CameraTransform.position).normalized;
            Vector3 offset = dir * 5.0f * (m_OldScale - m_CurScale);

            m_OldScale = m_CurScale;
            m_CameraTransform.position = m_CameraTransform.position + offset;
        }
    }

    void UpdateLookAt()
    {
        float t = (m_CurScale - m_ScaleMin) / (m_ScaleMax - m_ScaleMin);
        m_LookAtOffset = Mathf.Lerp(LookAtOffestMin, LookAtOffestMax, t);
        m_CameraTransform.LookAt(GetLookAtPos());
    }

    void OnPinch(PinchGesture gesture)
    {
        /*
        if (null == ObjManager.MainPlayer)
        {
            return;
        }
        */

        if (IsPinchLock)
        {
            return;
        }

        if (UIManager.IsSubUIShow() &&
            RoleTouchController.Instance == null)
        {
            return;
        }
        
        if (m_bIsCameraResting)
            return;

        if (m_isResetingScale)
            return;

        if (CameraTraceMgr.Ins != null && CameraTraceMgr.Ins.IsPlaying())
        {
            return;
        }

        // 特殊逻辑 剧情副本引导妖气灯笼时不可拉镜头
        if (TutorialRoot.IsGroupStep(TutorialGroup.StoryCopyScene, 6) ||
            TutorialRoot.IsGroupStep(TutorialGroup.StoryCopyScene, 8))
        {
            return;
        }

        if (gesture.Phase == ContinuousGesturePhase.Started)
        {
            m_bPinching = true;
        }
        else if (gesture.Phase == ContinuousGesturePhase.Updated)
        {
            float fGestureDelta = gesture.Delta;
            if (fGestureDelta > m_PinchMax)
            {
                fGestureDelta = m_PinchMax;
            }

            if (fGestureDelta < m_PinchMin)
            {
                fGestureDelta = m_PinchMin;
            }

            if (m_pinchSpeed != 0)
            {
                m_ScaleTarget -= fGestureDelta / m_pinchSpeed;
                if (m_ScaleTarget <= m_ScaleMin)
                    m_ScaleTarget = m_ScaleMin;

                if (m_ScaleTarget >= m_ScaleMax)
                    m_ScaleTarget = m_ScaleMax;
            }

            if (m_ScaleTarget != m_fVerticalMinSceleMode && m_bVerticalMinScaleMode)
            {
                m_fVerticalMinSceleMode = m_ScaleTarget;
            }

            CameraController.IsEverPinched = true;
        }
        else
        {
            m_bPinching = false;
        }
    }

    public void InitCameraController()
    {
        InitCameraController(m_FollowTarget);
    }

    public void InitCameraController(Transform followTarget)
    {
        //摄像机的Transform不能为空
        if (null == m_CameraTransform)
        {
            return;
        }

        //给摄像机绑定玩家Transform
        if (null == followTarget || m_FollowTarget != followTarget)
        {
            m_FollowTarget = followTarget;
        }
        if (null == m_FollowTarget)
        {
            return;
        }

        //将上次玩家位置设置为当前先，否则第一次Update就会对主摄像机进行一次很大的偏移修正
        m_vecLastPos = m_FollowTarget.position;

        //m_fVerticalRotCur = m_CameraResetVerticalDst;

        
        SetUp();
        Clear();
    }

    private void SetUp()
    {
        m_CurScale = m_Scale;
        m_OldScale = m_CurScale;
        m_ScaleTarget = m_CurScale;
        m_CameraTransform.position = GetInitPos();
        UpdateLookAt();
        m_MainCamera.fieldOfView = m_Fov;

        if (isFlip)
        {
            m_CameraTransform.RotateAround(m_FollowTarget.position, m_FollowTarget.up, 180f);
        }
    }

    public void ScaleTo(float scaleTarget)
    {
        m_ScaleTarget = scaleTarget;
    }

    public Vector3 GetLookAtPos()
    {
        if (m_FollowTarget == null)
        {
            return Vector3.zero;
        }

        Vector3 pos = m_FollowTarget.position;
        pos.y += m_CenterOffest + m_LookAtOffset;
        return pos;
    }

    public Vector3 GetCenterPos()
    {
        if (m_FollowTarget == null)
        {
            return Vector3.zero;
        }

        Vector3 pos = m_FollowTarget.position;
        pos.y += m_CenterOffest;
        return pos;
    }

    public Vector3 GetInitPos()
    {
        if (m_FollowTarget == null)
        {
            return Vector3.zero;
        }

        Vector3 camDisplacement = Vector3.zero;
        camDisplacement.x += m_CameraXOffset3D;
        camDisplacement.y += (m_CameraYOffset3D + m_CenterOffest);
        camDisplacement.z += m_CameraZOffset3D;

        Vector3 pos = m_FollowTarget.position + camDisplacement;

        //缩放
        Vector3 dir = (GetCenterPos() - pos).normalized;
        Vector3 scaleOffset = dir * 5.0f * (1.0f - m_Scale);

        return pos + scaleOffset;
    }

    #region 震屏
    public void PlayCameraRock(int nRockId)
    {
        if (m_CameraTransform == null)
        {
            return;
        }
        Tab_CameraRock _cameraRock = TableManager.GetCameraRockByID(nRockId, 0);
        if (_cameraRock == null)
        {
            return;
        }

        CameraRockInfo _tmpInfo = new CameraRockInfo();
        _tmpInfo.CleanUp();
        //初始化数据
        _tmpInfo.m_nCameraRockId = nRockId;
        _tmpInfo.m_fNeedRockTime = _cameraRock.NeedRockTime;
        _tmpInfo.m_fDelayTime = _cameraRock.DelayTime;
        //位置偏移动画曲线
        _tmpInfo.m_XRockOff = InitRockOff(_cameraRock.PosXAnimCurveId);
        _tmpInfo.m_YRockOff = InitRockOff(_cameraRock.PosYAnimCurveId);
        _tmpInfo.m_ZRockOff = InitRockOff(_cameraRock.PosZAnimCurveId);

        //旋转偏移动画曲线
        _tmpInfo.m_RXRockOff = InitRockOff(_cameraRock.RXAnimCurveId);
        _tmpInfo.m_RYRockOff = InitRockOff(_cameraRock.RYAnimCurveId);
        _tmpInfo.m_RZRockOff = InitRockOff(_cameraRock.RZAnimCurveId);
        _tmpInfo.m_bContinueRockDie = _cameraRock.IsContinueDie;

        _tmpInfo.m_fFieldDepthTime1 = _cameraRock.GetFieldDepthTimebyIndex(0);
        _tmpInfo.m_nFieldDepth1 = _cameraRock.GetFieldDepthbyIndex(0);
        _tmpInfo.m_fFieldDepthTime2 = _cameraRock.GetFieldDepthTimebyIndex(1);
        _tmpInfo.m_nFieldDepth2 = _cameraRock.GetFieldDepthbyIndex(1);
        _tmpInfo.m_fFieldDepthTime3 = _cameraRock.GetFieldDepthTimebyIndex(2);
        _tmpInfo.m_nFieldDepth3 = _cameraRock.GetFieldDepthbyIndex(2);

        m_CameraRockInfoList.Add(_tmpInfo);
    }

    void UpdateCameraRock()
    {
        if (IsCameraPlayRock())
        {
            if (m_CameraTransform == null)
            {
                return;
            }
            for (int i = 0; i < m_CameraRockInfoList.Count; i++)
            {
                if (m_CameraRockInfoList[i].IsValid() == false)
                {
                    continue;
                }
                //死了就不震了
                //if (false)
                //{
                //    m_CameraRockInfoList[i].CleanUp();
                //    m_CameraTransform.position -= deltaDisV;
                //    deltaDisV = Vector3.zero;
                //    m_MainCamera.fieldOfView -= deltaFieldValue;
                //    deltaFieldValue = 0;
                //}
                //else
                //{
                    if (m_CameraRockInfoList[i].m_fDelayTime > 0)
                    {
                        m_CameraRockInfoList[i].m_fDelayTime -= Time.deltaTime;
                    }
                    else
                    {
                        if (m_CameraRockInfoList[i].m_fDelayTime <= 0)
                        {
                            if (m_CameraRockInfoList[i].m_fRockTime - m_CameraRockInfoList[i].m_fNeedRockTime >= 0)
                            {
                                //震完了 清理数据
                                m_CameraRockInfoList[i].CleanUp();
                                m_CameraTransform.position -= deltaDisV;
                                deltaDisV = Vector3.zero;
                                m_MainCamera.fieldOfView -= deltaFieldValue;
                                deltaFieldValue = 0;
                            }
                            else
                            {
                                m_CameraRockInfoList[i].m_fRockTime += Time.deltaTime;
                                CameraRock(i, m_CameraRockInfoList[i]);
                            }
                        }
                    }
                //}
            }
            //清除无效的
            /*
            List<CameraRockInfo> _needMoveList = new List<CameraRockInfo>();
            for (int i = 0; i < m_CameraRockInfoList.Count; i++)
            {
                if (m_CameraRockInfoList[i].IsValid() == false)
                {
                    _needMoveList.Add(m_CameraRockInfoList[i]);
                }
            }
            for (int i = 0; i < _needMoveList.Count; i++)
            {
                m_CameraRockInfoList.Remove(_needMoveList[i]);
            }*/
            //wtm,gc优化
            m_CameraRockInfoList.RemoveAll((c) => !c.IsValid());
        }
    }

    public void CleanUpRockInfoById(int nRockId)
    {
        for (int i = 0; i < m_CameraRockInfoList.Count; i++)
        {
            if (m_CameraRockInfoList[i].m_nCameraRockId == nRockId)
            {
                m_CameraRockInfoList[i].CleanUp();
            }
        }
    }

    public bool IsCameraPlayRock()
    {
        return (m_CameraRockInfoList.Count > 0);
    }
    void CameraRock(int nRockIndex, CameraRockInfo _info)
    {
        if (m_CameraTransform == null)
        {
            return;
        }
        if (m_MainCamera == null)
        {
            return;
        }
        if (nRockIndex >= 0 && nRockIndex < m_CameraRockInfoList.Count)
        {
            if (m_CameraRockInfoList[nRockIndex].m_fRockTime <= 0.0f)
            {
                m_CameraTransform.localPosition -= deltaDisV;
                deltaDisV = Vector3.zero;
            }

            float dX = m_CameraRockInfoList[nRockIndex].m_XRockOff.Evaluate(m_CameraRockInfoList[nRockIndex].m_fRockTime);
            float dY = m_CameraRockInfoList[nRockIndex].m_YRockOff.Evaluate(m_CameraRockInfoList[nRockIndex].m_fRockTime);
            float dZ = m_CameraRockInfoList[nRockIndex].m_ZRockOff.Evaluate(m_CameraRockInfoList[nRockIndex].m_fRockTime);

            m_CameraTransform.localPosition -= deltaDisV;
            deltaDisV -= deltaDisV;

            m_CameraTransform.localPosition += new Vector3(dX, dY, dZ);
            deltaDisV += new Vector3(dX, dY, dZ);

            //更新摄像机的Look点，锁定Y轴偏移
            Vector3 lookAtDir = m_CameraTransform.forward;

            float nNewRXPoss = lookAtDir.x +
                               m_CameraRockInfoList[nRockIndex].m_RXRockOff.Evaluate(
                                   m_CameraRockInfoList[nRockIndex].m_fRockTime);
            float nNewRYPoss = lookAtDir.y +
                               m_CameraRockInfoList[nRockIndex].m_RYRockOff.Evaluate(
                                   m_CameraRockInfoList[nRockIndex].m_fRockTime);
            float nNewRZPoss = lookAtDir.z +
                               m_CameraRockInfoList[nRockIndex].m_RZRockOff.Evaluate(
                                   m_CameraRockInfoList[nRockIndex].m_fRockTime);
            m_CameraTransform.rotation = Quaternion.LookRotation(new Vector3(nNewRXPoss, nNewRYPoss, nNewRZPoss));

            CameraRockInfo _tmpinfo = m_CameraRockInfoList[nRockIndex];
            if (_tmpinfo.m_fRockTime >= _tmpinfo.m_fFieldDepthTime1 &&
                _tmpinfo.m_nFieldStat == 0)
            {
                _info.m_nFieldStat = 1;
                m_MainCamera.fieldOfView += _tmpinfo.m_nFieldDepth1;
                deltaFieldValue += _tmpinfo.m_nFieldDepth1;
            }
            else if (_tmpinfo.m_fRockTime >= _tmpinfo.m_fFieldDepthTime2 &&
                     _tmpinfo.m_nFieldStat == 1)
            {
                _info.m_nFieldStat = 2;
                m_MainCamera.fieldOfView += _tmpinfo.m_nFieldDepth2;
                deltaFieldValue += _tmpinfo.m_nFieldDepth2;
            }
            else if (_tmpinfo.m_fRockTime >= _tmpinfo.m_fFieldDepthTime3 &&
                     _tmpinfo.m_nFieldStat == 2)
            {
                _info.m_nFieldStat = 3;
                m_MainCamera.fieldOfView += _tmpinfo.m_nFieldDepth3;
                deltaFieldValue += _tmpinfo.m_nFieldDepth3;
            }
        }
    }
    Vector3 deltaDisV = Vector3.zero;
    float deltaFieldValue = 0;
    AnimationCurve InitRockOff(int nCurverId)
    {
        AnimationCurve RockCurve = new AnimationCurve();
        if (nCurverId != -1)
        {
            List<Tab_AnimationCurve> _curveList = TableManager.GetAnimationCurveByID(nCurverId);
            if (_curveList.Count > 0)
            {
                Keyframe[] CurverKeyframes = new Keyframe[_curveList.Count];
                for (int i = 0; i < _curveList.Count; i++)
                {
                    CurverKeyframes[i].time = _curveList[i].Time;
                    CurverKeyframes[i].value = _curveList[i].Value;
                    CurverKeyframes[i].inTangent = _curveList[i].InTangent;
                    CurverKeyframes[i].outTangent = _curveList[i].OutTangent;
                    CurverKeyframes[i].tangentMode = _curveList[i].TangentMode;
                }
                RockCurve = new AnimationCurve(CurverKeyframes);
                RockCurve.preWrapMode = (WrapMode)_curveList[0].PreWrapMode;
                RockCurve.postWrapMode = (WrapMode)_curveList[0].PostWrapMode;
            }
        }
        return RockCurve;
    }
    #endregion

    #region 复位

    public bool IsCamearNeedReset()
    {
        if (m_CameraTransform == null)
        {
            return false;
        }
        return !IsRotDone(m_fVerticalRotCur) || !IsRotDone(m_fHorizenRotCur);
    }

    private bool IsRotDone(float rot)
    {
        return Mathf.Abs(rot) <= 0.01f;
    }

    public void Reset()
    {
        m_bIsCameraResting = false;
        SetUp();
        Clear();
    }

    private float m_curVel4Hor;
    private float m_curVel4Ver;
    private float m_resetingTimer;
    public void StartReset()
    {
        m_bIsCameraResting = true;
        m_resetingTimer = 0f;
        m_fHorizenRotCur = Mathf.DeltaAngle(m_fHorizenRotCur, 0f);
        m_fVerticalRotCur = Mathf.DeltaAngle(m_fVerticalRotCur, 0f);
    }

    void UpdateReset()
    {
        if (m_bIsCameraResting)
        {
            m_resetingTimer += Time.deltaTime;
            Vector3 targetPos = GetCenterPos();
            
            //x轴
            if (!IsRotDone(m_fVerticalRotCur))
            {
                float last = m_fVerticalRotCur;
                m_fVerticalRotCur = Mathf.SmoothDampAngle(m_fVerticalRotCur, 0f, ref m_curVel4Ver, 0.3f);
                float delta = last - m_fVerticalRotCur;
                m_CameraTransform.RotateAround(targetPos, m_CameraTransform.right, delta);
            }

            //y轴
            if (!IsRotDone(m_fHorizenRotCur))
            {
                float last = m_fHorizenRotCur;
                m_fHorizenRotCur = Mathf.SmoothDampAngle(m_fHorizenRotCur, 0f, ref m_curVel4Hor, 0.3f);
                float delta = last - m_fHorizenRotCur;
                m_CameraTransform.RotateAround(targetPos, Vector3.up, delta);
            }


            m_CameraTransform.LookAt(GetLookAtPos());

            if (m_resetingTimer >= 1.2f)
            {
                m_bIsCameraResting = false;
                m_CameraTransform.RotateAround(targetPos, m_CameraTransform.right, m_fVerticalRotCur);
                m_CameraTransform.RotateAround(targetPos, Vector3.up, m_fHorizenRotCur);
                m_CameraTransform.LookAt(GetLookAtPos());
                m_fVerticalRotCur = 0f;
                m_fHorizenRotCur = 0f;
            }
        }
    }

    private float m_curVel4Scale;
    private float m_resetingTimer4Scale;
    private bool m_isResetingScale;

    public void StartResetScale()
    {
        m_isResetingScale = true;
        m_resetingTimer4Scale = 0f;
    }

    public bool IsScaleDone()
    {
        return Mathf.Abs(m_CurScale - m_Scale) <= 0.01f;
    }

    void UpdateResetScale()
    {
        if (!m_isResetingScale)
        {
            return;
        }
        
        if (!IsScaleDone())
        {
            m_ScaleTarget = Mathf.SmoothDampAngle(m_ScaleTarget, m_Scale, ref m_curVel4Scale, 0.2f);
        }

        if (m_resetingTimer4Scale >= 0.8f)
        {
            m_isResetingScale = false;
            m_resetingTimer4Scale = 0f;
            m_ScaleTarget = m_Scale;
        }
        m_resetingTimer4Scale += Time.deltaTime;
    }
    #endregion

    void AdjustCameraHeightByTerrainHeight()
    {
        if (null == m_CameraTransform)
            return;

        Vector3 currPos = m_CameraTransform.position;

        bool bHit = false;
        float fTerrainHeight = Scene.GetTerrainPosition(currPos, out bHit).y;
        if (false == bHit)
            return;

        //给一个高度
        fTerrainHeight += 0.2f;

        if (currPos.y > fTerrainHeight)
        {
            return;
        }

        //计算夹角
        Vector3 pos = new Vector3(m_FollowTarget.position.x, m_FollowTarget.position.y + m_CenterOffest, m_FollowTarget.position.z);

        Vector3 validPos = new Vector3(currPos.x, fTerrainHeight, currPos.z);

        float angle = Vector3.Angle(currPos - pos, validPos - pos);
        UpdateVerticalPosition(angle);

        //while (m_CameraTransform.position.y < fTerrainHeight && nCount < 60)
        //{
        //    UpdateVerticalPosition(0.05f);
        //    nCount++;
        //}
    }

    #region cameraCfg_change
    public delegate void OnCameraChangeFinished();
    private float m_TargetCameraXOffset3D = 8.2f;                //3D摄像机相对主角的X偏移(目标记忆）
    private float m_TargetCameraYOffset3D = 4.3f;                //3D摄像机相对主角的Y偏移(目标记忆）
    private float m_TargetCameraZOffset3D = 4.0f;                //3D摄像机相对主角的Z偏移(目标记忆）
    private float m_TargetCenterOffest = 1.0f;                   //摄像机相对抬高一个距离(目标记忆）
    private float m_TargetCameraXOffset3DSpeed = 0.0f;
    private float m_TargetCameraYOffset3DSpeed = 0.0f;
    private float m_TargetCameraZOffset3DSpeed = 0.0f;
    private float m_TargetCenterOffsetSpeed = 0.0f;
    private bool m_IsChanging = false;
    private OnCameraChangeFinished m_OnCameraChangeFinished = null;

    public void CameraChangeParam(int cfgId, float speed, OnCameraChangeFinished onCameraChangeFinished)
    {
        if (GlobeVar.INVALID_ID == cfgId)
        {
            return;
        }

        Tab_CameraCfg cameraCfg = TableManager.GetCameraCfgByID(cfgId, 0);
        if (cameraCfg != null)
        {
            if (speed <= 0)
            {
                InitParam(cameraCfg);
                if (null != onCameraChangeFinished)
                {
                    onCameraChangeFinished();
                }
            }
            else
            {
                //注意，有几个值需要缓慢变更
                float memCameraXOffset3D = m_CameraXOffset3D;
                float memCameraYOffset3D = m_CameraYOffset3D;
                float memCameraZOffset3D = m_CameraZOffset3D;
                float memCenterOffset = m_CenterOffest;

                InitParam(cameraCfg);

                m_TargetCameraXOffset3D = m_CameraXOffset3D;
                m_TargetCameraYOffset3D = m_CameraYOffset3D;
                m_TargetCameraZOffset3D = m_CameraZOffset3D;
                m_TargetCenterOffest = m_CenterOffest;
                m_TargetCameraXOffset3DSpeed = (m_CameraXOffset3D - memCameraXOffset3D) / speed;
                m_TargetCameraYOffset3DSpeed = (m_CameraYOffset3D - memCameraYOffset3D) / speed;
                m_TargetCameraZOffset3DSpeed = (m_CameraZOffset3D - memCameraZOffset3D) / speed;
                m_TargetCenterOffsetSpeed = (m_CenterOffest - memCenterOffset) / speed;

                m_CameraXOffset3D = memCameraXOffset3D;
                m_CameraYOffset3D = memCameraYOffset3D;
                m_CameraZOffset3D = memCameraZOffset3D;
                m_CenterOffest = memCenterOffset;

                m_IsChanging = true;
                m_OnCameraChangeFinished = onCameraChangeFinished;
            }

            m_CameraTransform.position = GetInitPos();
        }
    }

    private void UpdateCameraChange(float delayTime)
    {
        if (m_IsChanging)
        {
            m_CameraXOffset3D += delayTime * m_TargetCameraXOffset3DSpeed;
            m_CameraYOffset3D += delayTime * m_TargetCameraYOffset3DSpeed;
            m_CameraZOffset3D += delayTime * m_TargetCameraZOffset3DSpeed;
            m_CenterOffest += delayTime * m_TargetCenterOffsetSpeed;

            if ((m_TargetCameraXOffset3DSpeed < 0 && m_CameraXOffset3D <= m_TargetCameraXOffset3D
                    || m_TargetCameraXOffset3DSpeed >= 0 && m_CameraXOffset3D >= m_TargetCameraXOffset3D)
                && (m_TargetCameraYOffset3DSpeed < 0 && m_CameraYOffset3D <= m_TargetCameraYOffset3D
                    || m_TargetCameraYOffset3DSpeed >= 0 && m_CameraYOffset3D >= m_TargetCameraYOffset3D)
                && (m_TargetCameraZOffset3DSpeed < 0 && m_CameraZOffset3D <= m_TargetCameraZOffset3D
                    || m_TargetCameraZOffset3DSpeed >= 0 && m_CameraZOffset3D >= m_TargetCameraZOffset3D)
                && (m_TargetCenterOffsetSpeed < 0 && m_CenterOffest <= m_TargetCenterOffest
                    || m_TargetCenterOffsetSpeed >= 0 && m_CenterOffest >= m_TargetCenterOffest))
            {
                StopCameraChange(false);
            }
            else
            {
                m_CameraTransform.position = GetInitPos();
                UpdateLookAt();
            }
        }
    }

    public void StopCameraChange(bool bKeepCurState)
    {
        if (m_IsChanging)
        {
            m_IsChanging = false;

            if (!bKeepCurState)
            {
                m_CameraXOffset3D = m_TargetCameraXOffset3D;
                m_CameraYOffset3D = m_TargetCameraYOffset3D;
                m_CameraZOffset3D = m_TargetCameraZOffset3D;
                m_CenterOffest = m_TargetCenterOffest;

                m_CameraTransform.position = GetInitPos();
            }
            if (null != m_OnCameraChangeFinished)
            {
                m_OnCameraChangeFinished();
                m_OnCameraChangeFinished = null;
            }
        }
    }
    #endregion


#region 新版相机震动

    public ShakeGroup shakeGroup = new ShakeGroup();

    public void CameraShake(Shake shake)
    {
        shakeGroup.AddShake(shake);
    }

    private void UpdateShake()
    {
        if (CameraTransform != null)
        {
            shakeGroup.UpdateShake(CameraTransform);
        }
    }

    #endregion
}
