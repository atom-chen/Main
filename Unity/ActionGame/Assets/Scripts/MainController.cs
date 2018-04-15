using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MainController : MonoBehaviour {
    public Color purple;
    public Mesh[] headMeshArray;
    private int headMeshIndex = 0;
    public SkinnedMeshRenderer headRenderer;

    public Mesh[] handMeshArray;
    private int handMeshIndex = 0;
    public SkinnedMeshRenderer handRenderer;
  
    public SkinnedMeshRenderer[] bodyArray;
    private Color[] colorArray;
    private int colorIndex = -1;
    void start()
    {
        colorArray = new Color[] { Color.blue, Color.green, Color.red, this.purple, Color.cyan };
        DontDestroyOnLoad(this.gameObject);
    }
    public void OnHeadMeshNext()
    {
        headMeshIndex++;
        headMeshIndex %= headMeshArray.Length;
        headRenderer.sharedMesh = headMeshArray[headMeshIndex];
    }
    public void OnHandMeshNext()
    {
        handMeshIndex++;
        handMeshIndex %= handMeshArray.Length;
        handRenderer.sharedMesh = handMeshArray[handMeshIndex];
    }
    public void OnChangeColorBlue()
    {
        colorIndex = 0;
        OnChangeColor(Color.blue);
    }
    public void OnChangeColorCyan()
    {
        colorIndex = 1;
        OnChangeColor(Color.cyan);
    }
    public void OnChangeColorGreen()
    {
        colorIndex = 2;
        OnChangeColor(Color.green);
    }
    public void OnChangeColorPuple()
    {
        colorIndex = 3;
        OnChangeColor(this.purple);
    }
    public void OnChangeColorRed()
    {
        colorIndex = 4;
        OnChangeColor(Color.red);
    }
    private void OnChangeColor(Color c)
    {
        foreach(SkinnedMeshRenderer renderer in bodyArray)
        {
            renderer.material.color = c;
        }
    }
    public void OnPlay()
    {
        //信息存储
        this.Save();
        SceneManager.LoadScene(1);

    }
    private void Save()
    {
        PlayerPrefs.SetInt("HeadMeshIndex", headMeshIndex);
        PlayerPrefs.SetInt("HandMeshIndex", handMeshIndex);
        PlayerPrefs.SetInt("ColorIndex", colorIndex);

    }

}
