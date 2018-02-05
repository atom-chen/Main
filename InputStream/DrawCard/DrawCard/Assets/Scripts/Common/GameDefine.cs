using System;
using System.Collections.Generic;
using UnityEngine.WSA;

class GameDefine
{
    public const string TablePath = "C:\\MyWorkSpace\\InputStream\\DrawCard\\DrawCard\\p4.txt";
    public const int Total_Three = 88;
    public const int Total_Two = 50;
    public const int Total_One = 30;
    public const int Total_Best = 20;
}

class SceneName
{
    
    public const string Order = "GameOrder";
    public const string Free = "GameFree";
    public const string Main = "Menu";
}
public class People
{
    public string WorkID; //员工号
    public string Name;//姓名
    public string ProjectGroup;//所属项目组
    public string Department;//所属部门
    public string WorkType;//员工类型
    public string IsDraw;//是否已经抽过了
    public People()
    {
        WorkID = "0";
        Name = "0";
        ProjectGroup = "0";
        Department = "0";
    }
    public People(string workID,string name,string projectGroup="0",string department="0")
    {
        this.WorkID = workID;
        this.Name = name;
        this.ProjectGroup = projectGroup;
        this.Department = department;
    }
    
}

