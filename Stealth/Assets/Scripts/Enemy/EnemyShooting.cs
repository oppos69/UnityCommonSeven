/***
 * 
 *      ProjectName:Stealth
 * 
 *      Anthor     :wangchong
 * 
 *      CreateTime: 2015-05-01
 * 
 *      DESC       ：敌人射击，在发现主角后，向主角的腹部射击，距离越近伤害越高（射击音效，射击动画，弹道轨迹）
 * 
 *      
 */
using UnityEngine;
using System.Collections;

public class EnemyShooting : MonoBehaviour {
                                                                               //
    public float maximumDamage  = 120f;                                        //最大射击伤害
    public float minimumDamage  = 45f;                                         //最小射击伤害
    public float flashIntensity = 3f;                                          //射击光照强度
    public float fadeSpeed     = 10f;                                          //射击光照强度变化速率
    public AudioClip shotClip;                                                 //射击音效


    private Animator anim;                                                     //引用敌人动画组件
    private LineRenderer laserShotLine;                                        //引用LineRenderer组件
    private Light laserShotLight;                                              //引用Light 组件
    private SphereCollider col;                                                //引用ShpereCollider组件
    private Transform player;                                                  //引用Player Transform组件
    private PlayerHealth playerHealth;                                         //引用 PlayerHealth组件
    private HashIDs hash;                                                      //引用 HashIDs 组件
    private bool shooting;                                                     //判断是否射击
    private float scaledDamage;                                                //判断射击伤害范围


    void Awake()
    {
        //获取组件
        anim           = GetComponent<Animator>();                               //
        laserShotLine  = GetComponentInChildren<LineRenderer>();                 //
        laserShotLight = laserShotLine.gameObject.GetComponent<Light>();                         //
        col            = GetComponent<SphereCollider>();                         //
        player         = GameObject.FindGameObjectWithTag(Tags.player).transform;//
        playerHealth   = player.gameObject.GetComponent<PlayerHealth>();         //
        hash           = GameObject.FindGameObjectWithTag(Tags.gameContorller)   //
            .GetComponent<HashIDs>();                                            //

        
        shooting     = false;                                                    //
        //射击伤害范围 = 射击最大伤害 - 射击最小伤害
        scaledDamage = maximumDamage - minimumDamage;                            //

        //游戏初始时 关闭line renderer和light组件
        laserShotLight.intensity = 0;
        laserShotLine.enabled = false;
    }


    void Update()
    { 
        //获取射击动画曲线值
        // 缓存shot curve当前的参数值
        float shot = anim.GetFloat(hash.shotFloat);

        //判断曲线值是否在射击范围中，在范围中只射击一次
        if (shot >= 0.5f && !shooting)
        {
            shoot();
        }
        
        //如果峰值小于0.5f那么
        if(shot < 0.5f)
        {
            //敌人就不在射击了且关闭line renderer组件
            shooting = false;
            laserShotLine.enabled = false;
        }

        // 让枪口灯光由当前强度值渐变至0
        laserShotLight.intensity = Mathf.Lerp(laserShotLight.intensity, 0f, fadeSpeed * Time.deltaTime);
    }

    void OnAnimatorIK()
    {
        // 缓存AimWeight curve当前的参数值
        float aimWeight = anim.GetFloat(hash.aimWeightFloat);
        // 让敌人右手指向玩家中心位置
        anim.SetIKPosition(AvatarIKGoal.RightHand, player.position);
        // 设置IK的权重等于缓存的aimWeight参数值
        anim.SetIKPositionWeight(AvatarIKGoal.RightHand, aimWeight);
    }

    void shoot()
    { 
        //敌人射击了
        shooting = true;

        //计算伤害
        //  计算计算枪口距离主角的距离
        float fractionalDistance = (col.radius - Vector3.Distance(transform.position, player.position)) / col.radius;

        //  计算伤害
        float damage = scaledDamage * fractionalDistance;

        //  给玩家照成伤害
        playerHealth.TakeDamage(damage);

        //显示射击效果
        shootEffect();
    }
    /// <summary>
    /// 显示射击效果
    /// </summary>
    void shootEffect()
    { 
        //射击弹道
        laserShotLine.SetPosition(0,transform.position);
        //玩家的腹部
        laserShotLine.SetPosition(1, player.position + Vector3.up * 1.5f);
        //启用组件
        laserShotLine.enabled = true;

        //射击枪口光
        laserShotLight.intensity = flashIntensity;
        //射击声音
        AudioSource.PlayClipAtPoint(shotClip, laserShotLight.transform.position);

    }
}
