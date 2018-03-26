using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
/*
 * 一个警告的模板类
 */ 
public delegate void WaringResult();
 public class WaringModel
{
     public WaringResult result;//委托实例，用来实现回调
     public string value;
     public WaringModel(string value,WaringResult result = null)
     {
         this.value = value;
         this.result = result;
     }
 
    
 
}

