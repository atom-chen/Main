using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public enum ITEM_FIRST
{
  INVALID,
  DRUG,
  EQUIP,
}

public enum EQUIP_TYPE
{
  INVALID,
  HELM,
  CLOTH,
  WEAPON,
  SHOES,
  NECKLACE,
  BRACELET,
  RING,
  WING
}
public abstract class Item
{
  public int id;//ID
  public string name;//NAME
  public string icon;//Icon
  public ITEM_FIRST firstType;//所属类型
  public int level=1;//使用等级
  public int count = 1;
  public int price;
  public INFO_TYPE infoType;//作用类型
  public int applyValue;//作用值
  public string desc;//描述
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


