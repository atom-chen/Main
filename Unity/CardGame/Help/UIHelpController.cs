using UnityEngine;
using System.Collections;
using Games.Table;

public class UIHelpController : UIControllerBase<UIHelpController>
{
    [SerializeField]
    UILabel m_lblContent;
    [SerializeField]
    UIButton m_btnClose;

    void Awake ()
    {
        SetInstance(this);
	}

    public void SetContent(int helpID)
    {
        Tab_HelpContent hc = TableManager.GetHelpContentByID(helpID, 0);
        if (hc != null)
        {
            string content = hc.Content.Replace("#r", "\n");
            m_lblContent.text = content;
            m_lblContent.width = hc.Width;
            m_lblContent.height = hc.Height;
            Vector3 pos = Vector3.zero;
            pos.x = hc.PosX;
            pos.y = hc.PosY;
            m_lblContent.transform.localPosition = pos;
        }
        else
        {
            m_lblContent.text = string.Format("help contend id {0} not exists", helpID);
            m_lblContent.width = 500; // 给个默认大小
            m_lblContent.height = 300;
        }
    }

    public void OnCloseHelp()
    {
        UIManager.CloseUI(UIInfo.HelpRoot);
    }
}
