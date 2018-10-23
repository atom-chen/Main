using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Games.Table;
public class HelpGroupType_Content : MonoBehaviour 
{
    public UILabel m_ContentLabel;

    public void Show(string content)
    {
        if(!string.IsNullOrEmpty(content))
        {
            this.gameObject.SetActive(true);
            content = content.Replace("#r", "\n");
            m_ContentLabel.text = content;
        }
    }

    public void Show(int helpContentId)
    {
        Tab_HelpContent tabHelpContent = TableManager.GetHelpContentByID(helpContentId, 0);
        if(tabHelpContent!=null)
        {
            this.gameObject.SetActive(true);
            string content = tabHelpContent.Content.Replace("#r", "\n");
            m_ContentLabel.text = content;
        }

    }
}
