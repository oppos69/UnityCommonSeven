/***
 * 
 *      ProjectName:Stealth
 * 
 *      Anthor     :wangchong
 * 
 *      CreateTime: 2014-11-21
 * 
 *      DESC       ：门的自动开关
 * 
 *      
 */
using UnityEngine;
using System.Collections;

public class DoorAnimation : MonoBehaviour {

    public AudioClip doorSwishClip;                                                  //门的开关声音
    public AudioClip accessDeniedClip;                                               //不能通过的声音
    public bool requireKey;                                                          //是否需要key

    private Animator anim;                                                            //动画组件
    private HashIDs hash;                                                             //动画hash值
    private HasKeycard hasKeycard;                                                    //是否有钥匙
    private AudioSource audio;                                                        //声音组件
    private int count;                                                                //同时接触到门的游戏对象
    private GameObject player;                                                        //主角

    void Awake()
    {
        anim       = GetComponent<Animator>();
        hash       = GameObject.FindGameObjectWithTag(Tags.gameContorller)
            .GetComponent<HashIDs>();
        player     = GameObject.FindGameObjectWithTag(Tags.player);
        hasKeycard = player.GetComponent<HasKeycard>();
        audio      = GetComponent<AudioSource>();
    }

    void OnTriggerEnter(Collider other)
    {
        //如果是主角碰到
        if (other.gameObject == player)
        {
            //判断是否需要钥匙才能开门
            if (requireKey)
            {
                //如果有钥匙count+1；
                if (hasKeycard.hasKeycard)
                {
                    count++;
                }
                    //没有就播放不能通过声音
                else
                {
                    audio.clip = accessDeniedClip;
                    audio.Play();
                }
            }
            else
            {
                count++;
            }

        }
        else if (other.tag == Tags.enemy)
        {
            if (other is CapsuleCollider)
            {
                count++;
            }
        }

    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject == player || (other.tag == Tags.enemy && other is CapsuleCollider))
        {
            
            count = Mathf.Max(0,count-1);
        }
    }

    void Update()
    {
        //如果count >0说明附近有人 需要开门
        anim.SetBool(hash.openBool, count > 0);
        //如果在播放动画并且没有播放声音 ，就不放声音
        if (anim.IsInTransition(0) && !audio.isPlaying)
        {
            audio.clip = doorSwishClip;
            audio.Play();

        }
    }
}
