using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HouseHeadMenu : MonoBehaviour
{
    private Transform mTrans;
    private Transform mTarget;

    void Awake()
    {
        mTrans = transform;
    }

    public void SetTarget(Transform target)
    {
        mTarget = target;
        gameObject.SetActive(mTarget != null);
    }

    void Update()
    {
        if (mTarget == null)
            return;

        Vector3 spos = HouseScene.GetScreenPos(mTarget.position);
        mTrans.localPosition = spos;
    }

    public void OnRotateStart()
    {
        var scene = GameManager.CurScene as HouseScene;
        if (scene == null)
            return;
        //scene.SwitchEditorState(HouseScene.EditorState.ROTATE);
    }

    public void OnRotateEnd()
    {
        var scene = GameManager.CurScene as HouseScene;
        if (scene == null)
            return;
        //scene.SwitchEditorState(HouseScene.EditorState.NONE);
    }

    public void OnMoveStart()
    {
        var scene = GameManager.CurScene as HouseScene;
        if (scene == null)
            return;
        //scene.SwitchEditorState(HouseScene.EditorState.MOVE);
    }

    public void OnMoveEnd()
    {
        var scene = GameManager.CurScene as HouseScene;
        if (scene == null)
            return;
        //scene.SwitchEditorState(HouseScene.EditorState.NONE);
    }

    public void OnDelete()
    {
        //var scene = GameManager.CurScene as HouseScene;
        //if (scene == null)
        //    return;
        //scene.TakeCurrentCard();
    }
}
