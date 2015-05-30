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

public class AlarmLight : MonoBehaviour {

    public float fadeSpeed = 2f;
    public float highIntensity = 2f;
    public float lowIntensity = 0.5f;
    public float changeMangin = 0.2f;
    public bool alarmOn;

    private float targetIntensiey;

    void Awake()
    {
        GetComponent<Light>().intensity = 0f;
        targetIntensiey = highIntensity;

    }

    void Update()
    {
        if (alarmOn)
        {
            GetComponent<Light>().intensity = Mathf.Lerp(GetComponent<Light>().intensity, targetIntensiey, fadeSpeed * Time.deltaTime);
            CheckTargetIntensity();
        }
        else
        {
            GetComponent<Light>().intensity = Mathf.Lerp(GetComponent<Light>().intensity,lowIntensity,fadeSpeed*Time.deltaTime);
        }
    }

    void CheckTargetIntensity()
    {
        if (Mathf.Abs(targetIntensiey - GetComponent<Light>().intensity) < changeMangin)
        {
            if (targetIntensiey == highIntensity)
            {
                targetIntensiey = lowIntensity;
            }
            else
            {
                targetIntensiey = highIntensity;
            }
        }
    }
}
