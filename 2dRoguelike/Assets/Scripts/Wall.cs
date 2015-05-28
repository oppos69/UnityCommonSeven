/**
 * 
 * 
 * 
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

public class Wall : MonoBehaviour {

    //
    public Sprite dmgSprite;

    public int hp = 4;

    private SpriteRenderer spriteRender;

    void Awake()
    {
        spriteRender = GetComponent<SpriteRenderer>();
    }
    public void DamageWall(int loss)
    { 
        //当墙体受到攻击将会显示破损Sprite
        spriteRender.sprite = dmgSprite;
        //减血
        hp -= loss;
        //如果墙体破坏后,销毁
        if (hp <= 0)
        {
            Destroy(gameObject);
        }

    }
}
