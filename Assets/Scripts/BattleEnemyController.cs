using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 敵戦闘時の行動
/// </summary>
public class BattleEnemyController : BattleStatusControllerBase
{
    [SerializeField] GameObject m_DeadParticlePrefab;

    /// <summary>
    /// 行動(敵)
    /// </summary>
    public override void StartAction()
    {
        base.StartAction();
        EnemyAction();
    }

    /// <summary>
    /// 行動(Enemy)
    /// </summary>
    void EnemyAction()
    {
        //m_currentSkill = Random.Range(m_skills);
        SetTriggerAnimator("Attack");//(m_currentSkill.m_animation.neme)
    }

    /// <summary>
    /// アニメイベントAttack()
    /// </summary>
    public override void Hit()
    {
        base.Hit();
        Attack(FindObjectOfType<BattlePlayerController>());//(BEC, m_currentSkill.m_power)
    }

    void Dead()
    {
        Instantiate(m_DeadParticlePrefab, this.gameObject.transform.position, Quaternion.identity);
        this.gameObject.SetActive(false);
    }
}
