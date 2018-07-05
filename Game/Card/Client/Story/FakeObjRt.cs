using Games.LogicObj;
using UnityEngine;

public class FakeObjRt : MonoBehaviour
{
    public int width = 256;
    public int height = 256;
    public FakeObjRootRt rootPref;
    public UITexture texture;
    public int fakeObjId = -1;
    public bool rightFake = false;

    public Camera rtCamera;
    public Transform modelRoot;

    private RenderTexture rt;

    void Awake()
    {
        if (rootPref != null)
        {
            GameObject go = Instantiate(rootPref.gameObject);
            if (go == null)
            {
                enabled = false;
                return;
            }
            AssetManager.SetObjParent(go, transform);
            go.transform.localScale = Vector3.one * 360.0f; //放大UIRoot的缩放
            FakeObjRootRt comp = go.GetComponent<FakeObjRootRt>();
            if (comp != null)
            {
                rtCamera = comp.rtCamera;
                modelRoot = comp.modelTrans;
            }
        }

        if (rtCamera == null)
        {
            LogModule.WarningLog("camera missing!");
            enabled = false;
            return;
        }
        if (modelRoot == null)
        {
            LogModule.WarningLog("model root missing");
            enabled = false;
            return;
        }
    }

    void OnEnable()
    {
        rt = new RenderTexture(width, height, 16, RenderTextureFormat.ARGB32);
        rt.name = "fakeObjRt";
        if (Display.IsAA())
        {
            rt.antiAliasing = 2;
        }
        rt.autoGenerateMips = false;
        rt.wrapMode = TextureWrapMode.Clamp;
        rt.filterMode = FilterMode.Trilinear;
        rtCamera.targetTexture = rt;

        texture.mainTexture = rt;
    }

    void OnDisable()
    {
        if (rtCamera != null)
        {
            rtCamera.targetTexture = null;
        }
        if (texture != null)
        {
            texture.mainTexture = null;
        }
        if (rt != null)
        {
            DestroyImmediate(rt);
            rt = null;
        }
    }

    public Obj_Fake Refresh(int fakeId, Obj_Fake.OnLoadFakeObjModelOver del = null)
    {
        fakeObjId = fakeId;
        return ObjManager.CreateFakeObj(fakeId, null, modelRoot, 12,del, rightFake);
    }

    public Obj_Fake Refresh(int fakeId,AvatarLoadInfo loadInfo, Obj_Fake.OnLoadFakeObjModelOver del = null)
    {
        return ObjManager.CreateFakeObj(fakeId,loadInfo, null, modelRoot, 12, del, true, rightFake);
    }

    [ContextMenu("Refresh")]
    public void Refresh()
    {
        if (fakeObjId != -1)
        {
            Refresh(fakeObjId);
        }
    }
}