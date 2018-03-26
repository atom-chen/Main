using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LoginScreen : MonoBehaviour {
    [SerializeField]
    private InputField login_username;
    [SerializeField]
    private InputField login_password;
    [SerializeField]
    private Button login_enterGame;//登陆按钮
    [SerializeField]
    private Button btns_reg;//btns/reg按钮

    [SerializeField]
    private GameObject reg;//注册账号UI
    [SerializeField]
    private Button reg_close;//reg/close按钮
    [SerializeField]
    private InputField reg_username;
    [SerializeField]
    private InputField reg_password;
    [SerializeField]
    private InputField reg_repassword;
    [SerializeField]
    private Button reg_zhucezhanghao;


    //监听登陆按钮点击事件
    public void loginOnclick()
    {
        Debug.Log("用户想要登陆");
        if(login_username.text.Length==0 || login_username.text.Length>6)
        {
            //弹出提示窗
            WaringManager.addWaring(new WaringModel("账号不合法！"));
            return;
        }
        else if (login_password.text.Length == 0 || login_password.text.Length > 6)
        {
            WaringManager.addWaring(new WaringModel("密码不合法！"));
            return;
        }
        //验证通过 申请登陆
        //loginBtn.enabled = false;
        login_enterGame.interactable = false;
    }
	//监听btns/reg 按钮点击事件
    public void btns_regOnClick()
    {
        //注册界面可见
        Debug.Log("用户想要注册账号");
        reg.gameObject.SetActive(true);
    }
    //监听reg/close
    public void reg_closeOnClick()
    {
        //清空输入框
        reg_username.text = "";
        reg_password.text = "";
        reg_repassword.text = "";
        //关闭注册UI
        reg.SetActive(false);

    }
    //监听reg/zhucezhanghao 
    public void reg_zhucezhanghaoOnClick()
    {
        Debug.Log("用户想要注册");
        if (reg_username.text.Length == 0 || reg_username.text.Length > 6)
        {
            WaringManager.addWaring(new WaringModel("账号不合法！"));
            return;
        }
        else if (reg_password.text.Length == 0 || reg_password.text.Length > 6)
        {
            WaringManager.addWaring(new WaringModel("密码不合法！"));
            return;
        }
        else if (!reg_password.text.Equals(reg_repassword.text))
        {
            WaringManager.addWaring(new WaringModel("两次输入密码不一致"));
            return;
        }
        //验证通过，申请注册...

        //清空输入框
        reg_username.text = "";
        reg_password.text = "";
        reg_repassword.text = "";
        //禁用注册按钮
        reg.SetActive(false);
    }
}
