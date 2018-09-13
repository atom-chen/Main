using System.Collections;
using System.Collections.Generic;
using Games;
using UnityEngine;
//界面2d点击特效管理器


public class AutoDestroyClickEffect : MonoBehaviour
{
    public float livetime;

    private float timer = 0f;
    private Transform m_Trans;
    public Transform CachedTrans
    {
        get { return m_Trans ?? (m_Trans = transform); }
    }

    public GameObject effect { get; set; }

    void OnEnable()
    {
        timer = 0f;
    }

    void OnDisable()
    {
        timer = 0f;
    }

    void OnDestroy()
    {

        if (UIEffectsManager.Ins != null)
        {
            UIEffectsManager.Ins.DestroyBulletRes(effect);
        }
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= livetime)
        {
            DestroyObject(gameObject);
        }
    }
}




public class UIEffectsManager : MonoBehaviour
{
    private bool flag = false;
    private float _livetime = 5.0f;
    private string _clickPath = "e_ui_pingmudianji";
    private string _dragPath = "e_ui_pingmudianji_loop";
    private GameObjectPool _pool;
    private Transform m_Trans;
    public Transform CachedTrans
    {
        get { return m_Trans ?? (m_Trans = transform); }
    }

    public delegate void BulletCreateDel(GameObject obj);

    public static UIEffectsManager Ins;


    private GameObject _dragGo;


    private Transform _dragGoCachedTrans;

    public static GameObject CreateRoot()
    {
        if (Ins != null)
        {
            DestroyImmediate(Ins.gameObject);
        }

        if (null == UIManager.Instance()) return null;
        var mgrRoot = UIManager.Instance().transform;

        if (null == mgrRoot) return null;

        var go = new GameObject("UIEffectsManager");
        go.AddComponent<UIEffectsManager>();
        AssetManager.SetObjParent(go, mgrRoot);
        return go;
    }

    void Start()
    {
        _pool.CreateEffectFromBundle(_dragPath, _dragPath, OnDragEffectLoaded);        
    }

    private void OnDragEffectLoaded(GameObject obj, object param1, object param2)
    {

        _dragGo = obj;
        if(obj == null) return;
        obj.SetActive(false);
        obj.name = "DragEffect";
        _dragGoCachedTrans = obj.transform;
        AssetManager.SetObjParent(obj, CachedTrans);
        Utils.SetAllChildLayer(obj.transform, LayerMask.NameToLayer("TopTopUI"));
    }


    void Awake()
    {
        Ins = this;
        _pool = new GameObjectPool("UIClickEffects", CachedTrans);
    }

    void OnDestroy()
    {
        _pool.ClearAllPool();
        Ins = null;
    }

    public static void Show()
    {
        if( null == Ins) return;
        Ins._show();
    }



    public static void Close()
    {
        if (null == Ins) return;
        Ins._close();
    }

    public void _show()
    {
        flag = true;
    }


    public void _close()
    {
        flag = false;
    }

    private void DelOnClick(GameObject _)
    {
        if (null == UICamera.currentTouch || flag == false) return;

        var go = new GameObject("non_pooled_go");
        var auto = Utils.TryAddComponent<AutoDestroyClickEffect>(go);
        if (auto != null)
        {
            auto.livetime = _livetime;
            auto.enabled = _livetime > 0f;
        }
        else
        {
            DestroyImmediate(go);
            return;
        }
        AssetManager.SetObjParent(go, CachedTrans);

        _pool.CreateEffectFromBundle(_clickPath, _clickPath, OnEffectLoaded, auto, null);


        var pos = UICamera.currentCamera.ScreenToWorldPoint(new Vector3
        {
            x = UICamera.currentTouch.pos.x,
            y = UICamera.currentTouch.pos.y
        });
        pos.z = 0;
        go.transform.position = pos;
        Utils.SetAllChildLayer(go.transform, LayerMask.NameToLayer("TopTopUI"));
    }


    private void DelOnDrag(GameObject go, Vector2 delta)
    {
        if (null == _dragGoCachedTrans || 
            null == UICamera.currentCamera ||
            null == UICamera.currentTouch || 
            flag == false) return;

        var pos = UICamera.currentCamera.ScreenToWorldPoint(new Vector3
        {
            x = UICamera.currentTouch.pos.x,
            y = UICamera.currentTouch.pos.y
        });
        pos.z = 0;
        _dragGoCachedTrans.position = pos;
    }

    private void DelOnragEnd(GameObject _)
    {
        if(null == _dragGo) return;

        _dragGo.SetActive(false);
    }

    private void DelOnDragStart(GameObject _)
    {
        if (null == _dragGo || flag == false) return;
        _dragGo.SetActive(true);
    }


    private void OnEffectLoaded(GameObject obj, object param1, object param2)
    {
        if (obj == null) return;

        var bullet = param1 as AutoDestroyClickEffect;
        if (bullet == null)
        {
            DestroyBulletRes(obj);
            return;
        }
        AssetManager.SetObjParent(obj, bullet.CachedTrans);
        bullet.effect = obj;
        obj.SetActive(true);
    }

    public void DestroyBulletRes(GameObject res)
    {
        if (res == null)
        {
            return;
        }
        if (this == null)
        {
            Object.Destroy(res);
            return;
        }
        res.transform.SetParent(CachedTrans);
        _pool.Remove(res);
    }


    void OnEnable()
    {
        UICamera.onClick += DelOnClick;
        UICamera.onDrag += DelOnDrag;
        UICamera.onDragStart += DelOnDragStart;
        UICamera.onDragEnd += DelOnragEnd;
    }


    void OnDisable()
    {
        if (UICamera.onClick != null) UICamera.onClick -= DelOnClick;
        if (UICamera.onDrag != null) UICamera.onDrag -= DelOnDrag;
        if (UICamera.onDragStart != null) UICamera.onDragStart -= DelOnDragStart;
        if (UICamera.onDragEnd != null) UICamera.onDragEnd -= DelOnragEnd;
    }
}
