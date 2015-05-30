/***
 * 
 *      ProjectName:Stealth
 * 
 *      Anthor     :wangchong
 * 
 *      CreateTime : 2015-05-02
 * 
 *      DESC       ：
 * 
 *      
 */
using UnityEngine;
using System.Collections;

public class EnemySight : MonoBehaviour {

    public float fieldofViewAngle = 110f;                                         //敌人的视角范围
    public bool playerSight;                                                      //判断敌人是否看见玩家
    public Vector3 personalLastSighting;                                          //每个敌人独立的变量 用于储存敌人听到玩家脚步声/喊叫声的位置

    private NavMeshAgent nav;                                                     //引用Nav组件
    private Animator anim;                                                       //引用敌人动画组件
    private GameObject player;                                                    //引用主角游戏对象
    private LastPlayerSighting lastPlayerSighting;                                //引用最后发现敌人的位置
    private SphereCollider col;                                                   //引用sphere Collider触发器（看见或听见）
    private HashIDs hash;                                                         //引用动画hashid
    private PlayerHealth playerHealth;                                            //引用主角健康状态
    private Animator playerAnim;                                                  //引用主角动画组件
    private Vector3 previousSighting;                                             //上一帧玩家被发现的位置

    void Awake()
    {
        //获得引用对象
        nav                = GetComponent<NavMeshAgent>();                        //
        anim               = GetComponent<Animator>();                            //
        player             = GameObject.FindGameObjectWithTag(Tags.player);       //
        playerAnim         = player.GetComponent<Animator>();                     //
        playerHealth       = player.GetComponent<PlayerHealth>();
        col                = GetComponent<SphereCollider>();
        lastPlayerSighting = GameObject.FindGameObjectWithTag(Tags.gameContorller)//
            .GetComponent<LastPlayerSighting>();                                  //
        hash = GameObject.FindGameObjectWithTag(Tags.gameContorller)              //
            .GetComponent<HashIDs>();                                             //
        
                                                                      
        //将发现敌人的位置都重置
        previousSighting = lastPlayerSighting.resetPosition;                      //
        personalLastSighting = lastPlayerSighting.resetPosition;                  //
    }

    void Update()
    {
        //如果上一次发现敌人的位置不为默认位置，则将发现敌人的坐标赋值
        if (previousSighting != lastPlayerSighting.position)
            personalLastSighting = lastPlayerSighting.position;
        //如果其他“朋友、CCTV、激光门”发现敌人，则赋值上一帧发现
        previousSighting = lastPlayerSighting.position;

        // 如果玩家HP值小于0 也就是玩家死了
        if (playerHealth.health <= 0)
        {
            // 否则设为false
            anim.SetBool(hash.playerInSightBool,false);

        }
        else
        {
            // 把playerInSight的值传递到Animator中相应的参数 也就是设为true
            anim.SetBool(hash.playerInSightBool,playerSight);
        }
    }

    void OnTriggerStay(Collider other)
    {
        //如果玩家进入触发器范围内
        if (other.gameObject == player)
        { 
            //默认玩家没有被发现
            playerSight = false;

            //判断敌人是否看见玩家
            //  创建敌人到玩家的向量，然后获取他与敌人前方向量的夹角
            Vector3 direction = player.transform.position - transform.position;
            float angle = Vector3.Angle(direction,transform.forward);

            //  夹角是否在如果夹角小于敌人视角的二分之一
            if (angle <= fieldofViewAngle * 0.5f)
            { 
                //判断敌人与主角之间是否有障碍物
                RaycastHit hit;
                
                //如果敌人与主角方向一定距离内发现障碍物
                if (Physics.Raycast(transform.position + transform.up, direction,out hit, col.radius))
                { 
                    //判断是否是主角
                    if (hit.collider.gameObject == player)
                    {
                        //玩家被发现
                        playerSight = true;
                        //让玩家最后被发现的位置的全局变量 等于玩家当前位置
                        lastPlayerSighting.position = player.transform.position;
                    }
                }
            }
        }

        //如果在范围内玩家行走（非潜行）或这喊叫 ，则敌人过去看看
        int playerLayerZeroStateHash = playerAnim.GetCurrentAnimatorStateInfo(0).nameHash;
        int playerLayerOneStateHash = playerAnim.GetCurrentAnimatorStateInfo(1).nameHash;

        if (playerLayerOneStateHash == hash.shoutState || playerLayerZeroStateHash == hash.locomotionState)
        { 
            //并且玩家在敌人听觉范围以内
            if (CalculatePathLength(player.transform.position) <= col.radius)
            {
                
                //存储发现点
                personalLastSighting = player.transform.position;
            }
        }
    }

    /// <summary>
    /// 计算自己到目标点的距离
    /// </summary>
    /// <param name="targetPosition"></param>
    float CalculatePathLength(Vector3 targetPosition)
    {
        //创建路径 并让路径基于目标位置 (玩家位置)
        NavMeshPath path = new NavMeshPath();
        //如果组件可用
        if (nav.enabled)
        {
            //计算路径
            nav.CalculatePath(targetPosition, path);
        }
        // 创建一个数组 长度等于path.cornners.Length+2
        Vector3 [] allWayPath = new Vector3[path.corners.Length + 2];

        //创建第一个点（自己的位置）
        allWayPath[0] = transform.position;
        //创建最后一个点（目标点）
        allWayPath[allWayPath.Length - 1] = targetPosition;

        //计算路径
        float pathLength = 0;
        //赋值到总路程中
        for (int i = 0; i < path.corners.Length; i++)
        {
            allWayPath[i + 1] = path.corners[i];
        }
        //开始计算总路径中的距离
        for (int i = 0; i < allWayPath.Length - 1; i++)
        {
            pathLength += Vector3.Distance(allWayPath[i], allWayPath[i + 1]);
        }

        return pathLength;
    }

    void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<Collider>().gameObject == player)
        {
            playerSight = false;
        }
    }


}
