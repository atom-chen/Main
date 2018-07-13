using UnityEngine;
using System.Collections;
using Games.GlobeDefine;
using Games.LogicObj;
public class JoyStickController : MonoBehaviour
{
    private static JoyStickController m_Instance = null;
    public static JoyStickController Instance()
    {
        return m_Instance;
    }

    private void OnEnable()
    {
        UpdateTutorialOnShow();
        m_JoyStick.ResetJoyStick();
        UICamera.onDragStart += UpdateTutorialOnDragStart;
        UICamera.onDragEnd += UpdateTutorialOnDragEnd;
    }

    public JoyStick m_JoyStick;
    private GameObject m_JoyStickObj;
    public UISprite m_JoyStickIcon;
    private TutorialGroup m_TutorialGroupOnDragEnd;
    private int m_TutorialStepOnDragEnd;
    void Awake()
    {
        m_Instance = this;
        m_JoyStickObj = m_JoyStick.gameObject;
    }

    void OnDestroy()
    {
        m_Instance = null;
    }
    

    void OnDisable()
    {
        //LogModule.ErrorLog("disable joystick---------------------------");
        ReleaseJoyStick();
        UICamera.onDragStart -= UpdateTutorialOnDragStart;
        UICamera.onDragEnd -= UpdateTutorialOnDragEnd;
    }

    public void ReleaseJoyStick()
    {
        if ( m_JoyStick != null )
            m_JoyStick.ReleaseJoyStick();

        if (ObjManager.MainPlayer == null)
        {
            return;
        }

        if (ObjManager.MainPlayer.ThirdController == null)
        {
            return;
        }
        ObjManager.MainPlayer.ThirdController.HorizonRaw = 0f;
        ObjManager.MainPlayer.ThirdController.VerticalRaw = 0f;
    }

    public void SendMoveDirection(float fHorizon, float fVertical)
    {
        if (ObjManager.MainPlayer == null)
        {
            return;
        }

        if (ObjManager.MainPlayer.ThirdController == null)
        {
            return;
        }
        
        ObjManager.MainPlayer.ThirdController.HorizonRaw = fHorizon;
        ObjManager.MainPlayer.ThirdController.VerticalRaw = fVertical;
        UpdateTutorialOnMoveDirection();
    }

    public static void ShowRocker(bool show)
    {
        if (null != m_Instance && null != m_Instance.m_JoyStickObj)
        {
            m_Instance.gameObject.SetActive(show);
        }
    }

    void UpdateTutorialOnShow()
    {
        if (GameManager.storyManager == null || GameManager.PlayerDataPool == null)
        {
            return;
        }
        if (GameManager.storyManager.StoryMode && BeforeLoginSceneLogic.IsStoryBeforeLogin && GameManager.PlayerDataPool.m_nStoryLine == GlobeVar.TutorialMoveRot_StoryLineId &&
            GameManager.PlayerDataPool.m_nStroyID == GlobeVar.TutorialMoveRot_StoryId)
        {

            if (TutorialManager.IsOpenTutorial)
            {
                if (ObjManager.MainPlayer != null)
                {
                    ObjManager.MainPlayer.IsStoryMove = true;
                }
                m_JoyStickIcon.enabled = false;
                m_JoyStick.gameObject.SetActive(false);
                TutorialRoot.ShowTutorial(TutorialGroup.MoveRot, 1);
            }
        }
    }

    private void UpdateTutorialOnMoveDirection()
    {
        if (TutorialRoot.IsGroupStep(TutorialGroup.MoveRot, 2))
        {
            m_JoyStickIcon.enabled = true;
            Obj_NPC objNpc = ObjManager.GetObjNPCBySceneNPCID(GlobeVar.TutorialMoveRot_SceneNpcId);
            if (objNpc != null && objNpc.NpcHeadInfoLogic!=null)
            {
                objNpc.NpcHeadInfoLogic.PlayTutorialEffect();
            }
            TutorialRoot.ShowTutorial(TutorialGroup.MoveRot, 3);
        }
    }
    private void UpdateTutorialOnDragStart(GameObject go)
    {
        if (TutorialRoot.IsGroupStep(TutorialGroup.MoveRot, 1))
        {
            if (ObjManager.MainPlayer != null)
            {
                ObjManager.MainPlayer.IsStoryMove = false;
            }
            TutorialRoot.TutorialOver();
            m_TutorialGroupOnDragEnd = TutorialGroup.MoveRot;
            m_TutorialStepOnDragEnd = 2;
        }
    }
    private void UpdateTutorialOnDragEnd(GameObject go)
    {
        if (m_TutorialGroupOnDragEnd == TutorialGroup.MoveRot && m_TutorialStepOnDragEnd == 2)
        {
            m_JoyStick.gameObject.SetActive(true);
            TutorialRoot.ShowTutorial(TutorialGroup.MoveRot, 2, m_JoyStickIcon.gameObject, m_JoyStickIcon.width + 100, m_JoyStickIcon.height + 100);
            m_TutorialGroupOnDragEnd = TutorialGroup.Invalid;
            m_TutorialStepOnDragEnd = GlobeVar.INVALID_ID;
        }
    }

    public void UpdateTutorialOnAutoClose()
    {
        m_JoyStick.gameObject.SetActive(true);
        m_JoyStickIcon.enabled = true;
        if (ObjManager.MainPlayer != null)
        {
            ObjManager.MainPlayer.IsStoryMove = false;
        }
    }
}
