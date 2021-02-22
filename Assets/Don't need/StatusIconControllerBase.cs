using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ステータスアイコンを操作
/// </summary>
public class StatusIconControllerBase : MonoBehaviour
{
    [SerializeField] Slider m_HPBar;

    /// <summary>
    /// ステータスアイコンのセットアップ
    /// </summary>
    /// <param name="maxHP"></param>
    /// <param name="currentHP"></param>
    public virtual void SetupStatus(int maxHP, int currentHP)
    {
        UpdateHPBar(maxHP, currentHP);
    }

    /// <summary>
    /// HPBarを更新
    /// </summary>
    /// <param name="maxHP"></param>
    /// <param name="currentHP"></param>
    public virtual void UpdateHPBar(int maxHP, int currentHP)
    {
        m_HPBar.value = (float)currentHP / (float)maxHP;
    }
}
