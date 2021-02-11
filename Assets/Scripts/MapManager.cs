using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// MapSceneを管理
/// </summary>
public class MapManager : MonoBehaviour
{
    [SerializeField] bool m_hideSystemMouseCursor = false;

    void Start()
    {
        if (m_hideSystemMouseCursor)
        {
            Cursor.visible = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
