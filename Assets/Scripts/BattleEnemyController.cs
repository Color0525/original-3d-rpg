using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 敵戦闘時の行動
/// </summary>
public class BattleEnemyController : BattleStatusControllerBase
{
    [SerializeField] GameObject m_DeadParticlePrefab;
    [SerializeField] bool m_questTarget = false;

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
        //持っているスキルからランダムに使用
        m_CurrentSkill = m_HavesSkills[Random.Range(0, m_HavesSkills.Length)];
        //UseSP(m_CurrentSkill.m_CostSP); // 敵はSP消費なし
        PlayStateAnimator(m_CurrentSkill.m_StateName);
    }

    //public override void Death(BattleStatusControllerBase deadUnit)
    //{
    //    base.Death(deadUnit);
    //    if (m_questTarget)
    //    {
    //        SceneController.m_Instance.AddQuestCount();
    //    }
    //}

    // アニメイベント
    public override void Hit(BattleStatusControllerBase target = null)
    {
        BattlePlayerController thisTarget = FindObjectOfType<BattlePlayerController>();
        base.Hit(thisTarget); 
        //Instantiate(m_CurrentSkill.m_HitEffectPrefab, target.transform.position, target.transform.rotation);
        //Attack(target, m_CurrentSkill.GetPowerRate(this));
    }

    void Dead()
    {
        Instantiate(m_DeadParticlePrefab, this.gameObject.transform.position, Quaternion.identity);
        this.gameObject.SetActive(false);
    }
}
