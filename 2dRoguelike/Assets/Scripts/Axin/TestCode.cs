using UnityEngine;  
using System.Collections;
public class TestCode : MonoBehaviour
{
    private Transform startPos, endPos,end2Pos;
    public Node startNode { get; set; }
    public Node goalNode { get; set; }
    public Node goal2Node { get; set; }
    public Node goal3Node { get; set; }
    public ArrayList pathArray,pathArray2;
    GameObject objStartCube, objEndCube,objEnd2Cube;
    private float elapsedTime = 0.0f;
    //Interval time between pathfinding  
    public float intervalTime = 1.0f;

    void Start()
    {
        objStartCube = GameObject.FindGameObjectWithTag("Player");
        objEndCube = GameObject.FindGameObjectWithTag("Enemy");

        objEnd2Cube = GameObject.FindGameObjectWithTag("Soda");
        pathArray = new ArrayList();
        pathArray2 = new ArrayList();
        FindPath();
    }
    void Update()
    {
        elapsedTime += Time.deltaTime;
        if (elapsedTime >= intervalTime)
        {
            elapsedTime = 0.0f;
            FindPath();
        }
    }
    void FindPath()
    {
        startPos = objStartCube.transform;
        endPos = objEndCube.transform;
        int Index = GridManager.instance.GetGridIndex(startPos.position);

        startNode = new Node(GridManager.instance.GetGridCellCenter(
                Index));

        goalNode = new Node(GridManager.instance.GetGridCellCenter(
                GridManager.instance.GetGridIndex(endPos.position)));
        pathArray = AStar.FindPath(startNode, goalNode);

        goal3Node = new Node(GridManager.instance.GetGridCellCenter(
               Index));
        goal2Node = new Node(GridManager.instance.GetGridCellCenter(
                GridManager.instance.GetGridIndex(objEnd2Cube.transform.position)));
        pathArray2 = AStar.FindPath(goal3Node, goal2Node);


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

        if (pathArray2.Count > 0)
        {

            //计算偏移量
            //    让线在移动物体的中心
            float xOff = GridManager.instance.gridCellSize / 2 * -1;
            float yOff = GridManager.instance.gridCellSize / 2 * -1;
            Vector3 offVector = new Vector3(xOff, yOff);
            int index = 1;
            foreach (Node node in pathArray2)
            {
                if (index < pathArray2.Count)
                {
                    Node nextNode = (Node)pathArray2[index];
                    Debug.DrawLine(node.position + offVector, nextNode.position + offVector,
                        Color.red);
                    index++;
                }
            }
        }
    }
}