using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 常にカメラと同じ方向を向く
/// </summary>
public class BillboardController : MonoBehaviour
{ 
    void Update()
    {
        this.transform.forward = Camera.main.transform.forward;
    }
}
