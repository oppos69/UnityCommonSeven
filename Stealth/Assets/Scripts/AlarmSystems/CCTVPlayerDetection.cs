/***
 * 
 *      ProjectName:Stealth
 * 
 *      Anthor     :wangchong
 * 
 *      CreateTime: 2014-11-21
 * 
 *      DESC       ：CCTV发现主角报警
 * 
 *      
 */
using UnityEngine;
using System.Collections;

public class CCTVPlayerDetection : MonoBehaviour {

    private GameObject player;                                                                               //主角信息

    private LastPlayerSighting lastPlayerSighting;                                                           //最后发现主角的位置

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag(Tags.player) as GameObject;

        lastPlayerSighting = GameObject.FindGameObjectWithTag(Tags.gameContorller).GetComponent<LastPlayerSighting>();

    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == player)
        {
            RaycastHit hit;

            Vector3 ralPlayerPos = player.transform.position - transform.position;

            if (Physics.Raycast(transform.position, ralPlayerPos, out hit))
            {
                if (hit.collider.gameObject == player)
                {
                    lastPlayerSighting.position = player.transform.position;
                }
            }
        }
    }
}
