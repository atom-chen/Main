using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmailUI : MonoBehaviour {

  public GameObject m_ItemPrefabs;
  public GameObject m_ScrollView;
  public List<EmailItem> m_Items;
  void OnEnable()
  {
    ShowItems();
  }
  public void ShowItems()
  {
    for(int i=0;i<50;i++)
    {
      EmailItem item = NGUITools.AddChild(m_ScrollView, m_ItemPrefabs).GetComponent<EmailItem>();
      item.gameObject.transform.localPosition = new Vector3(0, 105 - i * 90, 0);
      if(item!=null)
      {
        item.InitEmailItem("",i.ToString());
        m_Items.Add(item);
        item.m_RemoveBtn.onClick.Add(new EventDelegate(OnItemDestory));
      }
    }

  }


  public void OnItemDestory()
  {

  }
}
