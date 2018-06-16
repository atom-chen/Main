using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyGizmo : MonoBehaviour {
    private Color yellow = Color.yellow;
    private float radius = 0.1f;

    void OnDrawGizmos()
    {
        Gizmos.color = yellow;
        Gizmos.DrawSphere(transform.position, radius);
    }
}
