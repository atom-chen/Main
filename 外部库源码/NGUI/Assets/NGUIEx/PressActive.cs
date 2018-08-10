using UnityEngine;

public class PressActive : MonoBehaviour
{
    public GameObject activeGo;

    void OnPress(bool press)
    {
        activeGo.SetActive(press);
    }
}