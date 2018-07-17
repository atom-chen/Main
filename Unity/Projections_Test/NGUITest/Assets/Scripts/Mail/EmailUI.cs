using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmailUI : MonoBehaviour
{

    public GameObject m_ItemPrefabs;
    public GameObject m_ScrollView;
    public List<EmailItem> m_Items = new List<EmailItem>();
    public UIEventTrigger m_Mask;
    void Start()
    {
        m_Mask.onClick.Add(new EventDelegate(DestoryMail));
    }
    void OnEnable()
    {
        ShowItems();
    }
    public void ShowItems()
    {
        UIEventListener listener;
        for (int i = 0; i < 50; i++)
        {
            listener = null;
            EmailItem item = NGUITools.AddChild(m_ScrollView, m_ItemPrefabs).GetComponent<EmailItem>();
            item.transform.localPosition = new Vector3(0, 105 - i * 90, 0);
            if (item != null)
            {
                item.InitEmailItem("", i.ToString(), DateTime.Now.ToString());
                m_Items.Add(item);

                listener = UIEventListener.Get(item.GetComponentInChildren<UIEventTrigger>().gameObject);
                if (listener != null)
                {
                    listener.onClick += OnItemDestory;
                }
            }
        }
    }

    //点击删除时的回调
    private void OnItemDestory(GameObject click)
    {
        EmailItem emailItem = click.transform.parent.GetComponent<EmailItem>();
        if (emailItem != null)
        {
            m_Items.Remove(emailItem);
            Destroy(click.transform.parent.gameObject);//销毁
            ReSetItemPos();
        }
    }

    //重排列
    private void ReSetItemPos()
    {
        for (int i = 0; i < m_Items.Count; i++)
        {
            m_Items[i].transform.localPosition = new Vector3(0, 105 - i * 90, 0);
        }
    }
    private void DestoryMail()
    {
        Destroy(this.gameObject);
    }
}
