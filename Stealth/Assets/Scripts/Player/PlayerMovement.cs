/***
 * 
 *      ProjectName:Stealth
 * 
 *      Anthor     :wangchong
 * 
 *      CreateTime: 2014-11-21
 * 
 *      DESC       ：控制玩家移动
 * 
 *      
 */
using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour {

    public AudioClip shoutingClip;                                             //叫声
    public float turnSmoothing = 15f;                                          //玩家平滑转向的速度
    public float speedDampTime = 0.1f;                                         //速度缓冲时间
                                                                               //
    private Animator anim;                                                     //Animator组件引用
    private HashIDs hash;                                                      //HashIDs脚本引用

    void Awake()
    {
        anim = this.GetComponent<Animator>();
        hash = GameObject.FindGameObjectWithTag(Tags.gameContorller).GetComponent<HashIDs>();
    }

    void FixedUpdate()
    { 
        //缓存用户输入                                                          //
        float h = Input.GetAxis("Horizontal");                                  //横向移动建
        float v = Input.GetAxis("Vertical");                                    //纵向移动建
        bool sneak = Input.GetButton("Sneak");                                  //潜行建

        MovementManager(h,v,sneak);
    }

    void Update()
    {
        bool shout = Input.GetButtonDown("Attract");

        anim.SetBool(hash.shoutingBool, shout); 

        AudioManger(shout);
    }

    /// <summary>
    /// 主角的移动
    /// </summary>
    /// <param name="horizontal"></param>
    /// <param name="vertical"></param>
    /// <param name="sneak"></param>
    void MovementManager(float horizontal, float vertical,bool sneak)
    {
        // 设置 animator 中的 sneaking 参数
        anim.SetBool(hash.sneakingBool, sneak);                                   //函数参数解释 anim.SetBool(sneaking当前值,  改变后的值)
        // 如果横向或纵向按键被按下 也就是说角色处于移动中
        if (horizontal != 0 || vertical != 0)
        {
            Rotating(horizontal, vertical);
            anim.SetFloat(hash.speedFloat,5.5f,speedDampTime,Time.deltaTime);
        }
        else
        {
            anim.SetFloat(hash.speedFloat,0);
        }
    }

    /// <summary>
    /// 控制主角旋转
    /// </summary>
    /// <param name="horizontal"></param>
    /// <param name="vertical"></param>
    void Rotating(float horizontal,float vertical)
    { 
        //创建角色的目标向量
        Vector3 tagetDirection = new Vector3(horizontal, 0, vertical);

        // 创建目标旋转值 并假设Y轴正方向为"上"方向
        Quaternion targetRotation = Quaternion.LookRotation(tagetDirection,Vector3.up);

        // 创建新旋转值 并根据转向速度平滑转至目标旋转值
        Quaternion newRotation = Quaternion.Lerp(GetComponent<Rigidbody>().rotation,targetRotation,turnSmoothing* Time.deltaTime);

        GetComponent<Rigidbody>().MoveRotation(newRotation);
    }

    void AudioManger(bool shout)
    { 
        //如果角色移动播放脚步声
        if (anim.GetCurrentAnimatorStateInfo(0).nameHash == hash.locomotionState)
        {
            if (!GetComponent<AudioSource>().isPlaying)
            {
                GetComponent<AudioSource>().Play();
            }
        }
        else
        {
            GetComponent<AudioSource>().Stop();
        }

        //如果玩家按下喊叫建
        if (shout)
        {
            // 播放指定的喊叫声
            AudioSource.PlayClipAtPoint(shoutingClip,transform.position);
        }
    }
}
