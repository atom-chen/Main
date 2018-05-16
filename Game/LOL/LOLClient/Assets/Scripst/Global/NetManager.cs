using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetManager : MonoBehaviour {
  private static NetManager _Instance;
  public static NetManager Instance
  {
    get
    {
      return _Instance;
    }
  }
  void Awake()
  {
    _Instance = this;
  }
	// Use this for initialization
	void Start () {
    NetIO net = NetIO.Instance;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
