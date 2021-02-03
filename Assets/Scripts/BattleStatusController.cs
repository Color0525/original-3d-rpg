using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleStatusController : MonoBehaviour
{
    [SerializeField] int m_maxLife = 10;
    [SerializeField] int m_currentLife = 10;
    [SerializeField] int m_power = 3;
    public Slider m_HPBar = null;

    /// <summary>
    /// 攻撃する
    /// </summary>
    /// <param name="target"></param>
    public void Attack(BattleStatusController target)
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
            FindObjectOfType<BattleManager>().DeleteUnitsList(this.gameObject);
        }
        m_HPBar.value = (float)m_currentLife / (float)m_maxLife;
    }
}
