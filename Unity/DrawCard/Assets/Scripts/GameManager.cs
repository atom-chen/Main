using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 全局数据结构
/// </summary>

public class GameManager : MonoBehaviour 
{
    private static GameManager m_Instance;
    private  static List<People> m_Peoples = new List<People>();//存放全部人数的数据结构
    public static GameManager Instance()
    {
        return m_Instance;
    }
    void Awake()
    {
        m_Instance = this;
        TableManager.InitPeopleList();
        TableManager.ReadConfig();
    }
	void Start () 
    {
        DontDestroyOnLoad(this.gameObject);
	}
    public static void AddItemToList(People people)
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
    public static int GetDrawRange()
    {
        return m_Peoples.Count;
    }
    public static People GetPeople(int index)
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
	public static void RemoveAtList(List<People> list)
	{
		for (int i=0; i<list.Count; i++) 
		{
			People people=list[i];
            for (int j = 0; j < m_Peoples.Count;j++)
            {
                //移除
                if(m_Peoples[j].WorkID == people.WorkID && m_Peoples[j].Name == people.Name)
                {
                    m_Peoples.RemoveAt(j);
                    break;
                }
            }
		}
		Debug.Log (m_Peoples.Count);
	}

    /// <summary>
    /// 随机count个幸运儿返回给界面层
    /// </summary>
    /// <param name="count">幸运儿的个数</param>
    /// <returns>返回幸运儿集合</returns>
    ///         
    static List<People> luckBoy = new List<People>();
    public static List<People> GetLuckBoys(int count)
    {
        luckBoy.Clear();
        int curListSize = GetDrawRange();
        //调整抽奖值
        if (count > curListSize)
        {
            count = curListSize;
        }
        //在curListSize范围内开始抽奖
        for (int i = 0; i < count; )
        {
            int LuckIndex = Random.Range(0, curListSize);
            //取出该位置的员工
            People people = GameManager.GetPeople(LuckIndex);
            if (people == null)
            {
                continue;
            }
            //如果又是这个幸运儿
            if (luckBoy.Contains(people))
            {
                continue;
            }
            //添加到幸运儿集合
            luckBoy.Add(people);
            i++;
        }
        return luckBoy;
    }

    public static void Stop(List<People> luckBoy,DRAW_TYPE type)
    {
        //通知逻辑层移除已经中奖的幸运同学...
        GameManager.RemoveAtList(luckBoy);
        //写入文件
        TableManager.RememberName(luckBoy, type);
    }
}



















