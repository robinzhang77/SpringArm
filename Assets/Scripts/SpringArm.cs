using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpringArm : MonoBehaviour
{
    [Space]
    [Header("SpringArm是一个摄像机的弹簧杆，杆分两头，一头挂着摄像机，一头attach到目标对象")]
    [Space]
    public Transform m_target = null;
    public Camera m_camera = null;
    public float m_armLength = 3.0f;
    public Vector3 m_socketOffset = Vector3.zero;
    public Vector3 m_tarOffset = Vector3.zero;

    [Space]
    [Header("碰撞检测")]
    [Space]
    public bool m_isCheckCollision = true;
    [Header("像机碰撞圆盘半径")]
    public float m_discRadius = 0.3f;
    [Header("从球边缘与目标连成的线段数量")]
    public int m_raycastNum = 4; //按360/m_raycastNum来采样点
    public LayerMask m_collisionLayerMask;

    [Space]
    [Header("弹簧，延时跟随")]
    [Space]
    public float m_smoothTime = 0.5f;

    private Vector3 m_cameraSlotPos;
    private RaycastHit[] hitedPoints; //发生的碰撞点
    private Vector3 m_collisionVelocity;
    void Start()
    {
        this.transform.position = Vector3.zero;
    
        //初始化相机的位置和角度
        
        hitedPoints = new RaycastHit[m_raycastNum + 1];
    }

    // Update is called once per frame
    void Update()
    {
        if (!m_target || !m_camera)
        {
            return;
        }

       if (m_camera)
        {
            m_camera.transform.LookAt(GetTarPos());
        }

        CalculateCameraPos();

        if (m_isCheckCollision)
        {
            CheckCollisions();
        }

        //设置像机槽的位置
        UpdateCameraSlotPostion();
    }

    private Vector3 GetTarPos()
    {
        return (m_target.position + m_target.rotation * m_tarOffset);
    }

    private void CalculateCameraPos()
    {
        Vector3 tarOffset = m_socketOffset - new Vector3(0, 0, m_armLength);
        m_cameraSlotPos = GetTarPos() + m_target.rotation * tarOffset;
    }

    private void UpdateCameraSlotPostion()
    {
        Vector3 pos = m_cameraSlotPos;
        Vector3 tarPos = GetTarPos();
        bool isCollistion = false;
        if (m_isCheckCollision)
        {
            float minDistance = m_armLength;
            for (int i=0; i<hitedPoints.Length; i++)
            {
                RaycastHit hitInfo = hitedPoints[i];
                if (!hitInfo.collider)
                {
                    continue;
                }
                float distance = Vector3.Distance(hitInfo.point, tarPos);
                if (distance < minDistance)
                {
                    minDistance = distance;
                }

                isCollistion = true;
                Vector3 dir = (m_cameraSlotPos - tarPos).normalized;
                Vector3 offset = dir * (m_armLength - minDistance);
                pos = m_cameraSlotPos - offset;
            }
        }

        float fSmooth = m_smoothTime;
        if (isCollistion)
        {
            //fSmooth = m_smoothTime * 0.1f;
        }

        foreach(Transform child in transform)
        {
            child.position = Vector3.SmoothDamp(child.position, pos, ref m_collisionVelocity, fSmooth);
        }
    }

    // private void CheckCollisions()
    // {
    //     float fAngle = 0f;
    //     float fAngleStep = 360 / m_raycastNum;
    //     for (int i=0; i<m_raycastNum; i++)
    //     {
    //         fAngle = fAngleStep * i;
    //         float fRadian = fAngle * Mathf.Deg2Rad;
    //         //按角度算出在圈盘上的采样点
    //         Vector3 samplePos = new Vector3(Mathf.Cos(fRadian), Mathf.Sin(fRadian), 0) * m_discRadius;

    //         Vector3 rayStartPos = m_cameraSlotPos + m_target.rotation * samplePos;
            
    //         Physics.Linecast(rayStartPos, GetTarPos(), out hitedPoints[i], m_collisionLayerMask);
    //     }
    // }

    private void CheckCollisions()
    {
        //从目标向摄像机方向引出一条射线
        Physics.Linecast(GetTarPos(), m_cameraSlotPos, out hitedPoints[0], m_collisionLayerMask);
    }
}
