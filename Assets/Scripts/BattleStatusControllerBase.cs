using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 戦闘ユニットのステータス管理（継承用）
/// </summary>
public class BattleStatusControllerBase : MonoBehaviour
{
    [SerializeField] int m_maxLife = 10;
    [SerializeField] int m_currentLife = 10;
    [SerializeField] int m_power = 3;
    public Slider m_HPBarSlider;

    BattleManager m_bm;

    public virtual void Start()
    {
        m_bm = FindObjectOfType<BattleManager>();
    }

    private void Update()
    {
        if (m_bm == null)
        {
            Debug.Log(m_bm);
        }
    }

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
        m_currentLife = Mathf.Max(m_currentLife - power, 0);
        Debug.Log($"{this.gameObject.name} {power}Damage @{m_currentLife}");
        if (m_currentLife == 0)
        {
            Debug.Log(this.gameObject.name + " Dead");
            m_bm.DeleteUnitsList(this.gameObject);
        }
        m_HPBarSlider.value = (float)m_currentLife / (float)m_maxLife;
    }

    /// <summary>
    /// 行動開始
    /// </summary>
    public virtual void StartAction()
    {
        //m_bm = FindObjectOfType<BattleManager>();

        m_bm.StartActingTurn();
    }

    /// <summary>
    /// 行動終了
    /// </summary>
    public virtual void EndAction()
    {
        m_bm.EndActingTurn();
    }
}
