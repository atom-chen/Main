using UnityEngine;
using System.Collections;

public class DestoryByBoundary : MonoBehaviour {
    //碰到边界的物体被销毁
    void OnTriggerExit(Collider other)
    {
        Destroy(other.gameObject);
    }
}
