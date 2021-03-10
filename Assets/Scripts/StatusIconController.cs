using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Playerのステータスアイコンを操作
/// </summary>
public class StatusIconController : MonoBehaviour
{
    [SerializeField] bool m_noMoveEffect = true;
    float m_effectTime = 0.2f;
    [SerializeField] Slider m_HPBar;
    [SerializeField] TextMeshProUGUI m_HPValue = null;
    [SerializeField] Slider m_SPBar = null;
    [SerializeField] TextMeshProUGUI m_SPValue = null;

    /// <summary>
    /// ステータスアイコンのセットアップ
    /// </summary>
    /// <param name="name"></param>
    /// <param name="maxHP"></param>
    /// <param name="currentHP"></param>
    /// <param name="maxSP"></param>
    /// <param name="currentSP"></param>
    public void SetupStatus(BattleStatusControllerBase status)
    {
        UpdateHPBar(status.m_MaxHP, status.m_CurrentHP);
        UpdateSPBar(status.m_MaxSP, status.m_CurrentSP);
    }

    /// <summary>
    /// HPBarを更新
    /// </summary>
    /// <param name="maxHP"></param>
    /// <param name="currentHP"></param>
    public void UpdateHPBar(int maxHP, int currentHP)
    {
        if (m_noMoveEffect)
        {
            m_HPBar.DOValue((float)currentHP / (float)maxHP, m_effectTime);
        }
        else
        {
            m_HPBar.transform.DOScale(new Vector3(1.5f, 0.9f, 1), 0.1f);
            m_HPBar.DOValue((float)currentHP / (float)maxHP, m_effectTime);
            m_HPBar.transform.DOScale(1f, 0.1f).SetDelay(m_effectTime);
        }
        
        if (m_HPValue)
        {
            if (m_noMoveEffect)
            {
                m_HPValue.text = currentHP.ToString();
            }
            else
            {
                DOTween.To(() => int.Parse(m_HPValue.text), x => m_HPValue.text = x.ToString(), currentHP, m_effectTime);
            }
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
            if (m_noMoveEffect)
            {
                m_SPBar.DOValue((float)currentSP / (float)maxSP, m_effectTime);
            }
            else
            {
                m_SPBar.transform.DOScale(new Vector3(1.5f, 0.9f, 1), 0.1f);
                m_SPBar.DOValue((float)currentSP / (float)maxSP, m_effectTime);
                m_SPBar.transform.DOScale(1f, 0.1f).SetDelay(m_effectTime);
            }
            
        }
        if (m_SPValue)
        {
            if (m_noMoveEffect)
            {
                m_SPValue.text = currentSP.ToString();
            }
            else
            {
                DOTween.To(() => int.Parse(m_SPValue.text), x => m_SPValue.text = x.ToString(), currentSP, m_effectTime);
            }
        }
    }
}
