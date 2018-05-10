using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utils{
  //英里/小时 ->米/秒
  public static float MilePerHour2MeterPerSecond(float v)
  {
    return (float)(v * 0.44704);
  }
  //米/秒 -> 英里/小时
  public static float MeterPerSecond2MilePerHour(float v)
  {
    return (float)(v * 2.23693629);
  }

  //符号是否相同
  public static bool SameSign(float fitst,float second)
  {
    return (Mathf.Sign(fitst) == Mathf.Sign(second));
  }

  //归一化赛车牵引力
  public static float EvaluateNormPower(float normPower)
  {
    if(normPower<1)
    {
      return 10 - normPower * 9;
    }
    else
    {
      return 1.9f - normPower * 0.9f;
    }
  }

  public static void GetTime(float time,out int minute,out int second,out int ms)
  {
    minute = (int)time / 60;
    second = (int)Math.Floor(time % 60);//不满60的整数部分
    ms = (int)(Math.Round(time % 1, 2) * 100);      //小数前两位
  }
}
