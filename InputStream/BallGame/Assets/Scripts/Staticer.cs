using UnityEngine;
using System.Collections;
/*
 * 通用常量类
 */

public class Staticer{
    public static Vector3 ballPos2 = new Vector3(0, 0.4f, 9.7f);
    public static string animationName = null;
    public static bool isPlay=false;
    //碰撞板
    public static bool isCrash = false;//是否与测试板碰撞
    public static Vector3 crashPoint = new Vector3();//与测试板碰撞的坐标
    //落点
    public const float endZ = 24;
    //球到达球门 时间
    public static float timeToGoal=0;
    //球穿过球门时的坐标
    public static Vector3 endPoint = new Vector3();
    //测试板与球门距离
    public const float distance = 7.4f;
    //是否已保存新位置
    public static bool isSave = false;
    //球门宽度
    public const float goalStartX = -2.526555f;//左下角
    public const float goalStartY = 0.2716197f;//Y坐标
    public const float goalEndX = 1.875407f;//右上角
    public const float goalEndY = 1.54676f;//Y下
    public const float goalWidth = goalEndX -goalStartX;
    public const float goalHeight = goalEndY - goalStartY;


    public static Vector3 cameraPosMid = new Vector3(0, 1, 7.6f);
    public static Vector3 cameraRotMid = new Vector3(0, -0.4f, 0);
    public static Vector3 ballPosMid = new Vector3(0, 0.4f, 9.7f);
    public static Vector3 playerPos = new Vector3(-0.25f, 0.75f, 23);

 
    

    //游戏状态
    public static int STATUS;//当前
    public const int NORMAL = -1;
    public const int MISS = 0;
    public const int SAVED = 1;//撞击门将
    public const int GOAL=2;//穿过球门但没中靶子
    public const int ONEPOINT=3;//击中靶子外环
    public const int TWOPOINT = 4;//击中靶子中环
    public const int THREEPOINT = 5;//击中靶子内环

    //靶子坐标
    public static float goalX;
    public static float goalY;
    //音乐
    public static float valueMusic;
    public static float valueSound;
    
}
