using Games;
using ProtobufPacket;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Games.GlobeDefine;

public class HouseNoteController : CardlinkSenderBase
{
    private static HouseNoteController _Ins = null;
    public static HouseNoteController Instance
    {
        get { return _Ins; }
    }
    public static void Show()
    {
        UIManager.ShowUI(UIInfo.HouseNoteRoot);
    }

    public void Close()
    {
        UIManager.CloseUI(UIInfo.HouseNoteRoot);
        HouseScene hs = GameManager.CurScene as HouseScene;
        if (hs != null && hs.IsOwner(LoginData.user.guid))
        {
            //发一个已阅的包
            CG_YARD_NOTE_ALREADY_READ_PAK pak = new CG_YARD_NOTE_ALREADY_READ_PAK();
            pak.SendPacket();
        }
    }
    void Awake()
    {
        _Ins = this;
        HouseScene.OnNoteUpdate += Refresh;
    }
    void OnDestroy()
    {
        _Ins = null;
        HouseScene.OnNoteUpdate -= Refresh;
    }
    //主界面
    public UILabel m_YardNameLabel;
    public UILabel m_YardSignatureLabel;
    public UISprite m_OwnerIcon;
    public GameObject m_SettingBtnObj;
    public ListWrapController m_Wrap;
    public GameObject m_EmptyObj;
    public TweenPosition m_RootTween;
    public UILabel m_LabelTips;

    //其他界面
    public GameObject m_SettingWindowObj;
    public UIInput m_SettingNameInput;
    public UIInput m_SettingSignatureInput;

    public HouseNoteItem m_DetailWindow;

    public EmojTween2 m_EmojTween;
    public GameObject m_EmojRootObj;
    public GameObject ExpressionListCloseMask;
    private List<_YardNote> infoList = null;
    bool m_bFirst = true;
    private Vector3 from;
    private Vector3 to;
    bool isUp = false;
    void OnEnable()
    {
        m_bFirst = true;

        Refresh();
        OnClickCloseSetting();
        OnClickCloseDetailWindow();
    }
    void Start()
    {
        input.delOnSelect += delOnSelect;
        input.delOnDeselect += delOnDeselect;
        from = m_RootTween.from;
        to = m_RootTween.to;
    }
    void Refresh()
    {
        HouseScene hs = HouseTool.GetHouseScene();
        if (hs == null)
        {
            Close();
            return;
        }
        infoList = hs.GetSortedNote();
        if (infoList == null || infoList.Count <= 0)
        {
            m_EmptyObj.SetActive(true);
            m_Wrap.gameObject.SetActive(false);
        }
        else
        {
            m_Wrap.gameObject.SetActive(true);
            m_EmptyObj.SetActive(false);
            if (m_bFirst)
            {
                m_Wrap.InitList(infoList.Count, OnNoteWrapUpdate);
                m_bFirst = false;
            }
            else
            {
                m_Wrap.UpdateItemCount(infoList.Count, true);
            }

        }
        if (string.IsNullOrEmpty(hs.YardName))
        {
            m_YardNameLabel.text = StrDictionary.GetDicByID(8629, LoginData.user.name);
        }
        else
        {
            m_YardNameLabel.text = hs.YardName;
        }
        if (string.IsNullOrEmpty(hs.YardSignature))
        {
            m_YardSignatureLabel.text = StrDictionary.GetDicByID(8630);
        }
        else
        {
            m_YardSignatureLabel.text = hs.YardSignature;
        }
        m_OwnerIcon.spriteName = Utils.GetIconStrByID(hs.GetOwnerIcon(), true);
    }

    void OnNoteWrapUpdate(GameObject obj, int idx)
    {
        if (obj == null || infoList == null || idx >= infoList.Count || idx < 0)
        {
            return;
        }
        HouseNoteItem item = obj.GetComponent<HouseNoteItem>();
        if (item == null)
        {
            return;
        }
        _YardNote info = infoList[idx];
        if(info == null)
        {
            return;
        }
        item.Refresh(info);
    }

    //点击设置
    public void OnClickSetting()
    {
        HouseScene hs = HouseTool.GetHouseScene();
        if (hs != null)
        {
            m_SettingWindowObj.SetActive(true);
            m_SettingNameInput.value = hs.YardName;
            m_SettingSignatureInput.value = hs.YardSignature;
        }
    }

    //点击表情
    public void OpenEmoj()
    {
        m_EmojRootObj.SetActive(true);
        m_EmojTween.OnBegin();
        ExpressionListCloseMask.SetActive(!ExpressionListCloseMask.activeSelf);
    }


    public void ChooseEmoj()
    {
        if (input.value.Length > GlobeVar.EMOJINPUTMAXCOUNT)//保证表情不能太多而导致报错
        {
            return;
        }

        Transform _labelTrans = input.label.transform;
        if (null == _labelTrans)
        {
            return;
        }

        int emojCount = _labelTrans.childCount;
        if (emojCount != 0)
        {
            for (int i = 0; i < emojCount; i++)
            {
                Destroy(_labelTrans.GetChild(i).gameObject);
            }
        }
        if (null == UICamera.currentTouch || null == UICamera.currentTouch.current)
        {
            return;
        }

        UISprite spriteTmp = UICamera.currentTouch.current.GetComponent<UISprite>();//选择表情 得到表情的名字
        if (null == spriteTmp)
        {
            return;
        }
        if (string.IsNullOrEmpty(spriteTmp.spriteName))
        {
            return;
        }
        var emojName = spriteTmp.spriteName;
        string emojStr = "&" + emojName;
        input.value = string.Format("{0}{1}", input.value, emojStr);

        if (!ChatEmotionPage.IsNotBigEmojName(emojName))
        {
            input.value = "";
            return;
        }

        Talk_ListWindow.SaveEmojHistory(emojName);//存储历史表情
    }

    //点击发送
    public void OnClickSend()
    {
        if (string.IsNullOrEmpty(input.value))
        {
            return;
        }
        //如果过长，则阻止
        if (input.value.Length > GlobeVar.YARD_NOTE_MAX_LENGTH)
        {
            Utils.CenterNotice(StrDictionary.GetDicByID(5331));
            return;
        }
        CG_YARD_NOTE_ADD_PAK pak = new CG_YARD_NOTE_ADD_PAK();
        _YardNote note = new _YardNote();
        note.content = input.value;
        pak.data.info = note;
        if (m_linkObjs.objs.Count > 0)
        {
            pak.data.info.linkObjs = m_linkObjs;
        }
        pak.SendPacket();
        input.value = "";
        m_linkObjs.objs.Clear();
    }


    public void OnClickClear()
    {
        input.value = "";
        m_linkObjs.objs.Clear();
    }

    public void OnClickCommitSetting()
    {
        if (string.IsNullOrEmpty(m_SettingNameInput.value))
        {
            Utils.CenterNotice(8635);
            return;
        }
        //如果过长，则阻止
        if (m_SettingNameInput.value.Length > GlobeVar.YARD_NAME_MAX)
        {
            Utils.CenterNotice(8636);
            return;
        }
        //如果过长，则阻止
        if (m_SettingSignatureInput.value.Length > GlobeVar.YARD_SIGNATURE_MAX)
        {
            Utils.CenterNotice(8637);
            return;
        }
        //如果都一样 则不提交
        HouseScene hs = HouseTool.GetHouseScene();
        if(hs!=null)
        {
            if(hs.YardName == m_SettingNameInput.value && hs.YardSignature == m_SettingSignatureInput.value)
            {
                return;
            }
            CG_YARD_NAME_SIGNATURE_EDIT_PAK pak = new CG_YARD_NAME_SIGNATURE_EDIT_PAK();
            pak.data.yardName = m_SettingNameInput.value;
            pak.data.yardSignature = m_SettingSignatureInput.value;
            pak.SendPacket();
            OnClickCloseSetting();
        }
    }
    public void OnClickCloseSetting()
    {
        m_SettingSignatureInput.value = "";
        m_SettingNameInput.value = "";
        m_SettingWindowObj.SetActive(false);
    }
    private void delOnSelect()
    {
        m_RootTween.onFinished.Clear();
        TweenRoot();
    }

    private void delOnDeselect()
    {
        m_RootTween.AddOnFinished(OnDeselectTweenFinish);
        TweenRoot();
    }
    private void OnDeselectTweenFinish()
    {
        m_RootTween.onFinished.Clear();
    }
    public void TweenRoot()
    {
        if (isUp)
        {
            m_RootTween.ResetToBeginning();
            m_RootTween.from = to;
            m_RootTween.to = from;

        }
        else
        {
            m_RootTween.ResetToBeginning();
            m_RootTween.from = from;
            m_RootTween.to = to;
        }

        m_RootTween.PlayForward();

        isUp = !isUp;
    }
    //展示详情
    public void HandleShowDetail(HouseNoteItem item)
    {
        if(item == null || item.m_NoteData == null)
        {
            return;
        }
        m_DetailWindow.gameObject.SetActive(true);
        m_DetailWindow.Refresh(item.m_NoteData);
    }

    public void OnClickCloseDetailWindow()
    {
        m_DetailWindow.gameObject.SetActive(false);
    }

    public void OnInputChange()
    {
        if(string.IsNullOrEmpty(input.value))
        {
            m_LabelTips.gameObject.SetActive(true);
        }
        else
        {
            m_LabelTips.gameObject.SetActive(false);
        }
    }

}
