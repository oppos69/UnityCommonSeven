/***
 * 
 *      ProjectName:Stealth
 * 
 *      Anthor     :wangchong
 * 
 *      CreateTime: 2014-11-21
 * 
 *      DESC       ：
 * 
 *      
 */
using UnityEngine;
using System.Collections;

public class LaserPlayerDetection : MonoBehaviour {

    private GameObject player;                                                    // 玩家对象引用
    private LastPlayerSighting lastPlayerSighting;                                // lastPlayerSighting变量引用

    void Awake()
    {
        // 获取引用对象
        player = GameObject.FindGameObjectWithTag(Tags.player) as GameObject;
        lastPlayerSighting = GameObject.FindGameObjectWithTag(Tags.gameContorller).GetComponent<LastPlayerSighting>();
    }

    void OnTriggerStay(Collider other)
    {
        // 如果激光门开启
        if (GetComponent<Renderer>().enabled)
        {
            // 并且探测到玩家时
            if (other.gameObject == player)
            {
                // 更新最后发现玩家的位置 为 当前位置
                lastPlayerSighting.position = other.transform.position;
            }
        }
    }

   
}
