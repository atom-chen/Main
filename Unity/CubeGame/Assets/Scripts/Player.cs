using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
    public float jump_speed = 12.0f;
    private bool isLanding = true;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (isLanding)
        {
            if (Input.GetMouseButtonDown(0))
            {
                this.GetComponent<Rigidbody>().velocity = Vector3.up * this.jump_speed;
                isLanding = false;
            }
        }
        
	}
    void OnCollisionEnter(Collision other)
    {
        if(other.gameObject.tag=="Floor")
        {
            isLanding = true;
        }
        
    }
}
