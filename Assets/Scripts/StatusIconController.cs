using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ステータスアイコンを操作
/// </summary>
public class StatusIconController : MonoBehaviour
{
    [SerializeField] Text m_name;
    [SerializeField] Slider m_HPBar;
    [SerializeField] Slider m_SPBar;

    /// <summary>
    /// ステータスアイコンのセットアップ
    /// </summary>
    /// <param name="name"></param>
    /// <param name="maxHP"></param>
    /// <param name="currentHP"></param>
    /// <param name="maxSP"></param>
    /// <param name="currentSP"></param>
    public void SetupStatus(string name, int maxHP, int currentHP, int maxSP, int currentSP)
    {
        if (name != null)
        {
            m_name.text = name;
        }
        else
        {
            m_name.gameObject.SetActive(false);
        }

        UpdateHPBar(maxHP, currentHP);

        if (maxSP > 0)
        {
            UpdateSPBar(maxSP, currentSP);
        }
        else
        {
            m_SPBar.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// HPBarを更新
    /// </summary>
    /// <param name="maxHP"></param>
    /// <param name="currentHP"></param>
    public void UpdateHPBar(int maxHP, int currentHP)
    {
        m_HPBar.value = (float)currentHP / (float)maxHP;
    }

    /// <summary>
    /// SPBarを更新
    /// </summary>
    /// <param name="maxSP"></param>
    /// <param name="currentSP"></param>
    public void UpdateSPBar(int maxSP, int currentSP)
    {
        m_SPBar.value = (float)currentSP / (float)maxSP;
    }
}
