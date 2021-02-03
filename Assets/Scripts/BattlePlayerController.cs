using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattlePlayerController : BattleUnitControllerBase
{
    void Start()
    {
        
    }

    /// <summary>
    /// 行動開始(味方)
    /// </summary>
    public override void StartAction()
    {
        base.StartAction();
        FindObjectOfType<BattleManager>().StartCommandSelect(this);
    }

    /// <summary>
    /// 行動終了(味方)
    /// </summary>
    public override void EndAction()
    {
        base.EndAction();
        FindObjectOfType<BattleManager>().EndCommandSelect();
    }

    /// <summary>
    /// 攻撃スキル
    /// </summary>
    public void AttackCommand()
    {
        BattleStatusController target = GameObject.FindObjectOfType<BattleEnemyController>().GetComponent<BattleStatusController>();
        GetComponent<BattleStatusController>().Attack(target);
        EndAction();
    }

}
