using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 味方戦闘時の行動
/// </summary>
public class BattlePlayerController : BattleStatusControllerBase
{
    [SerializeField] GameObject m_statusIconPrefab;

    void Awake()
    {
        //statusIconをセット
        GameObject statusIcon = Instantiate(m_statusIconPrefab, GameObject.FindWithTag("StatusPanel").transform);
        SetStatusIcon(statusIcon.GetComponent<StatusIconController>());
    }

    /// <summary>
    /// 行動開始(味方)
    /// </summary>
    public override void StartAction()
    {
        base.StartAction();
        m_battleManager.StartCommandSelect(m_HavesSkills, this);
    }

    /// <summary>
    /// 行動コマンド(Player)
    /// </summary>
    public void PlayerActionCommand(SkillData skill)
    {
        m_battleManager.EndCommandSelect();
        m_CurrentSkill = skill;
        UseSP(m_CurrentSkill.m_CostSP);
        PlayStateAnimator(m_CurrentSkill.m_StateName);
    }

    // アニメイベント    
    public override void Hit()
    {
        base.Hit();
        Attack(FindObjectOfType<BattleEnemyController>(), m_CurrentSkill.GetPowerRate(this));
    }
}
