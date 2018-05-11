using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoleSelectLogic : MonoBehaviour {
  private static RoleSelectLogic _Instance;
  public static RoleSelectLogic Instance
  {
    get
    {
      return _Instance;
    }
  }

  public UIButton m_Exit;
  public List<RoleSelectItem> m_Items;

  void Awake()
  {
    _Instance = this;
  }

  void OnEnable()
  {
    m_Exit.onClick.Add(new EventDelegate(LaunchSceneLogic.Instance.SwitchToStartMenu));
  }
  public void Init(List<Role> role)
  {
    if(role==null)
    {
      for (int i = 0; i < m_Items.Count; i++)
      {
          m_Items[i].Init(null, false);
      }
      return;
    }
    //用角色列表去初始化
    for(int i=0;i<m_Items.Count;i++)
    {
      if(i<role.Count)
      {
        m_Items[i].Init(role[i], i == 0);
      }
      else
      {
        m_Items[i].Init(null, false);
      }
    }
  }

  public  void OnClickCreateRole()
  {
    //新建角色
    LaunchSceneLogic.Instance.SwitchToCreateRoleMenu();
  }

  //点击选择角色
  public void OnClickRole(RoleSelectItem role)
  {
    foreach(var item in m_Items)
    {
      if(item==role)
      {
        item.Select(true);
      }
      else
      {
        item.Select(false);
      }
    }
  }


}
