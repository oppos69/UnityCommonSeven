/**
 * 
 * 模块名称:游戏管理
 * 模块功能：对游戏关卡，回合，场景生成控制
 * 作者：王冲
 * 时间：2015-05-16
 * 描述：
 * 一记录玩家的食品
 * 二控制当前回合是玩家回合还是敌人回合
 * 三控制关卡生成
 * 
 * 
 * 
 */
using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;                                       //单例

    #region 场景控制======================
    private BoardManager boardManger;                                         //引用boardManger脚本

    #endregion

    #region 关卡控制
    private int level = 1;                                                    //关卡数量
    public float levelTimeDelay = 2f;                                         //关卡之间幕布的动画时间
    private Text txtLevel;                                                    //关卡显示字幕
    private GameObject imgBG;                                                 //幕布
    public Text txtFood;                                                     //主角的Food
    private bool doingSetup = true;                                           //在场景加载时敌人和主角都不能移动

    #endregion

    
    
    
    public int playerFood = 100;                                              //食物的数量
    private Player player;
    #region 回合控制======================
    [HideInInspector]
    public bool playerTurn = true;                                             //回合
    private bool enemyMoveing;                                                //敌人回合
    public float Delay = 0.1f;                                                //停滞时间
    #endregion
    #region 敌人数量
    private List<EnemyMoveing> enemies;                                       //引用所有的敌人
    #endregion

    #region 重新加载关卡
    private bool isReStart = false;
    #endregion

    void Awake()
    {   
        //判断当前实例
        if (instance == null)
        {
            instance = this;
        }
        else if(instance != this)
        {
            Destroy(instance);
        }
        //不希望重新加载场景时重新生成gamemanager
        DontDestroyOnLoad(instance);

        //实例敌人对象
        enemies = new List<EnemyMoveing>();
        
       

        boardManger = GetComponent<BoardManager>();
        InitGame();
       

    }

    /// <summary>
    /// 重新开始
    /// </summary>
    void ReStart()
    {
        level = 0;
        GameManager.instance.playerFood = 100;
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
       
        player.food = 100;
        isReStart = false;
        Application.LoadLevel(Application.loadedLevel);
        
    }

    /// <summary>
    /// 新关卡被加载的时候就会被调用一次
    /// </summary>
    /// <param name="index"></param>
    void OnLevelWasLoaded(int index)
    {
        level++;
        InitGame();
    }

    void InitGame()
    {
        //开始加载关卡
        doingSetup = true;
        //获得引用
        imgBG = GameObject.Find("ImgLevel");
        txtLevel = GameObject.Find("txtLevel").GetComponent<Text>();
        //获取Food引用
        txtFood = GameObject.Find("txtFood").GetComponent<Text>();
       
        //设置关卡显示字幕
        txtLevel.text = "Day " + level;
        //启动幕布
        imgBG.SetActive(true);
        //过2秒后隐藏关卡幕布
        Invoke("HideLevelImage",levelTimeDelay);
        //清空敌人
        enemies.Clear();
        //初始化游戏场景
        boardManger.InitLevel(level);
        //显示Food
        ShowFood(playerFood);
    }

    void HideLevelImage()
    { 
        imgBG.SetActive(false);
        //玩家和敌人可以移动了
        doingSetup = false;
    }

    public void GameOver()
    {
        //活了多少关
        txtLevel.text = "After "+level +" days,you started";
        imgBG.SetActive(true);
        //enabled = false;
        //是否重新加载关卡
        isReStart = true;
    }

    void Update()
    {
        //判断是否游戏结束重新加载关卡
        if (isReStart)
        {
            if (Input.GetMouseButtonDown(0))
            {
                ReStart();
                return;
            }
        }

        //当玩家不能移动、敌人不能移动
        if (playerTurn || enemyMoveing || doingSetup)
        {
            return;
        }

        StartCoroutine(MoveEnemey());
    }

    /// <summary>
    /// 添加敌人
    /// </summary>
    /// <param name="enemy"></param>
    public void AddEnemy(EnemyMoveing enemy)
    {
        enemies.Add(enemy);
    }

    IEnumerator MoveEnemey()
    {
        //敌人开始移动
        enemyMoveing = true;
        //停止移动
        yield return new WaitForSeconds(Delay);
        

        for (int i = 0; i < enemies.Count; i++)
        {
            enemies[i].MoveEnemy();

            yield return new WaitForSeconds(enemies[i].MoveTime);
           
        }

        //敌人移动完后，玩家开始移动
        playerTurn = true;
        //敌人不能移动
        enemyMoveing = false;
    }

    /// <summary>
    /// 显示Food
    /// </summary>
    /// <param name="num"></param>
    public void ShowFood(int num)
    {
        txtFood.text = "Food: " + num;
    }
}
