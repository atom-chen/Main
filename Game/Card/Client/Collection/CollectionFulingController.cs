using Games;
using Games.GlobeDefine;
using Games.Table;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectionFulingController : MonoBehaviour 
{
    private static CollectionFulingController _Ins = null;
    public static CollectionFulingController Instance
    {
        get { return _Ins; }
    }
    public TabController m_TotalTab;
    public GameObject m_InfoObj;
    public GameObject m_MapObj;
    public GameObject m_GroupObj;

    public TabController m_DetailStoryTab;
    public GameObject m_DetailObj;
    public GameObject m_StoryObj;

    public UILabel m_Poetry;
    public UILabel m_Intro;
    public UILabel m_NameLabel;
    public UILabel m_CVLabel;
    public UISprite m_RareIcon;
    public PaintNormalizer m_Texture;
    public UITexture m_BaseTex;
    public ListWrapController m_Wrap;
    public OptionList m_RareOpList;
    public UITexture[] m_Skill;
    public CollectionDetailSkillWindow m_SkillWindow;
    public GameObject[] m_SkillChoose;
    public GameObject m_LeftObj;
    public GameObject m_RightObj;

    public UILabel m_StoryName;
    public UILabel m_StoryContent;
    public UILabel m_StoryCVNameLabel;
    public UISprite m_StoryPlaySoundPic;
    public UIScrollView m_StorySV;

    public ListWrapController m_GroupWrap;
    public GameObject m_GroupPreviewObj;
    public GameObject m_GroupDeatilObj;
    public UILabel m_CurGroupNameLabel;
    public GameObject m_GroupTabRedPoint;
    public GameObject[] m_CurGroupRedPoint;
    private CollectionFulingItem[] m_GroupFulingItem;
    public StateGroup m_AwardSG;

    public TabController m_MapTab;
    public GameObject m_CardPhotoPrefab;
    public GameObject m_PhtotRoot;

    private int[] m_InitSkillList = new int[GlobeVar.CARD_SKILLCOUNT]; // 初始技能列表
    private List<int> cardIDList = new List<int>();
    [HideInInspector]
    public int mCurChooseCard = GlobeVar.INVALID_ID;
    [HideInInspector]
    public CARD_RARE mCurChooseRare = CARD_RARE.INVALID;
    private CARD_RARE mCurChoosePhtotRare = CARD_RARE.UR;
    private int m_TipSkillIndex = GlobeVar.INVALID_ID;
    private int m_StoryIdx = 0;
    private int mNowPlaySound = GlobeVar.INVALID_ID;
    private List<Tab_Handbook> m_HandBook = new List<Tab_Handbook>();
    private int mNowOpenGroupId = GlobeVar.INVALID_ID;
    void Awake()
    {
        _Ins = this;
        m_RareOpList.delOnOptionItemChoose += OnCardRareOptionChoose;        //排序规则
        m_DetailStoryTab.delTabChanged += OnDetailStoryTabChange;
        m_TotalTab.delTabChanged += OnTotalTabChange;
        m_MapTab.delTabChanged += OnMapOptionChoose;
        CollectionRedTool.sUpdateEvent += RefreshGroup;
    }
    void Destroy()
    {
        _Ins = null;
        CollectionRedTool.sUpdateEvent -= RefreshGroup;
    }
    void Start()
    {
        m_RareOpList.SetDefaultOption("4");
        m_TotalTab.ChangeTab(0);
        m_DetailStoryTab.ChangeTab(0);
        m_MapTab.ChangeTab("3");
        m_SkillWindow.gameObject.SetActive(false);
        m_GroupFulingItem = m_GroupDeatilObj.GetComponentsInChildren<CollectionFulingItem>();
        m_GroupTabRedPoint.SetActive(CollectionRedTool.IsCanAward_Card());
        RefreshInfo();
        InitGroup();
    }

    void RefreshInfo()
    {
        cardIDList.Clear();
        foreach (var item in TableManager.GetCard().Values)
        {
            foreach (var v in item)
            {
                if (v == null)
                {
                    continue;
                }
                if(v.ClassId!=(int)CardClass.Normal)
                {
                    continue;
                }
                if(v.CollectionPhtotId == GlobeVar.INVALID_ID)
                {
                    continue;
                }
                if (mCurChooseRare == CARD_RARE.INVALID || (int)mCurChooseRare == v.Rare)
                {
                    cardIDList.Add(v.Id);
                }
                cardIDList.Sort(CollectionRedTool.CardSort);
            }
        }
        
        if (cardIDList.Count > 0)
        {
            mCurChooseCard = cardIDList[0];
        }
        m_Wrap.InitList(cardIDList.Count, UpdateCardList);
        HandleOnClickItem(mCurChooseCard);
    }

    void RefreshCardPhtot()
    {
        Utils.CleanGrid(m_PhtotRoot);
        StopCoroutine("LoadFulingTex");
        //遍历
        StartCoroutine(LoadFulingTex());
    }

    //每帧加载一个
    IEnumerator LoadFulingTex()
    {
        foreach (var item in TableManager.GetCard().Values)
        {
            foreach (var v in item)
            {
                if (v == null)
                {
                    continue;
                }
                if ((int)mCurChoosePhtotRare == v.Rare)
                {
                    Tab_RoleBaseAttr tRole = TableManager.GetRoleBaseAttrByID(v.GetRoleBaseIDStepbyIndex(0), 0);
                    if (tRole == null)
                    {
                        continue;
                    }
                    Tab_CollectionCardPhoto tPhoto = TableManager.GetCollectionCardPhotoByID(v.CollectionPhtotId, 0);
                    if (tPhoto == null)
                    {
                        continue;
                    }
                    //创建
                    GameObject obj = NGUITools.AddChild(m_PhtotRoot, m_CardPhotoPrefab);
                    if (obj != null)
                    {
                        PaintNormalizer paint = obj.GetComponent<PaintNormalizer>();
                        if (paint != null)
                        {
                            paint.name = tRole.Name;
                            paint.SetupReduceTex(tRole.CharModelID);
                            UIPanel panel = paint.GetComponent<UIPanel>();
                            if(panel!=null)
                            {
                                panel.depth = tPhoto.Depth;
                            }
                            Transform tr = paint.transform;
                            tr.localScale = Vector3.one * tPhoto.Scale;
                            tr.localPosition = new Vector3(tPhoto.PosX, tPhoto.PosY, tPhoto.PosZ);
                            //如果已拥有
                            if (GameManager.PlayerDataPool.CollectionData.IsGetCard(v.Id))
                            {
                                paint.UIMainTexture.color = GlobeVar.NORMALCOLOR;
                            }
                            else
                            {
                                paint.UIMainTexture.color = GlobeVar.DISABLECOLOR;
                            }
                        }
                        yield return null;
                    }
                }
            }
        }
    }

    void InitGroup()
    {
        m_HandBook.Clear();
        foreach (var item in TableManager.GetHandbook().Values)
        {
            foreach (var v in item)
            {
                if (v == null)
                {
                    continue;
                }
                if (v.Style == (int)COLLECTION_TYPE.COLLECTION_CARD)
                {
                    m_HandBook.Add(v);
                }
            }
        }
        m_HandBook.Sort(CollectionRedTool.GroupSort);
        m_GroupWrap.InitList(m_HandBook.Count, UpdateGroupList, false);
        
    }
    public void OnClickSkillMask()
    {
        m_SkillWindow.gameObject.SetActive(false);
        m_TipSkillIndex = GlobeVar.INVALID_ID;
        UpdateSkillChoose();
    }

    public void OnClickReturnInfo()
    {
        if(m_GroupDeatilObj.activeInHierarchy)
        {
            m_GroupDeatilObj.SetActive(false);
            m_GroupPreviewObj.SetActive(true);
            mNowOpenGroupId = GlobeVar.INVALID_ID;
        }
        else
        {
            m_TotalTab.ChangeTab(0);
        }
    }

    public void OnClickStoryLeft()
    {
        Tab_CardStory tStory = TableManager.GetCardStoryByID(mCurChooseCard, 0);
        if (tStory == null)
            return;
        //idx检测
        if (m_StoryIdx - 1 >= 0)
        {
            //解锁检测
            if (PlayerPreferenceData.IsClickCardLegend(mCurChooseCard, m_StoryIdx - 1))
            {
                m_StoryIdx--;
                RefreshStory();
            }
            else
            {
                Utils.CenterNotice(8907);
            }
        }
        StopPlaySound(); 
    }

    public void OnClickStoryRight()
    {
        Tab_CardStory tStory = TableManager.GetCardStoryByID(mCurChooseCard, 0);
        if (tStory == null)
            return;
        if (m_StoryIdx + 1 < tStory.getBiographyCount())
        {
            if (PlayerPreferenceData.IsClickCardLegend(mCurChooseCard, m_StoryIdx + 1))
            {
                m_StoryIdx++;
                RefreshStory();
            }
            else
            {
                Utils.CenterNotice(8907);
            }
        }
        StopPlaySound(); 
    }
    public void OnClickStoryPlaySound()
    {
        Tab_CardStory tStory = TableManager.GetCardStoryByID(mCurChooseCard, 0);
        if (tStory != null)
        {
            if (m_StoryIdx >= 0 && m_StoryIdx < tStory.getBiographyVoiceCount())
            {
                int soundId = tStory.GetBiographyVoicebyIndex(m_StoryIdx);
                if (soundId == GlobeVar.INVALID_ID)
                {
                    return;
                }
                if (GameManager.SoundManager.IsRealSoundPlaying())
                {
                    if (GameManager.SoundManager.GetCurRealSoundId() != soundId)
                    {
                        GameManager.SoundManager.StopRealSound();
                        GameManager.SoundManager.PlayRealSound(soundId);
                        mNowPlaySound = soundId;
                    }
                }
                else
                {
                    GameManager.SoundManager.PlayRealSound(soundId);
                    mNowPlaySound = soundId;
                }
            }
        }
    }

    public void OnClickGroupAward()
    {
        Tab_Handbook tab = TableManager.GetHandbookByID(mNowOpenGroupId, 0);
        if(tab == null)
        {
            return;
        }
        //已完成，只弹出提示
        if (GameManager.PlayerDataPool.CollectionData.CollectionGroupFinishList.AlreadyReceived.Contains(mNowOpenGroupId))
        {
            DropPreviewWindow.Show(tab.DropPreviewId, StrDictionary.GetDicByID(5178));
            Utils.CenterNotice(6718);
        }
        else
        {
            //未达成条件 展示奖励
            if (!CollectionRedTool.IsCanAward_Handbook(tab))
            {
                DropPreviewWindow.Show(tab.DropPreviewId);
            }
            else
            {
                //可领取 则发奖励
                CG_CollectionGroup_ReceiveAward_PAK pak = new CG_CollectionGroup_ReceiveAward_PAK();
                pak.data.GroupId = mNowOpenGroupId;
                pak.SendPacket();
            }
        }
    }
    public void OnClickExit()
    {
        UIManager.CloseUI(UIInfo.CollectionCardRoot);
        if (GameManager.SoundManager.IsRealSoundPlaying())
        {
            if (GameManager.SoundManager.GetCurRealSoundId() == mNowPlaySound)
            {
                GameManager.SoundManager.StopRealSound();
            }
        }
    }

    void UpdateCardList(GameObject obj,int idx)
    {
        if(obj == null)
        {
            return;
        }
        CollectionFulingItem item = obj.GetComponent<CollectionFulingItem>();
        if (item == null)
        {
            return;
        }
        if(idx<0 || idx>=cardIDList.Count)
        {
            return;
        }
        item.Refresh(cardIDList[idx]);
    }

    void UpdateGroupList(GameObject obj,int idx)
    {
        if (obj == null)
        {
            return;
        }
        CollectionGroupItem item = obj.GetComponent<CollectionGroupItem>();
        if (item == null)
        {
            return;
        }
        if (idx < 0 || idx >= m_HandBook.Count)
        {
            return;
        }
        item.Refresh(m_HandBook[idx]);
    }

    void OnCardRareOptionChoose(OptionItem item)
    {
        switch(item.name)
        {
            case "0":
                mCurChooseRare = CARD_RARE.R;
                break;
            case "1":
                mCurChooseRare = CARD_RARE.SR;
                break;
            case "2":
                mCurChooseRare = CARD_RARE.SSR;
                break;
            case "3":
                mCurChooseRare = CARD_RARE.UR;
                break;
            case "4":
                mCurChooseRare = CARD_RARE.INVALID;
                break;
        }
        RefreshInfo();
    }

    void OnDetailStoryTabChange(TabButton btn)
    {
        StopPlaySound();
        switch(btn.name)
        {
            case "DetailButton":
                m_DetailObj.SetActive(true);
                m_StoryObj.SetActive(false);
                break;
            case "StoryButton":
                m_DetailObj.SetActive(false);
                m_StoryObj.SetActive(true);
                break;
                
        }
    }

    void OnTotalTabChange(TabButton btn)
    {
        StopPlaySound();
        switch(btn.name)
        {
            case "Info":
                m_InfoObj.SetActive(true);
                m_MapObj.SetActive(false);
                m_GroupObj.SetActive(false);
                break;
            case "Directory":
                m_InfoObj.SetActive(false);
                m_MapObj.SetActive(false);
                m_GroupObj.SetActive(true);
                m_GroupPreviewObj.SetActive(true);
                m_GroupDeatilObj.SetActive(false);
                break;
        }
    }

    public void OnClickFamily()
    {
        m_InfoObj.SetActive(false);
        m_MapObj.SetActive(true);
        m_GroupObj.SetActive(false);
        m_MapTab.ChangeTab("3");
        StopPlaySound(); 
    }

    void OnMapOptionChoose(TabButton item)
    {
        switch (item.name)
        {
            case "0":
                mCurChoosePhtotRare = CARD_RARE.R;
                break;
            case "1":
                mCurChoosePhtotRare = CARD_RARE.SR;
                break;
            case "2":
                mCurChoosePhtotRare = CARD_RARE.SSR;
                break;
            case "3":
                mCurChoosePhtotRare = CARD_RARE.UR;
                break;
        }
        RefreshCardPhtot();
    }

    // 设置技能显示
    private void SetSkill(Tab_Card card)
    {
        m_SkillWindow.gameObject.SetActive(false);
        if (null == m_InitSkillList || card == null)
        {
            return;
        }


        // 基础和3阶觉醒对应属性
        Tab_RoleBaseAttr[] attr = new Tab_RoleBaseAttr[4];

        for (int i = 0; i < attr.Length && i < card.getRoleBaseIDStepCount(); ++i)
        {
            attr[i] = TableManager.GetRoleBaseAttrByID(card.GetRoleBaseIDStepbyIndex(i), 0);//拿到各阶技能
        }

        for (int i = 0; i < GlobeVar.CARD_SKILLCOUNT; ++i)
        {
            m_InitSkillList[i] = -1;
        }

        for (int i = 0; i < GlobeVar.CARD_SKILLCOUNT; ++i)
        {
            if (null == attr[0])
            {
                break;
            }

            // 1阶技能
            m_InitSkillList[i]  = attr[0].GetSkillbyIndex(i);

            // 2阶技能
            if (null != attr[1] && m_InitSkillList[i] != attr[1].GetSkillbyIndex(i))
            {
                m_InitSkillList[i] = attr[1].GetSkillbyIndex(i);
                continue;
            }

            // 3阶技能
            if (null != attr[2] && m_InitSkillList[i] != attr[2].GetSkillbyIndex(i))
            {
                m_InitSkillList[i] = attr[2].GetSkillbyIndex(i);
                continue;
            }

            // 4阶技能
            if (null != attr[3] && m_InitSkillList[i] != attr[3].GetSkillbyIndex(i))
            {
                m_InitSkillList[i] = attr[3].GetSkillbyIndex(i);
                continue;
            }

        }

        // 设置技能图标
        SetSkillIcon(m_Skill[0], m_InitSkillList[0]);
        SetSkillIcon(m_Skill[1], m_InitSkillList[1]);
        SetSkillIcon(m_Skill[2], m_InitSkillList[2]);
        m_TipSkillIndex = GlobeVar.INVALID_ID;
        UpdateSkillChoose();
    }

    // 设置技能图标
    private void SetSkillIcon(UITexture icon, int skillExId)
    {
        Tab_SkillEx skillEx = TableManager.GetSkillExByID(skillExId, 0);
        Tab_SkillBase skill = null;
        if (null != skillEx)
        {
            skill = TableManager.GetSkillBaseByID(skillEx.BaseID, 0);
        }
        if (null == skill || skillExId == -1)
        {
            icon.gameObject.SetActive(false);

        }
        else
        {
            AssetManager.SetIconSkillTexture(icon, skill.Icon);
            icon.gameObject.SetActive(true);
        }
    }

    public void OnSkillClick_1()
    {
        ShowSkillTip(0);
    }

    public void OnSkillClick_2()
    {
        ShowSkillTip(1);
    }

    public void OnSkillClick_3()
    {
        ShowSkillTip(2);
    }

    private void ShowSkillTip(int index)
    {
        if (index < 0 || index >= m_InitSkillList.Length)
        {
            return;
        }

        if (m_TipSkillIndex == index)
        {
            m_SkillWindow.gameObject.SetActive(false);
            m_TipSkillIndex = GlobeVar.INVALID_ID;
            UpdateSkillChoose();
            return;
        }
        m_TipSkillIndex = index;
        UpdateSkillChoose();
        m_SkillWindow.gameObject.SetActive(true);
        m_SkillWindow.Show(m_InitSkillList[index]);

    }

    private void UpdateSkillChoose()
    {
        for(int i = 0;i<m_SkillChoose.Length;i++)
        {
            m_SkillChoose[i].SetActive(i == m_TipSkillIndex);
        }
    }


    private void RefreshStory()
    {
        m_StorySV.ResetPosition();
        Tab_CardStory tStory = TableManager.GetCardStoryByID(mCurChooseCard, 0);
        if (tStory == null)
            return;
        if (m_StoryIdx >= tStory.getBiographyCount() || m_StoryIdx >= tStory.getBiographyVoiceCount())
        {
            return;
        }
        //如果没有前一页 隐藏左箭头
        m_LeftObj.SetActive(m_StoryIdx != 0 && PlayerPreferenceData.IsClickCardLegend(mCurChooseCard, m_StoryIdx - 1));
        m_RightObj.SetActive(m_StoryIdx + 1 < tStory.getBiographyCount() && PlayerPreferenceData.IsClickCardLegend(mCurChooseCard, m_StoryIdx + 1));
        

        m_StoryName.text = tStory.GetBiographyTitlebyIndex(m_StoryIdx);
        m_StoryContent.text = tStory.GetBiographybyIndex(m_StoryIdx).Replace("#r", "\n");
        BoxCollider box = m_StoryContent.GetComponent<BoxCollider>();
        if(box!=null)
        {
            box.size = new Vector3(m_StoryContent.localSize.x,m_StoryContent.localSize.y,0);
            box.center = m_StoryContent.localCenter;
        }
        m_StoryCVNameLabel.text = StrDictionary.GetDicByID(8278, CardTool.GetCardDefaultCVName(mCurChooseCard));
        if (tStory.GetBiographyVoicebyIndex(m_StoryIdx) != GlobeVar.INVALID_ID)
        {
            m_StoryPlaySoundPic.color = GlobeVar.NORMALCOLOR;
        }
        else
        {
            m_StoryPlaySoundPic.color = GlobeVar.GRAYCOLOR;
        }
    }

    void RefreshGroup()
    {
        if (m_GroupWrap==null)
        {
            return;
        }
        m_GroupWrap.UpdateAllItem();
        HandleOnGroupItemClick(mNowOpenGroupId);
        if(m_GroupTabRedPoint!=null && m_GroupTabRedPoint.activeSelf)
        {
            m_GroupTabRedPoint.SetActive(CollectionRedTool.IsCanAward_Card());
        }
    }

    void StopPlaySound()
    {
        if (GameManager.SoundManager.IsRealSoundPlaying())
        {
            GameManager.SoundManager.StopRealSound();
        }
    }

    public void HandleOnClickItem(int CurChooseCard)
    {
        StopPlaySound();
        if(m_TotalTab.GetHighlightTab()==null)
        {
            return;
        }
        switch(m_TotalTab.GetHighlightTab().name)
        {
            case "Info":
                mCurChooseCard = CurChooseCard;
                m_Wrap.UpdateAllItem();
                Tab_CardStory tStory = TableManager.GetCardStoryByID(mCurChooseCard, 0);
                if (tStory == null)
                    return;
                Tab_Card tCard = TableManager.GetCardByID(CurChooseCard, 0);
                if (tCard == null)
                {
                    return;
                }
                Tab_RoleBaseAttr tRoleBase = TableManager.GetRoleBaseAttrByID(tCard.GetRoleBaseIDStepbyIndex(0), 0);
                if (tRoleBase == null)
                {
                    return;
                }
                m_Poetry.text = tStory.Poem.Replace("#r", "\n");
                m_Intro.text = tStory.Intro.Replace("#r", "\n");
                m_Texture.Setup(tRoleBase.CharModelID);
                m_NameLabel.text = tRoleBase.Name;
                m_CVLabel.text = CardTool.GetCardDefaultCVName(CurChooseCard);
                m_RareIcon.spriteName = CardTool.GetRareIcon(tCard.Rare);
                SetSkill(tCard);
                m_StoryIdx = 0;
                RefreshStory();
                //看是否获取
                if (!GameManager.PlayerDataPool.CollectionData.IsGetCard(mCurChooseCard))
                {
                    m_BaseTex.color = GlobeVar.GRAYCOLOR;
                }
                else
                {
                    m_BaseTex.color = GlobeVar.NORMALCOLOR;
                }
                break;
            case "Directory":
                CardDynamicInfoWindow.Show(CurChooseCard);
                break;
        }
    }
    public void HandleOnGroupItemClick(int groupId)
    {
        if (groupId == GlobeVar.INVALID_ID)
        {
            return;
        }
        Tab_Handbook tab = TableManager.GetHandbookByID(groupId, 0);
        if(tab == null)
        {
            return;
        }
        if (m_GroupPreviewObj!=null)
        {
            m_GroupPreviewObj.SetActive(false);
        }
        if (m_GroupDeatilObj != null)
        {
            m_GroupDeatilObj.SetActive(true);
        }
        for (int i = 0;i<m_GroupFulingItem.Length;i++)
        {
            if(tab.getGroupIDCount()<=i || tab.GetGroupIDbyIndex(i) == GlobeVar.INVALID_ID)
            {
                m_GroupFulingItem[i].gameObject.SetActive(false);
                continue;
            }
            m_GroupFulingItem[i].gameObject.SetActive(true);
            m_GroupFulingItem[i].Refresh(tab.GetGroupIDbyIndex(i));
        }
        if (m_CurGroupNameLabel != null)
        {
            m_CurGroupNameLabel.text = tab.Group2Name;
        }
        foreach (GameObject obj in m_CurGroupRedPoint)
        {
            obj.SetActive(CollectionRedTool.IsCanAward_Handbook(tab));
        }
        if(GameManager.PlayerDataPool.CollectionData.CollectionGroupFinishList.AlreadyReceived.Contains(tab.Id))
        {
            if (m_AwardSG!=null)
            {
                m_AwardSG.ChangeState(0);
            }
        }
        else
        {
            if (m_AwardSG != null)
            {
                m_AwardSG.ChangeState(1);
            }
        }
        mNowOpenGroupId = tab.Id;
    }

    #region share
    public PaintNormalizer m_sharePainterNormalizer = null;
    public UITexture m_shareBgTexture = null;
    public bool m_shareHideBgAndPainter = true;
    public void OnShare()
    {
        m_shareBgTexture.gameObject.SetActive(true);

        if (null != IntimacyController.Instance() && null != IntimacyController.Instance().Card)
        {
            m_sharePainterNormalizer.Setup(IntimacyController.Instance().Card.GetCharModelId());
            m_sharePainterNormalizer.gameObject.SetActive(true);
        }
        if (null != CollectionIntimacyController.Instance)
        {
            int cardId = CollectionIntimacyController.Instance.mCurChooseCard;
            Tab_Card tCard = TableManager.GetCardByID(cardId, 0);
            if (tCard != null)
            {
                Tab_RoleBaseAttr tRoleBase = TableManager.GetRoleBaseAttrByID(tCard.GetRoleBaseIDStepbyIndex(0), 0);
                if (tRoleBase != null)
                {
                    m_sharePainterNormalizer.Setup(tRoleBase.CharModelID);
                    m_sharePainterNormalizer.gameObject.SetActive(true);
                }
            }
        }
        var shareType = ShareWindowControllor.ENUM_SHARETYPE.ENUM_SHARETYPE_CARDLETTER;
        var shareSetting = TableManager.GetShareSettingByID((int)shareType, 0);
        if (null == shareSetting)
        {
            LogModule.ErrorLog("OpenShareWindow error, invalid shareType:" + shareType);
            return;
        }

        var backGroundURL = ShareWindowControllor.GetShareBGBySetting(shareSetting);
        if (!string.IsNullOrEmpty(backGroundURL))
        {
            ulong uID;
            if (ulong.TryParse(backGroundURL, out uID) && uID > 0)
            {
                LoadTextureController.LoadTexture(uID, LoadTextureController.BucketType.Share, m_shareBgTexture, null, OnShareBgTextureLoaded, LoadTextureController.LoadImageStyle.ORIGINAL);
                return;
            }
        }
        OnShareBgTextureLoaded(true, null);
    }

    private void OnShareBgTextureLoaded(bool bSucc, string textureName)
    {
        ShareWindowControllor.Show(OnShareBegin, OnShareEnd, ShareWindowControllor.SHARECAMERA.SCREEN, ShareWindowControllor.ENUM_SHARETYPE.ENUM_SHARETYPE_CARDLETTER);
    }

    private bool[] m_isVisibleShareObjs;
    public GameObject[] m_shareHideList;
    private void OnShareBegin()
    {
        if (null != ShareWindowControllor.Instance)
        {
            ShareWindowControllor.Instance.BackGroundURL = "0";
            ShareWindowControllor.Instance.SetOnShareSuccessDelegate(OnShareFinished);
        }
    }

    private void OnShareFinished()
    {
        if (m_shareHideBgAndPainter)
        {
            m_shareBgTexture.gameObject.SetActive(false);
            m_sharePainterNormalizer.gameObject.SetActive(false);
        }
    }

    private void OnShareEnd()
    {
        AssetManager.SetTexture(m_sharePainterNormalizer.UIMainTexture, null);

        m_shareBgTexture.gameObject.SetActive(false);
        m_sharePainterNormalizer.gameObject.SetActive(false);
    }
    #endregion
}
