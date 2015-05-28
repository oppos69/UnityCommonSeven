/*
 * 模块名称:A*地图节点 
 * 模块功能：生成关卡地图
 * 作者：王冲
 * 时间：2015-05-17
 * 描述：
 * 保存路径消耗数量
 * 是否是无法通过的障碍物
 * 实现同类排序功能
 * 
 * 
 * 
 * 
 */
using UnityEngine;
using System.Collections;
using System;

public class Node : IComparable
{
    public float nodeTotalCost;
    public float estimatedCost;
    public bool bObstacle;
    public Node parent;
    public Vector3 position;

    public Node()
    {
        this.estimatedCost = 0.0f;
        this.nodeTotalCost = 1.0f;
        this.bObstacle = false;
        this.parent = null;
    }

    public Node(Vector3 pos)
    {
        this.estimatedCost = 0.0f;
        this.nodeTotalCost = 1.0f;
        this.bObstacle = false;
        this.parent = null;
        this.position = pos;
    }

    public void MarkAsObstacle()
    {
        this.bObstacle = true;
    }

    public int CompareTo(object obj)
    {
        Node node = (Node)obj;
        //负数意味着在obj之前  
        if (this.estimatedCost < node.estimatedCost)
            return -1;
        //正数意味着在obj之后 
        if (this.estimatedCost > node.estimatedCost)
            return 1;
        return 0;
    }  
}
