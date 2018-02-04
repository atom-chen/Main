using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameController : MonoBehaviour {
    public GameObject hazard;
    public Vector3 spawnValue = new Vector3(5.5f, 0, 14);
    private Vector3 spawnPosition = Vector3.zero;
    private Quaternion spawnRotation;
    //炸弹总数
    public int hazardCount = 10;

    //炸弹开始出现的时间间隔
    public float startWait = 1;
    //每个炸弹出现间隔时间
    public float spawnWait=0.5f;
    //几波炸弹之间的间隔时间
    public float waveWait = 5;
    //计分
    public Text scoreText;
    private int score=0;
    //游戏结束
    public Text gameOverText;
    private bool isOver;
    //重新开始
    public Text restartText;
    private bool isRestart;
	// Use this for initialization
    public void AddScore(int newScoreValue)
    {
        score += newScoreValue;
        UpdateScore();
    }
    public void GameOver()
    {
        isOver = true;
        gameOverText.text = "游戏结束";
    }
    void UpdateScore()
    {
        scoreText.text = "得分：" + score;
    }
	void Start () {
        UpdateScore();
        gameOverText.text = "";
        isOver = false;
        restartText.text = "";
        
        //启动协程
        StartCoroutine(SpawnWaves());
	}
    IEnumerator SpawnWaves()
    {
        yield return new WaitForSeconds(spawnWait);
        //源源不断地产生炸弹
        while(true)
        {
            if(isOver)
            {
                //提示是否重新开始
                restartText.text = "按【R】重新开始";
                isRestart = true;
                break;      
            }
            for (int i = 0; i < hazardCount; i++)
            {
                //初始化新的坐标点
                spawnPosition.x = Random.Range(-spawnValue.x, spawnValue.x);
                spawnPosition.z = spawnValue.z;
                spawnRotation = Quaternion.identity;
                GameObject.Instantiate(hazard, spawnPosition, spawnRotation);
                yield return new WaitForSeconds(spawnWait);
            }
        }    
    }
    void Update()
    {
        if(isRestart)
        {
            if(Input.GetKeyDown(KeyCode.R))
            {
                Application.LoadLevel(Application.loadedLevel);
            }
        }
    }
}
