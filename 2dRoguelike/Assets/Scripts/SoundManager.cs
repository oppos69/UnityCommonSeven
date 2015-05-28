/*
 * 模块名称:人物移动攻击模块
 * 模块功能：生成关卡地图
 * 作者：王冲
 * 时间：2015-05-16
 * 描述：
 * 
 * 
 * 
 * 
 * 
 * 
 * 
 */
using UnityEngine;
using System.Collections;

public class SoundManager : MonoBehaviour {

    public AudioSource efxSource;                                              //影响效果声音
    public AudioSource musicSource;                                            //随机播放音乐
    public static SoundManager instance = null;                                //单例
    public float lowPitchRange = .95f;                                         //最小音效
    public float highPitchRange = 1.05f;		                               //最大音效

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(instance);
        }
        DontDestroyOnLoad(instance);
    }

    //播放单个音乐
    public void PlaySingle(AudioClip clip)
    {
        //设置需要播放的剪辑
        efxSource.clip = clip;
        //开始播放
        efxSource.Play();
    }

    public void RandomFx(params AudioClip [] clips)
    { 
        //生成随机变量
        int randomIndex = Random.Range(0,clips.Length);
        //随机生成一个音调值
        float randomPitch = Random.Range(lowPitchRange, highPitchRange);
        //设置剪辑
        musicSource.clip = clips[randomIndex];
        //设置pitch
        musicSource.pitch = randomPitch;
        //开始播放
        musicSource.Play();

    }

}
