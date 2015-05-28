/*
 * 模块名称:A*地图队列
 * 模块功能：生成关卡地图
 * 作者：王冲
 * 时间：2015-05-17
 * 描述：
 * 保持路径
 * 
 * 
 * 
 * 
 * 
 */
using UnityEngine;
using System.Collections;
public class PriorityQueue
{
    private ArrayList nodes = new ArrayList();

    public int Length
    {
        get { return this.nodes.Count; }
    }

    public bool Contains(object node)
    {
        return this.nodes.Contains(node);
    }

    public Node First()
    {
        if (this.nodes.Count > 0)
        {
            return (Node)this.nodes[0];
        }
        return null;
    }

    public void Push(Node node)
    {
        this.nodes.Add(node);
        this.nodes.Sort();
    }

    public void Remove(Node node)
    {
        this.nodes.Remove(node);
        //进行排序
        this.nodes.Sort();
    }


}