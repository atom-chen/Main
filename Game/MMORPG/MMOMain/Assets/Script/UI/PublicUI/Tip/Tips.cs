using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tips
{
  private static List<TipItem> m_Tips = new List<TipItem>();

  public static void ShowTip(string tip)
  {
    GameObject obj = UIManager.CreateUI("tip", UIManager.PopUI);
    obj.transform.localPosition += new Vector3(0, m_Tips.Count * 45, 0);
    if (obj != null)
    {
      TipItem item = obj.GetComponent<TipItem>();
      item.Init(tip);
      m_Tips.Add(item);
    }

  }

  public static void OnItemDestory(TipItem item)
  {
    m_Tips.Remove(item);
  }



}
