using UnityEngine;

public class PacmanMove : MonoBehaviour
{   
    //吃豆人的速度
    public float speed = 0.35f;
    //吃豆人下一次移动到达的目的地
    private Vector2 des = Vector2.zero;

    private void Start()
    {   
        //保证吃豆人在游戏刚开始的时候不会动
        des = transform.position;
    }

    private void FixedUpdate()
    {   
        //接受吃豆人与目的地的插值
        Vector2 temp = Vector2.MoveTowards(transform.position, des, speed);
        //移动刚体到目的地
        GetComponent<Rigidbody2D>().MovePosition(temp);

        if((Vector2)transform.position==des)
        {
            //按键检测
            if (Input.GetKey(KeyCode.W)&&valid(Vector2.up))
            {
                des = (Vector2)transform.position + Vector2.up;
            }
            if (Input.GetKey(KeyCode.S) && valid(Vector2.down))
            {
                des = (Vector2)transform.position + Vector2.down;
            }
            if (Input.GetKey(KeyCode.D) && valid(Vector2.right))
            {
                des = (Vector2)transform.position + Vector2.right;
            }
            if (Input.GetKey(KeyCode.A) && valid(Vector2.left))
            {
                des = (Vector2)transform.position + Vector2.left;
            }
            //获取移动方向
            Vector2 dir = des - (Vector2)transform.position;
            //把获取到的移动方向设置给动画状态机 
            GetComponent<Animator>().SetFloat("DirX",dir.x);
            GetComponent<Animator>().SetFloat("DirY",dir.y);


        }
    }
    private bool valid(Vector2 dir)
    {
        //记录当前位置
        Vector2 pos = transform.position;
        //从将要到达的位置向当前位置发射一条射线，并存储下射线信息
        RaycastHit2D hit = Physics2D.Linecast(pos + dir, pos);
        //返回此射线是否打到了刚体上的碰撞器上
        return (hit.collider == GetComponent<Collider2D>());


    }


}
