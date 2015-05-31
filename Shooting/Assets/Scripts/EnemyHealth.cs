/***
 * 
 *      ProjectName:NightShooting
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

public class EnemyHealth : MonoBehaviour {
    /// <summary>
    /// 敌人的血量
    /// </summary>
    public float hp = 100;
    /// <summary>
    /// 死亡的声音
    /// </summary>
    public AudioClip mAudioDead;
    /// <summary>
    /// 受到伤害的声音
    /// </summary>
    public AudioClip mAudioHurt;

    /// <summary>
    /// 导航组件
    /// </summary>
    private NavMeshAgent mAgent;
    /// <summary>
    /// 敌人的动画状态机
    /// </summary>
    private Animator mAnim;

    /// <summary>
    /// 敌人移动主键
    /// </summary>
    private EnemyMove mEnemyMove;

    /// <summary>
    /// 中枪后的效果
    /// </summary>
    private ParticleSystem mHurtParticleSys;

    /// <summary>
    /// 碰撞器
    /// </summary>
    private CapsuleCollider mCollider;

	/// <summary>
	/// 攻击组件
	/// </summary>
	private EnemyAttack mAttack;

    /// <summary>
    /// 游戏管理对象
    /// </summary>
    private GameManager mGameManager;

    /// <summary>
    /// 分数
    /// </summary>
    public float EnemySource = 5f;



    void Awake()
    {
        mAnim = this.GetComponent<Animator>();
        mEnemyMove = this.GetComponent<EnemyMove>();
        mAgent = this.GetComponent<NavMeshAgent>();
        mCollider = this.GetComponent<CapsuleCollider>();
        mHurtParticleSys = this.GetComponentInChildren<ParticleSystem>();
		mAttack = this.GetComponent<EnemyAttack> ();
        mGameManager = GameObject.FindGameObjectWithTag(Tags.GameManagerTagName).GetComponent<GameManager>();
    }

	// Update is called once per frame
	void Update () {
        if (hp <= 0) {
            transform.Translate(Vector3.down * Time.deltaTime);
        }
        if (transform.position.y <= -6)
        {
            Destroy(this.gameObject);
        }
	}

    

    public void TakeGamage(float Damage,Vector3 point)
    {
        if (hp <= 0)
        {
            return;
        }

        hp -= Damage;

        if (hp <= 0) {
            Dead();
            return;
        }
        if (mAudioHurt != null)
        {
            AudioSource.PlayClipAtPoint(mAudioHurt,transform.position);
        }
        mHurtParticleSys.transform.position = point;
        mHurtParticleSys.Play();

    }

    void Dead()
    {
        mAnim.SetBool("Dead",true);
        mAgent.enabled = false;
        //mEnemyMove.enabled = false;
		//死后无法攻击
		mAttack.enabled = false;
        mCollider.enabled = false;
        if (mAudioDead != null)
        {
            AudioSource.PlayClipAtPoint(mAudioDead, transform.position);
        }
        //添加分数
        mGameManager.AddSource(EnemySource);
    }
}
