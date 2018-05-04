using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tools {

  public void SetChild(Transform parent,Transform child,Vector3 localPos)
  {
    child.parent = parent;
    child.localPosition = localPos;
  }
}
