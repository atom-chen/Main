using UnityEngine;
using System.Collections;

public class playerIcon : MonoBehaviour {
    void Awake()
    {

    }
    private Transform Icon;

	// Use this for initialization
	void Start () {
        Icon = minMap.map.getPlayerIcon();
	}
	
	// Update is called once per frame
	void Update () {
        float y = transform.eulerAngles.y;
        Icon.localEulerAngles = new Vector3(0, 0, -y);
	}
}
