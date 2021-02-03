using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BattleEnemyController : BattleUnitControllerBase
{
    void Start()
    {
        
    }

    /// <summary>
    /// 行動(敵)
    /// </summary>
    public override void StartAction()
    {
        base.StartAction();
        BattleStatusController target = GameObject.FindObjectOfType<BattlePlayerController>().GetComponent<BattleStatusController>();
        GetComponent<BattleStatusController>().Attack(target);
        EndAction();
    }
}
