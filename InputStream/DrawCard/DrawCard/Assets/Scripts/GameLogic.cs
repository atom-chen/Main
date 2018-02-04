using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 全局数据结构
/// </summary>

public class GameLogic : MonoBehaviour {
    private static GameLogic m_Instance;
    private  static List<People> m_Peoples = new List<People>();//存放全部人数的数据结构
    public static GameLogic Instance()
    {
        return m_Instance;
    }
    void Awake()
    {
        m_Instance = this;
    }
	void Start () {
        TableManager.InitPeopleList();
        DontDestroyOnLoad(this.gameObject);
	}
    public void AddItemToList(People people)
    {
        if(m_Peoples==null)
        {
            return;
        }
        if (m_Peoples.Contains(people)==false)
        {
            m_Peoples.Add(people);
        }
    
    }
    public void RemoveAtList(int index)
    {
        if(index<m_Peoples.Count)
        {
            m_Peoples.RemoveAt(index);
        }    
    }
    public void RemoveItemAtList(People item)
    {
        m_Peoples.Remove(item);
    }
    public int GetDrawRange()
    {
        return m_Peoples.Count-1;
    }
    public People GetPeople(int index)
    {
        if(index>=m_Peoples.Count)
        {
            return null;
        }
        else
        {
            return m_Peoples[index];
        }
    }
	public void RemoveAtList(List<People> list)
	{
		for (int i=0; i<list.Count; i++) 
		{
			People people=list[i];
			if(m_Peoples.Contains(people))
			{
				//移除
				m_Peoples.Remove(people);
			}
		}
		Debug.Log (m_Peoples.Count);
	}



    
}



















