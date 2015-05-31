/***
 * 
 *      ProjectName:Stealth
 * 
 *      Anthor     :wangchong
 * 
 *      CreateTime: 2015-05-02
 * 
 *      DESC       ：动画辅助类
 * 
 *      
 */
using UnityEngine;
using System.Collections;

public class AnimatorSetup  {

    public float speedDampTime     = 0.1f;                                     //速度缓冲时间
    public float AngularSpeedTime  = 0.7f;                                     //角速度缓冲时间
    public float angleResponseTime = 0.6f;                                     //把角度转变成角速度

    private Animator anim;                                                     //引用Animator组件
    private HashIDs hash;                                                      //引用HashIDs组件

    public AnimatorSetup(Animator animator, HashIDs hashIDs)
    {
        //获得引用对象
        anim = animator;
        hash = hashIDs;
    }

    public void Setup(float speed ,float angle)
    { 
        //角速度 = 角度 / 角度旋转的时间
        angleResponseTime = AngularSpeedTime / angle;
        //设置动画参数 并设置缓冲时间
        anim.SetFloat(hash.speedFloat,speed,speedDampTime,Time.deltaTime);
        anim.SetFloat(hash.angularSpeedFloat,angle,AngularSpeedTime,Time.deltaTime);
    }
}
