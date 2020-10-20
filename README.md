# 游戏用到的一些Unity术语、UnityAPI和C#接口

### Unity术语
1. sprite mode：精灵模式
2. packing tag:包装标签
3. single：单一的
4. multiple:多种的
5. polygon:多边形的
6. pixels per unit：多少像素一单位
7. slice：片，切图
8. grid by cell size：网格按单元大小
9. grid by cell count：网格按单元格计数
10. Animatin:动画
11. Animator:动画状态机
12. Animator Override Controller:重写状态机
13. GameManager:管理游戏各个对象。
14. order in layer:层级顺序
15. rigidbody 2D:刚体（赋予对象力的作用，物理化,2D注意禁止掉z轴旋转）
16. box collider:碰撞盒子
17. Vedio Source:音乐组件
### UnityAPI和C#接口
1.
```
        //接受吃豆人与目的地的插值
        Vector2 temp = Vector2.MoveTowards(transform.position, des, speed);
        //移动刚体到目的地
        GetComponent<Rigidbody2D>().MovePosition(temp);
```
2.
```         
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
```
3.
```
            //获取移动方向
            Vector2 dir = des - (Vector2)transform.position;
            //把获取到的移动方向设置给动画状态机 
            GetComponent<Animator>().SetFloat("DirX",dir.x);
            GetComponent<Animator>().SetFloat("DirY",dir.y);
```

4.
```
        //记录当前位置
        Vector2 pos = transform.position;
        //从将要到达的位置向当前位置发射一条射线，并存储下射线信息
        RaycastHit2D hit = Physics2D.Linecast(pos + dir, pos);
        //返回此射线是否打到了刚体上的碰撞器上
        return (hit.collider == GetComponent<Collider2D>());
```
5.
```
        foreach (Transform t in GameObject.Find("Maze").transform)//将迷宫所有豆子存入list中
        {
            PacdotArr.Add(t.gameObject);
        }
```
6.
```
        if (nowEat==pacdotNum&&Pacman.GetComponent<PacmanMove>().enabled!=false)//如果吃完豆子，取得胜利
        {
            gamePanel.SetActive(false);//隐藏gamePanel
            Instantiate(winPrefab);//实例化winPrefab
            StopAllCoroutines();
            SetGameState(false);//游戏停止
        }
```
7.
```
    public void OnStartButtom()//按下开始游戏
    {
        StartCoroutine(PlayStarCountDown());//协程
        AudioSource.PlayClipAtPoint(startClip, new Vector3(0,0,-9));//播放开始音乐
        startPanel.SetActive(false);//按下开始游戏后开始界面隐藏
        
        
    }

    IEnumerator PlayStarCountDown()//游戏开始倒计时
    {
        GameObject go = Instantiate(startCountDownPrefab);//实例化对象
        yield return new WaitForSeconds(4f);//协程，等待
        Destroy(go);
        SetGameState(true);//游戏启动
        Invoke("CreateSuperPacdot", 5f);//游戏开始后5s,产生超级豆子
        gamePanel.SetActive(true);//显示游戏界面
        GetComponent<AudioSource>().Play();//播放游戏音乐
    }
```
8.
```
        Pacman.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);//改变颜色
```
9.
```
        Binky.GetComponent<GhostMove>().enabled = false;//禁用update
        Binky.GetComponent<GhostMove>().enabled = true;//启动update
```
10.
```
        Invoke("RecoverEnemy", 3f);//延迟调用，过3秒恢复正常
```





















































