using Games;
using Games.Table;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIBossShowController : MonoBehaviour 
{
    public static int BossShowID;
    //private float closeTime = 20.0f;
    private UIBossShow m_UIShow;
    private UITweener m_Tween;
    private UITexture m_Texture;
    bool m_IsRevert = false;
    private Tab_BossShow tab_BossShow;
    void OnEnable()
    {
        tab_BossShow = TableManager.GetBossShowByID(BossShowID,0);
        if(tab_BossShow!=null)
        {
            Transform childTrans = transform.Find("UIBossShow");
            if (childTrans != null)
            {
                m_UIShow = childTrans.GetComponent<UIBossShow>();
                if (m_UIShow != null)
                {
                    m_UIShow.gameObject.SetActive(false);
                }
            }

            childTrans = transform.Find("UIBossInfo");
            if (childTrans != null)
            {
                //childTrans.gameObject.AddComponent<TweenAlpha>();
                m_Tween = childTrans.GetComponent<UITweener>();
                m_Texture = childTrans.GetComponentInChildren<UITexture>();
                m_Tween.onFinished.Add(new EventDelegate(OnClickBg));
            }
            m_Texture.mainTexture = Utils.LoadTexture(tab_BossShow.BossInfoTexture); /*AssetManager.LoadResource("Texture/Tv_Texture/Tv_Shanhui_01/1") as Texture;*/
            //m_Texture.transform.localPosition = new Vector3(-315, 27,0);
            StartCoroutine(ShowTween(tab_BossShow.TweenDelay));
        }
        
        //Invoke("OnClickBg", closeTime);
    }

    IEnumerator ShowTween(float delta)
    {
        yield return new WaitForSeconds(delta);
        //播放Tween
        if(m_Tween!=null)
        {
            m_Tween.PlayForward();
            m_IsRevert = false;
        }
        StartCoroutine(TweenEnd(tab_BossShow.Duration));
    }
    IEnumerator TweenEnd(float delta)
    {
        yield return new WaitForSeconds(delta);
        if (m_Tween != null)
        {
            m_Tween.PlayReverse();
            m_IsRevert = true;
        }
    }

    //废弃
    public void ShowUI()
    {
        m_UIShow.BossShowTabId = BossShowID;
        m_UIShow.gameObject.SetActive(true);
    }

    public void OnClickBg()
    {
        if(m_IsRevert)
        {
            StopAllCoroutines();
            UIManager.CloseUI(UIInfo.UIBossShow);
            //通知战斗恢复
            if (BattleController.CurBattleController != null)
            {
                BattleController.CurBattleController.ResumeBattle();
            }
        }
    }
    
}
