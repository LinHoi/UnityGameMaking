using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;//加载场景
public class BlinkyMove : MonoBehaviour
{   
    //建立一个物体用于储存路径，路径以transform的形式储存
    public GameObject[] waypointsGos;
    public float speed = 0.2f;
    //建立一个数组用于储存路径的position
    List<Vector3> waypoints = new List<Vector3>();
    int num = 0;
    private Vector3 StarPosition;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Pacman")
        {
            if (GameManager.Instance.isSuperPacman)
            {
                GameManager.Instance.score += 500;
                //TODO
                //真倒霉，碰到超级吃豆人，我要回家躲躲
                transform.position = StarPosition - new Vector3(0, 3, 0);
                num = 0;

            }
            else
            {
                collision.gameObject.SetActive(false);
                GameManager.Instance.GamePanel.SetActive(false);
                Instantiate(GameManager.Instance.GameoverPrefab);
                Invoke("ReStart", 3f);
                //Destroy(collision.gameObject);
            }
        }
    }

    private void ReStart()
    {
        SceneManager.LoadScene(0);
    }
    private void Start()
    {   //鬼的初始位置设定为向上三个单位
        StarPosition = transform.position +new Vector3 (0, 3, 0);

        //遍历物体内的transform，将transform的position依次存入waypoints数组中
        //LoadPath(waypointsGos[Random.Range(0,waypointsGos.Length)]);
        //Random.Range(0, 3)]=[0,3),左闭右开

        LoadPath(waypointsGos[GameManager.Instance.usingIndex[GetComponent<SpriteRenderer>().sortingOrder - 2]]);


    }

    private void FixedUpdate()
    {
        if (transform.position != waypoints[num])
        {//插值得到要移动到dest位置的下一次坐标
            Vector2 newposition = Vector2.MoveTowards(transform.position, waypoints[num], speed);
            //通过刚体组件使物体移动到新位置
            GetComponent<Rigidbody2D>().MovePosition(newposition);
        }
        else
        {
            if (num != waypoints.Count - 1)
            {
                num = (num + 1) % waypoints.Count;
            }
            else
            {
                num = 0;
                LoadPath(waypointsGos[Random.Range(0, waypointsGos.Length)]);
            }
        }
        
        //位移
        Vector2 dis = waypoints[num] - transform.position;
        GetComponent<Animator>().SetFloat("DieX", dis.x);
        GetComponent<Animator>().SetFloat("DieY", dis.y);
    }

    void LoadPath(GameObject go)
    {
        waypoints.Clear();
        foreach (Transform t in go.transform)
        {   
            waypoints.Add(t.position);
        }
        waypoints.Insert(0, StarPosition);
        waypoints.Add(StarPosition);
    }
}