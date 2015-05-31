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

public class EnemyDestory : MonoBehaviour {

    void OnBecameInvisible()
    {
        Destroy(this.gameObject);
    }
}
