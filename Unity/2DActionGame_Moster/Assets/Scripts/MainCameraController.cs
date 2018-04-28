using UnityEngine;
using System.Collections;

public class MainCameraController : MonoBehaviour {
	void Update () {
        transform.position = new Vector3(GameManager._Instance.m_Player.transform.position.x, transform.position.y, transform.position.z);
	}
}
