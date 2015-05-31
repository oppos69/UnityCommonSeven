/***
 * 
 *      ProjectName:Stealth
 * 
 *      Anthor     :wangchong
 * 
 *      CreateTime: 2014-11-21
 * 
 *      DESC       ：开关激光门
 * 
 *      
 */
using UnityEngine;
using System.Collections;

public class LaserSwitchDeactivation : MonoBehaviour {

    public GameObject laser;                                                                             //开关将控制的激光门对象的引用
    private GameObject player;                                                                           //玩家对象引用
    public Material unLockedMat;                                                                         //开关屏幕 提示激光门解锁标志的材质

    void Awake()
    {
        //获取玩家
        player = GameObject.FindGameObjectWithTag(Tags.player) as  GameObject;

    }

    void OnTriggerStay(Collider other)
    { 
        //如果探测到玩家进入开关范围
        if (other.gameObject == player)
        {
            //用户按下switch按钮
            if (Input.GetButton("Switch"))
            {
                LaserSwitch();
            }
        }
    }

    void LaserSwitch()
    { 
        //禁用laser
        laser.SetActive(false);

        //获取屏幕渲染
        Renderer screen = transform.Find("prop_switchUnit_screen").GetComponent<Renderer>();
        
        //跟换材质
        screen.material = unLockedMat;

        //播放关闭声音
        GetComponent<AudioSource>().Play();
    }
}
