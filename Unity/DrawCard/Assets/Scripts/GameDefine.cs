using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;


public enum DRAW_TYPE
{
    INVALID = -1,
    BEST = 0,
    ONE = 1,
    TWO = 2,
    THREE = 3,
    FREE = 4,
}

class GameDefine
{
    private static string FileRoot = Application.absoluteURL;
    public static string TablePath = FileRoot + "NameList.txt";
    public static string RememberPath = FileRoot + "Remember.txt";
    public static string ConfigPath = FileRoot + "Config.ini";
    public static int Total_Three = 0;
    public static int ThreeEveryDraw = 0;
    public static int ThreeAlreadyDraw = 0;

    public static int Total_Two = 0;
    public static int TwoEveryDraw = 0;
    public static int TwoAlreadyDraw = 0;

    public static int Total_One = 0;
    public static int OneEveryDraw = 0;
    public static int OneAlreadyDraw = 0;

    public static int Total_Best = 0;
    public static int BestEveryDraw = 0;
    public static int BestAlreadyDraw = 0;

    public const int PageMax = 12;
}
public class People
{
    public string WorkID; //员工号
    public string Name;//姓名
    public string ProjectGroup;//所属项目组
    public string Department;//所属部门
    public People()
    {
        WorkID = "0";
        Name = "0";
        ProjectGroup = "0";
        Department = "0";
    }
    public People(string workID, string name, string projectGroup = "0", string department = "0")
    {
        this.WorkID = workID;
        this.Name = name;
        this.ProjectGroup = projectGroup;
        this.Department = department;
    }
    
}

