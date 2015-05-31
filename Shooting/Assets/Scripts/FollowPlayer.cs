/***
 * 
 *      ProjectName:NightShooting
 * 
 *      Anthor     :wangchong
 * 
 *      CreateTime: 2014-11-21
 * 
 *      DESC       ：跟随主角
 * 
 *      
 */
using UnityEngine;
using System.Collections;

public class FollowPlayer : MonoBehaviour {

    /// <summary>
    /// 流畅的程度
    /// </summary>
    public float smoothing = 3;

    /// <summary>
    /// 定义主角Transform组件
    /// </summary>
    private Transform mTraPlayer;

    /// <summary>
    /// 定义摄像机的Transform组件
    /// </summary>
    private Transform mTraCamera;

    void Awake()
    {
        mTraPlayer = GameObject.FindGameObjectWithTag(Tags.PlayerTagName).transform;                         //获取主角Transform组件
        mTraCamera = this.transform;                                                                         //获取摄像机的Transform组件

    }

	// Use this for initialization
	void Start () {
	    
	}
	
	// Update is called once per frame
	void FixedUpdate () {

        Vector3 TargerPos = mTraPlayer.position + new Vector3(2,10,-12);                                        //计算摄像机所需要到达的坐标

        mTraCamera.position = Vector3.Lerp(mTraCamera.position, TargerPos, smoothing*Time.deltaTime);         //平滑移动摄像机
	}
}
