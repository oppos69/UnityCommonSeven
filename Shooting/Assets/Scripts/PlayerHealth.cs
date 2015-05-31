/***
 * 
 *      ProjectName:NightShooting
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
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour {
    /// <summary>
    /// 主角的血量
    /// </summary>
    public float hp = 100;

    /// <summary>
    /// 定义动画状态机
    /// </summary>
    private Animator mAnim;

    /// <summary>
    /// 定义移动脚本组件
    /// </summary>
    private MovePlayer mMovePlayer;

    /// <summary>
    /// 受到伤害要改得材质
    /// </summary>
    private SkinnedMeshRenderer render;

    /// <summary>
    /// 游戏管理对象
    /// </summary>
    private GameManager mGameManager;

    /// <summary>
    /// 死亡声音
    /// </summary>
    public AudioClip mAudioDead;

    /// <summary>
    /// 受到伤害声音
    /// </summary>
    public AudioClip mAudioHurt;

    /// <summary>
    /// 用于显示的HP
    /// </summary>
    private Text mPlayerHP;

    private float smoothing = 3;

    void Awake() {
        mAnim = this.GetComponent<Animator>();
        mMovePlayer = this.GetComponent<MovePlayer>();
        render = this.transform.Find(Tags.PlayerTagName).renderer as SkinnedMeshRenderer;
        mGameManager = GameObject.FindGameObjectWithTag(Tags.GameManagerTagName).GetComponent<GameManager>();
        mPlayerHP = GameObject.Find("Canvas/LifeHP").GetComponent<Text>();
    }

    void Update()
    {

        render.material.color = Color.Lerp(render.material.color, Color.white, smoothing*Time.deltaTime);    //将受到伤害的的颜色变为白色
    }

    /// <summary>
    /// 受到伤害
    /// </summary>
    /// <param name="Damage"></param>
    public void TakeDamage(float Damage)
    {
        if (hp <= 0) {
            return;
        }

        hp -= Damage;
        //显示PH
        mPlayerHP.text = hp.ToString();

        render.material.color = Color.red;
        if (hp <= 0) {
            mAnim.SetBool("Dead", true);
            Death();
            return;
        }

        AudioSource.PlayClipAtPoint(mAudioHurt, this.transform.position);                                    //播放受到伤害的声音
    }
    /// <summary>
    /// 死后动作
    /// </summary>
    void Death()
    {
        
        mMovePlayer.enabled = false;
        AudioSource.PlayClipAtPoint(mAudioDead,this.transform.position,1f);
        mGameManager.PlayerDead();
    }
}
