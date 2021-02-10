using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 戦闘ユニットのステータス管理（継承用）
/// </summary>
public class BattleStatusControllerBase : MonoBehaviour
{
    [SerializeField] string m_name;
    [SerializeField] int m_maxHP = 10;
    [SerializeField] int m_currentHP = 10;
    [SerializeField] int m_maxSP = 0;
    [SerializeField] int m_currentSP = 0;
    [SerializeField] int m_power = 3;

    public StatusIconController m_statusIcon;

    public static BattleManager m_bm;

    void Start()
    {
        m_bm = FindObjectOfType<BattleManager>();
        m_statusIcon.SetupStatus(m_name, m_maxHP, m_currentHP, m_maxSP, m_currentSP);
        //SetupAwake();
    }

    ///// <summary>
    ///// Awake時の処理(StatusIconをセット)
    ///// </summary>
    //public virtual void SetupAwake()
    //{
    //    m_statusIcon.SetupStatus(m_name, m_maxHP, m_currentHP, m_maxSP, m_currentSP);
    //}

    /// <summary>
    /// 攻撃する
    /// </summary>
    /// <param name="target"></param>
    public void Attack(BattleStatusControllerBase target)
    {
        Debug.Log(this.gameObject.name + " Attack");
        target.Damage(m_power);
    }

    /// <summary>
    /// ダメージを受ける
    /// </summary>
    /// <param name="power"></param>
    void Damage(int power)
    {
        UpdateHP(-power);
        Debug.Log($"{this.gameObject.name} {power}Damage @{m_currentHP}");
        if (m_currentHP == 0)
        {
            Debug.Log(this.gameObject.name + " Dead");
            m_bm.DeleteUnitsList(this.gameObject);
        }
    }

    /// <summary>
    /// HPを更新
    /// </summary>
    /// <param name="value"></param>
    void UpdateHP(int value = 0)
    {
        m_currentHP = Mathf.Max(m_currentHP + value, 0);
        m_statusIcon.UpdateHPBar(m_maxHP, m_currentHP);
    }

    /// <summary>
    /// 行動開始
    /// </summary>
    public virtual void StartAction()
    {
        m_bm.StartActingTurn();
    }

    /// <summary>
    /// 行動終了
    /// </summary>
    public void EndAction()
    {
        m_bm.EndActingTurn();
    }
}
