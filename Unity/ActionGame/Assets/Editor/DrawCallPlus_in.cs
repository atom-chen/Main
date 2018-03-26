using UnityEngine;
using UnityEditor;
using System.Collections;
/*
 * 功能：调整所有UI物体层级，但不会使它们的遮盖顺序改变
 */
class Pair
{
    public int TopMin { get; set; }
    public int BottomMax { get; set; }
    public Pair(int topMin, int bottomMax)
    {
        // TODO: Complete member initialization
        this.TopMin = topMin;
        this.BottomMax = bottomMax;
    }
}

public class DrawCallPlus_in : ScriptableWizard
{
    //变量定义
    [MenuItem("Tools/Adjust DrawCall Depth")]
    /*
    static void CreateWindow()
    {
        Debug.Log("方法被调用");
        //拿到所有UI控件
        GameObject[] UIArray = GameObject.FindGameObjectsWithTag("UI");
        //进行两两比较
        for(int i=0;i<UIArray.Length;i++)
        {
            for(int j=i;j<UIArray.Length;j++)
            {
                //如果他们的MTS相同
                if (CompareMTS(UIArray[i], UIArray[j]))
                {
                    //则尝试去调整他们的层级
                    AdjustDepth(UIArray[i], UIArray[j]);
                }
            }
        }
    }
     */
    static void CreateWindow()
    {
        Debug.Log("方法被调用");
        //拿到所有UI控件
        GameObject[] UIArray = GameObject.FindGameObjectsWithTag("UI");
        for(int i=0;i<UIArray.Length;i++)
        {

        }
    }
    //对比obj1和obj2的Material、Texture、shader是否相同
    private static bool CompareMTS(GameObject obj1,GameObject obj2)
    {
        UISprite sprite1 = obj1.GetComponent<UISprite>();
        UISprite sprite2 = obj2.GetComponent<UISprite>();
        if(sprite1.material==sprite2.material && sprite1.mainTexture==sprite2.mainTexture && 
            sprite1.shader==sprite2.shader)
        {
            return true;
        }
        return false;
    }
    //获取所有和obj发生遮罩的面板
    private static GameObject[] getMaskUI(GameObject[] allUI,GameObject obj)
    {
        return null;
    }

    //拿到原本在Depth上面的 但最小的
    //拿到原本在Depth下面的 但最大的
    private Pair getMaskDepth(GameObject[] maskUI,GameObject obj)
    {
        int m_bottomMax = int.MinValue;
        int m_topMin = int.MaxValue;
        UISprite m_sp1=obj.GetComponent<UISprite>();
        for (int i = 0; i < maskUI.Length;i++)
        {
            UISprite m_maskSprite=maskUI[i].GetComponent<UISprite>();
            //如果当前比obj大 走这个逻辑
            if(m_maskSprite.depth>m_sp1.depth)
            {
                //如果是最小的，则更新
                if(m_maskSprite.depth<m_topMin)
                {
                    m_topMin = m_maskSprite.depth;
                }
            }
            //如果比当前obj的深度小 走这个逻辑
            else if(m_maskSprite.depth<m_sp1.depth)
            {
                //如果是最大的 则更新
                if(m_maskSprite.depth>m_bottomMax)
                {
                    m_bottomMax = m_maskSprite.depth;
                }
            }
        }
        Pair pair = new Pair(m_topMin, m_bottomMax);
        return pair;
    }
    private bool isMask(GameObject obj1,GameObject obj2)
    {
        //做叉乘
        return false;
    }
    

    //alter obj1.depth to obj2.depth 
    private static bool AdjustDepth(GameObject obj1, GameObject obj2, int m_TopMin = int.MinValue, int m_BottomMax = int.MaxValue)
    {
        //基准的深度
        int dep1 = obj1.GetComponent<UISprite>().depth;
        //想要移动到基准的深度
        int dep2 = obj2.GetComponent<UISprite>().depth;
        //如果已经相邻
        if(dep1==dep2 || (dep1+1)==dep2 || (dep1-1==dep2))
        {
            return true;
        }
        //如果取值范围在中间
        if (m_TopMin <= dep1 && m_BottomMax >= dep1)
        {
            obj2.GetComponent<UISprite>().depth = dep1;
            return true;
        }
        //如果可以取上确界
        else if (dep1 - 1 == m_BottomMax)
        {
            obj2.GetComponent<UISprite>().depth = dep1 - 1;
            return true;
        }
        //如果可以取下确界
        else if (dep1 + 1 == m_BottomMax)
        {
            obj2.GetComponent<UISprite>().depth = dep1 + 1;
            return true;
        }
        //不做调整，因为会影响原UI布局
        return false;
    }


}
