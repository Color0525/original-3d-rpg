using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// BattleController継承用
/// </summary>
public class BattleUnitControllerBase : MonoBehaviour
{
//    [SerializeField] GameObject m_statusIconPrefab;
//    public RectTransform m_nowLifeBer;

//    void Start()
//    {
//        GameObject icon = Instantiate(m_statusIconPrefab, this.transform.position, Quaternion.identity);
//        icon.transform.SetParent(this.transform);
//    }

    /// <summary>
    /// 行動開始
    /// </summary>
    public virtual void StartAction() 
    {
        FindObjectOfType<BattleManager>().StartActingTurn();
    }

    /// <summary>
    /// 行動開始
    /// </summary>
    public virtual void EndAction()
    {
        FindObjectOfType<BattleManager>().EndActingTurn();
    }
}
