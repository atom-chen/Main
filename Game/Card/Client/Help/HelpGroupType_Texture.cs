using Games;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelpGroupType_Texture : MonoBehaviour 
{
    public UITexture m_Texture;
    public void Show(string path)
    {
        this.gameObject.SetActive(true);
        AssetManager.SetTexture(m_Texture, path);
    }

}
