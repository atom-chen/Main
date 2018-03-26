using UnityEngine;
using System.Collections;

public class WeaponGun : MonoBehaviour {
    //拿到生成子弹的位置
    public Transform bulletPos;
    //拿到子弹预设
    public GameObject bulletPerfab;

    public void Shot()
    {
        //实例化子弹
       GameObject go=(GameObject) GameObject.Instantiate(bulletPerfab, bulletPos.position, transform.root.rotation);
    }

}
