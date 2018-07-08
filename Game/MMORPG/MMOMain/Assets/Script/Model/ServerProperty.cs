using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public  partial class ServerPropert
{
  public bool Hot
  {
    get
    {
      return Count >= 50;
    }
  }
}
