/***
 * 
 *      ProjectName:NightShooting
 * 
 *      Anthor     :wangchong
 * 
 *      CreateTime: 2014-11-21
 * 
 *      DESC       ：主角射击
 * 
 *      
 */
using UnityEngine;
using System.Collections;

public class PlayerShooting : MonoBehaviour {

    /// <summary>
    /// 每秒射击多少次
    /// </summary>
    public float shootRate = 3;

    /// <summary>
    /// 抢的威力
    /// </summary>
    public float ShootPower = 30;

    /// <summary>
    /// 记时器
    /// </summary>
    private float Timer = 0;
    /// <summary>
    /// 枪口的光
    /// </summary>
    private Light mTrsLight;

    /// <summary>
    /// 枪口的火药
    /// </summary>
    private ParticleSystem mParticleSys;

    /// <summary>
    /// 射击声音
    /// </summary>
    private AudioSource mAudioShoot;

    /// <summary>
    /// 子弹轨迹
    /// </summary>
    private LineRenderer mShootLine;

    /// <summary>
    /// 获取主角健康状态
    /// </summary>
    private PlayerHealth mPlayerHealth;

    void Awake()
    {
        mTrsLight = this.GetComponent<Light>();
        mParticleSys = this.GetComponentInChildren<ParticleSystem>();
        mShootLine = this.GetComponent<LineRenderer>();
        mAudioShoot = this.GetComponent<AudioSource>();
        mPlayerHealth = GameObject.FindGameObjectWithTag(Tags.PlayerTagName).GetComponent<PlayerHealth>();
    }

    void Update()
    {
        //如果主角死了就不再自动发射
        if (mPlayerHealth.hp <= 0)
        {
            return;
        }

        Timer += Time.deltaTime;
        if (Timer > 1/shootRate)
        {
            Timer -= 1 / shootRate;
            Shooting();
        }
    }
	/// <summary>
	/// 开枪
	/// </summary>
    void Shooting()
    {
        mTrsLight.enabled = true;
        mParticleSys.Play();
        mAudioShoot.Play();
        #region 子弹轨迹
        mShootLine.enabled = true;
        mShootLine.SetPosition(0, transform.position);
        Ray ray = new Ray(transform.position,transform.forward);
        RaycastHit Hit;
        if (Physics.Raycast(ray, out Hit, 100))
        {
            mShootLine.SetPosition(1, Hit.point);
            if (Hit.transform.tag.Equals(Tags.EnemyTagName))
            {
                EnemyHealth health = Hit.transform.GetComponent<EnemyHealth>();
                health.TakeGamage(ShootPower,Hit.point);
            }
        }
        else
        {
            mShootLine.SetPosition(1, transform.position + transform.forward * 100);
        }

        #endregion

        Invoke("ClearEffect", 0.05f);
    }

    void ClearEffect()
    {
        mTrsLight.enabled = false;
        mShootLine.enabled = false;
    }
}
