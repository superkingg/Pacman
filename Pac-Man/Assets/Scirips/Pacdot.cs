using UnityEngine;

public class Pacdot : MonoBehaviour
{
    public bool isSuperDot = false;//布尔值，是否为超级豆子
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Pacman")
        {
            if (isSuperDot)//如果是超级豆子，告诉GameManager让吃豆人变为超级吃豆人
            {
                
                GameManager.Instance.OnEatPacdot(gameObject);//从GameManager调用函数，将数组中的该豆子删除
                GameManager.Instance.OnEatSuperDot();//告诉GameManager让吃豆人变为超级吃豆人
                Destroy(gameObject);//与主角发生碰撞检测，摧毁自身。
            }
            else
            {
                
                GameManager.Instance.OnEatPacdot(gameObject);//从GameManager调用函数，将数组中的该豆子删除
                Destroy(gameObject);//如果与主角发生碰撞检测，则摧毁自身。
            }
        }


    }
}
