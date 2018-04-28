using UnityEngine;
using System.Collections;

public class TerrainManager : MonoBehaviour {
    public float m_FloorWidth = 40.0f;
    public const float m_FloorNum = 3;
    public Transform[] m_FloorObj = new Transform[3];
    private int m_LastFloorIndex = 0;//最后一块板的下标

    public float m_MidPos=-1;
    private static TerrainManager _Instance;
    public static TerrainManager Instance
    {
        get
        {
            return _Instance;
        }
    }


    void Awake()
    {
        _Instance = this;
    }
    void Start()
    {
        m_MidPos = m_FloorObj[2].position.x;
    }
    public void FloorMove()
    {
        
       
        m_FloorObj[m_LastFloorIndex].position = new Vector3(m_FloorWidth * m_FloorNum + m_FloorObj[m_LastFloorIndex].position.x, m_FloorObj[m_LastFloorIndex].position.y, m_FloorObj[m_LastFloorIndex].position.z);
        //计算新的中点，为最后一块板的下标
        m_MidPos = m_FloorObj[m_LastFloorIndex].position.x;

        m_LastFloorIndex++;
        if (m_LastFloorIndex >= m_FloorNum)
        {
            m_LastFloorIndex = 0;
        }

    }
}
