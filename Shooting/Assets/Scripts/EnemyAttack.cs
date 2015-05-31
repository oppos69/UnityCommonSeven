/***
 * 
 *      ProjectName:NightShooting
 * 
 *      Anthor     :wangchong
 * 
 *      CreateTime: 2014-11-21
 * 
 *      DESC       ：敌人攻击
 * 
 *      
 */
using UnityEngine;
using System.Collections;

public class EnemyAttack : MonoBehaviour {

    /// <summary>
    /// 主角健康状态
    /// </summary>
    private PlayerHealth mPlayerHealth;

    /// <summary>
    /// 触碰时长
    /// </summary>
    private float Timer = 0f;

    /// <summary>
    /// 攻击力
    /// </summary>
    public float Gamage = 20;

    /// <summary>
    /// 攻击前延（抬手）
    /// </summary>
    public float BeforAttackTime = 0.5f;

    /// <summary>
    /// 攻击后延（收手）
    /// </summary>
    public float AfterAttackTime = 0.5f;

    /// <summary>
    /// 攻击时间里是否攻击
    /// </summary>
    private bool IsAttack = false;
    void Awake()
    {
        mPlayerHealth = GameObject.FindGameObjectWithTag(Tags.PlayerTagName).GetComponent<PlayerHealth>();


    }

	// Use this for initialization
	void Start () {
	    
	}

    void OnTriggerStay(Collider other)
    {
        if (mPlayerHealth.hp <= 0)                                                                           //如果hp小于等于0将无法攻击
        {                                                                                                    //
            return;                                                                                          //
        }                                                                                                    //
        if (other.tag == Tags.PlayerTagName)                                                                 //判断碰到的是不是敌人
        {                                                                                                    //
            Timer += Time.deltaTime;                                                                         //加上时间器
                                                                                                             //
            if (Timer >= BeforAttackTime && !IsAttack)                                                       //
            {                                                                                                //
                IsAttack = true;                                                                             //
                mPlayerHealth.TakeDamage(Gamage);                                                            //
            }                                                                                                //
            else if (Timer >= BeforAttackTime + AfterAttackTime)                                             //
            {                                                                                                //
                IsAttack = false;                                                                            //
                Timer -= BeforAttackTime + AfterAttackTime;                                                  //
            }
        }
    }
	
	// Update is called once per frame
	void Update () {
	    
	}
}
