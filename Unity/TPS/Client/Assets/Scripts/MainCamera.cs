using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamera : MonoBehaviour {
    private Transform m_CameraTransform;

    Transform m_TargetTransform;
    float distance = 2.5f;//水平距离
    float height = 3;//高度
    float cameraSpeed = 20.0f;
    float cameraRotateSpeed = 40.0f;
	void Start ()
    {
		m_CameraTransform=this.transform;
        m_TargetTransform = GameObject.FindGameObjectWithTag("Player").transform;
	}
	
	void Update () 
    {
        Translate();
        Rotate();

        
	}
    void Translate()
    {
        Vector3 targerPos = m_TargetTransform.position - m_TargetTransform.forward * distance + m_TargetTransform.up * height;

        m_CameraTransform.position = Vector3.Lerp(m_CameraTransform.position, targerPos, cameraSpeed * Time.deltaTime);
    }

    void Rotate()
    {
        //Debug.Log(m_TargetTransform.rotation);
        //修改摄像机朝向
        //Quaternion targetRotation = Quaternion.LookRotation((m_TargetTransform.position).normalized);
        m_CameraTransform.rotation = Quaternion.Slerp(m_TargetTransform.rotation, m_CameraTransform.rotation, cameraRotateSpeed * Time.deltaTime);
        //Vector3 targetPos = m_TargetTransform.position - (m_CameraTransform.position + Vector3.down * 3);
        //m_CameraTransform.LookAt(m_TargetTransform.position);
    }
}
