using System.Collections;
using System.Collections.Generic;
using Games;
using Games.GlobeDefine;
using Games.LogicObj;
using UnityEngine;

public class HouseMainUI : UIControllerBase<HouseMainUI>
{
    public int baseWidth;
    public int eachWidth;
    public UIGrid m_MainUIGrid;

    public UIEventTrigger m_BacktoCity;
    public UIEventTrigger m_SizhaiProduce;
    public GameObject m_ProduceRedPoint;
    public UIEventTrigger m_FulingShow;
    public UIEventTrigger m_FulingLu;
    public GameObject m_FulingLuRedPoint;
    public UIEventTrigger m_SizhaiSkin;
    public UIEventTrigger m_FriendList;
    public UIEventTrigger m_GoHome;
    public GameObject m_NoteRedPoint;
    public GameObject m_SizhaiMainUIRedPoint;
    private bool m_bHiding = false;               //默认打开
    bool isMine = false;

    public TweenRotation m_SpreadTween;
    public TweenPosition m_VspreadTween;
    public TweenPosition m_HspreadTween;
    public TweenPosition m_HspreadBgTween;
    public TweenWidth m_BGWidthTween;
    public UISprite m_TutorialCardShowBtn;
    public UISprite m_TutorialProduceBtn;

    public UILabel m_OwnerName;
    public GameObject m_btnChangeSkin;

    void Awake()
    {
        SetInstance(this);
    }
    void OnEnable()
    {
        m_SpreadTween.ResetToBeginning(m_bHiding);
        m_SpreadTween.enabled = false;
        m_VspreadTween.ResetToBeginning(m_bHiding);
        m_HspreadTween.ResetToBeginning(m_bHiding);
        m_HspreadBgTween.ResetToBeginning(m_bHiding);

        JoyStickController.ShowRocker(true);        //虚拟摇杆与MainUI同在
        UpdateTutorialOnShow();
        if (isMine)
        {
            StartCoroutine(Update_OneSecond());
        }
    }

    void Start()
    {
        if(GameManager.CurScene == null)
        {
            return;
        }
        if (GameManager.CurScene.IsHouseScene())
        {
            HouseScene hs = GameManager.CurScene as HouseScene;
            if (hs == null)
                return;
            if (hs.IsOwner(LoginData.user.guid))
                isMine = true;

            if (isMine)
                m_OwnerName.gameObject.SetActive(false);
            else if (hs.YardData != null)
            {
                m_OwnerName.gameObject.SetActive(true);
                m_OwnerName.text = StrDictionary.GetDicByID(8258, hs.YardData.OwnerName);
            }

        }
        int count = 0;
        m_SizhaiSkin.gameObject.SetActive(isMine);
        m_FulingLu.gameObject.SetActive(isMine); 
        m_FulingShow.gameObject.SetActive(isMine);
        m_SizhaiProduce.gameObject.SetActive(isMine);
        m_GoHome.gameObject.SetActive(!isMine);
        //找到所有控件
        foreach(Transform tr in m_MainUIGrid.GetChildList())
        {
            if(tr.gameObject.activeInHierarchy)
            {
                count++;
            }
        }
        m_BGWidthTween.to = baseWidth + eachWidth * count;
        m_BGWidthTween.enabled = false;
        m_BGWidthTween.ResetToBeginning(m_bHiding);
        m_MainUIGrid.Reposition();
        m_BacktoCity.onClick.Add(new EventDelegate(OnMainCityClick)); //返回主城
        m_SizhaiProduce.onClick.Add(new EventDelegate(OnProduceClick)); //私宅制造
        m_GoHome.onClick.Add(new EventDelegate(OnGoHouseClick)); //回自己的私宅
        m_FulingLu.onClick.Add(new EventDelegate(OnCardBagClick)); //符灵录
        m_FulingShow.onClick.Add(new EventDelegate(OnCardShowClick)); //展示列表
        UpdateRedPoint();
    }

    IEnumerator Update_OneSecond()
    {
        while(true)
        {
            yield return new WaitForSeconds(1.0f);
            UpdateRedPoint();
        }
    }
    public void ShowMenu(bool show)
    {
        gameObject.SetActive(show);
    }

    public void OnGoHouseClick()
    {
        if (ObjManager.MainPlayer == null || ObjManager.MainPlayer.IsInRelaxAnim())
        {
            Utils.CenterNotice(7009);
            return;
        }
        Yard.SendEnter(LoginData.user.guid);
    }

    public void OnMainCityClick()
    {
        PlatformHelper.RecordEvent("私宅返回洛阳");
        if (ObjManager.MainPlayer == null || ObjManager.MainPlayer.IsInRelaxAnim())
        {
            Utils.CenterNotice(7009);
            return;
        }
        GameManager.LeaveHouseScene();
    }

    public void OnProduceClick()
    {
        if (ObjManager.MainPlayer == null)
            return;

        if (ObjManager.MainPlayer.IsInRelaxAnim())
        {
            Utils.CenterNotice(7009);
            return;
        }

        HouseScene hs = GameManager.CurScene as HouseScene;
        if (hs == null || !hs.InMode(HouseScene.HouseMode.NORMAL))
            return;

        Obj_NPC obj = hs.GetProdNpc();
        if (obj == null)
            return;

        ObjManager.MainPlayer.MoveTo(obj, -1, (int)CHAR_ANIM_ID.Run);
    }

    public void OnCardBagClick()
    {
        if (ObjManager.MainPlayer == null || ObjManager.MainPlayer.IsInRelaxAnim())
        {
            Utils.CenterNotice(7009);
            return;
        }
        CardBagController.OpenCardBag();
    }

    public void OnCardShowClick()
    {
        if (ObjManager.MainPlayer == null || ObjManager.MainPlayer.IsInRelaxAnim())
        {
            Utils.CenterNotice(7009);
            return;
        }

        var scene = GameManager.CurScene as HouseScene;
        if (scene == null)
        {
            return;
        }
        scene.SwitchMode(HouseScene.HouseMode.EDIT);
    }

    public void OnHouseSkinClick()
    {
        if (ObjManager.MainPlayer == null || ObjManager.MainPlayer.IsInRelaxAnim())
        {
            Utils.CenterNotice(7009);
            return;
        }
        HouseChangeSkinController.Show();
    }

    public void OnRelationClick()
    {
        if (ObjManager.MainPlayer == null || ObjManager.MainPlayer.IsInRelaxAnim())
        {
            Utils.CenterNotice(7009);
            return;
        }
        HouseRelationRoot.Open();
    }

    public void OnNoteClick()
    {
        HouseNoteController.Show();
    }

    public void OnOpenRelaxAnimWindowClick()
    {
        RelaxAnimController.ShowRelaxAnim();
    }

    public void OpenSocialIntercourseRoot()
    {
        QuickChatController.OnClickExitButton();
    }

    public void SpreadMenu()
    {
        if (m_SpreadTween.isActiveAndEnabled || m_VspreadTween.isActiveAndEnabled || m_HspreadBgTween.isActiveAndEnabled || m_HspreadTween.isActiveAndEnabled)
        {
            return;
        }
        m_bHiding = !m_bHiding;


        if (m_bHiding)
        {
            // 隐藏菜单
            // JoyStickController.ShowRocker(true);
            m_SpreadTween.PlayReverse();

            m_HspreadTween.PlayReverse();
            m_VspreadTween.PlayReverse();
            m_HspreadBgTween.PlayReverse();
            m_BGWidthTween.PlayReverse();
        }
        else
        {
            // 展开菜单
            // JoyStickController.ShowRocker(false);
            m_SpreadTween.PlayForward();

            m_HspreadTween.PlayForward();
            m_VspreadTween.PlayForward();
            m_HspreadBgTween.PlayForward();
            m_BGWidthTween.PlayForward();
        }
    }

    private void UpdateRedPoint()
    {
        if(isMine)
        {
            m_FulingLuRedPoint.SetActive(GameManager.PlayerDataPool.IsCardBagRedShow());
            m_ProduceRedPoint.SetActive(GameManager.PlayerDataPool.IsYardProdRedShow());
            m_btnChangeSkin.SetActive(TutorialManager.IsFunctionUnlock((int)FunctionUnlockId.HouseChangeSkin));

            m_SizhaiMainUIRedPoint.SetActive(m_FulingLuRedPoint.activeSelf || m_ProduceRedPoint.activeSelf);
        }
        else
        {
            m_FulingLuRedPoint.SetActive(false);
            m_ProduceRedPoint.SetActive(false);

            m_SizhaiMainUIRedPoint.SetActive(false);
        }
        m_NoteRedPoint.SetActive(GameManager.PlayerDataPool.IsYardNoteRedPoint());
    }


    void UpdateTutorialOnShow()
    {
        if (false == TutorialManager.IsTutorialComplete(TutorialGroup.House, 2))
        {
            TutorialRoot.ShowTutorial(TutorialGroup.House,2);
        }
    }

    public void UpdateTutorialOnTutorialBottomClick()
    {
        if (TutorialRoot.IsGroupStep(TutorialGroup.House, 2))
        {
            TutorialRoot.ShowTutorial(TutorialGroup.House, 3);
        }
        else if (TutorialRoot.IsGroupStep(TutorialGroup.House, 3))
        {
            TutorialRoot.ShowTutorial(TutorialGroup.House, 4);
        }
        else if (TutorialRoot.IsGroupStep(TutorialGroup.House, 4))
        {
            TutorialRoot.ShowTutorial(TutorialGroup.House, 5);
        }
        else if (TutorialRoot.IsGroupStep(TutorialGroup.House, 5))
        {
            TutorialRoot.ShowTutorial(TutorialGroup.House, 6);
        }
        else if (TutorialRoot.IsGroupStep(TutorialGroup.House, 6))
        {
            TutorialRoot.ShowTutorial(TutorialGroup.House, 7);
        }
        else if (TutorialRoot.IsGroupStep(TutorialGroup.House, 7))
        {
            TutorialRoot.ShowTutorial(TutorialGroup.House, 8, m_TutorialCardShowBtn.gameObject, m_TutorialCardShowBtn.width, m_TutorialCardShowBtn.height);
        }
    }
    public void UpdateTutorialOnTutorialMaskClick()
    {
        if (TutorialRoot.IsGroupStep(TutorialGroup.House, 8))
        {
            TutorialRoot.ShowTutorial(TutorialGroup.House, 9, m_TutorialProduceBtn.gameObject, m_TutorialProduceBtn.width, m_TutorialProduceBtn.height);
        }
        else if (TutorialRoot.IsGroupStep(TutorialGroup.House, 9))
        {
            TutorialRoot.TutorialOver();
        }
    }

}