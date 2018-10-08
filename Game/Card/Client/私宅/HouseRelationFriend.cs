using System;
using System.Collections;
using System.Collections.Generic;
using ProtobufPacket;
using UnityEngine;

public class HouseRelationFriend : MonoBehaviour
{
    [SerializeField]
    private GameObject mEmpty;
    [SerializeField]
    private GameObject mList;
    [SerializeField]
    private ListWrapController mListWrapController;

    private bool mListInited = false;

    private List<Relation> mListData;

    void OnEnable()
    {
        //if (GameManager.PlayerDataPool == null ||
        //    GameManager.PlayerDataPool.YardData == null ||
        //    GameManager.PlayerDataPool.YardData.ProtoYard == null ||
        //    GameManager.PlayerDataPool.YardData.ProtoYard.OwnerGuid != LoginData.user.guid)
        //    return;

        RebuildData();

        //Yard.msDelOnYardSync += OnYardSync;
    }

    void OnDisable()
    {
        //Yard.msDelOnYardSync -= OnYardSync;
    }

    //void OnYardSync(ProtobufPacket.YardOp op, _Yard pYard)
    //{
    //    if (pYard == null || op != YardOp.YardOp_STEAL || op != YardOp.YardOp_HELP)
    //        return;
    //    RebuildData();
    //}

    void RebuildData()
    {
        mListData = GameManager.PlayerDataPool.Friends.SortedList;

        if (mListData == null)
            return;

        if (mListData.Count == 0)
        {
            mEmpty.SetActive(true);
            mList.SetActive(false);
        }
        else
        {
            mEmpty.SetActive(false);
            mList.SetActive(true);

            if (!mListInited)
                mListWrapController.InitList(mListData.Count, UpdateItem);
            else
                mListWrapController.UpdateItemCount(mListData.Count);

            mListInited = true;
        }
    }

    void UpdateItem(GameObject obj, int index)
    {
        if (obj == null)
            return;

        if (mListData == null || index < 0 || index >= mListData.Count)
        {
            obj.SetActive(false);
            return;
        }

        var item = obj.GetComponent<HouseRelationItem>();
        if (item == null)
        {
            obj.SetActive(false);
            return;
        }

        obj.SetActive(true);
        var data = mListData[index];

        ProtobufPacket.YardOp op = YardOp.YardOp_NONE;
        Int64 optime = 0;
        //if (GameManager.PlayerDataPool != null &&
        //    GameManager.PlayerDataPool.YardData != null &&
        //    GameManager.PlayerDataPool.YardData.ProtoYard != null)
        //{
        //    var reclist = GameManager.PlayerDataPool.YardData.ProtoYard.YardProdOpRec;
        //    foreach (var rec in reclist)
        //    {
        //        if (rec.OperatorInfo.guid == data.Guid)
        //        {
        //            op = (ProtobufPacket.YardOp)rec.YardOpType;
        //            optime = rec.OpTime;
        //        }
        //    }
        //}
        item.Setup(data, op, optime);
    }
}
