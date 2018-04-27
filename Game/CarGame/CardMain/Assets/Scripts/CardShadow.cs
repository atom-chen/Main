using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardShadow : MonoBehaviour {
  private Vector3 m_Offset = new Vector3(0, 5, 0);
  private Vector3 m_Rotate = new Vector3(90, 0, 0);

  public Transform m_Player;
  private Transform m_ThisTrans;
	// Use this for initialization
	void Start () {
		m_ThisTrans=this.GetComponent<Transform>();
	}
	
	// Update is called once per frame
	void Update () {
    m_ThisTrans.position = m_Player.position + m_Offset;
    m_ThisTrans.eulerAngles = m_Rotate;
	}
}
