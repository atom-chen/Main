using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamera : MonoBehaviour {
    private Transform m_CameraTransform;

    Transform m_TargetTransform;
    float distance = 5.0f;//水平距离
    float height = 5;//高度
    float cameraSpeed = 20.0f;
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
        Vector3 targerPos = m_TargetTransform.position - Vector3.forward * distance + Vector3.up * height;

        m_CameraTransform.position = Vector3.Lerp(m_CameraTransform.position, targerPos, cameraSpeed * Time.deltaTime);
    }

    void Rotate()
    {
        //修改摄像机朝向
        Quaternion targetRotation = Quaternion.LookRotation((m_TargetTransform.position - (m_CameraTransform.position+Vector3.down*3)).normalized);
        transform.rotation = Quaternion.Slerp(targetRotation,m_CameraTransform.rotation, cameraSpeed * Time.deltaTime);
        //m_CameraTransform.LookAt(m_TargetTransform.position);
    }
}
