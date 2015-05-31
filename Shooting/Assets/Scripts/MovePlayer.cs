/***
 * 
 *      ProjectName:NightShooting
 * 
 *      Anthor     :wangchong
 * 
 *      CreateTime: 2014-11-21
 * 
 *      DESC       ：控制主角移动
 * 
 *      
 */
using UnityEngine;
using System.Collections;

public class MovePlayer : MonoBehaviour {

    void OnEnable()
    {

        EasyJoystick.On_JoystickMove += OnJoystickMove;

        EasyJoystick.On_JoystickMoveEnd += OnJoystickMoveEnd;

    }

    //移动摇杆结束  

    void OnJoystickMoveEnd(MovingJoystick move)
    {

        //停止时，角色恢复idle  

        if (move.joystickName == "Player")
        {

            mAnim.SetBool("Move", false);                                                                    //设置动画Move

        }

    }

    //移动摇杆中  

    void OnJoystickMove(MovingJoystick move)
    {








        //获取摇杆中心偏移的坐标  

        float joyPositionX = move.joystickAxis.x;

        float joyPositionY = move.joystickAxis.y;


       


        if (joyPositionY != 0 || joyPositionX != 0)
        {

            //设置角色的朝向（朝向当前坐标+摇杆偏移量）  
            transform.LookAt(new Vector3(transform.position.x + joyPositionX, transform.position.y, transform.position.z + joyPositionY));

            if (move.joystickName != "Player")
            {

                return;

            }
            mRbdPlayer.MovePosition(mTranPlayer.position + new Vector3(joyPositionX , 0, joyPositionY ) * this.mfltSleep * Time.deltaTime);                                                          //移动主角
            //移动玩家的位置（按朝向位置移动）  

            //transform.Translate(Vector3.forward * Time.deltaTime * 5);

            //播放奔跑动画  
           
            mAnim.SetBool("Move", true);                                                                    //设置动画Move
        }
    }  

    /// <summary>
    /// 移动速度
    /// </summary>
    public float mfltSleep = 10;

    /// <summary>
    /// 
    /// </summary>
    private int GroundMaskLayerIndex = -1;

    #region 组件信息
    /// <summary>
    /// 主角Transform组件
    /// </summary>
    private Transform mTranPlayer;

    /// <summary>
    /// 主角刚体
    /// </summary>
    private Rigidbody mRbdPlayer;

    /// <summary>
    /// 动画状态机
    /// </summary>
    private Animator mAnim; 
    #endregion

    void Awake()
    {
        mTranPlayer = this.transform;                                                                     //获取主角Transform组件
        mRbdPlayer = this.rigidbody;                                                                      //获取主角刚体组件
        mAnim = this.GetComponent<Animator>();                                                            //获取主角动画状态机
    }
	// Use this for initialization
	void Start () {
        GroundMaskLayerIndex = LayerMask.GetMask("Ground");
	}
	
	// Update is called once per frame
	void Update () {
        //#region 控制主角移动

        //float h = Input.GetAxis("Horizontal");                                                              //获取X轴上的移动速度
        //float v = Input.GetAxis("Vertical");                                                                //获取Y轴上的移动速度

        //mRbdPlayer.MovePosition(mTranPlayer.position + new Vector3(h * this.mfltSleep * Time.deltaTime
        //    , 0, v * mfltSleep * Time.deltaTime));                                                          //移动主角

        //if (h != 0 || v != 0)
        //{
        //    mAnim.SetBool("Move", true);                                                                    //设置动画Move
        //}
        //else
        //{
        //    mAnim.SetBool("Move", false);                                                                   //设置动画Idla
        //} 
        //#endregion

        //#region 控制主角旋转

        //Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);                                         //定义屏幕到鼠标点击的地方射出一条射线
        //RaycastHit hit;                                                                                      //定义射击目标
        //if (Physics.Raycast(ray, out hit, 200, GroundMaskLayerIndex))                                        //射出射线
        //{                                                                                                    //
        //    Vector3 TargerPos = new Vector3(hit.point.x,0,hit.point.z);                                      //获取射线坐标
        //    mTranPlayer.LookAt(TargerPos);                                                                   //使主角面对坐标点
        //}

        //#endregion
	}
}
