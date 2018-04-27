using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
 * 
 * 游戏管理器
 */ 
public class GameManager : MonoBehaviour {
    public UILabel m_Point;
    public GameObject m_Mask;
    private float m_MaxPoint = 50;
    private float m_NowPoint = 0;
    public UILabel m_LifeText;
    private int m_Life = 3;
    private float m_fireCD = 0.25f;//攻击CD
    private float m_nextFire = 0;//已累计时间
    public AudioSource m_Audio;

    private int m_PointJianshao = 20;
    private bool m_IsGame = false;


    public PlayerController m_Player;
    public static GameManager Instance
    {
        get
        {
            return _Instance;
        }
    }
    private static  GameManager _Instance;
    void Awake()
    {
        _Instance = this;
        m_IsGame = true;
    }

    public void KillEnemy(float cd)
    {
        m_NowPoint += m_MaxPoint - 10 * cd;
        m_Point.text = m_NowPoint + "";
    }

    public void EnemyOK()
    {
        m_NowPoint -= m_PointJianshao;
        m_Point.text = m_NowPoint + "";
    }

    public void KillPlayer()
    {
        m_Life--;
        m_LifeText.text = m_Life + "";
        EnemyPool.Instance.ReSetCD();
        if (m_Life <= 0)
        {
            //游戏结束
            GameEnd();
        }
    }

    void Update()
    {
        m_nextFire += Time.deltaTime;
        if (Input.GetButton("Fire1") && m_nextFire >= m_fireCD)
        {
            m_nextFire = 0;
            BoltPool.Instance.GetBolt();
            m_Audio.Play();
        }
    }

    public float GetCd()
    {
        return m_fireCD;
    }

    //游戏结束：隐藏Player、敌人池、子弹池
    private void GameEnd()
    {
        m_Player.gameObject.SetActive(false);
        EnemyPool.Instance.gameObject.SetActive(false);
        BoltPool.Instance.gameObject.SetActive(false);
        m_Mask.SetActive(true);
    }

    
    public void ReSetGame()
    {
        m_Mask.SetActive(false);
        m_Player.gameObject.SetActive(true);
        EnemyPool.Instance.gameObject.SetActive(true);
        BoltPool.Instance.gameObject.SetActive(true);
        m_NowPoint = 0;
        m_Point.text = m_NowPoint + "";
        m_Life = 3;
        m_LifeText.text = m_Life + "";
        m_IsGame = true;
    }
}
