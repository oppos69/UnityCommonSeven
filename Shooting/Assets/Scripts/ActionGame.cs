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

public class ActionGame : MonoBehaviour {

    /// <summary>
    /// 再来一次
    /// </summary>
    public void GameAgain()
    {
        Application.LoadLevel(1);
    }
    /// <summary>
    /// 退出游戏
    /// </summary>
    public void GameOver()
    {
        Application.Quit();
    }
}
