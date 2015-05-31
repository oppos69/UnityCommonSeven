/***
 * 
 *      ProjectName:Stealth
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

public class SceneFaderInOut : MonoBehaviour {

    public float fadeSpeed = 1.5f;                     //屏幕颜色渐变速度

    private bool sceneStarting = true;                 //决定屏幕是否开始渐变


    void Awake()
    {
        
        //设置纹理覆盖整个屏幕
        GetComponent<GUITexture>().pixelInset = new Rect(0,0,Screen.width,Screen.height);
    }

    void FadeToClear()
    {
        //让纹理透明
        GetComponent<GUITexture>().color = Color.Lerp(GetComponent<GUITexture>().color,Color.clear, fadeSpeed * Time.deltaTime);
    }

    void FadeToBlack()
    {
        //让纹理变黑
        GetComponent<GUITexture>().color = Color.Lerp(GetComponent<GUITexture>().color, Color.black, fadeSpeed * Time.deltaTime);
    }

    void StartScene()
    {
        GetComponent<GUITexture>().enabled = true;
        //调用纹理透明
        FadeToClear();

        if (GetComponent<GUITexture>().color.a < 0.05f)
        {
            GetComponent<GUITexture>().color = Color.clear;
            GetComponent<GUITexture>().enabled = false;

            sceneStarting = false;
        }
    }


    public void EndScene()
    {
        GetComponent<GUITexture>().enabled = true;

        FadeToBlack();


        if (GetComponent<GUITexture>().color.a >= 0.95f)
        {
           //重新加载场景
            Application.LoadLevel(Application.loadedLevel);
        }
    }

    void Update()
    {
        if (sceneStarting)
            StartScene();
    }
}
