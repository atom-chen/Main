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
    //发包获取当前服务器下角色信息
    m_Exit.onClick.Add(new EventDelegate(LaunchSceneLogic.Instance.SwitchToStartMenu));
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
