using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public class Item
{
  public int id;//ID
  public int tabID;//表格ID
  public string name;//NAME
  public int count = 1;//当前数量
}

public class Equip:Item
{
  public EQUIP_TYPE secondType;//二级分类
  public int starLevel = 1;//星级
  public int quality = 1;//品质
  public int damage = 0;//伤害
  public int hp = 0;//生命值
  public int power = 0;//战斗力

}

public class Drug:Item
{
  
}


