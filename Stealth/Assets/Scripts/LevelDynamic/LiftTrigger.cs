/***
 * 
 *      ProjectName:Stealth
 * 
 *      Anthor     :wangchong
 * 
 *      CreateTime: 2015-05-01
 * 
 *      DESC       ：最后触发通关
 * 
 *      
 */
using UnityEngine;
using System.Collections;

public class LiftTrigger : MonoBehaviour
{
    private float timeToDoorsClose = 2f;                                       //进入电梯后关门的时间
    private float timeToLiftStart  = 3f;                                       //进入电梯后电梯启动时间
    private float timeToEndLevel   = 6f;                                       //关卡结束时间
    private float liftSpeed        = 3f;                                       //电梯上升速度

    private GameObject player;                                                 //玩家对象
    private Animator playerAnim;                                               //角色的animator组件
    private HashIDs hash;                                                      //Hashids脚本
    private CameraMovement camMovement;                                        //
    private SceneFaderInOut sceneFadeInOut;                                    //
    private LiftDoorsTracking liftDoorsTracking;                               //
    private bool playerInLift;                                                 //
    private float timer;                                                       //


    void Awake()
    {
        //获取引用对象
        player     = GameObject.FindGameObjectWithTag(Tags.player);            //
        playerAnim = player.GetComponent<Animator>();                          //
        hash       = GameObject.FindGameObjectWithTag(Tags.gameContorller)     //
            .GetComponent<HashIDs>();                                          //
        camMovement = Camera.main.gameObject.GetComponent<CameraMovement>();   //
        sceneFadeInOut = GameObject.FindGameObjectWithTag(Tags.fader)          //
            .GetComponent<SceneFaderInOut>();                                  //
        liftDoorsTracking = GetComponent<LiftDoorsTracking>();                 //
    }

    void OnTriggerEnter(Collider other)
    {
        //如果玩家进入触发器里面
        if (other.gameObject == player)
        {
            //判定玩家在电梯里面
            playerInLift = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject == player)
        {
            playerInLift = false;
            timer = 0;
        }
    }

    void Update()
    {
        if (playerInLift)
        {
            LiftActivation();
        }
        //如果计时器小于应关闭内层电梯的时间
        if (timer < timeToDoorsClose)
        {
            //让内侧门随这外侧们关闭
            liftDoorsTracking.DoorFollowing();
        }
        else
        {
            //直接关闭内侧门
            liftDoorsTracking.CloseDoors();
        }
    }

    void LiftActivation()
    { 
        //玩家进入电梯开始计时
        timer += Time.deltaTime;
        //如果计时器大于等于电梯应启动时间
        if (timer >= timeToLiftStart)
        { 
            //角色速度设为0 ，摄像机无法移动 让角色成为电梯的子物体
            playerAnim.SetFloat(hash.speedFloat,0);
            camMovement.enabled = false;
            player.transform.parent = transform;

            //让电梯上升移动
            transform.Translate(Vector3.up * liftSpeed * Time.deltaTime);

            //播放电梯声音
            if (!GetComponent<AudioSource>().isPlaying)
                GetComponent<AudioSource>().Play();

            //如果计时器 大于等于 该场景结束
            if (timer >= timeToEndLevel)
            {
                sceneFadeInOut.EndScene();
            }
        }
        //
    }
}
