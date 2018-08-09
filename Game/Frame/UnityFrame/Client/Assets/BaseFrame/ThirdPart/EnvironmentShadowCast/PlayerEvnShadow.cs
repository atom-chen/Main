using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEvnShadow : MonoBehaviour {


    public Texture2D shadowMap;
    GameObject terrain;
    Transform playerTransform;
    List<Material> materialList = new List<Material>();
    Vector3 playerPosition;
    Vector2 uv;
    Color ShadowColor;
    SkinnedMeshRenderer[] skins;
    Vector2 terrainSize;
    // Use this for initialization
    void Start () {
        playerTransform = gameObject.GetComponent<Transform>();
        skins = gameObject.transform.GetComponentsInChildren<SkinnedMeshRenderer>();
        terrain = GameObject.Find("Scene").transform.GetChild(0).gameObject;
        for (int i = 0; i < skins.Length; i++)
        {
            for (int j = 0; j < skins[i].materials.Length; j++)
            {
                if (!materialList.Contains(skins[i].materials[j]))
                    materialList.Add(skins[i].materials[j]);
            }
        }
        terrainSize.x = terrain.GetComponent<MeshCollider>().bounds.size.x;
        terrainSize.y = terrain.GetComponent<MeshCollider>().bounds.size.z;
}
	void Update () {
        if (skins.Length == 0)
        {
            skins = gameObject.transform.GetComponentsInChildren<SkinnedMeshRenderer>();
            if (skins.Length == 0)
                return;
            else
            {
                for (int i = 0; i < skins.Length; i++)
                {
                    for (int j = 0; j < skins[i].materials.Length; j++)
                    {
                        if (!materialList.Contains(skins[i].materials[j]))
                            materialList.Add(skins[i].materials[j]);
                    }
                }
            }
        }
        if (shadowMap == null || terrain == null)
            return;
        playerPosition = playerTransform.position;
        uv.x = playerPosition.x / terrainSize.x* shadowMap.width;
        uv.y = playerPosition.z / terrainSize.y * shadowMap.height;
        if(shadowMap != null)
            ShadowColor = shadowMap.GetPixel((int)uv.x, (int)uv.y);
        for (int k = 0; k < materialList.Count;k++)
        {
            materialList[k].SetColor("_MainColor", ShadowColor) ;
        }
	}
}
