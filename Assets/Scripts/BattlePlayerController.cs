using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 味方戦闘時の行動
/// </summary>
public class BattlePlayerController : BattleStatusControllerBase
{
    //[SerializeField] GameObject m_statusIconPrefab;

    Animator m_anim;

    void Start()
    {
        m_anim = GetComponent<Animator>();
    }

    ///// <summary>
    ///// PlayerのStatusIconを生成
    ///// </summary>
    //public override void SetupAwake()
    //{
    //    GameObject go = Instantiate(m_statusIconPrefab, GameObject.FindWithTag("StatusPanel").transform);
    //    m_statusIcon = go.GetComponent<StatusIconController>();
    //    m_bm.SetupStatusIcon();
    //    base.SetupAwake();
    //}

    /// <summary>
    /// 行動開始(味方)
    /// </summary>
    public override void StartAction()
    {
        base.StartAction();
        m_bm.StartCommandSelect(this);
    }

    /// <summary>
    /// 攻撃スキル
    /// </summary>
    public void ActionCommand()//(Skill skill)
    {
        m_anim.SetTrigger("Attack");
        m_bm.EndCommandSelect();
    }

    /// Attackアニメーションイベント
    void HitAttack()
    {
        Attack(FindObjectOfType<BattleEnemyController>());
    }
    void EndAttack()
    {
        EndAction();
    }
}
