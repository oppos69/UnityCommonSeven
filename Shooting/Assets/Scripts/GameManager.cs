/***
 * 
 *      ProjectName:NightShooting
 * 
 *      Anthor     :wangchong
 * 
 *      CreateTime: 2014-11-21
 * 
 *      DESC       ：游戏管理
 * 
 *      
 */
using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class GameManager : MonoBehaviour {


    
    /// <summary>
    /// 主角
    /// </summary>
    private PlayerHealth mPlayerHealth;

    /// <summary>
    /// 游戏分数
    /// </summary>
    private float PlayerSource = 0f;

    /// <summary>
    /// 结束面板
    /// </summary>
    public  RectTransform PanelGameOver;



    /// <summary>
    /// 得分
    /// </summary>
    private Text UITxtSource;

    void Awake() {
        mPlayerHealth = GameObject.FindGameObjectWithTag(Tags.PlayerTagName).GetComponent<PlayerHealth>();
        UITxtSource = GameObject.FindGameObjectWithTag(Tags.SourceTagName).GetComponent<Text>();
    }

    void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Time.timeScale = 0;
            PlayerDead();
        }
    }

    /// <summary>
    /// 所得游戏分数
    /// </summary>
    /// <param name="Source"></param>
    public void AddSource(float Source)
    { 

        PlayerSource += Source;
        UITxtSource.text = "SOURCE "+PlayerSource;
    }
    /// <summary>
    /// 再来一次
    /// </summary>
    public void GameAgain()
    {
        Time.timeScale = 1;
        Application.LoadLevel(1);
    }
    /// <summary>
    /// 退出游戏
    /// </summary>
    public void GameOver()
    {
        Application.Quit();
    }


    public void GameContinue()
    {
        Time.timeScale = 1;
        PanelGameOver.position = new Vector3(-1000,-500);
    }
    /// <summary>
    /// 主角死亡
    /// </summary>
    public void PlayerDead()
    {
        if (PanelGameOver != null)
        {
            //PanelGameOver.SetActive(true);

            PanelGameOver.position = new Vector3(Screen.width  / 2, Screen.height / 2);
        }
    }
}
