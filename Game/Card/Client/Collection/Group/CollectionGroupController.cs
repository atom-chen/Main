using System.Collections;
using System.Collections.Generic;
using Games;
using Games.Table;

//********************************************************************
// 描述: 图鉴/组合功能的控制器
// 作者: wangbiyu
// 创建时间: 2018-2-28
//
//********************************************************************
public enum COLLECTIONGROUPTYPE
{
    COLLECTIONGROUPTYPE_FULING = 0,
    COLLECTIONGROUPTYPE_TALISMAN = 1,
    COLLECTIONGROUPTYPE_STAR=2
}
public class Group1Item
{
    public string picPath="";
    public string title="";
    public int done=0;
    public int total=0;
}
public class CollectionGroupController{
    private static CollectionGroupController m_Instance=new CollectionGroupController();
    private static List<int> m_Id;//所有GroupID的集合
    public static CollectionGroupController Instance
    {
        get
        {
            return m_Instance;
        }
    }
    /// <summary>
    /// 拿到该类型的所有一级标题
    /// </summary>
    /// <param name="type">所属的图鉴分类</param>
    /// <returns>集合</returns>
    public List<Group1Item> GetGroup1NameInfo(COLLECTIONGROUPTYPE type)
    {
        if (m_Id == null || m_Id.Count == 0)
        {
            m_Id = new List<int>();
            Dictionary<int, List<Tab_Handbook>> AllHankBookDictionary = TableManager.GetHandbook();
            foreach (var item in AllHankBookDictionary)
            {
                m_Id.Add(item.Key);
            }
        }
        List<Group1Item> name1List = new List<Group1Item>();//返回数据
        for (int i = 0; i < m_Id.Count; i++)
        {
            //取出一条数据
            Tab_Handbook handBook = TableManager.GetHandbookByID(m_Id[i], 0);
            if (handBook == null)
            {
                continue;
            }
            //如果类型不同
            if (handBook.Style != (int)type)
            {
                continue;
            }
            //如果List中还未存在，已存在的逻辑在IsInList中处理
            if (!IsInList(name1List, handBook.Group1Name,handBook))
            {
                Group1Item groupItem = new Group1Item();
                groupItem.picPath = handBook.Group1Pic;
                groupItem.title = handBook.Group1Name;
                groupItem.total++;
                //如果已经完成了 则done++
                if (GameManager.PlayerDataPool.IsGroupAccess(handBook.Id))
                {
                    groupItem.done++;
                }
                name1List.Add(groupItem);
            }
        }
        return name1List;
    }

    private bool IsInList(List<Group1Item> name1List,string group1Name,Tab_Handbook handbook)
    {
        if(name1List==null)
        {
            return false;
        }
        for (int i = 0; i < name1List.Count;i++)
        {
            if(name1List[i].title.Equals(group1Name))
            {
                //如果这个组名已经出现过了，则记录数++
                name1List[i].total++;
                //如果已经完成了这个任务
                if (GameManager.PlayerDataPool.IsGroupAccess(handbook.Id))
                {
                    name1List[i].done++;
                }
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// 拿到该一级分类下的所有二级分类信息
    /// </summary>
    /// <param name="group1Name">一级分类名称</param>
    /// <returns>属于该一级分类的所有二级分类List条目</returns>
    public List<Tab_Handbook> GetGroup2NameInfo(string group1Name)
    {
        if(group1Name==null)
        {
            return null;
        }
        if (m_Id == null || m_Id.Count == 0)
        {
            m_Id = new List<int>();
            Dictionary<int, List<Tab_Handbook>> AllHankBookDictionary = TableManager.GetHandbook();
            foreach (var item in AllHankBookDictionary)
            {
                m_Id.Add(item.Key);
            }
        }
        List<Tab_Handbook> groupsInfo2 = new List<Tab_Handbook>();
        //取出所有数据 比较Name1
        for(int i=0;i<m_Id.Count;i++)
        {
            Tab_Handbook handbook = TableManager.GetHandbookByID(m_Id[i], 0);
            if(handbook!=null && handbook.Group1Name.Equals(group1Name))
            {
                groupsInfo2.Add(handbook);
            }
        }
        return groupsInfo2;
    }




    

    
    
}
