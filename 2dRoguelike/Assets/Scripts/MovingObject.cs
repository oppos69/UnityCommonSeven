/**
 * 
 *  模块名称:人物移动攻击模块
 *  模块功能：生成关卡地图
 *  作者：王冲
 *  时间：2015-05-16
 *  描述：
 *  人物的移动，分为 移动方向没有碰撞体则移动，有碰撞体（敌人、可摧毁的墙）那么无法移动，
 *  根据分析
 *  一 需要一个运动趋势方法
 * 当人物有运动趋势时开始判断。
 * 
 * 
 * 
 * 
 */
using UnityEngine;
using System.Collections;

public abstract class MovingObject : MonoBehaviour {
    /// <summary>
    /// 移动时间
    /// </summary>
    public float MoveTime = 0.1f;
    /// <summary>
    /// 碰撞层
    /// </summary>
    public LayerMask BlockingLayerMask;

    private Rigidbody2D rigid;                                                  //引用刚体组件
    private Collider2D collider;                                                //引用碰撞体组件
    private float inverseMoveTime;                                              //


    /// <summary>
    /// 移动趋势
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="xDir"></param>
    /// <param name="yDir"></param>
    protected virtual void AttemptMove<T>(int xDir, int yDir)
        where T : Component
    {
        //声明Hit
        RaycastHit2D hit;
        
        //移动物体
        bool canMove = Move(xDir,yDir,out hit);
        
        //如果什么都没有打到
        if (hit.transform == null)
            return;
        //返回被打中物体的组件
        T hitComponent = hit.transform.GetComponent<T>();

        //如果不能移动 且的到了打中目标的“指定”组件
        if(!canMove && hitComponent != null)
            //执行不能移动处理方法
            OnCantMove(hitComponent);
    }

    protected virtual bool Move(int xDir,int yDir,out RaycastHit2D hit)
    { 
        //获取起始移动位置
        Vector2 start = transform.position;
        //获取移动目标位置
        Vector2 end = start + new Vector2(xDir, yDir);
        //将自己的碰撞组件取消
        collider.enabled = false;
        //使用2d射线进行打击，获得打击对象
        hit = Physics2D.Linecast(start,end,BlockingLayerMask);
        //启用碰撞组件
        collider.enabled = true;
        
        //判断是否有打击到物体
        if (hit.transform == null)
        {
            //开始移动
            StartCoroutine(SmoothMovement(end));
            return true;
        }
        return false;
    }

    /// <summary>
    /// 移动到目标点
    /// </summary>
    /// <param name="targetVector"></param>
    protected virtual IEnumerator SmoothMovement(Vector3 targetVector)
    { 
        //计算距离
        float distance = (transform.position - targetVector).sqrMagnitude;
        //不断移动到目标位置
        while(distance > float.Epsilon)
        {
            //开始计算第一步移动的位置
            Vector3 newPostion = Vector3.MoveTowards(rigid.position, targetVector, inverseMoveTime * Time.deltaTime);
            //开始移动
            rigid.MovePosition(newPostion);
            //更新移动距离
            distance = (transform.position - targetVector).sqrMagnitude;

            yield return null;

        }
    }

    protected virtual void OnCantMove<T>(T component)
        where T:Component
    { 
        
    }

    //抽象函数在游戏开始时不会调用 Awake方法，所以把获取组件的引用放到了Start函数中
	// Use this for initialization
    protected virtual void Start()
    {
        //获得引用
        rigid = GetComponent<Rigidbody2D>();
        collider = GetComponent<Collider2D>();

        inverseMoveTime = 1f / MoveTime;
    }
	
}
