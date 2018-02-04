using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOrderController : MonoBehaviour {
    private static GameOrderController m_Instance;
    public static GameOrderController Instance()
    {
        return m_Instance;
    }
	// Use this for initialization
	void Awake () {
        m_Instance = this;
	}
	

    /// <summary>
    /// 随机count个幸运儿返回给界面层
    /// </summary>
    /// <param name="count">幸运儿的个数</param>
    /// <returns>返回幸运儿集合</returns>
    public List<People> GetLuckBoys(int count)
    {
        List<People> luckBoy = new List<People>();
        int curListSize = GameLogic.Instance().GetDrawRange();
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
            People people = GameLogic.Instance().GetPeople(LuckIndex);
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


    public void Begin()
    {
        //通知当前DrawGrid修改次数
        GameOrderView.Instance().m_CurTage.Begin();
        //更新Help显示
        GameOrderView.Instance().UpdateHelp();
    }
    public void Stop(List<People> luckBoy)
    {
        //通知逻辑层移除已经中奖的幸运同学...
        GameLogic.Instance().RemoveAtList(luckBoy);
    }

    public void  OnFreeUpdateCount(int count)
    {
        GameOrderView.Instance().m_PageFree.SetCount(count);
    }

}
