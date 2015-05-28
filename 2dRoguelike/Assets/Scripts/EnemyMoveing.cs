using UnityEngine;
using System.Collections;

public class EnemyMoveing : MovingObject {

    public int Damage;                                                        //攻击伤害
    public Transform target;                                                  //引用移动组件
    public Animator anim;                                                     //引用动画组件
    [HideInInspector]
    public bool cantMove = false;                                             //是否能移动
    private bool isVerticalMove;                                              //是否为纵向移动

    #region A星寻路参数
    /// <summary>
    /// 起始点
    /// </summary>
    public Node startNode { get; set; }
    /// <summary>
    /// 终点
    /// </summary>
    public Node goalNode { get; set; }
    /// <summary>
    /// 存放路径
    /// </summary>
    public ArrayList pathArray;


    #endregion
    void Awake()
    { 
        //获得引用
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        anim   = GetComponent<Animator>();
        //找到所有的障碍物
        GameObject[] objList = GameObject.FindGameObjectsWithTag("Obstacle");
        //计算地图
        GridManager.instance.CalculateObstacles(objList);
    }
	// Use this for initialization
    protected override void Start () {
        //在游戏控制器中添加自己
        GameManager.instance.AddEnemy(this);
        base.Start();
        //首先以横向移动
        isVerticalMove = false;
	}

    public void MoveEnemy()
    {

        //计算
        int Index = GridManager.instance.GetGridIndex(transform.position);

        startNode = new Node(GridManager.instance.GetGridCellCenter(
                Index));

        goalNode = new Node(GridManager.instance.GetGridCellCenter(
                GridManager.instance.GetGridIndex(target.position)));
        pathArray = AStar.FindPath(startNode, goalNode);

        //获取第二个节点的位置
        if (pathArray.Count > 1)
        {

            Node targetNode = pathArray[1] as Node;
            //加上偏移量（因为计算的是中心，半径是1）
            float off = GridManager.instance.gridCellSize;
            Vector3 vector = targetNode.position + new Vector3(-(off / 2), -(off/2), 0f);
            vector = vector - transform.position;
#if UNITY_EDITOR
            Debug.Log(targetNode.position.x + "," + Mathf.CeilToInt(targetNode.position.x));
            
#endif
            //开始移动
            AttemptMove<Player>((int)vector.x, (int)vector.y);
        }
    }

    void OnDrawGizmos()
    {
        if (pathArray == null)
            return;
        if (pathArray.Count > 0)
        {

            //计算偏移量
            //    让线在移动物体的中心
            float xOff = GridManager.instance.gridCellSize / 2 * -1;
            float yOff = GridManager.instance.gridCellSize / 2 * -1;
            Vector3 offVector = new Vector3(xOff, yOff);
            int index = 1;
            foreach (Node node in pathArray)
            {
                if (index < pathArray.Count)
                {
                    Node nextNode = (Node)pathArray[index];
                    Debug.DrawLine(node.position + offVector, nextNode.position + offVector,
                        Color.green);
                    index++;
                }
            }
        }
    }

    protected override void AttemptMove<T>(int xDir, int yDir)
    {
        //不是每次移动而是主角移动两次“敌人”移动一次
        if (cantMove)
        {
            cantMove = false;
            return;
        }

        //移动
        base.AttemptMove<T>(xDir, yDir);
        cantMove = true;
    }


    protected override void OnCantMove<T>(T component)
    {
        //判断是否为palyer
        Player player = component as Player;
        
        if (player != null)
        {
            //对主角进行伤害
            player.LossFood(Damage);
            //播放动画
            anim.SetTrigger("enemyattack");
        }
        //如果为墙我们重新改为纵向移动
        Wall wall = component as Wall;
        if (wall != null)
        { 
            //重新改为纵向移动
            Debug.Log("wall");
            isVerticalMove = true;
        }

    }
	
	// Update is called once per frame
	
}
