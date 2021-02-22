using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ステータスアイコンを操作
/// </summary>
public class StatusIconController : MonoBehaviour
{
    [SerializeField] Slider m_HPBar;
    [SerializeField] TextMeshProUGUI m_HPValue = null;
    [SerializeField] Slider m_SPBar = null;
    [SerializeField] TextMeshProUGUI m_SPValue = null;
    //[SerializeField] TextMeshProUGUI m_name;

    /// <summary>
    /// ステータスアイコンのセットアップ
    /// </summary>
    /// <param name="name"></param>
    /// <param name="maxHP"></param>
    /// <param name="currentHP"></param>
    /// <param name="maxSP"></param>
    /// <param name="currentSP"></param>
    public void SetupStatus(int maxHP, int currentHP, int maxSP, int currentSP /*, string name*/ )
    {
        UpdateHPBar(maxHP, currentHP);    
        UpdateSPBar(maxSP, currentSP);
        
        //m_name.text = name;
    }

    /// <summary>
    /// HPBarを更新
    /// </summary>
    /// <param name="maxHP"></param>
    /// <param name="currentHP"></param>
    public void UpdateHPBar(int maxHP, int currentHP)
    {
        m_HPBar.value = (float)currentHP / (float)maxHP;
        if (m_HPValue)
        {
            m_HPValue.text = currentHP.ToString();
        }
    }

    /// <summary>
    /// SPBarを更新
    /// </summary>
    /// <param name="maxSP"></param>
    /// <param name="currentSP"></param>
    public void UpdateSPBar(int maxSP, int currentSP)
    {
        if (m_SPBar)
        {
            m_SPBar.value = (float)currentSP / (float)maxSP;
        }
        if (m_SPValue)
        {
            m_SPValue.text = currentSP.ToString();
        }
    }
}
