/***
 * 
 *      ProjectName:NightShooting
 * 
 *      Anthor     :wangchong
 * 
 *      CreateTime: 2014-11-21
 * 
 *      DESC       ：敌人的移动
 * 
 *      
 */
using UnityEngine;
using System.Collections;

public class EnemyMove : MonoBehaviour {

    /// <summary>
    /// 导航组件
    /// </summary>
    private NavMeshAgent mAgent;

    /// <summary>
    /// 主角的位置
    /// </summary>
    private Transform mTrfPlayer;

    /// <summary>
    /// 敌人是否死亡
    /// </summary>
    private EnemyHealth mEnemyHealth;

    /// <summary>
    /// 敌人动画状态机
    /// </summary>
    private Animator mAnim;

    void Awake()
    {
        mAgent = this.GetComponent<NavMeshAgent>();
        mTrfPlayer = GameObject.FindGameObjectWithTag(Tags.PlayerTagName).transform;
        mAnim = this.GetComponent<Animator>();
        mEnemyHealth = GetComponent<EnemyHealth>();
    }

	// Update is called once per frame
	void Update () {
        if (mEnemyHealth.hp <= 0)
        {
            
            //mAgent.Stop();
            mAgent.enabled = false;
            return;
        }

        mAgent.SetDestination(mTrfPlayer.position);

        if (Vector3.Distance(transform.position, mTrfPlayer.position) > 1.5f)
        {
            mAnim.SetBool("Move", true);
           
        }
        else
        {
            mAnim.SetBool("Move", false);
            mAgent.Stop();
        }
	}
}
