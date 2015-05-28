using UnityEngine;
using System.Collections;

public class Loader : MonoBehaviour {

    public GameManager gameManager;
    public SoundManager soundManager;

    void Awake()
    {
        //游戏管理实例是否存在
        if (GameManager.instance == null)
        {

            Instantiate(gameManager);
        }
        //声音管理实例是否存在
        if (SoundManager.instance == null)
        {
            Instantiate(soundManager);

        }
    }

}
