using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// コマンドボタンを管理
/// </summary>
public class CommandButtonController : MonoBehaviour
{ 
    //スキル
    SkillData m_currentSkill;
    //行動者
    BattlePlayerController m_actor;
    //テキスト
    [SerializeField] TextMeshProUGUI m_commandName;
    TextMeshProUGUI m_commandInfo;

    /// <summary>
    /// コマンドボタンをセットアップ
    /// </summary>
    /// <param name="skill"></param>
    /// <param name="actor"></param>
    /// <param name="text"></param>
    public void SetupCammand(SkillData skill, BattlePlayerController actor, TextMeshProUGUI text)
    {
        m_currentSkill = skill;
        m_actor = actor;
        m_commandInfo = text;
        m_commandName.text = m_currentSkill.m_SkillName;
    }

    /// <summary>
    /// ポイントされたとき呼ばれる
    /// </summary>
    public void ShowCommandInfo()
    {
        m_commandInfo.text = m_currentSkill.m_SkillInfo;
    }
    
    /// <summary>
    /// ポイントが外れたとき呼ばれる
    /// </summary>
    public void HideCommandInfo()
    {
        m_commandInfo.text = null;
    }

    /// <summary>
    /// 押されたときコマンド使用
    /// </summary>
    public void PlayCommand()
    {
        if (m_currentSkill.m_CostSP <= m_actor.m_CurrentSP)
        {
            m_actor.PlayerActionCommand(m_currentSkill);
        }
        else
        {
            FindObjectOfType<BattleManager>().ActionText("SPが足りない！");
        }
    }
}
