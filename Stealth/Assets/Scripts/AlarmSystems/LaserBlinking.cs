/***
 * 
 *      ProjectName:Stealth
 * 
 *      Anthor     :wangchong
 * 
 *      CreateTime: 2014-11-21
 * 
 *      DESC       ：激光门闪烁
 * 
 *      
 */
using UnityEngine;
using System.Collections;

public class LaserBlinking : MonoBehaviour {

    public float onTime;                                                                                    //激光门开启时间
    public float offTime;                                                                                   //激光门关闭时间

    private float timer;                                                                                     //计时器


    void Update()
    {
        //计时器开始计时
        timer += Time.deltaTime;
        //如果渲染的时间大于开启时间，那么开始切换
        if (GetComponent<Renderer>().enabled && timer >= onTime)
        {
            SwitchBeam();
        }
        //如果没有渲染的时间大于关闭时间，那么开始切换
        else if(!GetComponent<Renderer>().enabled && timer >= offTime)
        {
            SwitchBeam();
        }

    }

    void SwitchBeam()
    {
        // 切换激光门的渲染组件和灯光组件 为与当前相反的状态（已启用则禁用 已禁用则启用）
        GetComponent<Renderer>().enabled = !GetComponent<Renderer>().enabled;
        GetComponent<Light>().enabled = !GetComponent<Light>().enabled;
        //重置计时器
        timer = 0;
    }
}
