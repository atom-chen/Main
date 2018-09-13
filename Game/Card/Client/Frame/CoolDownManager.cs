/********************************************************************************
 *	文件名：	CoolDownManager.cs
 *	全路径：	\Script\GlobalSystem\Manager\CoolDownManager.cs
 *	创建人：	李嘉
 *	创建时间：2016-12-28
 *
 *	功能说明：游戏全局CD管理器，所有的冷却都可以记录在这里
 *	         所有的CD都用一个结构记录
 *	         当为false的时候，不会进行Tick
 *	         当为true的时候，会进行Tick，剩余时间结束之后，则重新变为false
 *	修改记录：
*********************************************************************************/
using UnityEngine;
using System.Collections;

using Games.GlobeDefine;
public class CoolDownManager : MonoBehaviour
{
    class CoolDown
    {
        public bool bCDValue = false;           //冷却项数值
        public float fEclipseTime = 0.0f;      //剩余恢复时间

        public void Reset()
        {
            bCDValue = false;
            fEclipseTime = 0.0f;
        }
    }

    private CoolDown[] m_CoolDownList = new CoolDown[(int)COOLDOWN_TYPE.MAX];
    private static CheckDiffDay m_DiffDayChecker = new CheckDiffDay();
    void Awake()
    {
        ResetCoolDown();
    }

	void Update()
	{
		//由于CDManager会在一开始就创建，并且不随游戏销毁，所以全局的数据Tick可以写在这里
		if (GameManager.PlayerDataPool.StaminaNextRecovrTime > 0)
		{
			GameManager.PlayerDataPool.StaminaNextRecovrTime -= Time.deltaTime;
		}
        //跨天检测
        m_DiffDayChecker.Tick();
        //帮会邀请过期检测
        if (GameManager.PlayerDataPool.GuildData != null)
        {
            GameManager.PlayerDataPool.GuildData.Tick();
        }
    }

    void FixedUpdate()
    {
        for (int i = 0; i < m_CoolDownList.Length; ++i)
        {
            if (null != m_CoolDownList[i] && true == m_CoolDownList[i].bCDValue)
            {
                if (m_CoolDownList[i].fEclipseTime <= Time.time)
                {
                    m_CoolDownList[i].Reset();
                }
            }
        }
    }

    public void ResetCoolDown()
    {
        for (int i = 0; i < m_CoolDownList.Length; ++i)
        {
            m_CoolDownList[i] = new CoolDown();
        }
    }
    public void ResetCoolDown(COOLDOWN_TYPE type)
    {
        int nType = (int)type;
        if (nType < 0 || nType >= (int)COOLDOWN_TYPE.MAX)
        {
            return;
        }
        if (m_CoolDownList[nType] != null)
        {
            m_CoolDownList[nType].Reset();
        }
    }
    //设置CD，第一个参数为类型，第二个参数为时间（秒）
    public bool SetCoolDown(COOLDOWN_TYPE type, float fCoolDown)
    {
        int nType = (int)type;
        if (nType < 0 || nType >= (int)COOLDOWN_TYPE.MAX)
        {
            return false;
        }

        if (fCoolDown <= 0.01f)
        {
            return false;
        }

        if (m_CoolDownList[nType] != null)
        {
            m_CoolDownList[nType].bCDValue = true;
            m_CoolDownList[nType].fEclipseTime = Time.time + fCoolDown;
        }
        return false;
    }

    public bool GetCoolDown(COOLDOWN_TYPE type)
    {
        int nType = (int)type;
        if (nType < 0 || nType >= (int)COOLDOWN_TYPE.MAX)
        {
            return false;
        }

        if (m_CoolDownList[nType] != null)
        {
            return m_CoolDownList[nType].bCDValue;
        }

        return false;
    }

    //获取某个CD剩余的时间
    public float GetCoolDownEclipseTime(COOLDOWN_TYPE type)
    {
        int nType = (int)type;
        if (nType < 0 || nType >= (int)COOLDOWN_TYPE.MAX)
        {
            return 0.0f;
        }

        if (m_CoolDownList[nType] != null)
        {
            return m_CoolDownList[nType].fEclipseTime - Time.time;
        }

        return 0.0f;
    }

}

public class CheckDiffDay
{
    public const int INTERVAL_CHECKDIFFDAY = 1; //检测跨天间隔
    private float m_fIntervalTime = 0;
    private System.DateTime m_DateLast;
    public void Tick()
    {
        //没有关注跨天的，不用检查
        if (UIDelegate.OnDiffDay00 == null)
        {
            return;
        }
        m_fIntervalTime += Time.deltaTime;
        if ( m_fIntervalTime < INTERVAL_CHECKDIFFDAY)
        {
            return;
        }

        m_fIntervalTime = 0;
        System.DateTime curDate = Games.Utils.GetServerDateTime();
        if (IsDiffDay(m_DateLast, curDate))
        {
            UIDelegate.OnDiffDay00();
            m_DateLast = curDate;
        }

    }


    public bool IsDiffDay(System.DateTime left, System.DateTime right)
    {
        if (left.Year!= right.Year || left.DayOfYear !=right.DayOfYear)
        {
            return true;
        }
        return false;
    }
}
