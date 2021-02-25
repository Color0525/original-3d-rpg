using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// スキルデータ
/// </summary>
[CreateAssetMenu(fileName = "NewSkillData", menuName = "CreateSkillData")]
public class SkillData : ScriptableObject
{
    //データ
    [SerializeField] string m_skillName;
    [SerializeField, Multiline(4)] string m_skillInfo;
    [SerializeField] int m_costSP = 0;
    [SerializeField] float m_powerRate = 1f;
    [SerializeField] string m_stateName;
    [SerializeField] GameObject m_hitEffectPrefab = null;
    [SerializeField] bool m_fireEffect = false;

    //プロパティ
    public string m_SkillName { get { return m_skillName; } private set { m_skillName = value; } }
    public string m_SkillInfo { get { return m_skillInfo; } private set { m_skillInfo = value; } }
    public int m_CostSP { get { return m_costSP; } private set { m_costSP = value; } }
    public string m_StateName { get { return m_stateName; } private set { m_stateName = value; } }
    public GameObject m_HitEffectPrefab { get { return m_hitEffectPrefab; } private set { m_hitEffectPrefab = value; } }

    public bool m_FireEffect { get { return m_fireEffect; } private set { m_fireEffect = value; } }


    /// <summary>
    /// 威力の倍率を返す
    /// </summary>
    /// <param name="status"></param>
    /// <returns></returns>
    public virtual float GetPowerRate(BattleStatusControllerBase status)
    {
        return m_powerRate;
    }
}

