/***
*		ProjectName:NightShooting
*
*		Author	   :wangchong
*
*		CreateTime :2014-11-21
*
*		History    :敌人出生点
*
*/
using UnityEngine;
using System.Collections;

public class BornEnemy : MonoBehaviour {

	/// <summary>
	/// 出生的时间（秒）
	/// </summary>
	public float BornTime = 5f;

	/// <summary>
	/// 衰减率
	/// </summary>
	public float DecayRate = 0.05f;

	/// <summary>
	/// 衰减周期
	/// </summary>
	public float DecayCyc = 10f;

	/// <summary>
	/// enemy.
	/// </summary>
	public GameObject Enemy;

	/// <summary>
	/// 计时器
	/// </summary>
	public float Timer = 5;

	/// <summary>
	/// 衰减计时器
	/// </summary>
	private float DecayTimer = 0;

    /// <summary>
    /// 主角健康状态
    /// </summary>
    private PlayerHealth mPlayerH;

    void Awake()
    { 
        mPlayerH = GameObject.FindGameObjectWithTag(Tags.PlayerTagName).GetComponent<PlayerHealth>();
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        //主角死了就不要再生产僵尸
        if (mPlayerH.hp <= 0)
        {
            return;
        }
        
		Timer += Time.deltaTime;
		
		if (Timer >= BornTime) {
			Instantiate(Enemy,this.transform.position,Quaternion.identity);
			Timer-=BornTime;
		}

		DecayTimer += Time.deltaTime;
		if (DecayTimer >= DecayCyc) {
			BornTime = BornTime * (1-DecayRate);
			DecayCyc = DecayCyc* (1-DecayRate);
			DecayTimer -= DecayCyc;
		}
	}
}
