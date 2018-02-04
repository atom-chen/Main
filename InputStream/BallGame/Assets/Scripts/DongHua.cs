using UnityEngine;
using System.Collections;

public class DongHua : MonoBehaviour {
    public GameObject guanggaoban1;
    public GameObject guanggaoban2;
    public int materialIndex = 0;
    public Vector2 uvAnimationrate = new Vector2(1, 0);//纹理偏移方向
    public string textureName = "_MainTex";
    Vector2 uvOffset = Vector2.zero;//纹理偏移量

    private Renderer guanggaoban1_renderer;
    private Renderer guanggaoban2_renderer;

	// Use this for initialization
	void Start () {
        guanggaoban1_renderer = guanggaoban1.GetComponent<Renderer>();
        guanggaoban2_renderer = guanggaoban2.GetComponent<Renderer>();
	}
	
	// Update is called once per frame
	void Update () {
        uvOffset += (uvAnimationrate * 0.01f * Time.deltaTime);//偏移后的坐标
        if(guanggaoban1_renderer.enabled)
        {
            //设置广告版1偏移后的纹理坐标
            guanggaoban1_renderer.materials[materialIndex].SetTextureOffset(textureName, uvOffset);
        }
        if(guanggaoban2_renderer.enabled)
        {
            //设置广告版2偏移后的纹理坐标
            guanggaoban2_renderer.materials[materialIndex].SetTextureOffset(textureName, uvOffset);
        }
	}
}









