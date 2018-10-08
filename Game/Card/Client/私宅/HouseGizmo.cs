using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HouseGizmo : MonoBehaviour 
{
    private Color yellow = Color.yellow;
    public float radius = 1.0f;

    void OnDrawGizmos()
    {
        Gizmos.color = yellow;
        Gizmos.DrawSphere(transform.position, radius);
    }
}
