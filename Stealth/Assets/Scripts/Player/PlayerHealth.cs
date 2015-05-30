/***
 * 
 *      ProjectName:Stealth
 * 
 *      Anthor     :wangchong
 * 
 *      CreateTime: 2014-11-21
 * 
 *      DESC       ：主角的健康状态
 * 
 *      
 */
using UnityEngine;
using System.Collections;

public class PlayerHealth : MonoBehaviour {

	//主角的HP
    public float health = 100f;
    //死亡的声音剪辑
    public AudioClip deathClip;
    //死亡后延迟重新加载关卡时间
    public float resetAfterDesthTime = 5f;

    //是否死亡
    private bool playerDead;
    //动画组件
    private Animator anim;
    //GameManager报警组件
    private LastPlayerSighting lastPlayerSighting;
    //Movement组件
    private PlayerMovement playerMovement;
    //hashIDs组件
    private HashIDs hash;
    //Srceent淡出组件
    private SceneFaderInOut sceneFaderInOut;
    //死亡计时器
    private float timer;

    void Awake()
    {
        anim = GetComponent<Animator>();
        lastPlayerSighting = GameObject.FindGameObjectWithTag(Tags.gameContorller).GetComponent<LastPlayerSighting>();
        playerMovement = GetComponent<PlayerMovement>();
        hash = GameObject.FindGameObjectWithTag(Tags.gameContorller).GetComponent<HashIDs>();
        sceneFaderInOut = GameObject.FindGameObjectWithTag(Tags.fader).GetComponent<SceneFaderInOut>();
    }

    void Update()
    {
        if (health <= 0)
        {
            if (!playerDead)
            {
                PlayerDying();
            }
            else
            {
                PlayerDead();
                LevelReset();
            }
        }
    }

    void PlayerDying()
    { 
        //
        playerDead = true;

        //播放死亡动画
        anim.SetBool(hash.deadBool,true);

        //播放死亡声音
        AudioSource.PlayClipAtPoint(deathClip,transform.position);
    }

    void PlayerDead()
    { 
        //确定动画只播放一次
        if (anim.GetCurrentAnimatorStateInfo(0).nameHash == hash.deadBool)
        {
            anim.SetBool(hash.deadBool,false);
        }
        //把速度设为0
        anim.SetFloat(hash.speedFloat,0);
        //停止移动
        playerMovement.enabled = false;

        //停止报警
        lastPlayerSighting.position = lastPlayerSighting.resetPosition;

        //停止播放脚步声音
        GetComponent<AudioSource>().Stop();


    }

    void LevelReset()
    {
        timer += Time.deltaTime;

        if (timer >= resetAfterDesthTime)
        {
            sceneFaderInOut.EndScene();
        }

    }

    public void TakeDamage(float amout)
    {
        if (health >= 0)
        {
            health -= amout;
        }
    }

}
