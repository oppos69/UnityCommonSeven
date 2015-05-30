/***
 * 
 *      ProjectName:Stealth
 * 
 *      Anthor     :wangchong
 * 
 *      CreateTime: 2014-11-21
 * 
 *      DESC       ：钥匙
 * 
 *      
 */
using UnityEngine;
using System.Collections;

public class KeyPickup : MonoBehaviour {

    public AudioClip keyGrab;                                   //捡起钥匙的声音

    private GameObject player;                                  //主角
    private HasKeycard hasKeycard;                              //是否有钥匙

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag(Tags.player);//获取主角
                                                               //
        hasKeycard = player.GetComponent<HasKeycard>();        //获取是否带有钥匙主键
    }

    void OnTriggerEnter(Collider other)
    {
        //判断碰到钥匙的是不是主角
        if (other.gameObject == player)
        {
            //播放捡到钥匙的声音
            AudioSource.PlayClipAtPoint(keyGrab,transform.position);
            //设置主角捡到钥匙
            hasKeycard.hasKeycard = true;
            //销毁钥匙
            Destroy(this.gameObject);
        }
    }

}
