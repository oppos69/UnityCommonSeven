/*
 * 模块名称:人物移动攻击模块
 * 模块功能：生成关卡地图
 * 作者：王冲
 * 时间：2015-05-16
 * 描述：
 * 一、移动（动画、声音）
 * 二、攻击障碍物（动画、声音）
 * 三、吃东西（声音）
 * 四、受到攻击（动画、声音）
 * 五  显示Food
 */

using UnityEngine;
using System.Collections;

public class Player : MovingObject {

    private Animator animator;                                              //引用Animator组件
    public  int food;                                                       //引用食物
    private float ExitDelay;                                                //退出延时


    public int FoodHP     = 10;                                             //吃水果可以加多少food
    public int SodaHP       = 20;                                             //吃soda可以加多少food
    public int WallDamage = 1;                                              //对墙的损毁值

    
    void Awake()
    { 
        //获取引用
        animator = GetComponent<Animator>();

    }
	// Use this for initialization
    protected  void Start()
    {
        food = GameManager.instance.playerFood;

        ExitDelay = 1;

        base.Start(); 
    }

    private void OnDisable()
    {
        GameManager.instance.playerFood = food;
    }

    /// <summary>
    /// 检查游戏是否结束
    /// </summary>
    private void CheckGameOver()
    {
        //如果food <= 0 就游戏结束
        if (food <= 0)
        {
            GameManager.instance.GameOver();
        }
    }

    /// <summary>
    /// 掉血
    /// （动画）
    /// </summary>
    /// <param name="loss"></param>
    public void LossFood(int loss)
    {
        //显示动画
        animator.SetTrigger("playerhit");
        //
        food -= loss;

        //显示food
        GameManager.instance.ShowFood(food);    
        //检查是否搜到攻击
        CheckGameOver();
    }

    /// <summary>
    /// 当受到障碍物无法移动时，返回组件
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="component"></param>
    protected override void OnCantMove<T>(T component)
    {
        //判断传入的组件是否是墙
        if (component.GetType() == typeof(Wall))
        {
            //攻击动画
            animator.SetTrigger("playerattack");
            //照成伤害
            Wall wall = component as Wall;
            wall.DamageWall(WallDamage);
        }

        base.OnCantMove<T>(component);
    }

    /// <summary>
    /// 准备移动
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="xDir"></param>
    /// <param name="yDir"></param>
    protected override void AttemptMove<T>(int xDir, int yDir)
    {
        //每走一步消耗一个food
        food--;

        //显示Food;
        GameManager.instance.ShowFood(food);
        
        base.AttemptMove<T>(xDir, yDir);

        //检查游戏是否能结束
        CheckGameOver();

        GameManager.instance.playerTurn = false;
    }
	
	// Update is called once per frame
    void Update()
    {
        //是否能移动
        if (!GameManager.instance.playerTurn)
        {
            return;
        }
        
        int horizontal = 0;
        int vertical = 0;
        //获得用户输入上下左右的值
#if UNITY_STANDALONE || UNITY_WEBPLAYER

        //获得输入水平方向上的移动值
        horizontal = (int)(Input.GetAxisRaw("Horizontal"));

        //获得输入水平方向上的移动值
        vertical = (int)(Input.GetAxisRaw("Vertical"));

        //检查移动水平（x）方向有值，那么不能再垂直（y）方向上移动
        if (horizontal != 0)
        {
            vertical = 0;
        }
        //检查是否在IOS，Android，WP8或IPhone上运行程序
#elif UNITY_IOS || UNITY_ANDROID || UNITY_WP8 || UNITY_IPHONE
			
			//检查如果touch数量大于0
			if (Input.touchCount > 0)
			{
				//获取第一个触点
				Touch myTouch = Input.touches[0];
				
				//检查这个触点是不是开始触点
				if (myTouch.phase == TouchPhase.Began)
				{
					//如果是设置为触点原点，保存位置信息
					touchOrigin = myTouch.position;
				}
				
				//如果触点为结束触点，并且触点的x是大于零的
				else if (myTouch.phase == TouchPhase.Ended && touchOrigin.x >= 0)
				{
					//设置结束触点坐标等于现在的触点坐标
					Vector2 touchEnd = myTouch.position;
					
					//计算两个点x坐标上的差距
					float x = touchEnd.x - touchOrigin.x;
					
					//计算Y轴上的距离
					float y = touchEnd.y - touchOrigin.y;
					
					//设置接触原点为-1开始接受下一次用户操作
					touchOrigin.x = -1;
					
					//检查x轴上的差异与y轴上的差异
					if (Mathf.Abs(x) > Mathf.Abs(y))
						//如果x轴上的差异大于y轴上的认为是水平移动
						horizontal = x > 0 ? 1 : -1;
					else
						//如果x轴上的差异小于y轴上的认为是垂直移动
						vertical = y > 0 ? 1 : -1;
				}
			}
			
#endif

        if (horizontal != 0|| vertical != 0)
        {
            
            AttemptMove<Wall>(horizontal,vertical);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        //判断碰撞到的物体
        //退出就重新加载关卡
        if (other.tag == "Exit")
        { 
            //用Invoke
            Invoke("Restart", ExitDelay);

            enabled = false;

        }
            //食物就加food
        else if(other.tag == "Food")
        {
            food += FoodHP;

            Destroy(other.gameObject);
        }
            //soda就加food
        else if(other.tag == "Soda")
        {
            food += SodaHP;

            Destroy(other.gameObject);
        }
        //显示food
        GameManager.instance.ShowFood(food);    
    }

    private void Restart()
    {
        Application.LoadLevel(Application.loadedLevel);
    }
}
