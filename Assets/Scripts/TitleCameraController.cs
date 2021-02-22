using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// タイトルシーンのカメラの動き
/// </summary>
public class TitleCameraController : MonoBehaviour
{
    [SerializeField] float m_oneLoopSpeed = 1f;
    CinemachineTrackedDolly m_dolly;


    // Start is called before the first frame update
    void Start()
    {
        m_dolly = GetComponent<CinemachineVirtualCamera>().GetCinemachineComponent<CinemachineTrackedDolly>();
    }

    // Update is called once per frame
    void Update()
    {
        m_dolly.m_PathPosition += m_oneLoopSpeed * Time.deltaTime;
    }
}
