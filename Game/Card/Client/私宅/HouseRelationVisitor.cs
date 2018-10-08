using System.Collections;
using System.Collections.Generic;
using ProtobufPacket;
using UnityEngine;

public class HouseRelationVisitor : MonoBehaviour
{
    [SerializeField]
    private GameObject mEmpty;
    [SerializeField]
    private GameObject mList;
    [SerializeField]
    private ListWrapController mListWrapController;

    private bool mListInited = false;
    private List<ProtobufPacket._YardProdOpRec> mListData;

    void OnEnable ()
    {
        if (GameManager.PlayerDataPool == null ||
            GameManager.PlayerDataPool.YardData == null ||
            GameManager.PlayerDataPool.YardData.ProtoYard == null ||
            GameManager.PlayerDataPool.YardData.ProtoYard.OwnerGuid != LoginData.user.guid)
            return;

        RebuildData(GameManager.PlayerDataPool.YardData.ProtoYard.YardProdOpRec);

        Yard.msDelOnYardSync += OnYardSync;
    }

    void OnDisable()
    {
        Yard.msDelOnYardSync -= OnYardSync;
    }

    void OnYardSync(ProtobufPacket.YardOp op, _Yard pYard)
    {
        if (pYard == null || op != YardOp.YardOp_STEAL || op != YardOp.YardOp_HELP)
            return;
        RebuildData(pYard.YardProdOpRec);
    }

    void RebuildData(List<ProtobufPacket._YardProdOpRec> data)
    {
        mListData = data;
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
        Relation rel = new Relation();
        rel.Init(data.OperatorInfo);
        item.Setup(rel, (ProtobufPacket.YardOp)data.YardOpType, data.OpTime);
    }
}
