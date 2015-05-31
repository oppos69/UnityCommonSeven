/***
 * 
 *      ProjectName:Stealth
 * 
 *      Anthor     :wangchong
 * 
 *      CreateTime: 2014-11-21
 * 
 *      DESC       ：跟随主角
 * 
 *      
 */
using UnityEngine;
using System.Collections;

public class CameraMovement : MonoBehaviour {

    public float smooth = 1.5f;                                                                        // 摄像机跟踪速度

    public Transform player;                                                                            //主角的位置
    public Vector3 ralCameraPos;                                                                        //摄像机位置相对位置
    public float ralCameraPosMag;                                                                       //摄像机到主角的距离
    public Vector3 newPos;                                                                              //摄像机需要移动的新位置

    void Awake()
    {
        //获取主角的transform组件
        player = GameObject.FindGameObjectWithTag(Tags.player).transform;
        // 相对位置 = 角色位置- 摄像机位置  也可以 = 摄像机位置- 角色位置 具体看怎么用
        ralCameraPos = player.position - transform.position;                                        
        ralCameraPosMag = ralCameraPos.magnitude - 0.5f;
    }

    void FixedUpdate()
    { 
        //计算出初始摄像机位置
        Vector3 standardPos = player.position - ralCameraPos;
        //计算出主角头顶的摄像机位置
        Vector3 abovePos = player.position + Vector3.up * ralCameraPosMag;
        Vector3 [] checkPoints = new Vector3[5];
        //摄像机标准位置
        checkPoints[0] = standardPos;
        // 这三个检测位置为 标准位置到俯视位置之间的三个位置 插值分别为25% 50% 75%
        checkPoints[1] = Vector3.Lerp(standardPos,abovePos,0.25f);
        checkPoints[2] = Vector3.Lerp(standardPos, abovePos, 0.5f);
        checkPoints[3] = Vector3.Lerp(standardPos, abovePos, 0.75f);
        //摄像机在角色头顶位置
        checkPoints[4] = abovePos;
        // 通过循环检测每个位置是否可以看到角色
        for (int i = 0; i < checkPoints.Length; i++)
        {
            // 如果可以看到角色
            if (ViewingPosCheck(checkPoints[i]))
            {

                break;
            }
        }
        // 让摄像机位置 从当前位置 平滑转至 新位置
        transform.position = Vector3.Lerp(transform.position,newPos,smooth* Time.deltaTime);
        //让摄像机平滑的照向角色位置
        SmoothLookAt();
    }


    void SmoothLookAt()
    {
        //创建从摄像机到角色的向量
        Vector3 vec = player.position - transform.position;
        //计算旋转角度
        Quaternion roataion = Quaternion.LookRotation(vec, Vector3.up);
        //平滑旋转摄像机
        transform.rotation = Quaternion.Lerp(transform.rotation,roataion,smooth * Time.deltaTime);
    }

    bool ViewingPosCheck(Vector3 checkPos)
    {

        RaycastHit hit;
        //从检测点发出一条指向主角的射线，能否检测到主角
        if (Physics.Raycast(checkPos, player.position - checkPos, out hit, ralCameraPosMag))
        {
            if (hit.transform != player)
                return false;
        }
        //如果没有检测到任何东西 说明人物与检测点中间没有障碍物，所以这个点可用
        newPos = checkPos;
        return true;
    }

}
