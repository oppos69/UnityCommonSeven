/*
 * 模块名称:场景管理 
 * 模块功能：生成关卡地图
 * 作者：王冲
 * 时间：2015-05-16
 * 描述：能根据传入的关卡level来生成地图以及敌人。
 * 一level等级与敌人数量成 log关系
 * 二生成10*10地图
 * 其中包括最外围的墙，8*8的地板，最中间的6*6的随机可摧毁的墙、敌人、水果、苏打水
 * 出口（9*9）
 * 
 * 
 */
using UnityEngine;
using System.Collections.Generic;

using System;
using Random = UnityEngine.Random;

public class BoardManager : MonoBehaviour {

    [Serializable]
    /// <summary>
    /// 区间范围类
    /// </summary>
    public class Count {
        public Count(int minNum, int maxNum)
        {
            this.maxNum = maxNum;
            this.minNum = minNum;
        }

        public int maxNum;
        public int minNum;
    }
    
    public int cols        = 8;                                                //地图的列
    public int rows        = 8;                                                //地图的行
    public Count wallCount = new Count(5, 9);                                  //地图中间障碍物区 墙的数量区间
    public Count foodCount = new Count(1, 5);                                  //地图中间障碍物区 食物的数量区间

    public GameObject[] outerWall;                                             //声明外围墙引用
    public GameObject[] floorTiles;                                            //声明地板引用
    public GameObject[] wallTiles;                                             //声明可摧毁墙引用
    public GameObject[] enemyTiles;                                            //声明敌人引用
    public GameObject[] foodTiles;                                             //声明食物引用
    public GameObject exit;                                                    //声明退出引用

    private GameObject boardHolder;
    //生成的坐标
    private List<Vector2> gridPostion = new List<Vector2>();

    private GameObject[,] gridGameObject;
    /// <summary>
    /// 生成关卡
    /// </summary>
    /// <param name="level"></param>
    public void InitLevel(int level)
    {
        
        //生成地图父节点
        boardHolder = new GameObject("boardHolder");
        //初始化随机地图
        InitCenterMap(1,1,rows-1,cols-1);
        //生成外墙和地板
        InitMap(-1,-1,rows,cols);
        //随机生成墙、敌人、水果
        randomArea(level);
        //生成退出点
        exitPoint(7, 7);
    }
    /// <summary>
    /// 退出点的坐标
    /// </summary>
    /// <param name="xpos"></param>
    /// <param name="ypos"></param>
    void exitPoint(int xpos,int ypos)
    {
        //生成退出点
        GameObject obj = Instantiate(this.exit,new Vector2(xpos,ypos),Quaternion.identity) as GameObject;
        //设置父节点
        obj.transform.SetParent(boardHolder.transform);
    }

    /// <summary>
    /// 初始化随机生成物体区域地图
    /// </summary>
    /// <param name="pointx"></param>
    /// <param name="pointy"></param>
    /// <param name="mapRow"></param>
    /// <param name="mapCol"></param>
    void InitCenterMap(int pointx, int pointy, int mapRow, int mapCol)
    {
        gridGameObject = new GameObject[mapRow,mapCol];
        for (int x = pointx; x < mapCol; x++)
        {

            for (int y = pointy; y < mapRow; y++)
            {
                gridPostion.Add(new Vector2(x,y));
                GameObject obj = new GameObject("empty");
                obj.transform.position = new Vector2(x, y);
                gridGameObject[x, y] = obj;
            }
        }
    }

    /// <summary>
    /// 生成外墙和地板
    /// </summary>
    /// <param name="pointx">原点x</param>
    /// <param name="pointy">原点y</param>
    /// <param name="mapRow">行</param>
    /// <param name="mapCol">列</param>
    void InitMap(int pointx,int pointy,int mapRow,int mapCol)
    {
        for (int i = pointx; i < mapCol + 1; i++)
        {
            for (int j = pointy; j < mapRow + 1; j++)
            {
                //地板
                GameObject mapObject = floorTiles[Random.Range(0,floorTiles.Length)];
                //外围墙
                if (i == pointx || i == mapCol || j == pointy || j == mapRow )
                { 
                    //生成外围墙
                    mapObject = outerWall[Random.Range(0,outerWall.Length)];
                }

                GameObject instance = (GameObject)GameObject.Instantiate(mapObject, new Vector3(i, j), Quaternion.identity);
                //设置父节点
                instance.transform.SetParent(boardHolder.transform);
            }
        }
    }
    /// <summary>
    /// 随机区域中对象的生成
    /// </summary>
    void randomArea(int levelNum)
    {
        //生成墙
        GameObjecAtRandom(wallTiles,wallCount.minNum,wallCount.maxNum );
        //生成水果
        GameObjecAtRandom(foodTiles, foodCount.minNum, foodCount.maxNum);
        //生成敌人
        //敌人的数量与关数有关
        int enemyCount = (int)Mathf.Log(levelNum,2f);
        GameObjecAtRandom(enemyTiles, enemyCount, enemyCount);
        
    }

    void GameObjecAtRandom(GameObject[] objList,int minNum,int maxNum)
    {
        //随机出需要生成的数量
        int objectCount = Random.Range(minNum, maxNum + 1);
        //循环生成
        for (int i = 0; i < objectCount; i++)
        {
            //随机出需要生成的游戏对象
            GameObject obj = objList[Random.Range(0, objList.Length)];
            //随机出位置
            Vector2 pos = randomPosition();
            GameObject target = gridGameObject[(int)pos.x,(int) pos.y];
            Destroy(target);
            //生成对象
            obj = Instantiate(obj, pos, Quaternion.identity) as GameObject;
            gridGameObject[(int)pos.x, (int)pos.y] = obj;
            //设置父节点
            obj.transform.SetParent(boardHolder.transform);
        }
    }

    Vector2 randomPosition()
    {
        int count = gridGameObject.Length;
        
        //生成随机数
        int randomIndex = Random.Range(0, gridPostion.Count);
        Vector2 result = gridPostion[randomIndex];
        return result;
    }


	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	    
	}
}
