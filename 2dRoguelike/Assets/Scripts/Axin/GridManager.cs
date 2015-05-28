/*
 * 模块名称:A*地图管理
 * 模块功能：生成关卡地图
 * 作者：王冲
 * 时间：2015-05-17
 * 描述：
 * GridManager设置单例
 * 图行数和列数每个格子的大小，格子与障碍物（bool）
 * 
 * 
 * 
 * 
 */
using UnityEngine;  
using System.Collections;
public class GridManager : MonoBehaviour
{
    private static GridManager s_Instance = null;
    public static GridManager instance
    {
        get
        {
            if (s_Instance == null)
            {
                s_Instance = FindObjectOfType(typeof(GridManager))
                        as GridManager;
                if (s_Instance == null)
                    Debug.Log("Could not locate a GridManager " +
                            "object. \n You have to have exactly " +
                            "one GridManager in the scene.");
            }
            return s_Instance;
        }


    }
    public int numOfRows;                                                      //行的数量
    public int numOfColumns;                                                   //列的数量
    public float gridCellSize;                                                 //格子大小
    public bool showGrid = true;                                               //是否显示网格
    public bool showObstacleBlocks = true;                                     //显示障碍物块
    private Vector3 origin = new Vector3(-10,-10);                             //原点
    private GameObject[] obstacleList;                                         //隔绝物列表
    public Node[,] nodes { get; set; }                                         //所有节点
    /// <summary>
    /// 起点
    /// </summary>
    public Vector3 Origin                                                      
    {
        get { return origin; }
        
    }

    public float OriginX = -12;
    public float OriginY = -12;
    

    void Awake()
    {
        origin = new Vector3(OriginX, OriginY,0);
        //找到所有的障碍物
        obstacleList = GameObject.FindGameObjectsWithTag("Obstacle");
        CalculateObstacles();
        
    }
    void Start()
    {
    }
    // 
    //计算所有节点
    void CalculateObstacles()
    {
        nodes = new Node[numOfColumns, numOfRows];
        int index = 0;
        //设置地图格子
        for (int i = 0; i < numOfColumns; i++)
        {
            for (int j = 0; j < numOfRows; j++)
            {
                Vector3 cellPos = GetGridCellCenter(index);
                Node node = new Node(cellPos);
                nodes[i, j] = node;
                index++;
            }
        }
        //设置障碍物格子
        if (obstacleList != null && obstacleList.Length > 0)
        {
            //For each obstacle found on the map, record it in our list  
            foreach (GameObject data in obstacleList)
            {
                int indexCell = GetGridIndex(data.transform.position);
                
                int col = GetColumn(indexCell);
                int row = GetRow(indexCell);
                nodes[row, col].MarkAsObstacle();
            }
        }
    }
    /// <summary>
    /// 根据给定的物体计算地图
    /// </summary>
    public void CalculateObstacles(GameObject[] targetObstacleList)
    {
        nodes = new Node[numOfColumns, numOfRows];
        int index = 0;
        //设置地图格子
        for (int i = 0; i < numOfColumns; i++)
        {
            for (int j = 0; j < numOfRows; j++)
            {
                Vector3 cellPos = GetGridCellCenter(index);
                Node node = new Node(cellPos);
                nodes[i, j] = node;
                index++;
            }
        }
        //设置障碍物格子
        if (targetObstacleList != null && targetObstacleList.Length > 0)
        {
            //For each obstacle found on the map, record it in our list  
            foreach (GameObject data in targetObstacleList)
            {
                int indexCell = GetGridIndex(data.transform.position);

                int col = GetColumn(indexCell);
                int row = GetRow(indexCell);
                nodes[row, col].MarkAsObstacle();
            }
        }
    }

    /// <summary>
    /// 返回网格中心
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public Vector3 GetGridCellCenter(int index)
    {
        Vector3 cellPosition = GetGridCellPosition(index);
        cellPosition.x += (gridCellSize / 2.0f);
        cellPosition.y += (gridCellSize / 2.0f);
        return cellPosition;
    }
    /// <summary>
    /// 返回网格格子的位置
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public Vector3 GetGridCellPosition(int index)
    {
        int row = GetRow(index);
        int col = GetColumn(index);
        float xPosInGrid = col * gridCellSize;
        float zPosInGrid = row * gridCellSize;
        return Origin + new Vector3(xPosInGrid, zPosInGrid, 0.0f);
    }
    /// <summary>
    /// 返回给定索引的行数
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public int GetRow(int index)
    {
        int row = index / numOfColumns;
        return row;
    }
    /// <summary>
    /// 返回给定索引的列数
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public int GetColumn(int index)
    {
        int col = index % numOfColumns;
        return col;
    }
    /// <summary>
    /// 根据Vector返回索引
    /// </summary>
    /// <param name="pos"></param>
    /// <returns></returns>
    public int GetGridIndex(Vector3 pos)
    {

        //是否超出界限
        if (!IsInBounds(pos))
        {
            return -1;
        }
        pos -= Origin;
        int col = (int)(pos.x / gridCellSize);
        int row = (int)(pos.y / gridCellSize);

        return (row * numOfColumns + col);
    }
    /// <summary>
    /// 是否超出界限
    /// </summary>
    /// <param name="pos"></param>
    /// <returns></returns>
    public bool IsInBounds(Vector3 pos)
    {
        float width = numOfColumns * gridCellSize;
        float height = numOfRows * gridCellSize;

        return (pos.x >= Origin.x && pos.x <= Origin.x + width &&
                pos.y <= Origin.y + height && pos.y >= Origin.y);
    }
    /// <summary>
    /// 用于检索特定节点的邻接点
    /// </summary>
    /// <param name="node"></param>
    /// <param name="neighbors"></param>
    public void GetNeighbours(Node node, ArrayList neighbors)
    {
        Vector3 neighborPos = node.position;
        int neighborIndex = GetGridIndex(neighborPos);
        int row = GetRow(neighborIndex);
        int column = GetColumn(neighborIndex);
        //Bottom  
        int leftNodeRow = row - 1;
        int leftNodeColumn = column;
        AssignNeighbour(leftNodeRow, leftNodeColumn, neighbors);
        //Top  
        leftNodeRow = row + 1;
        leftNodeColumn = column;
        AssignNeighbour(leftNodeRow, leftNodeColumn, neighbors);
        //Right  
        leftNodeRow = row;
        leftNodeColumn = column + 1;
        AssignNeighbour(leftNodeRow, leftNodeColumn, neighbors);
        //Left  
        leftNodeRow = row;
        leftNodeColumn = column - 1;
        AssignNeighbour(leftNodeRow, leftNodeColumn, neighbors);
    }
    /// <summary>
    /// 查询指定row，column放到neighbors中
    /// </summary>
    /// <param name="row"></param>
    /// <param name="column"></param>
    /// <param name="neighbors"></param>
    void AssignNeighbour(int row, int column, ArrayList neighbors)
    {
        if (row != -1 && column != -1 &&
            row < numOfRows && column < numOfColumns)
        {
            Node nodeToAdd = nodes[row, column];
            if (!nodeToAdd.bObstacle)
            {
                neighbors.Add(nodeToAdd);
            }
        }
    }  
    /// <summary>
    /// 是否画出网格、障碍物
    /// </summary>
    void OnDrawGizmos() {  
        //原点
        Vector3 GizmosOrigion = origin;
        if (showGrid) {
            DebugDrawGrid(GizmosOrigion, numOfRows, numOfColumns,   
                    gridCellSize, Color.blue);  
        }
        Gizmos.DrawSphere(GizmosOrigion, 0.5f);  
        if (showObstacleBlocks) {  
            Vector3 cellSize = new Vector3(gridCellSize,gridCellSize,
                 1.0f);  
            if (obstacleList != null && obstacleList.Length > 0) {  
                foreach (GameObject data in obstacleList) {  
                    Gizmos.DrawCube(GetGridCellCenter(  
                            GetGridIndex(data.transform.position)), cellSize);  
                }  
            }  
        }  
    }  
    /// <summary>
    /// 画出网格
    /// </summary>
    /// <param name="origin">原点</param>
    /// <param name="numRows">行数</param>
    /// <param name="numCols">列数</param>
    /// <param name="cellSize">单元格大小</param>
    /// <param name="color">颜色</param>
    public void DebugDrawGrid(Vector3 origin, int numRows, int  
        numCols,float cellSize, Color color) {  
        float width = (numCols * cellSize);  
        float height = (numRows * cellSize);  
        //计算偏移量
        //  将移动对象包含在网格中，而不是用网格节点表示点
        float xOff = gridCellSize / 2 * -1;
        float yOff = gridCellSize / 2 * -1;
        // Draw the horizontal grid lines  
        for (int i = 0; i < numRows + 1; i++) {  
            Vector3 startPos = origin + i * cellSize * new Vector3(0.0f,
                 1.0f, 0.0f) + new Vector3(xOff, yOff);  
            Vector3 endPos = startPos + width * new Vector3(1.0f, 0.0f,
                0.0f) + new Vector3(0, 0);  
            Debug.DrawLine(startPos, endPos, color);  
        }  
            // Draw the vertial grid lines  
        for (int i = 0; i < numCols + 1; i++) {  
            Vector3 startPos = origin + i * cellSize * new Vector3(1.0f,
                0.0f, 0.0f) + new Vector3(xOff, yOff);  
            Vector3 endPos = startPos + height * new Vector3(0.0f,1.0f,
                 0.0f) + new Vector3(0, 0);  
            Debug.DrawLine(startPos, endPos, color);  
        }  
    }  
}  
