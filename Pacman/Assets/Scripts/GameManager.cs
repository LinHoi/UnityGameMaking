using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance
    {
        get
        {
            return _instance;
        }
    }


    public GameObject pacman;
    public GameObject blinky;
    public GameObject clyde;
    public GameObject inky;
    public GameObject pinky;

    //UI
    public GameObject StartPanel;
    public GameObject GamePanel;
    public GameObject StartCountDownPrefab;
    public GameObject GameoverPrefab;
    public GameObject WinPrefab;
    public AudioClip StartClip;
    public Text remainText;
    public Text nowText;
    public Text scoreText;

    //计数
    private int pacdotNum = 0;
    private int nowEat = 0;
    public int score = 0;


    public bool isSuperPacman = false;
    public List<int> usingIndex = new List<int>();
    public List<int> rowIndex = new List<int> { 0, 1, 2, 3 };
    private List<GameObject> pacdotGos = new List<GameObject>();


    private void Awake()
    {   
        _instance = this;
        Screen.SetResolution(1024, 768, false);
        int tempcount = rowIndex.Count;
        for (int i = 0; i < tempcount; i++)
        {
            int temp = Random.Range(0, rowIndex.Count);
            usingIndex.Add(rowIndex[temp]);
            rowIndex.Remove(rowIndex[temp]);
        }


        foreach (Transform t in GameObject.Find("Maze").transform)
        {
            pacdotGos.Add(t.gameObject);
        }
        pacdotNum = GameObject.Find("Maze").transform.childCount;
    }

    private void Start()
    {
        SetGameState(false);
        
    }

    private void Update()
    {
        if (nowEat == pacdotNum&&pacman.GetComponent<PacmanMove>().enabled!=false)
        {
            GamePanel.SetActive(false);
            Instantiate(WinPrefab);
            StopAllCoroutines();
            SetGameState(false);
        }
        if (nowEat == pacdotNum)
        {
            if (Input.anyKeyDown)
            {
                SceneManager.LoadScene(0);
            }
        }
        if (GamePanel.activeInHierarchy)
        {
            remainText.text = "Remain:" + (pacdotNum - nowEat);
            nowText.text = "Eaten:" + nowEat;
            scoreText.text = "Score:" + score;
        }
    }
    //点击Start按钮后，关闭Start面板
    public void OnStartButton()
    {
        StartPanel.SetActive(false);
        StartCoroutine(PlayStartCountDown());
        //播放音乐
        AudioSource.PlayClipAtPoint(StartClip, Vector3.zero);

    }
    public void OnExitButton()
    {
        Application.Quit();
    }

    //播放倒计时
    IEnumerator   PlayStartCountDown()
    {   
        //实体化倒计时
        GameObject go=Instantiate(StartCountDownPrefab);
        //等待3秒后销毁倒计时
        yield return new WaitForSeconds(3f);
        Destroy(go);
        //游戏状态设为真
        SetGameState(true);
        
        //开启Game面板
        GamePanel.SetActive(true);
        GetComponent<AudioSource>().Play();
        //延迟10秒后运行CreateSuperDot
        Invoke("CreateSuperPacdot", 10f);
    }


    private  void CreateSuperPacdot()
    {   
        if (pacdotGos.Count < 5)
        {
            return;
        }
        int tempIndex = Random.Range(0, pacdotGos.Count);
        pacdotGos[tempIndex].transform.localScale = new Vector3(3, 3, 3);
        pacdotGos[tempIndex].GetComponent<Pacdot>().isSuperdot = true;
    }

    public void OnEatPacdot(GameObject go)
    {
        nowEat++;
        score += 100;
        pacdotGos.Remove(go);

    }

    public void OnEatSuperPacdot()
    {
        score += 200;
        Invoke("CreateSuperPacdot", 10f);
        isSuperPacman = true;
        FreezeEnemy();

        //调用携程
        StartCoroutine(RecoveryEnemy());
    }

    //携程
    IEnumerator RecoveryEnemy()
    {
        yield return new WaitForSeconds(3f);
        DisFreezeEnemy();
        isSuperPacman = false;
    }



    private  void FreezeEnemy()
    {
        blinky.GetComponent<BlinkyMove>().enabled = false;
        clyde.GetComponent<BlinkyMove>().enabled = false;
        pinky.GetComponent<BlinkyMove>().enabled = false;
        inky.GetComponent<BlinkyMove>().enabled = false;
        blinky.GetComponent<SpriteRenderer>().color = new Color(0.7f, 0.7f, 0.7f);
        clyde.GetComponent<SpriteRenderer>().color = new Color(0.7f, 0.7f, 0.7f);
        inky.GetComponent<SpriteRenderer>().color = new Color(0.7f, 0.7f, 0.7f);
        pinky.GetComponent<SpriteRenderer>().color = new Color(0.7f, 0.7f, 0.7f);
    }

    private void DisFreezeEnemy()
    {
        blinky.GetComponent<BlinkyMove>().enabled = true;
        clyde.GetComponent<BlinkyMove>().enabled = true;
        pinky.GetComponent<BlinkyMove>().enabled = true;
        inky.GetComponent<BlinkyMove>().enabled = true;
        blinky.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f);
        clyde.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f);
        inky.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f);
        pinky.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f);
    }

    private void SetGameState(bool state)
    {
        pacman.GetComponent<PacmanMove>().enabled = state;
        blinky.GetComponent<BlinkyMove>().enabled = state;
        clyde.GetComponent<BlinkyMove>().enabled = state;
        pinky.GetComponent<BlinkyMove>().enabled = state;
        inky.GetComponent<BlinkyMove>().enabled = state;

    }
    
}
