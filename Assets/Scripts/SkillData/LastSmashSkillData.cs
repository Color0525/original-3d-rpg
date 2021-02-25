using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 体力が少ないほど威力が上がるスキル
/// </summary>
[CreateAssetMenu(fileName = "NewLastSmashSkillData", menuName = "CreateLastSmashSkillData")]
public class LastSmashSkillData : SkillData
{
    public override float GetPowerRate(BattleStatusControllerBase status)
    {
        return (float)status.m_MaxHP / (float)status.m_CurrentHP;
    }
}
