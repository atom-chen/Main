using UnityEngine;
using System.Collections;

public class InputLimit : MonoBehaviour {
    public GameObject userNameInputField;
    public GameObject passwordInputField;
    public GameObject eMailInputField;
    public void EmailValueChange()
    {
       //拿到当前的值
        if(eMailInputField==null)
        {
            GameObject.Find("EMailInputField");
        }
        string Str = eMailInputField.GetComponent<UILabel>().text;
        for(int i=0;i<Str.Length;i++)
        {
            bool first=true;
            if(Str[i]=='.')
            {
                //如果是第一次遇到'.' 且不为结束
                if(first && i<Str.Length-1)
                {
                    first = false;
                    if(Str[i+1]=='@')
                    {
                        //阻止提交
                    }
                }
            }
        }
    }
}
