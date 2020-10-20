using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;//引用
    public static GameManager Instance
    {
        get
        {
            return _instance;
        }
    }

    //所以游戏对象
    public GameObject Pacman;
    public GameObject Binky;
    public GameObject Pinky;
    public GameObject Clyde;
    public GameObject Inky;

    public GameObject startPanel;//游戏开始界面
    public GameObject gamePanel;//游戏中界面
    public GameObject startCountDownPrefab;//游戏倒计时动画
    public GameObject gameoverPrefab;//游戏失败界面
    public GameObject winPrefab;//游戏成功界面
    public AudioClip startClip;//游戏音乐
    public Text remainText;
    public Text scoreText;
    public Text eatText;

    public bool isSuperPacman = false;//是否为超级吃豆人
    public List<int> usingIndex = new List<int>();//存放新的路径
    private List<int> pathIndex = new List<int> { 0, 1, 2, 3 };//原来路径
    private List<GameObject> PacdotArr = new List<GameObject>();//存放所有豆子的集合
    private int pacdotNum = 0;//豆子的值
    private int nowEat = 0;//吃掉豆子的值
    public int score = 0;//得分




    private void Awake()
    {   
        
        _instance = this;
        Screen.SetResolution(1024, 768, false);
        int temp = pathIndex.Count;
        for(int i=0;i<temp;i++)
        {   
            //随机的从原来路径随机传入新的新的路劲
            int randIndex = Random.Range(0, pathIndex.Count);
            usingIndex.Add(pathIndex[randIndex]);
            pathIndex.RemoveAt(randIndex);
        }

        foreach (Transform t in GameObject.Find("Maze").transform)//将迷宫所有豆子存入list中
        {
            PacdotArr.Add(t.gameObject);
        }
        pacdotNum = GameObject.Find("Maze").transform.childCount;//Maze下所有孩子的数量

    }
    private void Start()
    {
        SetGameState(false);//游戏初始状态
       
    }

    private void Update()
    {
        if (nowEat==pacdotNum&&Pacman.GetComponent<PacmanMove>().enabled!=false)//如果吃完豆子，取得胜利
        {
            gamePanel.SetActive(false);
            Instantiate(winPrefab);
            StopAllCoroutines();
            SetGameState(false);
        }

        if(nowEat == pacdotNum)
        {
            if(Input.anyKeyDown)//胜利后按下任意键重新开始
            {
                SceneManager.LoadScene(0);
            }
        }


        if(gamePanel.activeInHierarchy)//如果游戏界面显示出出来
        {
            remainText.text = "Remain:\n\n" + (pacdotNum - nowEat);
            eatText.text = "Eat:\n\n" + nowEat;
            scoreText.text = "Score:\n\n" + score;
        }
    }

    public void OnStartButtom()//按下开始游戏
    {
        StartCoroutine(PlayStarCountDown());
        AudioSource.PlayClipAtPoint(startClip, new Vector3(0,0,-9));//播放开始音乐
        startPanel.SetActive(false);//按下开始游戏后开始界面隐藏
        
        
    }

    public void OnExitButtom()//按下退出游戏
    {
        Application.Quit();
    }

    IEnumerator PlayStarCountDown()//游戏开始倒计时
    {
        GameObject go = Instantiate(startCountDownPrefab);//实例化对象
        yield return new WaitForSeconds(4f);
        Destroy(go);
        SetGameState(true);//游戏启动
        Invoke("CreateSuperPacdot", 5f);//游戏开始后5s,产生超级豆子
        gamePanel.SetActive(true);//显示游戏界面
        GetComponent<AudioSource>().Play();//播放游戏音乐
    }



    private void CreateSuperPacdot()//制造超级豆子
    {
        int RandIndex = Random.Range(0, PacdotArr.Count);//产生随机数
        GameObject SuperPacdot = PacdotArr[RandIndex];// 确定超级豆子
        SuperPacdot.transform.localScale = new Vector3(5, 5, 5);//超级豆子的大小变为5倍
        SuperPacdot.GetComponent<Pacdot>().isSuperDot = true;//将豆子中是否为超级豆子布尔值等于真

    }
    public void OnEatPacdot(GameObject dot)//当豆子被吃掉，从数组中删除该豆子
    {
        nowEat++;
        score += 100;//吃到豆子加100
        PacdotArr.Remove(dot);
    }

    public void OnEatSuperDot()//当吃到超级豆子，将吃豆人变为超级吃豆人
    {
        if (PacdotArr.Count<10)//如果豆子数量小于10，不再产生豆子
        {
            return;
        }
        score += 200;
        Invoke("CreateSuperPacdot", 10f);//再过10s,产生超级豆子
        isSuperPacman = true;
        Pacman.GetComponent<SpriteRenderer>().color = new Color(1f,0f,0f,1f);
        FreezeEnemy();//冻结敌人
        Invoke("RecoverEnemy", 3f);//延迟调用，过3秒恢复正常
    }

    public void RecoverEnemy()//恢复敌人
    {
        DisFreeEnemy();
        isSuperPacman = false;
        Pacman.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
    }



    public void FreezeEnemy()//冻结敌人
    {
        Binky.GetComponent<GhostMove>().enabled = false;
        Pinky.GetComponent<GhostMove>().enabled = false;
        Clyde.GetComponent<GhostMove>().enabled = false;
        Inky.GetComponent<GhostMove>().enabled = false;

        Binky.GetComponent<SpriteRenderer>().color = new Color(0.7f, 0.7f, 0.7f, 0.7f);
        Pinky.GetComponent<SpriteRenderer>().color = new Color(0.7f, 0.7f, 0.7f, 0.7f);
        Clyde.GetComponent<SpriteRenderer>().color = new Color(0.7f, 0.7f, 0.7f, 0.7f);
        Inky.GetComponent<SpriteRenderer>().color = new Color(0.7f, 0.7f, 0.7f, 0.7f);
    }
    public void DisFreeEnemy()//解冻敌人
    {
        Binky.GetComponent<GhostMove>().enabled = true;
        Pinky.GetComponent<GhostMove>().enabled = true;
        Clyde.GetComponent<GhostMove>().enabled = true;
        Inky.GetComponent<GhostMove>().enabled = true;

        Binky.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
        Pinky.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
        Clyde.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
        Inky.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
    }

    private void SetGameState(bool state)//游戏在开始时静止
    {
        Pacman.GetComponent<PacmanMove>().enabled = state;
        Binky.GetComponent<GhostMove>().enabled = state;
        Pinky.GetComponent<GhostMove>().enabled = state;
        Clyde.GetComponent<GhostMove>().enabled = state;
        Inky.GetComponent<GhostMove>().enabled = state;
    }


}
