/***
 * 
 *      ProjectName:Stealth
 * 
 *      Anthor     :wangchoeng
 * 
 *      CreateTime: 2014-11-21
 * 
 *      DESC       ：
 * 
 *      
 */
using UnityEngine;
using System.Collections;

public class LastPlayerSighting : MonoBehaviour {

	public Vector3 position = new Vector3(1000f,1000f,1000f);                                                 //主角最后一次被发现的地方
	public Vector3 resetPosition = new Vector3(1000f,1000f,1000f);                                            //没有被发现的位置
	public float lightHighIntensity = 0.25f;                                                                  //主灯光的最大亮度
	public float lightLowIntensity = 0f;                                                                      //主灯光最小亮度
	public float fadeSpeed = 7f;                                                                              //灯光变换速度
	public float musicFadeSpeed = 1f;                                                                         //音乐变换速度
                                                                                                              //
	private AlarmLight alarm;                                                                                 //报警灯光
	private Light mainLight;                                                                                  //主灯光
	private AudioSource panicAudio;                                                                           //背景音乐
	private AudioSource[] sirens;                                                                             //所有报警音乐

    void Awake()
    {
        alarm = GameObject.FindGameObjectWithTag(Tags.alarm).GetComponent<AlarmLight>();                      //
        mainLight = GameObject.FindGameObjectWithTag(Tags.mainLight).GetComponent<Light>();                                   //
        panicAudio = transform.Find("secondaryMusic").GetComponent<AudioSource>();                                                   //
        GameObject[] sirenGameObjects = GameObject.FindGameObjectsWithTag(Tags.siren);                        //
        sirens = new AudioSource[sirenGameObjects.Length];                                                    //
        for (int i = 0; i < sirenGameObjects.Length; i++)                                                     //
        {                                                                                                     //
            sirens[i] = sirenGameObjects[i].GetComponent<AudioSource>();                                                            //
        }                                                                                                     //
    }

	public void SwitchAlarm()
	{
		alarm.alarmOn = (position != resetPosition);

		//主灯光的目标强度
		float newIntensity;

		if (position != resetPosition) {
			newIntensity = lightLowIntensity;
				} else {
			newIntensity = lightHighIntensity;	
		}

		mainLight.intensity = Mathf.Lerp (mainLight.intensity,newIntensity,fadeSpeed* Time.deltaTime);

		//对于所有报警喇叭
        for (int i = 0; i < sirens.Length; i++)
        {
            if (position != resetPosition && !sirens[i].isPlaying)
            {
                sirens[i].Play();
            }
            else if(position == resetPosition)
            {
                sirens[i].Stop();
            }
        }
	}

    /// <summary>
    /// 音乐变换
    /// </summary>
    public void MusicFading()
    {
        //报警
        if (position != resetPosition)
        {
            //减小背景音乐到0
            GetComponent<AudioSource>().volume = Mathf.Lerp(GetComponent<AudioSource>().volume,0f,musicFadeSpeed* Time.deltaTime);
            //增大紧张音乐到0.8
            panicAudio.volume  = Mathf.Lerp(panicAudio.volume,0.8f,musicFadeSpeed * Time.deltaTime);


        }
        else if (position == resetPosition)
        {

            //增大背景音乐到0.8
            GetComponent<AudioSource>().volume = Mathf.Lerp(GetComponent<AudioSource>().volume, 0.8f, musicFadeSpeed * Time.deltaTime);
            //减小紧张音乐到0
            panicAudio.volume = Mathf.Lerp(panicAudio.volume, 0f, musicFadeSpeed * Time.deltaTime);
   
        }
    }

    void Update()
    {
        SwitchAlarm();
        MusicFading();
    }

}
