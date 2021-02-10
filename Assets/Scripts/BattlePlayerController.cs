using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 味方戦闘時の行動
/// </summary>
public class BattlePlayerController : BattleStatusControllerBase
{
    /// <summary>
    /// 行動開始(味方)
    /// </summary>
    public override void StartAction()
    {
        base.StartAction();
        m_battleManager.StartCommandSelect(this);
    }

    /// <summary>
    /// 行動コマンド(Player)
    /// </summary>
    public void PlayerActionCommand()//(Skill skill)
    {
        m_battleManager.EndCommandSelect();
        //m_currentSkill = skill;
        SetTriggerAnimator("Attack");//(m_currentSkill.m_animation.neme)
    }

    /// <summary>
    /// アニメイベントAttack()
    /// </summary>
    public override void Hit()
    {
        base.Hit();
        Attack(FindObjectOfType<BattleEnemyController>());//(BEC, m_currentSkill.m_power)
    }
}
