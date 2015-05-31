/***
 * 
 *      ProjectName:Stealth
 * 
 *      Anthor     :wangchong
 * 
 *      CreateTime: 2015-05-02
 * 
 *      DESC       ：敌人的AI（射击、追击 、巡逻）
 * 
 *      
 */
using UnityEngine;
using System.Collections;

public class EnemyAI : MonoBehaviour
{
    public float patrolSpeed    = 2f;                                           //巡逻速度
    public float chaseSpeed     = 5f;                                           //追击速度
    public float chaseWaitTime  = 5f;                                           //追击停止时间
    public float patrolWaitTime = 1f;                                           //巡逻速度
    public Transform[] patrolWayPoints;                                          //巡逻点

    private EnemySight enemySight;                                              //引用敌人视听组件
    private LastPlayerSighting lastPlayerSighting;                              //引用lastplayerSighting组件
    private NavMeshAgent nav;                                                   //引用寻路组件
    private Transform player;                                                   //引用player transform组件
    private PlayerHealth playerHealth;                                          //引用playerHealth组件
    private float chaseTime;                                                    //声明追击计时器
    private float patrolTime;                                                   //声明巡逻计时器
    private int wayPointIndex;                                                  //声明路径点数组的索引

    void Awake()
    {
        //获得引用
        enemySight         = GetComponent<EnemySight>();
        lastPlayerSighting = GameObject.FindGameObjectWithTag(Tags.gameContorller)
            .GetComponent<LastPlayerSighting>();
        nav = GetComponent<NavMeshAgent>();
        player       = GameObject.FindGameObjectWithTag(Tags.player).GetComponent<Transform>();    
        playerHealth = player.gameObject.GetComponent<PlayerHealth>();


    }

    void Update()
    { 
        //射击动作最优
        if(playerHealth.health > 0 && enemySight.playerSight )
        {
            Shooting();
            
        }
        //如果玩家被发现(被听见或触发警报) 且生命值大于0
        else if (enemySight.personalLastSighting != lastPlayerSighting.resetPosition && playerHealth.health > 0f)
        {
            Chasing();
        }
        else
        { 
            //巡逻
            Patrolling();
        }
    }

    void Shooting()
    { 
        //射击时候敌人不许移动
        nav.Stop();
    }

    void Chasing()
    {
        //从敌人当前位置到最后发现玩家位位置 创建一个向量 
        Vector3 sightingDeltaPos = player.position - transform.position;

        //如果玩家距离敌人较远
        if (sightingDeltaPos.sqrMagnitude > 4f)
        {
            // 让敌人跑向追踪位置
            nav.destination = enemySight.personalLastSighting;
        }

        // 移动速度设为追踪速度
        nav.speed = chaseSpeed;

        // 当敌人与目标点的距离 小于 可停止距离
        if (nav.remainingDistance < nav.stoppingDistance)
        {
            // 追踪等待时间的计时器开始计时
            chaseTime += Time.deltaTime;

            //当敌人等待了足够的时间时
            if (chaseTime >= chaseWaitTime)
            {
                //重置时间
                chaseTime = 0f;
                lastPlayerSighting.position = lastPlayerSighting.resetPosition;
                enemySight.personalLastSighting = lastPlayerSighting.resetPosition;
            }

        }
        else
        {
            //如果敌人没有靠近玩家最后出现的位置 那么重置计时器
            //（因为追踪位置不是固定的 玩家可能还会在其他位置触发警报 从而刷新追踪位置）
            chaseTime = 0f;
        }
    }

    void Patrolling()
    {
        // 移动速度等于巡逻速度
        nav.speed = patrolSpeed;

        //如果没有目标点 或者 已接近目标巡逻点
        if (nav.destination == lastPlayerSighting.resetPosition || nav.remainingDistance < nav.stoppingDistance)
        {
            // 巡逻等待时间的计时器开始计时
            patrolTime += Time.deltaTime;

            //等待时间过后
            if(patrolTime >= patrolWaitTime)
            {
                //移动索引至下一个位置
                wayPointIndex = ++wayPointIndex % patrolWayPoints.Length;
                // 重置计时器
                patrolTime = 0f;
            }
        }
        else
        {
            // 如果没有靠近任何一个目标点 重置计时器
            patrolTime = 0f;
        }
        // 目标点设为路径点
        nav.destination = patrolWayPoints[wayPointIndex].position;
    }
}
