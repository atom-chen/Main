using UnityEngine;
using System.Collections;
using UnityEngine.UI;
//错误提示面板

public class WaringWindow : MonoBehaviour {
    [SerializeField]
    private Text Text;
    WaringResult result;
    public void active(WaringModel model)
    {
        Text.text = model.value;
        this.result = model.result;
       gameObject.SetActive(true);//将自己激活
    }
	public void close()
    {
        gameObject.SetActive(false);
        if(result!=null)
        {
            result();
        }
    }

}
