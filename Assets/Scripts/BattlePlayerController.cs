using FUnit.GameObjectExtensions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 味方戦闘時の行動
/// </summary>
public class BattlePlayerController : BattleStatusControllerBase
{
    [SerializeField] GameObject m_statusIconPrefab;
    Animator m_anim;

    /// <summary>
    /// PlayerのStatusIconを生成
    /// </summary>
    void Start()
    {
        GameObject go = Instantiate(m_statusIconPrefab, GameObject.FindWithTag("StatusPanel").transform);
        m_HPBarSlider = go.transform.Find("HPBar").GetComponent<Slider>();

        m_anim = GetComponent<Animator>();
    }

    /// <summary>
    /// 行動開始(味方)
    /// </summary>
    public override void StartAction()
    {
        base.StartAction();
        m_bm.StartCommandSelect(this);
    }

    /// <summary>
    /// 行動終了(味方)
    /// </summary>
    public override void EndAction()
    {
        base.EndAction();
        m_bm.EndCommandSelect();
    }

    /// <summary>
    /// 攻撃スキル
    /// </summary>
    public void AttackCommand()
    {
        m_anim.SetTrigger("Attack");
        Attack(FindObjectOfType<BattleEnemyController>());
        EndAction();
    }

}
