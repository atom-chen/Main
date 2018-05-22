using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
  public UIButton m_EnterGame;

  private int m_NowIndex = -1;

  void Awake()
  {
    _Instance = this;
  }
  void Start()
  {
    m_EnterGame.onClick.Add(new EventDelegate(OnClickEnterGame));
  }
  void OnEnable()
  {
    m_Exit.onClick.Add(new EventDelegate(LaunchSceneLogic.Instance.SwitchToStartMenu));
  }
  public void Init(List<Role> role)
  {
    m_NowIndex = 0;
    //角色为空
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
      //此角色不为空
      if(i<role.Count)
      {
        
        m_Items[i].Init(role[i], i == 0);
      }
      //此角色为空
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

  //进入游戏
  private void OnClickEnterGame()
  {
    if(m_NowIndex<PlayData.RoleList.Count)
    {
      //发送进入游戏的包
      Role role=PlayData.RoleList[m_NowIndex];
      Dictionary<byte, object> dic = new Dictionary<byte, object>();
      dic.Add((byte)ParameterCode.Role, ParaTools.GetJson<Role>(role));
      if (PhotoEngine.Instance != null)
      {
        PhotoEngine.Instance.SendRequest(OperationCode.StartGame, dic);
      }
    }
  }

  //点击选择角色
  public void OnClickRole(RoleSelectItem role)
  {
    for (int i = 0; i < m_Items.Count;i++ )
    {
      RoleSelectItem item = m_Items[i];
      if (item == role)
      {
        m_NowIndex = i;
        item.Select(true);
      }
      else
      {
        item.Select(false);
      }
    }
  }


}
