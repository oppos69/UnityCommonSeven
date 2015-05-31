/***
 * 
 *      ProjectName:Stealth
 * 
 *      Anthor     :wangchong
 * 
 *      CreateTime: 2014-11-21
 * 
 *      DESC       ：
 * 
 *      
 */
using UnityEngine;
using System.Collections;

public class LiftDoorsTracking : MonoBehaviour
{

    private float doorSpeed = 7;

    private Transform leftOuterDoor;
    private Transform rightOuterDoor;
    private Transform leftInnerDoor;
    private Transform rightInnerDoor;

    private float leftClosedPosx;
    private float rightClosedPosx;

    void Awake()
    {
        leftOuterDoor  = GameObject.Find("door_exit_outer_left_001").transform;
        rightOuterDoor = GameObject.Find("door_exit_outer_right_001").transform;
        leftInnerDoor  = GameObject.Find("door_exit_inner_left_001").transform;
        rightInnerDoor = GameObject.Find("door_exit_inner_right_001").transform;

        //设置当前门的坐标时的X坐标（因为门初始状态是关闭）
        leftClosedPosx  = leftInnerDoor.position.x;
        rightClosedPosx = rightInnerDoor.position.x;
    }

    void MoveDoors(float newLeftXTarget, float newRightXTarget)
    {
        // newX为左半边内层门移动时的X坐标
        float newX = Mathf.Lerp(leftInnerDoor.position.x, newLeftXTarget, doorSpeed * Time.deltaTime);

        // 让左半边内层门移动到相应的X坐标
        leftInnerDoor.position = new Vector3(newX, leftInnerDoor.position.y, leftInnerDoor.position.z);

        // 再让newX为右半边内层门移动的X坐标
        newX = Mathf.Lerp(rightInnerDoor.position.x, newRightXTarget, doorSpeed * Time.deltaTime);

        // 让右半边内层门移动到相应的X坐标
        rightInnerDoor.position = new Vector3(newX, rightInnerDoor.position.y, rightInnerDoor.position.z);
    }


    public void DoorFollowing()
    {
        // 让内层门随着外层门移动
        MoveDoors(leftOuterDoor.position.x, rightOuterDoor.position.x);
    }


    public void CloseDoors()
    {
        // 让内层门移动至关闭位置
        MoveDoors(leftClosedPosx, rightClosedPosx);
    }
}
