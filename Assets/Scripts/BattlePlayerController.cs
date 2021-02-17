using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 味方戦闘時の行動
/// </summary>
public class BattlePlayerController : BattleStatusControllerBase
{
    [SerializeField] GameObject m_statusIconPrefab;
    //[SerializeField] GameObject m_slideEffectPrefab;

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
        m_battleManager.StartCommandSelect(this);
        //StartCoroutine(StartCommandSelectDirecting());
    }
    //IEnumerator StartCommandSelectDirecting()
    //{
    //    GameObject go = Instantiate(m_slideEffectPrefab, GameObject.FindWithTag("MainCanvas").transform);
    //    Animator anim = go.GetComponent<Animator>();
    //    while (anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 1)
    //    {
    //        yield return new WaitForEndOfFrame();
    //    }
    //    Destroy(go);
    //    m_battleManager.StartCommandSelect(this);
    //}

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
