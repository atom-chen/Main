using UnityEngine;
using System.Collections;

public class UIHelpLauncher : MonoBehaviour
{
    [SerializeField]
    private int m_HelpID;

	public void OnShowHelp ()
    {
        UIManager.ShowUI(UIInfo.HelpRoot, OnUIHelpShow, m_HelpID);
    }

    void OnUIHelpShow(bool bSuccess, object param)
    {
        if (bSuccess && UIHelpController.Instance() != null)
            UIHelpController.Instance().SetContent(m_HelpID);
    }
}
