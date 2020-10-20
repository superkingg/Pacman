
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement;

public class GhostMove : MonoBehaviour
{
    public GameObject[] paths; //存放敌人移动路径

    public float speed = 0.4f;//敌人速度
    //public Transform[] Waypoint;

    private List<Vector3> Waypoint = new List<Vector3>(); //存放敌人准备移动的坐标点
    private int index = 0;
    private Vector3 x;//记录起始位置的变量

    private void getPath(GameObject path)//获取到敌人具体的哪条路径
    {


        foreach (Transform t in path.transform)
        {

            Waypoint.Add(t.position);//一次将路径点存入Waypoint三维向量中
        }
        Waypoint.Insert(0, x);//插入起始位置
        Waypoint.Add(x);//插入末尾位置
  

    }

    private void Start()
    {
        x = transform.position + new Vector3(0, 3, 0);//设置初始位置偏移值

        //起始位置避免重复路径
        getPath(paths[GameManager.Instance.usingIndex[GetComponent<SpriteRenderer>().sortingOrder - 1]]);

    }

    private void FixedUpdate()
    {   
        //敌人进行移动主代码
        if (transform.position != Waypoint[index])//如果没有到达目的路径点则继续进行移动
        {
            //对起始位置与目的位置进行插值
            Vector2 temp = Vector2.MoveTowards(transform.position, Waypoint[index], speed);
            //移动到插值位置
            GetComponent<Rigidbody2D>().MovePosition(temp);

        }
        else//如果到达目的路径点，则选择下一个目的路径点
        {
            
            index++;
            //如果所有目的路径点移动完毕，则随机获取新的一条路径
            if (index >= Waypoint.Count)
            {
                index = 0;
  
                getPath(paths[Random.Range(0,paths.Length)]);
            }
        }

        Vector2 dir = Waypoint[index] - transform.position;
        GetComponent<Animator>().SetFloat("DirX", dir.x);//通知动画左右移动状态机
        GetComponent<Animator>().SetFloat("DirY", dir.y);//通知动画上下移动状态机
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Pacman")//如果与吃豆人发生碰撞检测
        {
            if (GameManager.Instance.isSuperPacman)//如果是超级吃豆人
            {
                gameObject.transform.position = x - new Vector3(0, 3, 0);//返回出生地点
                index = 0;
                GameManager.Instance.score += 500;
            }
            else
            {
                collision.gameObject.SetActive(false);//停止吃豆人移动
                GameManager.Instance.gamePanel.SetActive(false);//隐藏游戏界面
                Instantiate(GameManager.Instance.gameoverPrefab);//实例化gameover
                Invoke("ReStart", 3f);//延迟3s后重新开始


            }
            
        }
    }

    private void ReStart()
    {
        SceneManager.LoadScene(0);//重新加载场景
    }



}
